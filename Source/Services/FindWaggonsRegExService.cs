using System.Text.RegularExpressions;

namespace RollingLineSavegameFix.Services
{
    public class FindWaggonsRegExService : IRegExService
    {
        private const string magicRegex = @"(#wagons#\d*,)((?:\s|.)*?)(#objects#|#track#|#points#)";
        private const string magicRegexWithWaggonsAtTheEnd = @"(#wagons#\d*,)((?:\s|.)*)";

        public RegexServiceResponseModel MatchRegex(string content)
        {
            var result = new RegexServiceResponseModel();            

            var match = Regex.Match(content, magicRegex);
            if (match.Success)
            {
                result.Prefix = match.Groups[0].Value;
                result.Content = match.Groups[1].Value;
                result.Suffix = match.Groups[2].Value;
                result.RegExMatched = true;
                result.MatchedRegEx = magicRegex;
            }
            else
            {
                match = Regex.Match(content, magicRegexWithWaggonsAtTheEnd);
                if (match.Success)
                {
                    result.Prefix = match.Groups[0].Value;                    
                    result.RegExMatched = true;
                    result.MatchedRegEx = magicRegexWithWaggonsAtTheEnd;
                }
            }

            return result;
        }

        public string Replace(string content, string replacement, string regex) => Regex.Replace(content, regex, replacement);        
    }
}
