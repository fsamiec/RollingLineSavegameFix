namespace RollingLineSavegameFix.Services
{

    public class FindObjectsRegExService : BaseFindRegExService, IFindObjectsRegExService
    {
        protected override string MagicRegex() => @"(#objects#\d*,)((?:\s|.)*?)(#track#|#waggons#|#points#)";

        protected override string AlternateMagicRegex() => @"(#objects#\d*,)((?:\s|.)*)";
    }
}
