using OpenRealEstate.Core;
using OpenRealEstate.Core.Residential;
using Ploeh.AutoFixture;

namespace OpenRealEstate.FakeData
{
    public class ResidentialListingCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<ResidentialListing>(x => x.With(y => y.PropertyType, PropertyType.House));
        }
    }
}