using RollingLineSavegameFix.Model;
using System.Text.RegularExpressions;

namespace RollingLineSavegameFix.Services
{
    public class MoveCoWireObjectService : IMoveCoWireObjectService
    {        
        private readonly IMainModel _model;
        private readonly IParseAndAddFloatValue _parseAndAddFloatValue;

        private readonly Regex _regexDesTodesOfDoom = new Regex(@"(.*,\d,)((-?\d*(?:\.\d*)?)_(-?\d*(?:\.\d*)?)_(-?\d*(?:\.\d*)?))(,.*,)\2&((-?\d*(?:\.\d*)?)_(-?\d*(?:\.\d*)?)_(-?\d*(?:\.\d*)?))");

        public MoveCoWireObjectService(
            IMainModel model,
            IParseAndAddFloatValue parseAndAddFloatValue)
        {
            _model = model ?? throw new System.ArgumentNullException(nameof(model));
            _parseAndAddFloatValue = parseAndAddFloatValue ?? throw new System.ArgumentNullException(nameof(parseAndAddFloatValue));
        }

        public string For(string input)
        {
            var match = _regexDesTodesOfDoom.Match(input);
            if (!match.Success)
            { 
                return input;
            }

            var firstX = _parseAndAddFloatValue.For(match.Groups[3].Value, _model.MoveXAxisValue);
            var firstY = _parseAndAddFloatValue.For(match.Groups[4].Value, _model.MoveYAxisValue);
            var firstZ = _parseAndAddFloatValue.For(match.Groups[5].Value, _model.MoveZAxisValue);            
            
            var firstCoordinates = $"{firstX}_{firstY}_{firstZ}";
            input = input.Replace(match.Groups[2].Value, firstCoordinates);

            var secondX = _parseAndAddFloatValue.For(match.Groups[8].Value, _model.MoveXAxisValue);
            var secondY = _parseAndAddFloatValue.For(match.Groups[9].Value, _model.MoveYAxisValue);
            var secondZ = _parseAndAddFloatValue.For(match.Groups[10].Value, _model.MoveZAxisValue);

            var secondCoordinates = $"{secondX}_{secondY}_{secondZ}";
            input = input.Replace(match.Groups[7].Value, secondCoordinates);

            return input;
        }
    }
}
