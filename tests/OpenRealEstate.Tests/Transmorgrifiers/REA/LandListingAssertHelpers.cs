using OpenRealEstate.Core.Land;
using Shouldly;

namespace OpenRealEstate.Tests.Transmorgrifiers.REA
{
    public static class LandListingAssertHelpers
    {
        public static void AssertLandListing(LandListing source,
                                             LandListing destination)
        {
            ListingAssertHelpers.AssertCommonData(source, destination);
            SalePricingAssertHelpers.AssertSalePrice(source.Pricing, destination.Pricing);
            AssertLandEstate(source.Estate, destination.Estate);

            source.AuctionOn.ShouldBe(destination.AuctionOn);
            source.CategoryType.ShouldBe(destination.CategoryType);
            source.CouncilRates.ShouldBe(destination.CouncilRates);
        }

        private static void AssertLandEstate(LandEstate source,
                                             LandEstate destination)
        {
            if (source == null &&
                destination == null)
            {
                return;
            }

            source.Name.ShouldBe(destination.Name);
            source.Stage.ShouldBe(destination.Stage);
        }
    }
}