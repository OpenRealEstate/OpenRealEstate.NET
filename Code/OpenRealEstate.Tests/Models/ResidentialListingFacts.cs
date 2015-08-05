using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Models
{
    public class ResidentialListingFacts
    {
        public class CopyOverNewDataFacts
        {
            [Fact]
            public void GivenAnExistingListingAndANewListingWithEverythingModified_CopyOverNewData_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing = HelperUtilities.ResidentialListing;
                var destinationListing = HelperUtilities.ResidentialListingFromFile;

                // Act.
                destinationListing.CopyOverNewData(sourceListing);

                // Assert.
                destinationListing.AuctionOn.ShouldBe(sourceListing.AuctionOn);
                destinationListing.IsAuctionOnModified.ShouldBe(false);
                destinationListing.CouncilRates.ShouldBe(sourceListing.CouncilRates);
                destinationListing.IsCouncilRatesModified.ShouldBe(false);
                destinationListing.PropertyType.ShouldBe(sourceListing.PropertyType);
                destinationListing.IsPropertyTypeModified.ShouldBe(false);

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
            }

            [Fact]
            public void GivenAnExistingListingAndANewListingWithANullValues_CopyOverNewData_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing = HelperUtilities.ResidentialListingFromFile;
                sourceListing.BuildingDetails = null;
                sourceListing.Pricing = null;

                var destinationListing = HelperUtilities.ResidentialListingFromFile;

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