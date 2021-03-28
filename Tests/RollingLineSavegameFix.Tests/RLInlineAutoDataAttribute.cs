using AutoFixture.Xunit2;

namespace RollingLineSavegameFix.Tests
{
    public class RLInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="values"></param>
        public RLInlineAutoDataAttribute(params object[] values)
            : base(new RLAutoDataAttribute(), values)
        {
        }
    }
}