# JRequest.Net
JRequest.NET is a powerful library which allows applications to call web APIs using JSON.
## Getting Started
You can call web APIs by feeding a JRequest JSON string to JRequestEngine.Run(string json) method.
### Sample JRequest call to https://jsonplaceholder.typicode.com/posts/1
```
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
