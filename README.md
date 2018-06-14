# JRequest.Net
JRequest.NET is a powerful library which allows applications to call web APIs using JSON.
## Benefits and Features
* Abstracts the complexity of calling web APIs from your .NET code.
* Supports HTTP and FTP web requests.
* Allows Request chaining (explained).
* Converts XML responses to JSON or JSON to XML.
* Very lightweight and simple to use.
* Built in .NET Standard 2.0

## Getting Started

### Installation
You can clone JRequest.Net from GitHub or install it directly into your project from [NuGet](https://www.nuget.org/packages/JRequest.NET/ "Get the latest version from NuGet") package manager.
```
PM> Install-Package JRequest.NET -Version 1.4.1
```
### Running JRequest.Net
#### JRequest JSON Schema
```
{
  'description': 'JRequest.Net',
  'type': 'object',
  'properties':
   {
    'protocol': {'type':'string'},
    'name': {'type':'string'},
    'requests': {
        'type': 'array',
        'items': {
            'allOf': [
                {
                'key': {'type': 'string'},
                'url': {'type': 'string'},
                'method': {'type': 'string'},
                'contenttype': {'type': 'string'},
                'parameters': {
                    'type': 'array',
                    'items': {'type': 'string'}
                    },
                'headers': {
                    'type': 'array',
                    'items': {'type': 'string'}
                    },
                'authorization': {
                    'type': 'object',
                    'properties': {
                        'type': {'type': 'string'},
                        'token': {'type': 'string'},
                        'username': {'type': 'string'},
                        'password': {'type': 'string'}
                        }
                    },
                'configuration': {
                    'type': 'object',
                    'properties': {
                        'output': {
                            'type': 'object',
                            'properties': {
                                'outputtype': {'type': 'string'}
                                }
                            }
                        }
                 }
             ]
         },                                 
    'ordinal': {'type': 'string'}
}
```

| Property | Value Type | Mandatory |	Default Value |	Allowed Values | Description |
| -------- | ---- | --------- | ------------- | ----------------- | -------- |
| `Protocol` |	string |	true |	HTTP |	HTTP(s) ,FTP | The type of protocol that is used in the internet.
| `Name` | string |	false | JRequest.Net | any string | The name of the root JRequest object.
| `Requests` | array |	true |	Null | Can be any number of HTTP(S) or FTP request objects. | We can add any number of request objects in `Requests` array by assigning a unique key to each request.
| `Key` | string | true | null | any string | Used to uniquely identify a request. Duplicate keys are not allowed.
| `URL` | string | true | null | valid HTTP/FTP URL | Is a reference to a web resource. Parameters can be included in the URL, however it is recommended to use the ``Parameters[]`` array to add parameter values.
| `Method` | string | false | GET | GET, POST | The methods used to send the request to a server.
| `ContentType` | string | false | application/json | valid HTTP Content-Type | Indicates the media type of the resource. Content type can also be specified inside the headers collection.
| `Parameters` | array | false | null | any number of key value paired objects | As an alternative of adding parameters in the url, we can add parameters in the parameters collection as {"key", "value"} pairs.
| `Headers` | array | false | null | any number of key value paired objects | Allows the request to send additional information to the server. Example: {"Authorization": "basic aGVsbG8gd29ybGQ="}
| `Body` | string | false | null | any string | Used to send data to the server when request method is POST.
| `Authorization` | object | false | null | authorization object | Used to send authentication credentials in the header of the request. There are two type of authorization that can be used in the Authorization object of JRequest. **Basic Authentication** transmits credentials as user ID/password pairs, encoded using base64. **Bearer Authentication(Token Authentication)** uses security tokens called bearer tokens to authenticate. **Note:** As the user ID and password are passed over the network as clear text (it is base64 encoded, but base64 is a reversible encoding), the basic authentication scheme is not secure. HTTPS/TLS should be used in conjunction with basic authentication. Without these additional security enhancements, basic authentication should not be used to protect sensitive or valuable information.
| `Configuration` | object | false | null | configuration object | It can be used to pass additional settings to the engine. |
| `Output` | object | false | null | Output object | Provides properties for output settings. |
| `OutputType` | string | false | The API response's content type | JSON, XML | Used to specify the convert to type. Output conversion only works from JSON to XML or viceversa.

#### JRequest Class
```
 public class Jrequest
  {
      public string Protocol { get; set; } = Enumerators.Protocol.http.ToString();
      public string Name { get; set; } = "JRequest";
      public List<Request> Requests { get; set; }
      public Configuration Configuration { get; set; }
  }
```
#### Request Class
```
 public class Request
  {
      public string Key { get; set; }
      public string URL { get; set; }
      public string Method { get; set; } = HttpMethod.GET.ToString();
      public string ContentType { get; set; } = "application/json";
      public List<Dictionary<string, string>> Parameters { get; set; }
      public List<Dictionary<string, string>> Headers { get; set; }
      public string Body { get; set; }
      public string Client { get; set; }
      public string FilePath { get; set; }
      public string FileType { get; set; }
      public int Ordinal { get; set; } = 0;
      public Configuration Configuration { get; set; }
      public Authorization Authorization { get; set; }
      public Response Response { get; set; }
  }
```
### Response Class
```
public class Response
{
    public string RequestKey { get; set; }
    public Dictionary<string, string> Headers { get; set; }
    public Dictionary<string, string> Cookies { get; set; }
    public string ContentType { get; set; }
    public long ContentLength { get; set; }
    public string Content { get; set; }
    public string Status { get; set; }
    public static List<Dictionary<string, object>> RequestResources;
}
```
### JRequestService Class
```
public class JRequestService
{
    public string Json { get; set; }
    public JRequestService(){}

    public JRequestService(string json)
    {
        Json = json;
    }

    public Jrequest Run()
    {
        Build(Json);
        return JRequestEngine.Run();
    }
    public Jrequest Run(string json)
    {
        Build(json);
        return JRequestEngine.Run();
    }
    protected Jrequest Build(string json)
    {
        return JRequestEngine.Build(json);
    }
}
```
#### Example 1
In this example we are calling [JSONPlaceholder](https://jsonplaceholder.typicode.com) REST API from a console app using JRequest.  
**Note** here we are using the JRequest with the bare minimum requirements.

```
using System;
using System.IO;
using JRequest.Net;

static void Main(string[] args)
{
    //JRequest JSON string
    string json = "@"{
    'Protocol': 'https',
    'Name': 'dummy request',
    'Requests': [
      {
        'Key': 'post1',
        'URL': 'https://jsonplaceholder.typicode.com/posts/1'
      }
    ]
  }"

  var jRequest = new JRequestService(json).Run();

  Console.WriteLine($"Name: {jRequest.Name}");
  foreach (var request in jRequest.Requests)
  {
      Console.WriteLine($"Request Key: {request.Key}");
      Console.WriteLine($"Response Status: {request.Response.Status}");
      Console.WriteLine("Response Content:");
      Console.WriteLine(request.Response.Content);
      Console.WriteLine(" ");
  }

  Console.Read();
}

```

### Request Chaining

### Cookies
