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
            var sourceListing =TestHelperUtilities.LandListing(false);
            var destinationListing =TestHelperUtilities.LandListingFromFile();

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
            var sourceListing =TestHelperUtilities.LandListingFromFile();
            sourceListing.Estate = null;
            sourceListing.Pricing = null;

            var destinationListing = TestHelperUtilities.LandListing();

            // Act.
            destinationListing.Copy(sourceListing);

            // Assert.
            destinationListing.Estate.ShouldBe(null);
            destinationListing.IsEstateModified.ShouldBe(true);
            destinationListing.Pricing.ShouldBe(null);
            destinationListing.IsPricingModified.ShouldBe(true);
        }
    }

    public class IsModifiedFacts
    {
        [Fact]
        public void GivenAnExistingListingAndTheIdIsUpdated_IsModified_ReturnsTrue()
        {
            // Arrange.
            var listing = TestHelperUtilities.LandListingFromFile();
            listing.IsModified.ShouldBe(false);
            const string id = "pewpew";

            // Act.
            listing.Id = id;

            // Arrange.
            listing.IsModified.ShouldBe(true);
        }
    }

    public class IsEstateIsModifiedFacts
    {
        [Fact]
        public void GivenAnExistingListingAndEstateNameUpdated_IsLandEstateIsModified_ReturnsTrue()
        {
            // Arrange.
            var listing = TestHelperUtilities.LandListingFromFile();
            listing.IsEstateModified.ShouldBe(false);
            const string name = "pewpew";

            // Act.
            listing.Estate.Name = name;

            // Arrange.
            listing.IsEstateModified.ShouldBe(true);
        }
    }

    public class IsPricingIsModifiedFacts
    {
        [Fact]
        public void GivenAnExistingListingAndPricingSalePriceUdated_IsPricingIsModified_ReturnsTrue()
        {
            // Arrange.
            var listing = TestHelperUtilities.LandListingFromFile();
            listing.IsPricingModified.ShouldBe(false);
            const decimal salePrice = 1000;

            // Act.
            listing.Pricing.SalePrice = salePrice;

            // Arrange.
            listing.IsPricingModified.ShouldBe(true);
        }
    }
}