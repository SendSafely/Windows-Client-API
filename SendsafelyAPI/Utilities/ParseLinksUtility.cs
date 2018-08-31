using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SendSafely.Utilities
{
    /// <summary>
    /// Utility for extracting a list of valid SendSafely package links from text input.
    /// </summary>
    public class ParseLinksUtility
    {
        private String REGEX = "(https:\\/\\/[a-zA-Z\\.-]+\\/receive\\/\\?[A-Za-z0-9&=\\-]+packageCode=[A-Za-z0-9\\-_]+#keyCode=[A-Za-z0-9\\-_]+)";

        /// <summary>
        /// Extract a list of valid SendSafely package links from text.
        /// </summary>
        /// <param name="text"> The text input that you want to check for package links.</param>
        public List<String> Parse(String text)
        {
            List<String> links = new List<String>();

            Regex rgx = new Regex(REGEX, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(text);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    links.Add(match.Value);
                }

            }

            return links;
        }
    }
}