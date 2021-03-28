using AutoFixture;

namespace RollingLineSavegameFix.Tests
{
    internal class OmitAutoPropertiesTrueCompositeCustomization : CompositeCustomization
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public OmitAutoPropertiesTrueCompositeCustomization(ICustomization customization)
            : base(new OmitAutoPropertiesTrueCustomization(), customization)
        {
        }
    }
}