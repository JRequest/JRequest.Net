using JRequest.Net.Enumerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace JRequest.Net
{
    public class Request
    {
        public string RequestType { get; set; } = Enumerators.RequestType.Output.ToString();
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
        public int Ordinal { get; set; }
        public Configuration Configuration { get; set; }
        public Authorization Authorization { get; set; }
    }
}
