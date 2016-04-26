using OpenRealEstate.Core;
using Ploeh.AutoFixture;

namespace OpenRealEstate.FakeData.Customizations
{
    public class AddressCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Address>(c =>
                c.With(address => address.IsStreetDisplayed, true)
                    .With(address => address.CountryIsoCode, "AU")
                    .With(address => address.State, "VIC")
                    .With(address => address.Latitude, 10)
                    .With(address => address.Longitude, 10));
        }
    }
}