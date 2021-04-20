using RollingLineSavegameFix.Model;
using System.Text.RegularExpressions;

namespace RollingLineSavegameFix.Services
{
    public class MoveObjectsService : IMoveObjectsService
    {
        private readonly IMainModel _model;
        private readonly IFindObjectsRegExService _findObjectsRegExService;
        private readonly IMoveCoWireObjectService _moveCoWireObjectService;
        private readonly IParseAndAddFloatValue _parseAndAddFloatValue;        

        public MoveObjectsService(
            IMainModel model,
            IFindObjectsRegExService findObjectsRegExService,
            IMoveCoWireObjectService moveCoWireObjectService,
            IParseAndAddFloatValue parseAndAddFloatValue)
        {
            _model = model ?? throw new System.ArgumentNullException(nameof(model));
            _findObjectsRegExService = findObjectsRegExService ?? throw new System.ArgumentNullException(nameof(findObjectsRegExService));
            _moveCoWireObjectService = moveCoWireObjectService ?? throw new System.ArgumentNullException(nameof(moveCoWireObjectService));
            _parseAndAddFloatValue = parseAndAddFloatValue ?? throw new System.ArgumentNullException(nameof(parseAndAddFloatValue));
        }

        public void Move()
        {
            var matchRegExResponse = _findObjectsRegExService.MatchRegex(_model.FileContent);
            if (!matchRegExResponse.HasMatched)
            {
                return;
            }
            
            var regex = new Regex(@"(.*,\d,)(-?\d*\.\d*)_(-?\d*\.\d*)_(-?\d*\.\d*)(,.*)");
            var objects = matchRegExResponse.Content.Split("%");

            for (var i = 0; i < objects.Length; i++)
            {
                if (objects[i].Contains("co_wire", System.StringComparison.OrdinalIgnoreCase))
                {
                    objects[i] = _moveCoWireObjectService.For(objects[i]);                    
                }
                else
                { 
                    var match = regex.Match(objects[i]);
                    var leadingValue = match.Groups[1].Value;
                    var x = _parseAndAddFloatValue.For(match.Groups[2].Value, _model.MoveXAxisValue);
                    var y = _parseAndAddFloatValue.For(match.Groups[3].Value, _model.MoveYAxisValue);
                    var z = _parseAndAddFloatValue.For(match.Groups[4].Value, _model.MoveZAxisValue);
                    var trailingValue = match.Groups[5].Value;

                    objects[i] = $"{leadingValue}{x}_{y}_{z}{trailingValue}";
                }
            }

            var result = string.Join("%", objects);
            _model.FileContent = _findObjectsRegExService.Replace(_model.FileContent, matchRegExResponse.Prefix + result + matchRegExResponse.Suffix, matchRegExResponse.MatchingRegEx);
        }
    }
}
