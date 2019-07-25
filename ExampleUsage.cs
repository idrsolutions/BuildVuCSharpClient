using System;
using System.Collections.Generic;
using buildvu_csharp_client;

class ExampleUsage
{
    static void Main(string[] args)
    {
        var buildvu = new BuildVu("http://localhost:8080/microservice-example");

        try
        {
            // Convert takes a Dictionary of the API parameters, that then get passed onto 
            // the server. For example, callbackUrl will provide a URL that you want to have
            // a request sent to when the conversion finishes.
            // See https://github.com/idrsolutions/buildvu-microservice-example/blob/master/API.md
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                ["input"] = BuildVu.UPLOAD,
                ["file"] = "path/to/input.pdf",
                ["callbackUrl"] = "http://listener.url"
            };

            // Alternatively send a URL for the server to download file from
            /*Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                ["input"] = BuildVu.DOWNLOAD,
                ["url"] = "http://link.to/filename"
            };*/

            // Convert() returns a Dictionary (<string, string>) which returns the values 
            // in the servers response
            Dictionary<string, string> conversionResults = buildvu.Convert(parameters);

            String outputUrl = conversionResults.GetValueOrDefault("downloadUrl", "No download URL provided");

            // You can also specify a directory to download the converted output to:
            //buildvu.DownloadResult(conversionResults, "path/to/output/dir");

            Console.WriteLine("Converted: " + outputUrl);
        }
        catch (Exception e)
        {
            Console.WriteLine("File conversion failed: " + e.Message);
        }
    }
}
