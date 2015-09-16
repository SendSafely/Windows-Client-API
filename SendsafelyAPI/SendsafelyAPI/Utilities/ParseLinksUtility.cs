using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SendSafely.Utilities
{
    class ParseLinksUtility
    {
        private String REGEX = "(https:\\/\\/[a-zA-Z\\.]+\\/receive\\/\\?[A-Za-z0-9&=\\-]+packageCode=[A-Za-z0-9\\-_]+#keyCode=[A-Za-z0-9\\-_]+)";

        public List<String> Parse(String text)
        {
            List<String> links = new List<String>();

            Regex rgx = new Regex(REGEX, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(text);
            if (matches.Count > 0)
            {
                Console.WriteLine("{0} ({1} matches):", text, matches.Count);
                foreach (Match match in matches)
                {
                    Console.WriteLine("   " + match.Value);
                    links.Add(match.Value);
                }
                   
            }

            /*
		    Pattern pattern = Pattern.compile(REGEX);
		    Matcher matcher = pattern.matcher(text);
		    while(matcher.find()) {
			    String link = matcher.group(0);
			    if(!links.contains(link)) {
				    links.add(link);
			    }
		    }*/
		    return links;
        }
    }
}
