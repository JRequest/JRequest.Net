using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace JRequest.Net
{
    internal class Utility
    {
        public static bool HasValue(object item)
        {
            return (item != null && !string.IsNullOrWhiteSpace(item.ToString()));
        }

        public static List<String> GetTokens(String str)
        {
            Regex regex = new Regex(@"(?<=\{\{)[^}]*(?=\}\})", RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(str);

            return matches.Cast<Match>().Select(m => m.Value).Distinct().ToList();
        }

        public static string Stringfy(List<string> list, Configuration config)
        {
            string delimeter = (Utility.HasValue(config?.Input?.Delimeter)) ? config.Input?.Delimeter : ",";
            return string.Join($"{delimeter}", list);
        }

        public static bool IsValidSource(string source)
        {
            string[] sources = { "headers", "cookies", "body" };
            return Array.IndexOf(sources, source.ToLower()) != -1;
        }

        public static string GetTemplateJson(string outputType)
        {
            try
            {
                string json = string.Empty;
                using (StreamReader r = new StreamReader("template.json"))
                {
                    json = r.ReadToEnd();
                }

                JRequest jRequest = JsonConvert.DeserializeObject<JRequest>(json);

                jRequest.Requests.Where(q => q.Key == "openweathermap")
                    .FirstOrDefault().Configuration.Output.Type = outputType;
                json = JsonConvert.SerializeObject(jRequest);

                return json;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static bool StringEquals(object obj1, object obj2)
        {
            return (obj1.ToString().ToLower() == obj2.ToString().ToLower());
        }
    }
}
