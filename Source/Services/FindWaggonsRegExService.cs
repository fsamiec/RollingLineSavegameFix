using System.Text.RegularExpressions;

namespace RollingLineSavegameFix.Services
{
    public class FindWaggonsRegExService : GenericRegExService
    {
        protected override string AlternateMagicRegex() => @"(#wagons#\d*,)((?:\s|.)*)";

        protected override string MagicRegex() => @"(#wagons#\d*,)((?:\s|.)*?)(#objects#|#track#|#points#)";
    }   
}
