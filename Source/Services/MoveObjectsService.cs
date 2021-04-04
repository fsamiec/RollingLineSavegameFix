using RollingLineSavegameFix.Model;
using System;

namespace RollingLineSavegameFix.Services
{
    public class MoveObjectsService : IMoveObjectsService
    {
        private readonly IMainModel _model;
        private readonly IRegExService _regExService;

        public MoveObjectsService(
            IMainModel mainModel,
            IRegExService regExService)
        {
            _model = mainModel ?? throw new ArgumentNullException(nameof(mainModel));
            _regExService = regExService ?? throw new ArgumentNullException(nameof(regExService));
        }

        public void MoveObjects()
        {

            var matchRegExResponse = _regExService.MatchRegex(_model.FileContent);
            if (!matchRegExResponse.RegExMatched)
            {
                return;
            }
            
            if (_model.MoveXAxisValue != 0)
            {
                var content = matchRegExResponse.Content;
                // I LIKE TO MOVE IT MOVE IT
            }
        }
    }
}
