using System.Collections.Generic;

namespace JRequest.Net
{
    public class JRequestService
    {

        public string Json { get; set; }
        public JRequestService()
        {

        }

        public JRequestService(string json)
        {
            Json = json;
        }
        public string Protocol { get; set; } = Enumerators.Protocol.http.ToString();
        public string Name { get; set; } = "JRequest";
        public List<Request> Requests { get; set; }
        public Configuration Configuration { get; set; }

        public JRequestService Run()
        {
            Build(Json);
            return JRequestEngine.Run();
        }
        public JRequestService Run(string json)
        {
            Build(json);
            return JRequestEngine.Run();
        }
        protected JRequestService Build(string json)
        {
            return JRequestEngine.Build(json);
        }
    }
}
