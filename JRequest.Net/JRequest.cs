using System.Collections.Generic;

namespace JRequest.Net
{
    public class Jrequest
    {
        public string Name { get; set; } = "JRequest";
        public List<Request> Requests { get; set; }
        public Configuration Config { get; set; }
    }
}
