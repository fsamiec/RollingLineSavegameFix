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

        protected MoveSomethingService(IMainModel mainModel, IRegExService regExService)
        {
            _model = mainModel ?? throw new ArgumentNullException(nameof(mainModel));
            _regExService = regExService ?? throw new ArgumentNullException(nameof(regExService));            
        }

        public void Move()
        {
            var matchRegExResponse = _regExService.MatchRegex(_model.FileContent);
            if (!matchRegExResponse.RegExMatched)
            {
                return;
            }

            var resultBuilder = new StringBuilder(matchRegExResponse.Content);           
            var coordinateRegex = new Regex(@"(-?\d*\.\d*)_(-?\d*\.\d*)_(-?\d*\.\d*)");
            var coordinateMatches = coordinateRegex.Matches(matchRegExResponse.Content);

            for(var i = coordinateMatches.Count -1; i >= 0; i--)
            {
                var match = coordinateMatches[i];
                var x = ParseAndAddValue(match.Groups[1].Value, _model.MoveXAxisValue);
                var y = ParseAndAddValue(match.Groups[2].Value, _model.MoveYAxisValue);
                var z = ParseAndAddValue(match.Groups[3].Value, _model.MoveZAxisValue);

                resultBuilder.Replace(match.Value, $"{x}_{y}_{z}", match.Index, match.Length);
            }
            var result = resultBuilder.ToString();
            
            _model.FileContent =_regExService.Replace(_model.FileContent, matchRegExResponse.Prefix + result + matchRegExResponse.Suffix, matchRegExResponse.MatchedRegEx);
        }

        private string ParseAndAddValue(string input, float addition) => (double.Parse(input, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US")) + addition).ToString(CultureInfo.GetCultureInfo("en-US"));        
    }
}
