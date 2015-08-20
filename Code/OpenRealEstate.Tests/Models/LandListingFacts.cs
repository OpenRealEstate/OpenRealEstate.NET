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
            var sourceListing = HelperUtilities.LandListing(false);
            var destinationListing = HelperUtilities.LandListingFromFile();

            // Act.
            destinationListing.Copy(sourceListing);

            // Assert.
            destinationListing.CategoryType.ShouldBe(sourceListing.CategoryType);
            destinationListing.IsCategoryTypeModified.ShouldBe(true);
            destinationListing.AuctionOn.ShouldBe(sourceListing.AuctionOn);
            destinationListing.IsAuctionOnModified.ShouldBe(true);
            destinationListing.CouncilRates.ShouldBe(sourceListing.CouncilRates);
            destinationListing.IsCouncilRatesModified.ShouldBe(true);

            destinationListing.Pricing.SalePrice.ShouldBe(sourceListing.Pricing.SalePrice);
            destinationListing.Pricing.SalePriceText.ShouldBe(sourceListing.Pricing.SalePriceText);
            destinationListing.Pricing.IsUnderOffer.ShouldBe(sourceListing.Pricing.IsUnderOffer);
            destinationListing.Pricing.SoldOn.ShouldBe(sourceListing.Pricing.SoldOn);
            destinationListing.Pricing.IsSoldOnModified.ShouldBe(true);
            destinationListing.Pricing.SoldPrice.ShouldBe(sourceListing.Pricing.SoldPrice);
            destinationListing.Pricing.IsSoldPriceModified.ShouldBe(true);
            destinationListing.Pricing.SoldPriceText.ShouldBe(sourceListing.Pricing.SoldPriceText);
            destinationListing.Pricing.IsSoldPriceTextModified.ShouldBe(true);
            destinationListing.IsPricingModified.ShouldBe(true);

            destinationListing.Estate.Name.ShouldBe(sourceListing.Estate.Name);
            destinationListing.Estate.IsNameModified.ShouldBe(true);
            destinationListing.Estate.Stage.ShouldBe(sourceListing.Estate.Stage);
            destinationListing.IsEstateModified.ShouldBe(true);
        }

        [Fact]
        public void GivenAnExistingListingAndANewListingWithANullValues_Copy_CopiesOverTheData()
        {
            // Arrange.
            var sourceListing = HelperUtilities.LandListingFromFile();
            sourceListing.Estate = null;
            sourceListing.Pricing = null;

            var destinationListing = HelperUtilities.LandListing();

            // Act.
            destinationListing.Copy(sourceListing);

            // Assert.
            destinationListing.Estate.ShouldBe(null);
            destinationListing.IsEstateModified.ShouldBe(true);
            destinationListing.Pricing.ShouldBe(null);
            destinationListing.IsPricingModified.ShouldBe(true);
        }
    }
}