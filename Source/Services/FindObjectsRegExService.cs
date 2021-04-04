namespace RollingLineSavegameFix.Services
{
    public class FindObjectsRegExService : GenericRegExService
    {
        protected override string MagicRegex() => @"(#objects#\d*,)((?:\s|.)*?)(#track#|#waggons#|#points#)";

        protected override string AlternateMagicRegex() => @"(#objects#\d*,)((?:\s|.)*)";
    }
}
