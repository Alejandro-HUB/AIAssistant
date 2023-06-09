using System.Globalization;
using System.Text.RegularExpressions;

namespace AIAssistant.Helpers
{
    public static class StringHelper
    {
        public static string CleanUpResponseString(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                string unescapedString = Regex.Replace(input, @"\\u(?<Value>[a-zA-Z0-9]{4})", m =>
                {
                    return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
                });

                string cleanedString = unescapedString.Trim('"').Replace("\\\"", "\"");

                return cleanedString;
            }
            return string.Empty;
        }
    }
}
