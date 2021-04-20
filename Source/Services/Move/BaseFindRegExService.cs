using RollingLineSavegameFix.Model;
using System.Text.RegularExpressions;

namespace RollingLineSavegameFix.Services
{
    public abstract class BaseFindRegExService : IRegExService
    {
        protected abstract string AlternateMagicRegex();

        protected abstract string MagicRegex();

        public IRegexServiceResponseModel MatchRegex(string content)
        {
            var result = new RegexServiceResponseModel();

            var match = Regex.Match(content, MagicRegex());
            if (match.Success)
            {
                result.Prefix = match.Groups[1].Value;
                result.Content = match.Groups[2].Value;
                result.Suffix = match.Groups[3].Value;
                result.HasMatched = true;
                result.MatchingRegEx = MagicRegex();
            }
            else
            {
                match = Regex.Match(content, AlternateMagicRegex());
                if (match.Success)
                {
                    result.Prefix = match.Groups[0].Value;
                    result.HasMatched = true;
                    result.MatchingRegEx = AlternateMagicRegex();
                }
            }

            return result;
        }

        public string Replace(string content, string replacement, string regex) => Regex.Replace(content, regex, replacement);
    }
}
