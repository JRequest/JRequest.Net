using Newtonsoft.Json;
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

        internal static void ToJson(Response response)
        {
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(response.Content)))
            {
                response.Content = JsonConvert.DeserializeObject(response.Content).ToString();
            }
        }

        internal static void ToXml(Response response)
        {
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(response.Content)))
            {
                var quotas = new XmlDictionaryReaderQuotas();
                response.Content = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(stream, quotas)).ToString();
            }
        }
    }
}
