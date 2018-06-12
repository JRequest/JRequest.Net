# JRequest.Net
JRequest.NET is a powerful library which allows applications to call web APIs using JSON.
## Benefits and Features
* Abstracts the complexity of calling web APIs from your .NET code.
* Supports HTTP and FTP web requests.
* Allows Request chaining.
* Converts response data from xml to json or viceversa.
* Support .NET Standard 2.0

## Getting Started

### Installation
You can clone JRequest.Net from GitHub or install it directly into your project from [NuGet](https://www.nuget.org/packages/JRequest.NET/ "Get the latest version from NuGet") package manager.
```
//example
PM> Install-Package JRequest.NET -Version 1.2.0
```
### Running JRequest.Net
#### JRequest JSON Schema
```
{
  "Protocol": "",
  "Name": "",
  "Requests": [
    {
      "RequestType": "",
      "Key": "",
      "URL": "",
      "Method": "",
      "ContentType": "",
      "Authorization": {
          "Type": "",
          "Token",
          "Username",
          "Password"
      },
      "Parameters": [],
      "Headers": [],
      "Configuration": {
        "Output": {
          "Type": ""
        }
      }
      "Ordinal": 1
    }
  ]
}
```
### Schema Description
| Property | Type | Mandatory |	Default Value |	Allowed Values | Description |
| -------- | ---- | --------- | ------------- | ----------------- | -------- |
| Protocol |	string |	true |	HTTP |	HTTP(s) ,FTP | The type of protocol that is used in the internet.
| Name | string |	false | "JRequest" | any string | The name of the root JRequest object.
| Requests | array |	True |	Null | Can be any number of HTTP(S) or FTP request objects. | Collection of request objects.
| RequestType | string | false | output | input,output | **input:** The response data will be saved in the global storage and the values can be used by other requests. **output:** The response data will not be saved in the global storage and used by other requests.
| Key | string | true | null | any string | Used to uniquely identify a request. Duplicate keys are not allowed.
| URL | string | true | null | valid URL | A reference to a web resource. Parameter values can be included in the URL, however it is recommended to use the parameters property to add parameter values.
| Method | string | false | GET | GET,POST | Methods used to send the request to a server.
| ContentType | string | false | application/json | application/json, application/xml | Indicates the media type of the resource. Content type can also be specified inside the header.
| Parameters | array | false | null | any number of key value paired objects | As an alternative of adding parameters in the url, it's recommended to add parameters in the parameters array in the format of {"key", "value"} pairs.
| Headers | array | false | null | any number of key value paired objects | Allows the request to send additional information to the server. Example: {"Authorization": "basic aGVsbG8gd29ybGQ="}
| Body | string | false | null | any string | Used to send data to the server when request method is POST
| Authorization | object | false | null | authorization object | Used to 

### Usage
#### Example 1
In this tutorial we are going to send http request to "https://jsonplaceholder" web API from a console application using C#.  
**Note** the json string used in the example is not properly formatted for readability purpose. If you want to copy and paste it in your code, just add a backslash ("\") before every double quote(").
```
using System;
using System.IO;
using JRequest.Net;
static void Main(string[] args)
{
    string json = "{
    "protocol": "https",
    "name": "Dummy Request",
    "requests": [
      {
        "key": "post1",
        "url": "https://jsonplaceholder.typicode.com/posts/1",
        "contenttype": "application/json"
      },
      {
        "requesttype": "output",
        "key": "post1comments",
        "url": "https://jsonplaceholder.typicode.com/posts/1/comments",
        "method": "GET",
        "headers": [
          { "Content-Type": "application/json" }
        ],
        "configuration": {
          "output": {
            "type": "xml"
          }
        }
      }
    ]
  }"

  var jRequest = new JRequestContext(json).Build().Run();

  Console.WriteLine($"JRequest Name: {jRequest.Name}");
  foreach (var request in jRequest.Requests)
  {
      Console.WriteLine($"Request Key: {request.Key}");
      Console.WriteLine($"Response Status: {request.Response.Status}");
      Console.WriteLine("Response Content:");
      Console.WriteLine(request.Response.Content);
      Console.WriteLine("--------------------------------------------------------------------------------");
      Console.WriteLine(" ");
  }

  Console.Read();
}

```

### Request Chaining

### Cookies
