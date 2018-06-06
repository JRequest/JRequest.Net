using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace JRequest.Net
{
    public class Output
    {
        public string Type { get; set; }

        internal static void ToJson(Dictionary<string, JResponse> jResponseDictionary)
        {
            foreach (KeyValuePair<string, JResponse> response in jResponseDictionary)
            {
                using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(response.Value.Content)))
                {
                    response.Value.Content = JsonConvert.SerializeObject(response.Value.Content);
                }
            }
        }

        internal static void ToXml(Dictionary<string, JResponse> jResponseDictionary)
        {
            foreach (KeyValuePair<string, JResponse> response in jResponseDictionary)
            {
                using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(response.Value.Content)))
                {
                    var quotas = new XmlDictionaryReaderQuotas();
                    response.Value.Content = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(stream, quotas)).ToString();
                }
            }
        }
    }
}
