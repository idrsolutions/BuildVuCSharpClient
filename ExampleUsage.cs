using System;
using BuildVu;

class ExampleUsage
{
    static void Main(string[] args)
    {
        var buildvu = new BuildVu("http://localhost:8080/microservice-example");
        var outputURL = buildvu.Convert("path/to/input.pdf", "path/to/output/dir");
    }
}