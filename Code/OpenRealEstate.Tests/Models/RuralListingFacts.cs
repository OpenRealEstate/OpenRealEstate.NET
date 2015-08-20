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
        public class CopyFacts
        {
            [Fact]
            public void GivenAnExistingListingAndANewListingWithEverythingModified_Copy_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing = HelperUtilities.RuralListing(false);
                var destinationListing = HelperUtilities.RuralListingFromFile();

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

                destinationListing.BuildingDetails.Area.Type.ShouldBe(sourceListing.BuildingDetails.Area.Type);
                destinationListing.BuildingDetails.Area.IsTypeModified.ShouldBe(true);
                destinationListing.BuildingDetails.Area.Value.ShouldBe(sourceListing.BuildingDetails.Area.Value);
                destinationListing.BuildingDetails.Area.IsValueModified.ShouldBe(true);
                destinationListing.BuildingDetails.IsAreaModified.ShouldBe(true);
                destinationListing.BuildingDetails.EnergyRating.ShouldBe(sourceListing.BuildingDetails.EnergyRating);
                destinationListing.BuildingDetails.IsEnergyRatingModified.ShouldBe(true);
                destinationListing.IsBuildingDetailsModified.ShouldBe(true);

                destinationListing.RuralFeatures.AnnualRainfall.ShouldBe(sourceListing.RuralFeatures.AnnualRainfall);
                destinationListing.RuralFeatures.IsAnnualRainfallModified.ShouldBe(true);
                destinationListing.RuralFeatures.CarryingCapacity.ShouldBe(sourceListing.RuralFeatures.CarryingCapacity);
                destinationListing.RuralFeatures.IsCarryingCapacityModified.ShouldBe(true);
                destinationListing.RuralFeatures.Fencing.ShouldBe(sourceListing.RuralFeatures.Fencing);
                destinationListing.RuralFeatures.IsFencingModified.ShouldBe(true);
                destinationListing.RuralFeatures.Improvements.ShouldBe(sourceListing.RuralFeatures.Improvements);
                destinationListing.RuralFeatures.IsImprovementsModified.ShouldBe(true);
                destinationListing.RuralFeatures.Irrigation.ShouldBe(sourceListing.RuralFeatures.Irrigation);
                destinationListing.RuralFeatures.IsIrrigationModified.ShouldBe(true);
                destinationListing.RuralFeatures.Services.ShouldBe(sourceListing.RuralFeatures.Services);
                destinationListing.RuralFeatures.IsServicesModified.ShouldBe(true);
                destinationListing.RuralFeatures.SoilTypes.ShouldBe(sourceListing.RuralFeatures.SoilTypes);
                destinationListing.RuralFeatures.IsSoilTypesModified.ShouldBe(true);
                destinationListing.IsRuralFeaturesModified.ShouldBe(true);
            }

            [Fact]
            public void GivenAnExistingListingAndANewListingWithANullValues_Copy_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing = HelperUtilities.RuralListingFromFile();
                sourceListing.BuildingDetails = null;
                sourceListing.Pricing = null;
                sourceListing.RuralFeatures = null;

                var destinationListing = HelperUtilities.RuralListing();

                // Act.
                destinationListing.Copy(sourceListing);

                // Assert.
                destinationListing.BuildingDetails.ShouldBe(null);
                destinationListing.IsBuildingDetailsModified.ShouldBe(true);
                destinationListing.Pricing.ShouldBe(null);
                destinationListing.IsPricingModified.ShouldBe(true);
                destinationListing.RuralFeatures.ShouldBe(null);
                destinationListing.IsRuralFeaturesModified.ShouldBe(true);
            }
        }
    }
}
