# JRequest.Net
JRequest.NET is a powerful library which allows applications to call web APIs using JSON.
## Benefits and Features
* Abstracts the complexity of calling web APIs from your .NET code.
* Supports HTTP and FTP web requests.
* Allows Request chaining (explained).
* Converts xml response to json or json to xml.
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
#### Protocol
##### Description
The type of protocol that is used in the internet.
#####
Arguments
| Property | Type | Mandatory |	Default Value |	Allowed Values | Description |
| -------- | ---- | --------- | ------------- | ----------------- | -------- |
| Protocol |	string |	true |	HTTP |	HTTP(s) ,FTP | The type of protocol that is used in the internet.
| Name | string |	false | "JRequest" | any string | The name of the root JRequest object.
| Requests | array |	True |	Null | Can be any number of HTTP(S) or FTP request objects. | Collection of request objects.
| RequestType | string | false | output | input, output | **input:** The response data will be saved in the global storage and the values can be used by other requests. **output:** The response data will not be saved in the global storage and used by other requests.
| Key | string | true | null | any string | Used to uniquely identify a request. Duplicate keys are not allowed.
| URL | string | true | null | valid URL | A reference to a web resource. Parameters can be included in the URL, however it is recommended to use the ``Parameters[]`` array to add parameter values.
| Method | string | false | GET | GET, POST | Methods used to send the request to a server.
| ContentType | string | false | application/json | application/json, application/xml | Indicates the media type of the resource. Content type can also be specified inside the header.
| Parameters | array | false | null | any number of key value paired objects | As an alternative of adding parameters in the url, it's recommended to add parameters in the parameters array in the format of {"key", "value"} pairs.
| Headers | array | false | null | any number of key value paired objects | Allows the request to send additional information to the server. Example: {"Authorization": "basic aGVsbG8gd29ybGQ="}
| Body | string | false | null | any string | Used to send data to the server when request method is POST
| Authorization | object | false | null | authorization object | Used to send authentication credentials in the header of the request. There are two type of authorization can be used in the Authorization object of JRequest. **Basic Authentication** transmits credentials as user ID/password pairs, encoded using base64. **Bearer Authentication(Token Authentication)** uses security tokens called bearer tokens to authenticate. **Note:** As the user ID and password are passed over the network as clear text (it is base64 encoded, but base64 is a reversible encoding), the basic authentication scheme is not secure. HTTPS / TLS should be used in conjunction with basic authentication. Without these additional security enhancements, basic authentication should not be used to protect sensitive or valuable information.

### Usage
#### Example 1
In this example we are using [JSONPlaceholder](https://jsonplaceholder.typicode.com), a simple fake REST API for testing and prototyping. The JRequest JSON object, named "Dummy", has two requests pointing to different resources. The first request sends a GET request to https://jsonplaceholder.typicode.com/posts/1. And the second request again sends a GET request to https://jsonplaceholder.typicode.com/posts/1/comments and configured to convert the response data to xml. The first request uses the bare minimum requirements to call an API from JRequest.Net.

```
using System;
using System.IO;
using JRequest.Net;

static void Main(string[] args)
{
    string json = "@"{
    'Protocol': 'https',
    'Name': 'Dummy',
    'Requests': [
      {
        'Key': 'post1',
        'URL': 'https://jsonplaceholder.typicode.com/posts/1'
      },
      {
        'RequestType': 'output',
        'Key': 'post1comments',
        'URL': 'https://jsonplaceholder.typicode.com/posts/1/comments',
        'Method': 'GET',
        'Headers': [
          { 'Content-Type': 'application/json' }
        ],
        'Configuration': {
          'Output': {
            'Type': 'xml'
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
