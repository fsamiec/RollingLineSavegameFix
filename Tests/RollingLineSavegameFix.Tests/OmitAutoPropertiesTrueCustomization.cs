using AutoFixture;

namespace RollingLineSavegameFix.Tests
{
    internal class OmitAutoPropertiesTrueCustomization : ICustomization
    {
        public void Customize(IFixture fixture) => fixture.OmitAutoProperties = true;
    }
}