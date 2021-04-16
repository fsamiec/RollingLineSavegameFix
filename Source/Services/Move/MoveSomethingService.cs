using RollingLineSavegameFix.Model;
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace RollingLineSavegameFix.Services
{
    public abstract class MoveSomethingService : IMoveObjectsService
    {
        private readonly IMainModel _model;
        private readonly IRegExService _regExService;
        private readonly IParseAndAddFloatValue _parseAndAddFloatValue;

        protected MoveSomethingService(IMainModel mainModel, IRegExService regExService, IParseAndAddFloatValue parseAndAddFloatValue)
        {
            _model = mainModel ?? throw new ArgumentNullException(nameof(mainModel));
            _regExService = regExService ?? throw new ArgumentNullException(nameof(regExService));
            _parseAndAddFloatValue = parseAndAddFloatValue ?? throw new ArgumentNullException(nameof(parseAndAddFloatValue));
        }

        //([a-zA-Z0-9_]{2,},[\d]),(-?\d*\.\d*)_(-?\d*\.\d*)_(-?\d*\.\d*),
        protected virtual Regex CoordinateRegex() => new Regex(@",([0-9]),(-?\d*\.\d*)_(-?\d*\.\d*)_(-?\d*\.\d*),");


        public void Move()
        {
            var matchRegExResponse = _regExService.MatchRegex(_model.FileContent);
            if (!matchRegExResponse.HasMatched)
            {
                return;
            }

            var resultBuilder = new StringBuilder(matchRegExResponse.Content);
                        
            var coordinateMatches = CoordinateRegex().Matches(matchRegExResponse.Content);

            for(var i = coordinateMatches.Count -1; i >= 0; i--)
            {
                var match = coordinateMatches[i];
                var preValue = match.Groups[1].Value;
                var x = _parseAndAddFloatValue.For(match.Groups[2].Value, _model.MoveXAxisValue);
                var y = _parseAndAddFloatValue.For(match.Groups[3].Value, _model.MoveYAxisValue);
                var z = _parseAndAddFloatValue.For(match.Groups[4].Value, _model.MoveZAxisValue);

                resultBuilder.Replace(match.Value, $",{preValue},{x}_{y}_{z},", match.Index, match.Length);
            }
            var result = resultBuilder.ToString();
            
            _model.FileContent =_regExService.Replace(_model.FileContent, matchRegExResponse.Prefix + result + matchRegExResponse.Suffix, matchRegExResponse.MatchingRegEx);
        }
    }

    public interface IParseAndAddFloatValue
    {
        string For(string input, float addition);
    }

    public class ParseAndAddFloatValue : IParseAndAddFloatValue
    {
        public string For(string input, float addition) => (double.Parse(input, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US")) + addition).ToString(CultureInfo.GetCultureInfo("en-US"));
    }
}
