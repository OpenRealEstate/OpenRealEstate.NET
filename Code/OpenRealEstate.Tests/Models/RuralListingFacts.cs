using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Models
{
    public class RuralListingFacts
    {
        public class CopyOverNewDataFacts
        {
            [Fact]
            public void GivenAnExistingListingAndANewListingWithEverythingModified_CopyOverNewData_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing = HelperUtilities.RuralListing;
                var destinationListing = HelperUtilities.RuralListingFromFile;

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

                destinationListing.RuralFeatures.AnnualRainfall.ShouldBe(sourceListing.RuralFeatures.AnnualRainfall);
                destinationListing.RuralFeatures.IsAnnualRainfallModified.ShouldBe(false);
                destinationListing.RuralFeatures.CarryingCapacity.ShouldBe(sourceListing.RuralFeatures.CarryingCapacity);
                destinationListing.RuralFeatures.IsCarryingCapacityModified.ShouldBe(false);
                destinationListing.RuralFeatures.Fencing.ShouldBe(sourceListing.RuralFeatures.Fencing);
                destinationListing.RuralFeatures.IsFencingModified.ShouldBe(false);
                destinationListing.RuralFeatures.Improvements.ShouldBe(sourceListing.RuralFeatures.Improvements);
                destinationListing.RuralFeatures.IsImprovementsModified.ShouldBe(false);
                destinationListing.RuralFeatures.Irrigation.ShouldBe(sourceListing.RuralFeatures.Irrigation);
                destinationListing.RuralFeatures.IsIrrigationModified.ShouldBe(false);
                destinationListing.RuralFeatures.Services.ShouldBe(sourceListing.RuralFeatures.Services);
                destinationListing.RuralFeatures.IsServicesModified.ShouldBe(false);
                destinationListing.RuralFeatures.SoilTypes.ShouldBe(sourceListing.RuralFeatures.SoilTypes);
                destinationListing.RuralFeatures.IsSoilTypesModified.ShouldBe(false);
                destinationListing.IsRuralFeaturesModified.ShouldBe(false);
            }

            [Fact]
            public void GivenAnExistingListingAndANewListingWithANullValues_CopyOverNewData_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing = HelperUtilities.RuralListingFromFile;
                sourceListing.BuildingDetails = null;
                sourceListing.Pricing = null;
                sourceListing.RuralFeatures = null;

                var destinationListing = HelperUtilities.RuralListing;

                // Act.
                destinationListing.CopyOverNewData(sourceListing);

                // Assert.
                destinationListing.BuildingDetails.ShouldBe(null);
                destinationListing.IsBuildingDetailsModified.ShouldBe(false);
                destinationListing.Pricing.ShouldBe(null);
                destinationListing.IsPricingModified.ShouldBe(false);
                destinationListing.RuralFeatures.ShouldBe(null);
                destinationListing.IsRuralFeaturesModified.ShouldBe(false);
            }
        }
    }
}
