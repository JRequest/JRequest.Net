using Newtonsoft.Json;
using System.Collections.Generic;

namespace JRequest.Net
{
    public static class JRequestService
    {
        public static object Run(string json, bool serialize = false)
        {
            Build(json);
            var result = JRequestEngine.Run();
            if (serialize)
            {
                return JsonConvert.SerializeObject(result);
            }
            return result;
        }
        public static object Run(Jrequest jRequest, bool serialize = false)
        {
            Build(jRequest);
            var result = JRequestEngine.Run();
            if (serialize)
            {
                return JsonConvert.SerializeObject(result);
            }
            return result;
        }
        private static Jrequest Build(string json)
        {
            return JRequestEngine.Build(json);
        }
        private static Jrequest Build(Jrequest jRequest)
        {
            return JRequestEngine.Build(jRequest);
        }
    }
}
