using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JRequest.Net
{
    internal class Utility
    {
        internal static bool HasValue(object item)
        {
            return (item != null && !string.IsNullOrWhiteSpace(item.ToString()));
        }

        internal static List<String> GetTokens(String str)
        {
            Regex regex = new Regex(@"(?<=\{\{)[^}]*(?=\}\})", RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(str);

            return matches.Cast<Match>().Select(m => m.Value).Distinct().ToList();
        }

        internal static string Stringfy(List<string> list, Configuration config)
        {
            string delimeter = (Utility.HasValue(config?.Input?.Delimeter)) ? config.Input?.Delimeter : ",";
            return string.Join($"{delimeter}", list);
        }

        internal static bool IsValidSource(string source)
        {
            string[] sources = { "headers", "cookies", "body" };
            return Array.IndexOf(sources, source.ToLower()) != -1;
        }

        internal static bool StringEquals(object obj1, object obj2)
        {
            return (obj1.ToString().ToLower() == obj2.ToString().ToLower());
        }
    }
}
