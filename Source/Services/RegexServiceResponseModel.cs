namespace RollingLineSavegameFix.Services
{
    public class RegexServiceResponseModel : IRegexServiceResponseModel
    {
        public bool RegExMatched { get; set; }
        public string MatchedRegEx { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string Content { get; set; }
    }
}
