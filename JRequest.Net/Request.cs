﻿using JRequest.Net.Enumerators;
using System.Collections.Generic;

namespace JRequest.Net
{
    public class Request
    {
        internal string Protocol { get; set; } = "http";
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
        public Configuration Config { get; set; }
        public Authorization Authorization { get; set; }
        public Response Response { get; set; }
    }
}
