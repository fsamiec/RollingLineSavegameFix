using RollingLineSavegameFix.Model;
using System;
using System.Text.RegularExpressions;

namespace RollingLineSavegameFix.Services
{
    public class MoveTracksService : IMoveTracksService
    {
        private readonly IMainModel _mainModel;
        private readonly IFindTracksRegExService _findTracksRegExService;
        private readonly IParseAndAddFloatValue _parseAndAddFloatValue;

        public MoveTracksService(
            IMainModel mainModel, 
            IFindTracksRegExService findTracksRegExService,
            IParseAndAddFloatValue parseAndAddFloatValue)
        {
            _mainModel = mainModel ?? throw new ArgumentNullException(nameof(mainModel));
            _findTracksRegExService = findTracksRegExService ?? throw new ArgumentNullException(nameof(findTracksRegExService));
            _parseAndAddFloatValue = parseAndAddFloatValue ?? throw new ArgumentNullException(nameof(parseAndAddFloatValue));
        }

        public void Move()
        {
            var matchRegExResponse = _findTracksRegExService.MatchRegex(_mainModel.FileContent);
            if (!matchRegExResponse.HasMatched)
            {
                return;
            }

            var tracks = matchRegExResponse.Content.Split("%");
            var regex = new Regex(@"(?:ct_dynamic(?:_flexy)?,\d),((-?\d*(?:\.\d*)?)_(-?\d(?:\.\d*)?)_(-?\d*(?:\.\d*)?))");

            for (var i = 0; i < tracks.Length; i++)
            {                
                var match = regex.Match(tracks[i]);
                if (!match.Success)
                {
                    continue;
                }

                var x = _parseAndAddFloatValue.For(match.Groups[2].Value, _mainModel.MoveXAxisValue);
                var y = _parseAndAddFloatValue.For(match.Groups[3].Value, _mainModel.MoveYAxisValue);
                var z = _parseAndAddFloatValue.For(match.Groups[4].Value, _mainModel.MoveZAxisValue);

                tracks[i] = tracks[i].Replace(match.Groups[1].Value, $"{x}_{y}_{z}");

            }

            var result = string.Join("%", tracks);
            _mainModel.FileContent = _findTracksRegExService.Replace(_mainModel.FileContent, matchRegExResponse.Prefix + result + matchRegExResponse.Suffix, matchRegExResponse.MatchingRegEx);
        }
    }
}
