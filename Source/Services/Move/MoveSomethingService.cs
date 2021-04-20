using RollingLineSavegameFix.Model;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace RollingLineSavegameFix.Services
{
    public abstract class MoveSomethingService : IMoveObjectsService
    {
        private readonly IMainModel _mainModel;
        private readonly IRegExService _regExService;
        private readonly IParseAndAddFloatValue _parseAndAddFloatValue;

        protected MoveSomethingService(IMainModel mainModel, IRegExService regExService, IParseAndAddFloatValue parseAndAddFloatValue)
        {
            _mainModel = mainModel ?? throw new ArgumentNullException(nameof(mainModel));
            _regExService = regExService ?? throw new ArgumentNullException(nameof(regExService));
            _parseAndAddFloatValue = parseAndAddFloatValue ?? throw new ArgumentNullException(nameof(parseAndAddFloatValue));
        }

        public void Move()
        {

            var matchRegExResponse = _regExService.MatchRegex(_mainModel.FileContent);
            if (!matchRegExResponse.HasMatched)
            {
                return;
            }

            var regex = new Regex(@"^(.*,\d,)(-?\d*\.\d*)_(-?\d*\.\d*)_(-?\d*\.\d*)(,.*)", RegexOptions.Multiline);
            var someObjects = matchRegExResponse.Content.Split("%");

            for (var i = 0; i < someObjects.Length; i++)
            {
                var match = regex.Match(someObjects[i]);

                var leadingValue = match.Groups[1].Value;
                var x = _parseAndAddFloatValue.For(match.Groups[2].Value, _mainModel.MoveXAxisValue);
                var y = _parseAndAddFloatValue.For(match.Groups[3].Value, _mainModel.MoveYAxisValue);
                var z = _parseAndAddFloatValue.For(match.Groups[4].Value, _mainModel.MoveZAxisValue);
                var trailingValue = match.Groups[5].Value;

                someObjects[i] = $"{leadingValue}{x}_{y}_{z}{trailingValue}";
            }

            var result = string.Join("%", someObjects);
            _mainModel.FileContent = _regExService.Replace(_mainModel.FileContent, matchRegExResponse.Prefix + result + matchRegExResponse.Suffix, matchRegExResponse.MatchingRegEx);
        }
    }
}
