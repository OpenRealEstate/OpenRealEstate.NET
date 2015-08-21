using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Models
{
    public class ResidentialListingFacts
    {
        public class CopyFacts
        {
            [Fact]
            public void GivenAnExistingListingAndANewListingWithEverythingModified_Copy_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing =TestHelperUtilities.ResidentialListing(false);
                var destinationListing =TestHelperUtilities.ResidentialListingFromFile();

                // Act.
                destinationListing.Copy(sourceListing);

                // Assert.
                destinationListing.AuctionOn.ShouldBe(sourceListing.AuctionOn);
                destinationListing.IsAuctionOnModified.ShouldBe(true);
                destinationListing.CouncilRates.ShouldBe(sourceListing.CouncilRates);
                destinationListing.IsCouncilRatesModified.ShouldBe(true);
                destinationListing.PropertyType.ShouldBe(sourceListing.PropertyType);
                destinationListing.IsPropertyTypeModified.ShouldBe(true);

                destinationListing.BuildingDetails.Area.Type.ShouldBe(sourceListing.BuildingDetails.Area.Type);
                destinationListing.BuildingDetails.Area.IsTypeModified.ShouldBe(true);
                destinationListing.BuildingDetails.Area.Value.ShouldBe(sourceListing.BuildingDetails.Area.Value);
                destinationListing.BuildingDetails.Area.IsValueModified.ShouldBe(true);
                destinationListing.BuildingDetails.IsAreaModified.ShouldBe(true);
                destinationListing.BuildingDetails.EnergyRating.ShouldBe(sourceListing.BuildingDetails.EnergyRating);
                destinationListing.BuildingDetails.IsEnergyRatingModified.ShouldBe(true);
                destinationListing.IsBuildingDetailsModified.ShouldBe(true);

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
            }

            [Fact]
            public void GivenAnExistingListingAndANewListingWithANullValues_Copy_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing =TestHelperUtilities.ResidentialListingFromFile();
                sourceListing.BuildingDetails = null;
                sourceListing.Pricing = null;

                var destinationListing =TestHelperUtilities.ResidentialListingFromFile();

                // Act.
                destinationListing.Copy(sourceListing);

                // Assert.
                destinationListing.BuildingDetails.ShouldBe(null);
                destinationListing.IsBuildingDetailsModified.ShouldBe(true);
                destinationListing.Pricing.ShouldBe(null);
                destinationListing.IsPricingModified.ShouldBe(true);
            }
        }
    }
}