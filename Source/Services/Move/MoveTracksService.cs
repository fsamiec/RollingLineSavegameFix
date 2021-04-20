using RollingLineSavegameFix.Model;
using System;
using System.Text.RegularExpressions;

namespace RollingLineSavegameFix.Services
{
    public class MoveTracksService : MoveSomethingService, IMoveTracksService
    {        
        public MoveTracksService(
            IMainModel mainModel, 
            IFindTracksRegExService findTracksRegExService, 
            IParseAndAddFloatValue parseAndAddFloatValue) : base(mainModel, findTracksRegExService, parseAndAddFloatValue)
        { }
    }
}
