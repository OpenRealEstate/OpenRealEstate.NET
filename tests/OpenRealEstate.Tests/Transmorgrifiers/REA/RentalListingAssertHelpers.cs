using OpenRealEstate.Core.Rental;
using Shouldly;

namespace OpenRealEstate.Tests.Transmorgrifiers.REA
{
    public static class RentalListingAssertHelpers
    {
        public static void AssertRuralListing(RentalListing source,
                                              RentalListing destination)
        {
            ListingAssertHelpers.AssertCommonData(source, destination);
            BuildingDetailsAssertHelpers.AssertBuildingDetails(source.BuildingDetails, destination.BuildingDetails);
            AssertRentalPricing(source.Pricing, destination.Pricing);

            source.AvailableOn.ShouldBe(destination.AvailableOn);
            source.PropertyType.ShouldBe(destination.PropertyType);
        }

        private static void AssertRentalPricing(RentalPricing source,
                                                RentalPricing destination)
        {
            if (source == null &&
                destination == null)
            {
                return;
            }

            source.Bond.ShouldBe(destination.Bond);
            source.PaymentFrequencyType.ShouldBe(destination.PaymentFrequencyType);
            source.RentalPrice.ShouldBe(destination.RentalPrice);
            source.RentalPriceText.ShouldBe(destination.RentalPriceText);
        }
    }
}