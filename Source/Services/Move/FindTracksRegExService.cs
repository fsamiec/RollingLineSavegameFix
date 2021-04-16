namespace RollingLineSavegameFix.Services
{
    public class FindTracksRegExService : BaseFindRegExService, IFindTracksRegExService
    {
        protected override string MagicRegex() => @"(#track#*)((?:\s|.)*?)(#objects#|#waggons#|#points#)";

        protected override string AlternateMagicRegex() => @"(#track#)((?:\s|.)*)";


    }
}
