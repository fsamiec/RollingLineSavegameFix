using RollingLineSavegameFix.Model;

namespace RollingLineSavegameFix.Services
{
    public class MoveWaggonsService : MoveSomethingService, IMoveWaggonsService
    {
        public MoveWaggonsService(IMainModel mainModel, IFindWaggonsRegExService findTracksRegExService, IParseAndAddFloatValue parseAndAddFloatValue) 
            : base(mainModel, findTracksRegExService, parseAndAddFloatValue)
        { }
    }
}
