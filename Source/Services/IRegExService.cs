namespace RollingLineSavegameFix.Services
{
    public interface IRegExService
    {
        RegexServiceResponseModel MatchRegex(string content);

        string Replace(string content, string replacement, string regex);
    }
}
