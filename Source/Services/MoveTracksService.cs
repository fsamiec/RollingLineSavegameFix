using RollingLineSavegameFix.Model;

namespace RollingLineSavegameFix.Services
{
    public class MoveTracksService : MoveSomethingService, IMoveTracksService
    {
        public MoveTracksService(IMainModel model, IFindTracksRegExService findTracksRegExService) : base(model, findTracksRegExService)
        { }
    }
}
