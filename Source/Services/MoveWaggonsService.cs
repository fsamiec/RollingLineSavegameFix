using RollingLineSavegameFix.Model;

namespace RollingLineSavegameFix.Services
{
    public class MoveWaggonsService : MoveSomethingService, IMoveWaggonsService
    {
        public MoveWaggonsService(IMainModel model, IFindWaggonsRegExService findTracksRegExService) : base(model, findTracksRegExService)
        { }
    }
}
