namespace RollingLineSavegameFix.Services
{
    public interface IRegexServiceResponseModel
    {
        bool RegExMatched { get; set; }
        string MatchedRegEx { get; set; }

        string Prefix { get; set; }
        string Suffix { get; set; }
        string Content { get; set; }
    }
}
