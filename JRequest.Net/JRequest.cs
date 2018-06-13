using System.Collections.Generic;

namespace JRequest.Net
{
    public class Jrequest
    {
        public string Protocol { get; set; } = Enumerators.Protocol.http.ToString();
        public string Name { get; set; } = "JRequest";
        public List<Request> Requests { get; set; }
        public Configuration Configuration { get; set; }
    }
}
