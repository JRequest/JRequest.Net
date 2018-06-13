using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace JRequest.Net
{
    internal class Storage
    {
        private static List<Dictionary<string, Response>> globalStorage = new List<Dictionary<string, Response>>();

        public static List<Dictionary<string, Response>> GlobalStorage { get => globalStorage; set => globalStorage = value; }

        internal static void Store(Dictionary<string, Response> response)
        {
            GlobalStorage.Add(response);
        }
        
        internal static List<string> Search(string dataPath)
        {
            try
            {
                List<string> searchResults = new List<string>();
                if (Utility.HasValue(dataPath))
                {
                    List<string> path = dataPath.Split(Convert.ToChar(".")).ToList();

                    if (path.Count > 2)
                    {
                        string requestKey = path[0];
                        string source = path[1];
                        path.RemoveRange(0, 2);

                        Response response = GetRequestResponse(requestKey);

                        if (!Utility.HasValue(response))
                            throw new JRequestException($"The specified request has no response data.");

                        if (!Utility.IsValidSource(source))
                            throw new JRequestException($"Invalid source specified.");

                        switch (source.ToLower())
                        {
                            case "headers":
                                FindHeaderValue(response, path[0], searchResults);
                                break;
                            case "cookies":
                                FindCookieValue(response, path[0], searchResults);
                                break;
                            case "body":
                                dynamic jsonObject = null;
                                if (response.ContentType.ToLower() == "application/xml")
                                {
                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(response.Content);

                                    jsonObject = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeXmlNode(doc));
                                }
                                else if (response.ContentType.Split(Convert.ToChar(";"))[0].ToLower() == "application/json")
                                {
                                    jsonObject = JsonConvert.DeserializeObject<dynamic>(response.Content);
                                }

                                SearchJson(path, jsonObject, searchResults);
                                break;
                            default:
                                break;
                        }
                    }

                    if (searchResults.Count == 0)
                        throw new JRequestException($"{dataPath} yields no result.");

                }

                return searchResults;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private static Response GetRequestResponse(string requestKey)
        {
            Response response = null;
            try
            {
                GlobalStorage.ForEach(items =>
                {
                    foreach (KeyValuePair<string, Response> item in items)
                    {
                        if (item.Key.ToLower() == requestKey.ToLower())
                        {
                            response = item.Value;
                        }
                    }
                });

                if (!Utility.HasValue(response))
                    throw new JRequestException($"Match not found.");
            }
            catch (Exception ex)
            {

                throw new JRequestException(ex.Message, ex.InnerException);
            }

            return response;
        }

        private static void FindHeaderValue(Response response, string headerKey, List<string> searchResults)
        {
            try
            {
                foreach (KeyValuePair<string, string> header in response.Headers)
                {
                    if (header.Key == headerKey)
                    {
                        searchResults.Add(header.Value);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new JRequestException(ex.Message, ex.InnerException);
            }
        }

        private static void FindCookieValue(Response response, string cookieKey, List<string> searchResults)
        {
            try
            {
                foreach (KeyValuePair<string, string> cookie in response.Cookies)
                {
                    if (cookie.Key.Contains(cookieKey))
                    {
                        searchResults.Add(cookie.Value);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {

                throw new JRequestException(ex.Message, ex.InnerException);
            }
        }

        private static void SearchJson(List<string> key, dynamic jsonObject, List<string> searchResults, int index = 0)
        {
            try
            {
                foreach (dynamic entry in jsonObject)
                {
                    if (entry.Type == null)
                    {
                        SearchJson(key, entry, searchResults, index);
                    }
                    else if (entry.Type.ToString() == "Property" && entry.Value.Type == null)
                    {
                        if (entry.Name.ToLower() == key[index].ToLower())
                            SearchJson(key, entry, searchResults, ++index);
                        else
                            SearchJson(key, entry, searchResults);
                    }
                    else if (entry.Type.ToString() == "Object" || entry.Type.ToString() == "Array")
                    {
                        foreach (var item in entry)
                        {
                            if (item.Type != null && item.Type.ToString() != "Array")
                            {
                                if (item.Value.ToString().ToLower() == key[index].ToLower())
                                    searchResults.Add(item.Value.ToString());
                            }
                            else if (item.Type == null)
                                SearchJson(key, item, searchResults, index);
                        }
                    }
                    else if (entry.Type.ToString() == "String")
                    {
                        if (entry.ToString().ToLower() == key[index])
                            searchResults.Add(entry.Value.ToString());
                    }
                    else if (entry.Value.Type.ToString() == "Array")
                    {
                        if (entry.Type.ToString() == "Property" && entry.Name.ToLower() == key[index].ToLower() && entry.Value.Count > 0)
                        {
                            if (index == key.Count - 1)
                                searchResults = JsonConvert.DeserializeObject<List<string>>(entry.Value.ToString());
                            else
                                SearchJson(key, entry, searchResults, ++index);
                        }
                        else
                            SearchJson(key, entry, searchResults, index);
                    }
                    else if (entry.Value != null && entry.Value.Type.ToString() != "Property" && entry.Value.Type.ToString() != "Array")
                    {
                        if (entry.Name.ToLower() == key[index].ToLower())
                            searchResults.Add(entry.Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
