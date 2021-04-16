namespace RollingLineSavegameFix.Model
{
    public interface IRegexServiceResponseModel
    {
        bool HasMatched { get; set; }
        string MatchingRegEx { get; set; }
        string Prefix { get; set; }
        string Suffix { get; set; }
        string Content { get; set; }
    }
}
