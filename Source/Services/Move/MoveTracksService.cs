using RollingLineSavegameFix.Model;
using System;
using System.Text.RegularExpressions;

namespace RollingLineSavegameFix.Services
{
    public class MoveTracksService : IMoveTracksService
    {
        private readonly IMainModel _model;
        private readonly IFindTracksRegExService _findTracksRegExService;
        private readonly IParseAndAddFloatValue _parseAndAddFloatValue;        


        public MoveTracksService(
            IMainModel model, 
            IFindTracksRegExService findTracksRegExService, 
            IParseAndAddFloatValue parseAndAddFloatValue)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _findTracksRegExService = findTracksRegExService ?? throw new ArgumentNullException(nameof(findTracksRegExService));
            _parseAndAddFloatValue = parseAndAddFloatValue ?? throw new ArgumentNullException(nameof(parseAndAddFloatValue));
        }

        public void Move()
        {
            
            var matchRegExResponse = _findTracksRegExService.MatchRegex(_model.FileContent);
            if (!matchRegExResponse.HasMatched)
            {
                return;
            }

            var tracks = matchRegExResponse.Content.Split("%");

            for (var i = 0; i < tracks.Length; i++)            
            {               
                var regex = new Regex(@"^(.*,\d,)(-?\d*\.\d*)_(-?\d*\.\d*)_(-?\d*\.\d*)(,.*)");
                var match = regex.Match(tracks[i]);

                var leadingValue = match.Groups[1].Value;
                var x = _parseAndAddFloatValue.For(match.Groups[2].Value, _model.MoveXAxisValue);
                var y = _parseAndAddFloatValue.For(match.Groups[3].Value, _model.MoveYAxisValue);
                var z = _parseAndAddFloatValue.For(match.Groups[4].Value, _model.MoveZAxisValue);
                var trailingValue = match.Groups[5].Value;

                tracks[i] = $"{leadingValue}{x}_{y}_{z}{trailingValue}";
            }

            var result = string.Join("%", tracks);
            _model.FileContent = _findTracksRegExService.Replace(_model.FileContent, matchRegExResponse.Prefix + result + matchRegExResponse.Suffix, matchRegExResponse.MatchingRegEx);         
        }
    }
}
