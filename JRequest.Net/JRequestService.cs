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
        
        public JRequest Run()
        {
            Build(Json);
            return JRequestEngine.Run();
        }
        public JRequest Run(string json)
        {
            Build(json);
            return JRequestEngine.Run();
        }
        protected JRequest Build(string json)
        {
            return JRequestEngine.Build(json);
        }
    }
}
