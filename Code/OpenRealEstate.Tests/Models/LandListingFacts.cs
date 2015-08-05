using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Models
{
    public class LandListingFacts
    {
        [Fact]
        public void GivenAnExistingListingAndANewListingWithEverythingModified_CopyOverNewData_CopiesOverTheData()
        {
            // Arrange.
            var sourceListing = HelperUtilities.LandListing;
            var destinationListing = HelperUtilities.LandListingFromFile;

            // Act.
            destinationListing.CopyOverNewData(sourceListing);

            // Assert.
            destinationListing.CategoryType.ShouldBe(sourceListing.CategoryType);
            destinationListing.IsCategoryTypeModified.ShouldBe(false);
            destinationListing.AuctionOn.ShouldBe(sourceListing.AuctionOn);
            destinationListing.IsAuctionOnModified.ShouldBe(false);
            destinationListing.CouncilRates.ShouldBe(sourceListing.CouncilRates);
            destinationListing.IsCouncilRatesModified.ShouldBe(false);

            destinationListing.Pricing.SalePrice.ShouldBe(sourceListing.Pricing.SalePrice);
            destinationListing.Pricing.SalePriceText.ShouldBe(sourceListing.Pricing.SalePriceText);
            destinationListing.Pricing.IsUnderOffer.ShouldBe(sourceListing.Pricing.IsUnderOffer);
            destinationListing.Pricing.SoldOn.ShouldBe(sourceListing.Pricing.SoldOn);
            destinationListing.Pricing.IsSoldOnModified.ShouldBe(false);
            destinationListing.Pricing.SoldPrice.ShouldBe(sourceListing.Pricing.SoldPrice);
            destinationListing.Pricing.IsSoldPriceModified.ShouldBe(false);
            destinationListing.Pricing.SoldPriceText.ShouldBe(sourceListing.Pricing.SoldPriceText);
            destinationListing.Pricing.IsSoldPriceTextModified.ShouldBe(false);
            destinationListing.IsPricingModified.ShouldBe(false);

            destinationListing.Estate.Name.ShouldBe(sourceListing.Estate.Name);
            destinationListing.Estate.IsNameModified.ShouldBe(false);
            destinationListing.Estate.Stage.ShouldBe(sourceListing.Estate.Stage);
            destinationListing.IsEstateModified.ShouldBe(false);
        }

        [Fact]
        public void GivenAnExistingListingAndANewListingWithANullValues_CopyOverNewData_CopiesOverTheData()
        {
            // Arrange.
            var sourceListing = HelperUtilities.LandListingFromFile;
            sourceListing.Estate = null;
            sourceListing.Pricing = null;

            var destinationListing = HelperUtilities.LandListing;

            // Act.
            destinationListing.CopyOverNewData(sourceListing);

            // Assert.
            destinationListing.Estate.ShouldBe(null);
            destinationListing.IsEstateModified.ShouldBe(false);
            destinationListing.Pricing.ShouldBe(null);
            destinationListing.IsPricingModified.ShouldBe(false);
        }
    }
}