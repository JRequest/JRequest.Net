# JRequest.Net
JRequest.NET is a powerful library which allows applications to call web APIs using JSON.
## Benefits and Features
* Abstracts the complexity of calling web APIs from your .NET code.
* Supports HTTP and FTP web requests.
* Allows Request dependency.
* Converts XML responses to JSON or JSON to XML.
* Lightweight and simple to use.
* Can be referenced from all .NET implementations, such as .NET Framework, .NET Core and Xamarin.

## Getting Started

### Installation
You can clone JRequest.Net from GitHub or install it directly into your project from [NuGet](https://www.nuget.org/packages/JRequest.NET/ "Get the latest version from NuGet") package manager.
```
PM> Install-Package JRequest.NET -Version 1.4.1
```
#### A Complete JRequest JSON Schema
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
            'oneOf': [
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
                'filepath': {'type': 'string'},
                'filetype': {'type': 'string'},
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
| `FilePath` | string | true (if Protocol is FTP) | null | any valid file path | Specifies the location of a file in the FTP server. |
| `FileType` | string | true (if Protocol is FTP) | null | any kind of File extension | Specifies the type of file extension. |
| `Ordinal` | number | false | 0 | Orders the 
| `Configuration` | object | false | null | configuration object | It can be used to pass additional settings to the engine. |
| `Output` | object | false | null | Output object | Provides properties for output settings. |
| `Type` | string | false | The API response's content type | JSON, XML | Used to specify the type of file to convert the response data to. Output conversion only supports converting JSON file to XML or viceversa.
| `Ordinal` | number | false | 0 | any number | Determines the order of requests to be executed first. |
---
#### Examples 1
Sending a simple **GET** request to [JSONPlaceholder](https://jsonplaceholder.typicode.com) web API.

```
using System;
using JRequest.Net;

static void Main(string[] args)
{
  //JRequest JSON string
  string json = @"{
            'protocol': 'https',
            'name': 'JsonPlaceholder',
            'requests': [
              {
                  'key': 'get_test',
                  'url': 'https://jsonplaceholder.typicode.com/posts/1',
              }
            ]
        }";

  var jRequest = new JRequestService(json).Run(); //returns a Jrequest object

  Console.WriteLine($"Name: {jRequest.Name}");
  foreach (var request in jRequest.Requests)
  {
      Console.WriteLine($"Request Key: {request.Key}");
      Console.WriteLine($"Status: {request.Response.Status}");
      Console.WriteLine("Response:");
      Console.WriteLine(request.Response.Content);
  }

  Console.Read();
}

```
#### output

![post1](https://user-images.githubusercontent.com/39979029/41504578-62aadd5c-71c1-11e8-883a-fd234623ffe3.png)

---
#### Examples 2
Sending a simple **POST** request to [RestTestTest](https://resttesttest.com/#) web API.

```
using System;
using JRequest.Net;

static void Main(string[] args)
{
  //JRequest JSON string
  string json = @"{
          'protocol': 'https',
          'name': 'Rest Test',
          'requests': [
            {
                'key': 'post_test',
                'url': 'https://httpbin.org/post',
                'method' : 'POST',
                'body': '{\'JRequest.Net\':\'Rocks!\'}'
            }
          ]
      }";

  var jRequest = new JRequestService(json).Run(); //returns a Jrequest object

  Console.WriteLine($"Name: {jRequest.Name}");
  foreach (var request in jRequest.Requests)
  {
      Console.WriteLine($"Request Key: {request.Key}");
      Console.WriteLine($"Status: {request.Response.Status}");
      Console.WriteLine("Response:");
      Console.WriteLine(request.Response.Content);
  }

  Console.Read();
}

```
#### output

![post_test](https://user-images.githubusercontent.com/39979029/41504777-c925a5d4-71c7-11e8-8310-6c7198dd0c7d.png)

---
#### Examples 3
**Converting** response content (JSON into XML).

```
using System;
using JRequest.Net;

static void Main(string[] args)
{
  //JRequest JSON string
  string json = @"{
            'protocol': 'https',
            'name': 'JsonPlaceholder',
            'requests': [
              {
                  'key': 'convert_xml',
                  'url': 'https://jsonplaceholder.typicode.com/posts/1',
                  'configuration': {
                    'output': {
                        'type': 'xml'
                        }
                    },
              }
            ]
        }";

  var jRequest = new JRequestService(json).Run(); //returns a Jrequest object

  Console.WriteLine($"Name: {jRequest.Name}");
  foreach (var request in jRequest.Requests)
  {
      Console.WriteLine($"Request Key: {request.Key}");
      Console.WriteLine($"Status: {request.Response.Status}");
      Console.WriteLine("Response:");
      Console.WriteLine(request.Response.Content);
  }

  Console.Read();
}

```
#### output

![jsontoxml](https://user-images.githubusercontent.com/39979029/41504873-807d989e-71c9-11e8-9ebb-5709bb1c86b5.png)

---
---
#### Examples 4
Sending **FTP** request to [test.rebex.net](http://test.rebex.net/) server.

```
using System;
using JRequest.Net;

static void Main(string[] args)
{
  //JRequest JSON string
 var json = @"
        {
          'protocol': 'ftp',
          'name': 'Rebex FTP Test',
          'requests': [
            {
              'key': 'readmeTxt',
              'url': 'ftp://test.rebex.net',
              'authorization': {
                'username': 'demo',
                'password': 'password'
              },
              'filepath': '/',
              'filetype': 'txt'
            }
          ]
        }";

  var jRequest = new JRequestService(json).Run(); //returns a Jrequest object

  Console.WriteLine($"Name: {jRequest.Name}");
  foreach (var request in jRequest.Requests)
  {
      Console.WriteLine($"Request Key: {request.Key}");
      Console.WriteLine($"Status: {request.Response.Status}");
      Console.WriteLine("Response:");
      Console.WriteLine(request.Response.Content);
  }

  Console.Read();
}

```
#### output

![ftptest](https://user-images.githubusercontent.com/39979029/41505478-47048410-71d8-11e8-9579-e27213aad2cc.png)
---
### Request Dependency
Request dependency is when a request depends on another request to send a complete or valid request to a web API. Suppose we need to call [OpenWeatherMap](https://openweathermap.org/) web API from our code to find the current location's weather forecast. Based on the web API's specification, the URL that we need to send the request looks like this `api.openweathermap.org/data/2.5/weather?q={city name},{country code}`, where `{city name}` is the name of the current location's city and `{country code}` is a two letter country code. In order to find the inforamtion about the current location and replace the varibales `{city name}` and `{country code}`, we use another web API [IP Geolocation API](http://ip-api.com/docs/) and find the values we need from the response data. In above scenario, we can say our weather forecast request depends on the request that returns the current location.
#### Example 5
In this example we are sending two different request to two different API endpoints. The first API request returns the current weather forecast for the specified city whereas the second request returns the current location.  
**Note** The order of execution (the first request to be called) is determined by the `ordinal` property. Therefore, request \"CurrentLocation\" will be the first one to be called.
```
using System;
using System.IO;
using JRequest.Net;

static void Main(string[] args)
{
  //JRequest JSON string
  var json = @"
              { 
                'protocol': 'http', 
                'name': 'WeatherForecast', 
                'requests': [
                    {
                        'key': 'OpenWeatherMapAPI',
                        'url': 'http://api.openweathermap.org/data/2.5/weather',
                        'parameters': [
                            { 'q': '{{currentlocation.body.city}},{{currentlocation.body.countryCode}}' },
                            { 'appid': 'de92ba40803f76968ec6c2d7668b377a' }
                        ],
                        'ordinal': 2
                    },
                    {
                        'key': 'CurrentLocation',
                        'url': 'http://ip-api.com/json',
                        'ordinal': 1
                    }
                ] 
              }";

  var jRequest = new JRequestService(json).Run(); //returns a Jrequest object

  Console.WriteLine($"JRequest Name: {jRequest.Name}\n");
  foreach (var request in jRequest.Requests)
  {
      Console.WriteLine($"Request Key: {request.Key}");
      Console.WriteLine($"Status: {request.Response.Status}");
      Console.WriteLine("Response:");
      Console.WriteLine($"{request.Response.Content}\n\n");
  }

  Console.Read();
}

```
#### output

![weatherforecast](https://user-images.githubusercontent.com/39979029/41505123-a0d6aff8-71cf-11e8-8275-ef5962fc7468.png)

---

### JRequest Variable Interpolation
#### Syntax `{ReqestKey.Field.ValuePath}`
`RequestKey`: Is the key of the request where the engine look for the value.   
`Field`: Field can be the response body, headers or cookies.  
`ValuePath`: Tells the engine where to find the value from the specified key.  
Let's take one of the variable interpolation from the above example `{currentlocation.body.city}`. The first part tells the engine to start searching in the \"currentlocation\" request's response. The second part tells the engine  

---
### Cookies
