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
                var sourceListing =TestHelperUtilities.RuralListing(false);
                var destinationListing =TestHelperUtilities.RuralListingFromFile();

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

                destinationListing.BuildingDetails.Area.Type.ShouldBe(sourceListing.BuildingDetails.Area.Type);
                destinationListing.BuildingDetails.Area.Value.ShouldBe(sourceListing.BuildingDetails.Area.Value);
                destinationListing.BuildingDetails.EnergyRating.ShouldBe(sourceListing.BuildingDetails.EnergyRating);

                destinationListing.RuralFeatures.AnnualRainfall.ShouldBe(sourceListing.RuralFeatures.AnnualRainfall);
                destinationListing.RuralFeatures.CarryingCapacity.ShouldBe(sourceListing.RuralFeatures.CarryingCapacity);
                destinationListing.RuralFeatures.Fencing.ShouldBe(sourceListing.RuralFeatures.Fencing);
                destinationListing.RuralFeatures.Improvements.ShouldBe(sourceListing.RuralFeatures.Improvements);
                destinationListing.RuralFeatures.Irrigation.ShouldBe(sourceListing.RuralFeatures.Irrigation);
                destinationListing.RuralFeatures.Services.ShouldBe(sourceListing.RuralFeatures.Services);
                destinationListing.RuralFeatures.SoilTypes.ShouldBe(sourceListing.RuralFeatures.SoilTypes);
            }

            [Fact]
            public void GivenAnExistingListingAndANewListingWithANullValues_Copy_CopiesOverTheData()
            {
                // Arrange.
                var sourceListing =TestHelperUtilities.RuralListingFromFile();
                sourceListing.BuildingDetails = null;
                sourceListing.Pricing = null;
                sourceListing.RuralFeatures = null;

                var destinationListing =TestHelperUtilities.RuralListing();

                // Act.
                destinationListing.Copy(sourceListing);

                // Assert.
                destinationListing.BuildingDetails.ShouldBeNull();
                destinationListing.Pricing.ShouldBeNull();
                destinationListing.RuralFeatures.ShouldBeNull();
            }
        }
    }
}
