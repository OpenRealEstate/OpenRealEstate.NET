using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Models
{
    public class RentalListingFacts
    {
        public class CopyOverNewDataFacts
        {
            [Fact]
            public void GivenAnExistingListingAndANewListingWithEverythingModified_CopyOverNewData_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing = HelperUtilities.RentalListing;
                var destinationListing = HelperUtilities.RentalListingFromFile;

                // Act.
                destinationListing.CopyOverNewData(sourceListing);

                // Assert.
                destinationListing.PropertyType.ShouldBe(sourceListing.PropertyType);
                destinationListing.IsPropertyTypeModified.ShouldBe(false);
                destinationListing.AvailableOn.ShouldBe(sourceListing.AvailableOn);
                destinationListing.IsAvailableOnModified.ShouldBe(false);

                destinationListing.Pricing.RentalPrice.ShouldBe(sourceListing.Pricing.RentalPrice);
                destinationListing.Pricing.IsRentalPriceModified.ShouldBe(false);
                destinationListing.Pricing.RentalPriceText.ShouldBe(sourceListing.Pricing.RentalPriceText);
                destinationListing.Pricing.IsRentalPriceTextModified.ShouldBe(false);
                destinationListing.Pricing.PaymentFrequencyType.ShouldBe(sourceListing.Pricing.PaymentFrequencyType);
                destinationListing.Pricing.IsPaymentFrequencyTypeModified.ShouldBe(false);
                destinationListing.Pricing.Bond.ShouldBe(sourceListing.Pricing.Bond);
                destinationListing.Pricing.IsBondModified.ShouldBe(false);
                destinationListing.IsPricingModified.ShouldBe(false);

                destinationListing.BuildingDetails.Area.Type.ShouldBe(sourceListing.BuildingDetails.Area.Type);
                destinationListing.BuildingDetails.Area.IsTypeModified.ShouldBe(false);
                destinationListing.BuildingDetails.Area.Value.ShouldBe(sourceListing.BuildingDetails.Area.Value);
                destinationListing.BuildingDetails.Area.IsValueModified.ShouldBe(false);
                destinationListing.BuildingDetails.IsAreaModified.ShouldBe(false);
                destinationListing.BuildingDetails.Tags.SetEquals(sourceListing.BuildingDetails.Tags).ShouldBe(true);
                destinationListing.BuildingDetails.IsTagsModified.ShouldBe(false);
                destinationListing.BuildingDetails.EnergyRating.ShouldBe(sourceListing.BuildingDetails.EnergyRating);
                destinationListing.BuildingDetails.IsEnergyRatingModified.ShouldBe(false);
                destinationListing.IsBuildingDetailsModified.ShouldBe(false);
            }

            [Fact]
            public void GivenAnExistingListingAndANewListingWithANullValues_CopyOverNewData_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing = HelperUtilities.RentalListingFromFile;
                sourceListing.BuildingDetails = null;
                sourceListing.Pricing = null;

                var destinationListing = HelperUtilities.RentalListing;

                // Act.
                destinationListing.CopyOverNewData(sourceListing);

                // Assert.
                destinationListing.BuildingDetails.ShouldBe(null);
                destinationListing.IsBuildingDetailsModified.ShouldBe(false);
                destinationListing.Pricing.ShouldBe(null);
                destinationListing.IsPricingModified.ShouldBe(false);
            }
        }
    }
}