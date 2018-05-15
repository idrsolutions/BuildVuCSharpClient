# BuildVu C# Client #

BuildVu C# Client is the C# API for IDRSolution's [BuildVu Microservice Example](https://github.com/idrsolutions/buildvu-microservice-example).

It functions as an easy to use, plug and play library that lets you use [BuildVu](https://www.idrsolutions.com/buildvu/) from C#. 

-----

# Installation #

## Using Nuget: ##

Run the following command to install the BuildVu C# Client in your project:

    $ nuget install BuildVuCSharpClient

-----

# Usage #

## Basic: #

Setup the converter details by creating a new `BuildVu` object:
```c#
using 'buildvu';
buildvu = new BuildVu('localhost:8080/microservice-example');
```

You can now convert files by calling `convert`:
```c#
// returns a URL where you can view the converted output in your web browser
var outputURL = buildvu.Convert('/path/to/input/file');

// you can optionally specify a directory to download the converted output to
buildvu.convert('/path/to/input/file', '/path/to/output/dir');
```

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