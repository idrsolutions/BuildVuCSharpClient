using System;
using BuildVu;

class ExampleUsage
{
    static void Main(string[] args)
    {
        var buildvu = new BuildVu("http://localhost:8080/microservice-example");

        // Convert() returns a URL (string) where you can view the converted output.
        var outputURL = buildvu.Convert("path/to/input.pdf");
        // or alternatively to send url for server to download file from
        // var outputURL = buildvu.Convert("http://link.to/file.pdf", inputType: BuildVu.DOWNLOAD);

        // You can also specify a directory to download the converted output to:
        // buildvu.Convert("path/to/input.pdf", "path/to/output/dir");
    }
}
