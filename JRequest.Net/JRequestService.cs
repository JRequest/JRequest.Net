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
        
        public Jrequest Run()
        {
            Build(Json);
            return JRequestEngine.Run();
        }
        public Jrequest Run(string json)
        {
            Build(json);
            return JRequestEngine.Run();
        }
        protected Jrequest Build(string json)
        {
            return JRequestEngine.Build(json);
        }
    }
}
