using System;
using System.Collections.Generic;
using System.Text;

namespace JRequest.Net
{
    public class JResponse
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
}
