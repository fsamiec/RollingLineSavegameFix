using RollingLineSavegameFix.Model;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace RollingLineSavegameFix.Services
{
    public class RemoveWaggonsService : IRemoveWaggonsService
    {
        private readonly IMainModel _model;
        private readonly IReformatService _reformatService;
        private readonly IRegExService _regexService;

        public RemoveWaggonsService(
            IMainModel model,
            IReformatService reformatService,
            IRegExService regexService)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _reformatService = reformatService ?? throw new ArgumentNullException(nameof(reformatService));
            _regexService = regexService ?? throw new ArgumentNullException(nameof(regexService));
        }

        public void RemoveAllWaggons()
        {
            var matchRegExResponse = _regexService.MatchRegex(_model.FileContent);

            if (matchRegExResponse.HasMatched)
            {
                _model.FileContent = _regexService.Replace(
                    _model.FileContent,
                    matchRegExResponse.Prefix + matchRegExResponse.Suffix,
                    matchRegExResponse.MatchingRegEx);
            }       
        }

        public void RemoveFaultyQuickmodWaggons()
        {
            _reformatService.Reformat();
            var matchRegExResponse = _regexService.MatchRegex(_model.FileContent);

            if (matchRegExResponse.HasMatched)
            {
                var resultBuilder = new StringBuilder();                
                var lines = matchRegExResponse.Content.Split(Environment.NewLine);
                for (var i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("Quickmod", StringComparison.OrdinalIgnoreCase))
                    {
                        var values = lines[i].Split(',');

                        if (Regex.IsMatch(values[0], "#|%")) //if the first value contains a # or an % its an faulty name
                            continue;

                        if (i != lines.Length - 1 && (values.Length - 1) % 32 != 0)
                            continue;

                        if (i == lines.Length - 1 && values.Length % 32 != 0)
                            continue;
                    }
                    resultBuilder.Append(lines[i]);
                }

                _model.FileContent = _regexService.Replace(_model.FileContent, matchRegExResponse.Prefix + resultBuilder + matchRegExResponse.Suffix, matchRegExResponse.MatchingRegEx);
            }
        }          
    }
}
