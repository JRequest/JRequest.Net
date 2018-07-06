using System.Collections.Generic;

namespace JRequest.Net
{
    public class JRequestService
    {

        public string Json { get; set; }
        public Jrequest Jrequest { get; set; } = null;
        public JRequestService()
        {

        }

        public JRequestService(string json)
        {
            Json = json;
        }

        public JRequestService(Jrequest jrequest)
        {
            Jrequest = jrequest;
        }

        public Jrequest Run()
        {
            if (Jrequest == null)
            {
                Build(Json);
            }
            else
            {
                Build(Jrequest);
            }
            return JRequestEngine.Run();
        }
        public Jrequest Run(string json)
        {
            Build(json);
            return JRequestEngine.Run();
        }
        public Jrequest Run(Jrequest jRequest)
        {
            Build(jRequest);
            return JRequestEngine.Run();
        }
        protected Jrequest Build(string json)
        {
            return JRequestEngine.Build(json);
        }
        protected Jrequest Build(Jrequest jRequest)
        {
            return JRequestEngine.Build(jRequest);
        }
    }
}
