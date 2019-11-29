/**
Copyright 2018 IDRsolutions

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.

You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

Main class used to interact with the buildvu web app
For detailed usage instructions, see the GitHub repository:
https://github.com/idrsolutions/BuildVuCSharpClient
**/

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Extensions;

namespace buildvu_csharp_client
{
    /// <summary>
    /// Used to interact with IDRsolutions' BuildVu web service
    /// For detailed usage instructions, see GitHub[https://github.com/idrsolutions/BuildVuCSharpClient]
    /// </summary>
    public class BuildVu
    {
        public const string DOWNLOAD = "download";
        public const string UPLOAD = "upload";

        private readonly string _baseEndpoint;
        private readonly string _endpoint;
        private readonly int _requestTimeout;
        private readonly int _conversionTimeout;
        private readonly RestClient _restClient;

        /// <summary>
        /// Constructor, setup the converter details
        /// </summary>
        /// <param name="url">string, the URL of the BuildVu web service.</param>
        /// <param name="conversionTimeout">int, (optional) the time to wait (in seconds) before timing out the conversion.
        /// Set to 30s by default.</param>
        /// <param name="requestTimeout">int, (optional) the time to wait (in milliseconds) before timing out each request.
        /// Set to 60000ms (60s) by default.</param>
        public BuildVu(string url, int conversionTimeout = 30, int requestTimeout = 60000)
        {
            _baseEndpoint = url;
            _endpoint = "buildvu";
            _requestTimeout = requestTimeout;
            _conversionTimeout = conversionTimeout;
            _restClient = new RestClient(_baseEndpoint);
        }

        /// <summary>
        /// Constructor, setup the converter details allowing for authentication details
        /// </summary>
        /// <param name="url">string, the URL of the BuildVu web service.</param>
        /// <param name="username">string, the Username required to connect to the BuildVu web service.</param>
        /// <param name="password">string, the Password required to connect to the BuildVu web service.</param>
        /// <param name="conversionTimeout">int, (optional) the time to wait (in seconds) before timing out the conversion.
        /// Set to 30s by default.</param>
        /// <param name="requestTimeout">int, (optional) the time to wait (in milliseconds) before timing out each request.
        /// Set to 60000ms (60s) by default.</param>
        public BuildVu(string url, string username, string password, int conversionTimeout = 30, int requestTimeout = 60000)
        {
            _baseEndpoint = url;
            _endpoint = "buildvu";
            _requestTimeout = requestTimeout;
            _conversionTimeout = conversionTimeout;
            _restClient = new RestClient(_baseEndpoint);
            _restClient.Authenticator = new HttpBasicAuthenticator(username, password);
        }

        /// <summary>
        /// Starts the conversion of a file and returns a dictionary with the response from the server.
        /// Details for the parameters passed can be found at:
        /// https://github.com/idrsolutions/buildvu-microservice-example/blob/master/API.md
        /// </summary>
        /// <param name="parameters">Dictionary(string, string), the parameters to be passed to the server</param>
        /// <returns>Dictionary(string, string), the response output from the server</returns>
        public Dictionary<string, string> Convert(Dictionary<string, string> parameters)
        {

            // Upload file and get conversion ID
            string uuid = Upload(parameters);

            // Define now so we can access response content outside of loop
            Dictionary<string, string> responseContent;

            // Check conversion status once every second until complete or error / timeout
            var i = 0;
            while (true)
            {
                Thread.Sleep(1000);

                string rawContent = PollStatus(uuid).Content;
                responseContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(rawContent);

                if (responseContent.ContainsKey("state"))
                {
                    if (responseContent["state"] == "processed")
                    {
                        break;
                    }

                    if (responseContent["state"] == "error")
                    {
                        throw new Exception("Server error getting conversion status, see server logs for details");
                    }
                }

                if (parameters.ContainsKey("callbackUrl"))
                {
                    break;
                }

                if (i >= _conversionTimeout)
                {
                    throw new Exception("Failed: File took longer than " + _conversionTimeout + " seconds to convert.");
                }

                i++;
            }

            if (responseContent == null)
            {
                responseContent = new Dictionary<string, string>();
            }

            return responseContent;
        }

        /// <summary>
        /// Download a copy of the converted output to the specified location.
        /// </summary>
        /// <param name="results">Dictionary(string, string), the results dictionary produced by <see cref="Convert"></param>
        /// <param name="outputFilePath">string, the directory the output will be saved in, i.e 'path/to/output/dir</param>
        /// <param name="fileName">string, (optional) the preferred name of the output zip file</param>
        public void DownloadResult(Dictionary<string, string> results, string outputFilePath, string fileName = null)
        {
            if (!results.ContainsKey("downloadUrl"))
            {
                throw new Exception("Failed: No URL to download from provided");
            }

            if (fileName != null)
            {
                outputFilePath += '/' + fileName + ".zip";
            }
            else
            {
                outputFilePath += '/' + Path.GetFileNameWithoutExtension(results["downloadUrl"]) + ".zip";
            }

            Download(results["downloadUrl"], outputFilePath);
        }

        private string Upload(Dictionary<string, string> parameters)
        {
            var request = new RestRequest(_endpoint)
            {
                Method = Method.POST,
                Resource = "buildvu",
                Timeout = _requestTimeout
            };

            if (parameters.ContainsKey("file"))
            {
                byte[] file = File.ReadAllBytes(parameters["file"]);
                string fileName = Path.GetFileName(parameters["file"]);

                if (file.Length > 0)
                {
                    request.AddFile("file", file, fileName);
                }
            }

            foreach (KeyValuePair<string, string> param in parameters)
            {
                if (!param.Key.Equals("file"))
                {
                    request.AddParameter(param.Key, param.Value);
                }
            }

            var response = _restClient.Execute(request);
            if (response.ErrorException != null)
            {
                throw new Exception("Error uploading file:\n" + response.ErrorException.GetType() + "\n"
                                    + response.ErrorMessage);
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Error uploading file:\nServer returned response\n" + response.StatusCode + " - "
                                    + response.StatusDescription);
            }

            var content = response.Content;
            Dictionary<string, string> parsedResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);

            if (!parsedResponse.ContainsKey("uuid"))
            {
                throw new Exception("Error uploading file:\nServer returned null UUID");
            }

            return parsedResponse["uuid"];
        }

        private IRestResponse PollStatus(string uuid)
        {
            var request = new RestRequest(_endpoint)
            {
                Method = Method.GET,
                Resource = "buildvu",
                Timeout = _requestTimeout
            };
            request.AddParameter("uuid", uuid);

            var response = _restClient.Execute(request);

            if (response.ErrorException != null)
            {
                throw new Exception("Error checking conversion status:\n" + response.ErrorException.GetType() + "\n"
                                    + response.ErrorMessage);
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Error checking conversion status:\n Server returned response\n"
                                    + response.StatusCode + " - " + response.StatusDescription);
            }

            return response;
        }

        private void Download(string downloadUrl, string outputFilePath)
        {
            try
            {
                var request = new RestRequest(downloadUrl, Method.GET);
                _restClient.DownloadData(request).SaveAs(outputFilePath);
            }
            catch (Exception e)
            {
                throw new Exception("Error downloading conversion output:\n" + e.Message);
            }
        }
    }
}
