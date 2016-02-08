using Ploeh.AutoFixture;

namespace OpenRealEstate.FakeData.Customizations
{
    public class ListingCompositeCustomization : CompositeCustomization
    {
        public ListingCompositeCustomization() : base(
            new AddressCustomization()
            )
        {
        }
    }
}