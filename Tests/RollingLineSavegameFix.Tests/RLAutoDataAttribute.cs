using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

namespace RollingLineSavegameFix.Tests
{
    public class RLAutoDataAttribute : AutoDataAttribute
    {
        public RLAutoDataAttribute()
            : base(() => new Fixture().Customize(new OmitAutoPropertiesTrueCompositeCustomization(new AutoNSubstituteCustomization())))
        {
        }
    }
}