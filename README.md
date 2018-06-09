# JRequest.Net
JRequest.NET is a powerful library which allows applications to call web APIs using JSON.
## Getting Started
Download JRequest.Net from NuGet package manager in to your .NET project.

```PM> Install-Package JRequest.NET -Version 1.0.2```

You can call web APIs by feeding a JRequest JSON string to JRequestEngine.Run(string json) method.
### Sample JRequest call to https://jsonplaceholder.typicode.com/posts/1
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
