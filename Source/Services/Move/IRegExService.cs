using RollingLineSavegameFix.Model;

namespace RollingLineSavegameFix.Services
{
    public interface IRegExService
    {
        IRegexServiceResponseModel MatchRegex(string content);

        string Replace(string content, string replacement, string regex);
    }
}
