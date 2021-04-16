namespace RollingLineSavegameFix.Model
{
    public class RegexServiceResponseModel : IRegexServiceResponseModel
    {
        public bool HasMatched { get; set; }
        public string MatchingRegEx { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string Content { get; set; }
    }
}
