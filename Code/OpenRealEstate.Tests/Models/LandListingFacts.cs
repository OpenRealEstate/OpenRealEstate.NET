using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Models
{
    public class LandListingFacts
    {
        [Fact]
        public void GivenAnExistingListingAndANewListingWithEverythingModified_Copy_CopiesOverTheData()
        {
            // Arrange.
            var sourceListing = TestHelperUtilities.LandListing(false);
            var destinationListing = TestHelperUtilities.LandListingFromFile();

            // Act.
            destinationListing.Copy(sourceListing);

            // Assert.
            destinationListing.CategoryType.ShouldBe(sourceListing.CategoryType);
            destinationListing.AuctionOn.ShouldBe(sourceListing.AuctionOn);
            destinationListing.CouncilRates.ShouldBe(sourceListing.CouncilRates);

            destinationListing.Pricing.SalePrice.ShouldBe(sourceListing.Pricing.SalePrice);
            destinationListing.Pricing.SalePriceText.ShouldBe(sourceListing.Pricing.SalePriceText);
            destinationListing.Pricing.IsUnderOffer.ShouldBe(sourceListing.Pricing.IsUnderOffer);
            destinationListing.Pricing.SoldOn.ShouldBe(sourceListing.Pricing.SoldOn);
            destinationListing.Pricing.SoldPrice.ShouldBe(sourceListing.Pricing.SoldPrice);
            destinationListing.Pricing.SoldPriceText.ShouldBe(sourceListing.Pricing.SoldPriceText);

            destinationListing.Estate.Name.ShouldBe(sourceListing.Estate.Name);
            destinationListing.Estate.Stage.ShouldBe(sourceListing.Estate.Stage);
        }

        [Fact]
        public void GivenAnExistingListingAndANewListingWithANullValues_Copy_CopiesOverTheData()
        {
            // Arrange.
            var sourceListing = TestHelperUtilities.LandListingFromFile();
            sourceListing.Estate = null;
            sourceListing.Pricing = null;

            var destinationListing = TestHelperUtilities.LandListing();

            // Act.
            destinationListing.Copy(sourceListing);

            // Assert.
            destinationListing.Estate.ShouldBe(null);
            destinationListing.Pricing.ShouldBe(null);
        }
    }
}