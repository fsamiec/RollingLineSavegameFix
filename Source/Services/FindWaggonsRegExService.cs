using System.Text.RegularExpressions;

namespace RollingLineSavegameFix.Services
{
    public interface IFindWaggonsRegExService : IRegExService
    {

    }

    public class FindWaggonsRegExService : GenericRegExService, IFindWaggonsRegExService
    {
        protected override string AlternateMagicRegex() => @"(#wagons#\d*,)((?:\s|.)*)";

        protected override string MagicRegex() => @"(#wagons#\d*,)((?:\s|.)*?)(#objects#|#track#|#points#)";
    }   
}
