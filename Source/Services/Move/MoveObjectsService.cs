using RollingLineSavegameFix.Model;

namespace RollingLineSavegameFix.Services
{
    public class MoveObjectsService : MoveSomethingService, IMoveObjectsService
    {
        public MoveObjectsService(IMainModel model, IFindObjectsRegExService findObjectsRegExService, IParseAndAddFloatValue parseAndAddFloatValue) : base(model, findObjectsRegExService, parseAndAddFloatValue)
        {

        }
    }
}
