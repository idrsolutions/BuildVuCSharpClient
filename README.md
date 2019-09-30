# BuildVu C# Client #

Convert PDF to HTML5 or SVG with C#, using the BuildVu C# Client to interact with IDRsolutions' [BuildVu Microservice Example](https://github.com/idrsolutions/buildvu-microservice-example).

The BuildVu Microservice Example is an open source project that allows you to convert PDF to HTML5 or SVG by running [BuildVu](https://www.idrsolutions.com/buildvu/) as an online service.

IDRsolutions offer a free trial service for running BuildVu with C#, more infomation on this can be found [here](https://www.idrsolutions.com/buildvu/convert-pdf-in-c-sharp/).

-----

# Installation #

## Using Nuget: ##

Run the following command to install the BuildVu C# Client in your project:

    $ nuget install BuildVuCSharpClient

-----

# Usage #

## Basic: (Upload) #

Setup the converter details by creating a new `BuildVu` object:
```c#
using buildvu_csharp_client;
BuildVu buildvu = new BuildVu("localhost:8080/buildvu-microservice");
```
Set up the conversion parameters
```c#
Dictionary<string, string> parameters = new Dictionary<string, string>
{
    ["input"] = BuildVu.UPLOAD,
    ["file"] = "path/to/input.pdf",
    ["callbackUrl"] = "http://listener.url" //Optional
};
```

You can now convert files by calling `Convert`:
```c#
// conversionResults are the values from the servers response
Dictionary<string, string> conversionResults = buildvu.Convert(parameters);

// You can also specify a directory to download the converted output to:
buildvu.DownloadResult(conversionResults, "path/to/output/dir");
```

## Basic: (Download) #

Setup the converter details by creating a new `BuildVu` object:
```c#
using buildvu_csharp_client;
BuildVu buildvu = new BuildVu("localhost:8080/buildvu-microservice");
```

Set up the conversion parameters
```c#
Dictionary<string, string> parameters = new Dictionary<string, string>
{
    ["input"] = BuildVu.DOWNLOAD,
    ["url"] = "http://link.to/filename"
};
```

You can now convert files by calling `Convert`:
```c#
// conversionResults are the values from the servers response
Dictionary<string, string> conversionResults = buildvu.Convert(parameters);

// You can also specify a directory to download the converted output to:
buildvu.DownloadResult(conversionResults, "path/to/output/dir");
```

The parameters used are defined in the API, see [API.md](https://github.com/idrsolutions/buildvu-microservice-example/blob/master/API.md) for more details.

See `example_usage.cs` for examples.

-----

# Who do I talk to? #

Found a bug, or have a suggestion / improvement? Let us know through the Issues page.

Got questions? You can contact us [here](https://idrsolutions.zendesk.com/hc/en-us/requests/new).

-----

# Code of Conduct #

Short version: Don't be an awful person.

Longer version: Everyone interacting in the BuildVu C# Client project's codebases, issue trackers, chat rooms and mailing lists is expected to follow the [code of conduct](CODE_OF_CONDUCT.md).  

-----
Copyright 2018 IDRsolutions

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

[http://www.apache.org/licenses/LICENSE-2.0](http://www.apache.org/licenses/LICENSE-2.0)

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
