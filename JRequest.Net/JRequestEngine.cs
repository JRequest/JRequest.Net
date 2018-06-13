using JRequest.Net.Enumerators;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace JRequest.Net
{
    internal static class JRequestEngine
    {
        internal static JRequest jRequest = null;

        internal static JRequest Build(string json)
        {
            try
            {
                if (!Utility.HasValue(json))
                    throw new JRequestException("The JSON string is empty.");

                jRequest = Validator.ValidateJson(json);
                Validator.ValidateJRequest(jRequest);
            }
            catch (Exception ex)
            {
                throw new JRequestException(ex.Message, ex.InnerException);
            }
            return jRequest;
        }

        internal static JRequest Run()
        {
            try
            {
                if (jRequest == null)
                    throw new JRequestException("JRequest object is not initialized.");

                Response response = null;

                jRequest.Requests
                        .OrderBy(o => o.Ordinal)
                        .ToList()
                        .ForEach(request =>
                        {
                            if (Utility.HasValue(request.Authorization))
                            {
                                Authorization.AddAuthorizationHeader(request);
                            }

                            ParseRequest(request);

                            if (Utility.StringEquals(jRequest.Protocol, Protocol.http) || Utility.StringEquals(jRequest.Protocol, Protocol.https))
                                response = SendHttpRequest(request);

                            if (Utility.StringEquals(jRequest.Protocol, Protocol.ftp))
                                response = SendFtpRequest(request);

                            if (Utility.HasValue(response))
                            {

                                request.Response = response;//add the response into the request

                                Storage.Store(new Dictionary<string, Response>
                                {
                                    { request.Key, response }
                                });

                                if (Utility.HasValue(request?.Configuration?.Output))
                                {
                                    if (Utility.StringEquals(request.Configuration.Output.Type, OutputType.json))
                                    {
                                        Output.ToJson(request.Response);
                                    }
                                    else if (Utility.StringEquals(request.Configuration.Output.Type, OutputType.xml))
                                    {
                                        Output.ToXml(request.Response);
                                    }
                                }
                            }
                        });
            }
            catch (Exception ex)
            {

                throw new JRequestException(ex.Message, ex.InnerException);
            }

            return jRequest;
        }

        private static Request ParseRequest(Request request)
        {
            try
            {
                var urlTokens = Utility.GetTokens(request.URL);
                List<string> searchResults = null;
                if (urlTokens.Count > 0)
                {
                    urlTokens.ForEach(token =>
                    {
                        searchResults = Storage.Search(token);
                        string value = Utility.Stringfy(searchResults, request.Configuration);
                        request.URL = request.URL.Replace($"{{{{{token}}}}}", value);
                    });
                }

                request.Parameters?.ForEach(parameter =>
                {
                    foreach (KeyValuePair<string, string> param in parameter.ToList())
                    {
                        var token = Utility.GetTokens(param.Value).FirstOrDefault();
                        if (Utility.HasValue(token))
                        {
                            searchResults = Storage.Search(token);
                            string value = Utility.Stringfy(searchResults, request.Configuration);
                            parameter[param.Key] = value;
                        }
                    }
                });

                request.Headers?.ForEach(headers =>
                {
                    foreach (KeyValuePair<string, string> header in headers.ToList())
                    {
                        var token = Utility.GetTokens(header.Value).FirstOrDefault();
                        if (Utility.HasValue(token))
                        {
                            searchResults = Storage.Search(token);
                            string value = Utility.Stringfy(searchResults, request.Configuration);
                            headers[header.Key] = value;
                        }
                    }
                });

                if (Utility.HasValue(request.Body))
                {
                    var bodyTokens = Utility.GetTokens(request.Body);

                    if (bodyTokens.Count > 0)
                    {
                        bodyTokens.ForEach(token =>
                        {
                            searchResults = Storage.Search(token);
                            string value = Utility.Stringfy(searchResults, request.Configuration);
                            request.Body = request.Body.Replace($"{{{{{token}}}}}", value);
                        });
                    }
                }

                return request;
            }
            catch (Exception ex)
            {
                if (ex is JRequestException)
                {
                    throw new JRequestException(ex.Message, ex.InnerException);
                }
                throw ex;
            }
        }

        private static Response SendHttpRequest(Request request)
        {
            Response response = null;

            try
            {
                string uriParmeters = "";
                Encoding encode = Encoding.GetEncoding("utf-8");
                ASCIIEncoding encoding = new ASCIIEncoding();
                HttpWebRequest httpWebRequest;

                var uriBuilder = new UriBuilder(request.URL);

                if (Utility.HasValue(request.Client))
                {
                    uriBuilder.Host = $"{request.Client}.{uriBuilder.Host}";
                }

                if (request.Parameters != null)
                {
                    request.Parameters.ForEach(parameter =>
                    {
                        foreach (KeyValuePair<string, string> item in parameter)
                        {
                            uriParmeters += $"{item.Key}={item.Value}&";
                        }
                    });
                }

                if (!string.IsNullOrWhiteSpace(uriParmeters))
                {
                    uriParmeters = uriParmeters.Remove(uriParmeters.Length - 1);//remove the last '&'

                    httpWebRequest = (HttpWebRequest)WebRequest.Create($"{uriBuilder.Uri.ToString()}?{uriParmeters}");//create uri with parameters
                }
                else
                {
                    httpWebRequest = (HttpWebRequest)WebRequest.Create($"{uriBuilder.Uri.ToString()}");//create uri without parameters
                }

                httpWebRequest.ContentType = request.ContentType;

                httpWebRequest.AllowAutoRedirect = false;
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                httpWebRequest.Method = request.Method;//add the method

                if (request.Headers != null)
                {
                    request.Headers.ForEach(header =>
                    {
                        foreach (KeyValuePair<string, string> item in header.ToList())
                        {
                            if (item.Key.Trim().ToLower() == "content-type")
                                httpWebRequest.ContentType = item.Value.Trim();

                            else
                                httpWebRequest.Headers.Add(item.Key.Trim(), item.Value.Trim());

                        }
                    });
                }

                if (request.Method == "POST")
                {
                    byte[] postDataBytes = encoding.GetBytes(request.Body);
                    httpWebRequest.ContentLength = postDataBytes.Length;
                    using (var stream = httpWebRequest.GetRequestStream())
                    {
                        stream.Write(postDataBytes, 0, postDataBytes.Length);
                        stream.Close();
                    }
                }

                using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Stream responseStream = httpWebResponse.GetResponseStream();
                    StreamReader streamReader = new StreamReader(responseStream, encode, true);

                    response = new Response
                    {
                        Status = httpWebResponse.StatusDescription,
                        ContentLength = httpWebResponse.ContentLength,
                        ContentType = httpWebResponse.ContentType,
                        Cookies = ParseCookies(httpWebResponse.Headers),
                        Headers = ParseHeaders(httpWebResponse.Headers),
                        RequestKey = request.Key
                    };

                    using (streamReader)
                    {
                        string strResponse;
                        strResponse = streamReader.ReadToEnd();
                        response.Content = HttpUtility.HtmlDecode(strResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        private static Response SendFtpRequest(Request request)
        {
            Response response = null;

            try
            {
                var ftpFiles = ListDirectories(request);
                foreach (var file in ftpFiles)
                {
                    try
                    {
                        var ftpPath = $"{request.FilePath}{file}";
                        FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create($"{request.URL}{ftpPath}");
                        ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                        ftpWebRequest.Credentials = new NetworkCredential(request.Authorization.Username, request.Authorization.Password);

                        using (var ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse())
                        {
                            Stream responseStream = ftpWebResponse.GetResponseStream();
                            StreamReader streamReader = new StreamReader(responseStream);

                            response = new Response
                            {
                                Status = ftpWebResponse.StatusDescription,
                                ContentLength = ftpWebResponse.ContentLength,
                                Cookies = ParseCookies(ftpWebResponse.Headers),
                                Headers = ParseHeaders(ftpWebResponse.Headers),
                                RequestKey = request.Key
                            };

                            using (streamReader)
                            {
                                var strResponse = streamReader.ReadToEnd();
                                response.Content = HttpUtility.HtmlDecode(strResponse);
                            }

                            streamReader.Close();
                            ftpWebResponse.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }

        private static Request GetRequest(JRequest jRequest, string requestKey)
        {
            Request request = null;

            if (Utility.HasValue(requestKey))
            {
                request = jRequest.Requests.Where(r => r.Key == requestKey).FirstOrDefault();
            }

            return request;
        }

        private static Dictionary<string, string> ParseHeaders(WebHeaderCollection headers)
        {
            Dictionary<string, string> headerDic = new Dictionary<string, string>();
            foreach (string key in headers.AllKeys)
            {
                headerDic.Add(key, headers[key]);
            }

            return headerDic;
        }

        private static Dictionary<string, string> ParseCookies(NameValueCollection headers)
        {
            Dictionary<string, string> cookieDic = new Dictionary<string, string>();
            foreach (string key in headers.AllKeys)
            {
                if (key == "Set-Cookie")
                {
                    string name = headers[key].ToString().Split(Convert.ToChar(";"))[0];
                    cookieDic.Add(name, headers[key]);
                }
            }

            return cookieDic;
        }

        private static IList<string> ListDirectories(Request request)
        {
            try
            {
                var ftpRequest = (FtpWebRequest)WebRequest.Create($"{request.URL}{request.FilePath}*.{request.FileType}");
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpRequest.Credentials = new NetworkCredential(request.Authorization.Username, request.Authorization.Password);
                ftpRequest.Proxy = null;

                var directories = new List<string>();

                using (var response = (FtpWebResponse)ftpRequest.GetResponse())
                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        directories.Add(line);
                    }
                }

                return directories;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
