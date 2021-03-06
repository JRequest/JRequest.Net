﻿using System.Collections.Generic;

namespace JRequest.Net
{
    public class Response
    {
        public string RequestKey { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Cookies { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public static List<Dictionary<string, object>> RequestResources;
    }
}
