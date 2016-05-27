using Ploeh.AutoFixture;

namespace OpenRealEstate.FakeData.Customizations
{
    public class ListingCompositeCustomization : CompositeCustomization
    {
        public ListingCompositeCustomization() : base(
            new ResidentialListingCustomization(),
            new AddressCustomization(),
            new CommunicationCustomization()
            )
        {
        }
    }
}