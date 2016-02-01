using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Models
{
    public class RentalListingTests
    {
        public class CopyTests
        {
            [Fact]
            public void GivenAnExistingListingAndANewListingWithEverythingModified_Copy_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing = TestHelperUtilities.RentalListingFromFile(false);
                var destinationListing = TestHelperUtilities.RentalListing();

                // Act.
                destinationListing.Copy(sourceListing);

                // Assert.
                destinationListing.PropertyType.ShouldBe(sourceListing.PropertyType);
                destinationListing.AvailableOn.ShouldBe(sourceListing.AvailableOn);

                destinationListing.Pricing.RentalPrice.ShouldBe(sourceListing.Pricing.RentalPrice);
                destinationListing.Pricing.RentalPriceText.ShouldBe(sourceListing.Pricing.RentalPriceText);
                destinationListing.Pricing.PaymentFrequencyType.ShouldBe(sourceListing.Pricing.PaymentFrequencyType);
                destinationListing.Pricing.Bond.ShouldBe(sourceListing.Pricing.Bond);

                destinationListing.BuildingDetails.Area.Type.ShouldBe(sourceListing.BuildingDetails.Area.Type);
                destinationListing.BuildingDetails.Area.Value.ShouldBe(sourceListing.BuildingDetails.Area.Value);
                destinationListing.BuildingDetails.EnergyRating.ShouldBe(sourceListing.BuildingDetails.EnergyRating);
            }

            [Fact]
            public void GivenAnExistingListingAndANewListingWithANullValues_Copy_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing = TestHelperUtilities.RentalListingFromFile();
                sourceListing.BuildingDetails = null;
                sourceListing.Pricing = null;

                var destinationListing = TestHelperUtilities.RentalListing();

                // Act.
                destinationListing.Copy(sourceListing);

                // Assert.
                destinationListing.BuildingDetails.ShouldBeNull();
                destinationListing.Pricing.ShouldBeNull();
            }
        }
    }
}