# JRequest.Net
JRequest.NET is a powerful library which allows applications to call web APIs using JSON.
## Getting Started
You can download JRequest.Net from GitHub or install it directly into your project from NuGet package manager.

```PM> Install-Package JRequest.NET -Version 1.0.2```

You can call web APIs by feeding a JRequest JSON string to JRequestEngine.Run(string json) method.
### Sample1
```
//format the json string accordingly
string json = "{
  "protocol": "https",
  "name": "Dummy Request",
  "requests": [
    {
      "requesttype": "output",
      "key": "placeholder",
      "url": "https://jsonplaceholder.typicode.com/posts/1",
      "method": "GET",
      "contenttype": "application/json",
      "parameters":[
      ],
      "headers": [
      ],
      "body":"",
      "authorization": {
      },
      "configuration": {
        "output": {
          "type": "json"
        }
      },
      "ordinal": "1"
    }
  ]
}"

var response = JRequestEngine.Run(json);
```
### Description
| Property | Type | Mandatory |	Default Value |	Allowed Values | Description |
| -------- | ---- | --------- | ------------- | ----------------- | -------- |
| protocol |	string |	true |	HTTP |	HTTP,HTTPS,FTP | The type of protocol that is used in the internet.
| name | string |	false | JRequest | any string | The name of the root JRequest object.
| requests | Array of objects |	True |	Null | Can be any number of HTTP(S) or FTP request objects which defines its own set of properties.
| requesttype | string | false | output | input,output | **input:** The response data will be saved in the global storage and the values can be used by other requests. **output:** The response data will not be saved in the global storage and used by other requests.
| key | string | true | null | any string | Used to uniquely identify a request. Duplicate keys are not allowed.
| url | string | true | null | valid URL | A reference to a web resource. Parameter values can be included in the URL, however it is recommended to use the parameters property to add parameter values.
| method | string | false | GET | GET,POST | Methods used to send the request to a server.
| contenttype | string | false | application/json | Indicates the media type of the resource. Content type can also be specified inside the header.
| parameters |

### Using values from other request response
