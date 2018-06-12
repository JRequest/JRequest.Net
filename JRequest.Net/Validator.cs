using JRequest.Net.Enumerators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JRequest.Net
{
    internal class Validator
    {

        internal static JRequestContext ValidateJson(string json)
        {
            try
            {
                JToken.Parse(json);//check if json string can be parsed
                JRequestContext jRequest = JsonConvert.DeserializeObject<JRequestContext>(json);//check if json string can be deserialized into a dynamic object
                return jRequest;
            }
            catch (JsonReaderException ex)
            {
                throw new JRequestException(ex.Message, ex.InnerException);
            }
            catch (Exception ex) //some other exception
            {
                throw new JRequestException(ex.Message, ex.InnerException);
            }
        }

        internal static bool ValidateJRequest(JRequestContext jRequest)
        {
            try
            {
                if (!Utility.HasValue(jRequest))
                    throw new JRequestException("JRequest object is not initialized.");

                if (!Utility.HasValue(jRequest.Requests) || jRequest.Requests.Count == 0)
                    throw new JRequestException("JRequest contains no request.");

                if (jRequest.Requests.Where(r => !Utility.HasValue(r.Key)).Count() > 0)
                    throw new JRequestException("Invalid request key/keys have been encountered.");

                if (!(jRequest.Requests.Select(r => r.Key).Distinct().Count() == jRequest.Requests.Select(r => r.Key).Count()))
                    throw new JRequestException("Some of the requests contain duplicate key.");

                if(!Utility.StringEquals(jRequest.Protocol,Protocol.http) && !Utility.StringEquals(jRequest.Protocol, Protocol.https) && !Utility.StringEquals(jRequest.Protocol, Protocol.ftp))
                    throw new JRequestException("Unsupported protocol.");

                if (Utility.StringEquals(jRequest.Protocol, Protocol.http) || Utility.StringEquals(jRequest.Protocol, Protocol.https))
                {
                    jRequest.Requests.ForEach(request =>
                    {
                        ValidateHttpRequest(request);
                    });
                }
                else
                {
                    jRequest.Requests.ForEach(request =>
                    {
                        ValidateFtpRequest(request);
                    });
                }

                return true;
            }
            catch (Exception ex)
            {

                throw new JRequestException(ex.Message, ex.InnerException);
            }
        }

        private static bool ValidateHttpRequest(Request request)
        {
            try
            {
                if (!Utility.HasValue(request.URL))
                    throw new JRequestException("Request URL is required.");

                if (!Utility.HasValue(request.Key))
                    throw new JRequestException("Request Key is required.");

                if ((Utility.StringEquals(request.Method, HttpMethod.POST)) && (!Utility.HasValue(request.Body)))
                    throw new JRequestException("A POST request requires data in the body.");

                if (request.Parameters != null)
                {
                    request.Parameters.ForEach(param =>
                    {
                        foreach (KeyValuePair<string, string> item in param.ToList())
                        {
                            if (!Utility.HasValue(item.Key))
                                throw new JRequestException($"Request {request.Key} contains invalid parameter key.");

                            if (Utility.HasValue(item.Key) && !Utility.HasValue(item.Value))
                                throw new JRequestException($"Request {request.Key} contains invalid parameter value at key {item.Key}.");
                        }
                    });
                }

                if (request.Headers != null)
                {
                    request.Headers.ForEach(header =>
                    {
                        foreach (KeyValuePair<string, string> item in header.ToList())
                        {
                            if (!Utility.HasValue(item.Key))
                                throw new JRequestException($"Request {request.Key} contains invalid header key.");

                            if (Utility.HasValue(item.Key) && !Utility.HasValue(item.Value))
                                throw new JRequestException($"Request {request.Key} contains invalid header value at key {item.Key}.");
                        }
                    });
                }

                if (Utility.HasValue(request?.Configuration?.Output))
                    ValidateOutput(request.Configuration.Output);

                return true;
            }
            catch (Exception ex)
            {

                throw new JRequestException(ex.Message, ex.InnerException);
            }
        }

        private static bool ValidateFtpRequest(Request request)
        {
            try
            {
                if (!Utility.HasValue(request.FilePath))
                    throw new JRequestException("File Path is required for FTP connection.");

                if (!Utility.HasValue(request.FileType))
                    throw new JRequestException("File Type is required for FTP connection.");

                return true;
            }
            catch (Exception ex)
            {

                throw new JRequestException(ex.Message, ex.InnerException);
            }
        }

        private static bool ValidateOutput(Output output)
        {
            try
            {
                if(!Utility.HasValue(output?.Type))
                    throw new JRequestException("Output 'Type' property is not defined.");
            }
            catch (Exception ex)
            {

                throw new JRequestException(ex.Message, ex.InnerException);
            }
            return true;
        }
    }
}
