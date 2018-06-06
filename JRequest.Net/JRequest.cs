using System;
using System.Collections.Generic;
using System.Text;

namespace JRequest.Net
{
    public class JRequest
    {
        public string Protocol { get; set; } = Enumerators.Protocol.http.ToString();
        public string Name { get; set; }
        public List<Request> Requests { get; set; }
        public Configuration Configuration { get; set; }
    }
}
