using System;
using buildvu_csharp_client;

class ExampleUsage
{
    static void Main(string[] args)
    {
        var buildvu = new BuildVu("http://localhost:8080/microservice-example");

        try
        {
            // Prepare a local file to be uploaded to the BuildVu MicroService
            buildvu.PrepareFile("path/to/input.pdf");

            // Convert takes a Dictionary of the API parameters, that then get passed onto 
            // the server. For example, callbackUrl will provide a URL that you want to have
            // a request sent to when the conversion finishes.
            // See https://github.com/idrsolutions/buildvu-microservice-example/blob/master/API.md
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                ["input"] = BuildVu.UPLOAD,
                ["callbackUrl"] = "http://listener.url"
            };

            // Alternatively send a URL for the server to download file from
            // Note: You do not require to PrepareFile() if you use this configuration
            /*Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                ["input"] = BuildVu.DOWNLOAD,
                ["url"] = "http://link.to/filename"
            };*/

            // Convert() returns a Dictionary (<string, string>) which returns the values 
            // in the servers response
            Dictionary<string, string> conversionResults = buildvu.Convert(parameters);

            String outputUrl = conversionResults.GetValueOrDefault("previewUrl", "No output URL provided");

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
