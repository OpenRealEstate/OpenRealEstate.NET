using System;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Land;
using OpenRealEstate.Core.Models.Rental;
using OpenRealEstate.Core.Models.Residential;
using OpenRealEstate.Core.Models.Rural;
using OpenRealEstate.Services;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests
{
    public class ListingHelpersFacts
    {
        public class CopyFacts
        {
            [Fact]
            public void GivenTwoResidentialListingsWhereOneIsEmpty_Copy_CopiesTheData()
            {
                // Arrange.
                var destinationListing = new ResidentialListing();
                var sourceListing = TestHelperUtilities.ResidentialListingFromFile(false);
                sourceListing.StatusType = StatusType.Sold;
                sourceListing.Pricing.SalePrice = 100;
                sourceListing.Pricing.IsUnderOffer = true;
                sourceListing.Pricing.SoldOn = DateTime.Now;
                sourceListing.Pricing.SoldPrice = 200;
                sourceListing.Pricing.SoldPriceText = "just sold yeah!";
                sourceListing.Features.LivingAreas = 111;
                sourceListing.Features.Toilets = 8;
                sourceListing.Features.CarParking.OpenSpaces = 111;

                // Act.
                ListingHelpers.Copy(destinationListing, sourceListing);

                // Assert.
                TestHelperUtilities.AssertResidentialListing(destinationListing, sourceListing);
            }

            [Fact]
            public void GivenTwoResidentialListings_Copy_CopiesTheData()
            {
                // Arrange.
                var destinationListing = TestHelperUtilities.ResidentialListingFromFile();
                var sourceListing = TestHelperUtilities.ResidentialListingFromFile(false);
                sourceListing.StatusType = StatusType.Sold;
                sourceListing.Pricing.SalePrice = 100;

                // Act.
                ListingHelpers.Copy(destinationListing, sourceListing);

                // Assert.
                destinationListing.StatusType.ShouldBe(sourceListing.StatusType);
                destinationListing.Pricing.SalePrice.ShouldBe(sourceListing.Pricing.SalePrice);
            }

            [Fact]
            public void GivenTwoRentalListingWhereOneIsEmptys_Copy_CopiesTheData()
            {
                // Arrange.
                var destinationListing = new RentalListing();
                var sourceListing = TestHelperUtilities.RentalListing(false);
                sourceListing.StatusType = StatusType.Leased;
                sourceListing.Pricing.RentalPrice = 100;

                // Act.
                ListingHelpers.Copy(destinationListing, sourceListing);

                // Assert.
                TestHelperUtilities.AssertRentalListing(destinationListing, sourceListing);
            }

            [Fact]
            public void GivenTwoRentalListings_Copy_CopiesTheData()
            {
                // Arrange.
                var destinationListing = TestHelperUtilities.RentalListing();
                var sourceListing = TestHelperUtilities.RentalListing(false);
                sourceListing.StatusType = StatusType.Leased;
                sourceListing.Pricing.RentalPrice = 100;

                // Act.
                ListingHelpers.Copy(destinationListing, sourceListing);

                // Assert.
                destinationListing.StatusType.ShouldBe(sourceListing.StatusType);
                destinationListing.ModifiedData.IsModified.ShouldBe(true);
                destinationListing.Pricing.RentalPrice.ShouldBe(sourceListing.Pricing.RentalPrice);
                destinationListing.Pricing.ModifiedData.IsModified.ShouldBe(true);
            }

            [Fact]
            public void GivenTwoLandListingsWhereOneIsEmpty_Copy_CopiesTheData()
            {
                // Arrange.
                var destinationListing = new LandListing();
                var sourceListing = TestHelperUtilities.LandListing(false);
                sourceListing.StatusType = StatusType.Leased;
                //sourceListing.Pricing.

                // Act.
                ListingHelpers.Copy(destinationListing, sourceListing);

                // Assert.
                TestHelperUtilities.AssertLandListing(destinationListing, sourceListing);
            }

            [Fact]
            public void GivenTwoLandListings_Copy_CopiesTheData()
            {
                // Arrange.
                var destinationListing = TestHelperUtilities.LandListing();
                var sourceListing = TestHelperUtilities.LandListing(false);
                sourceListing.StatusType = StatusType.Leased;
                sourceListing.CouncilRates = "yo"; 
                sourceListing.Pricing.SalePrice = 100212;
                sourceListing.Estate.Name = "some name";

                // Act.
                ListingHelpers.Copy(destinationListing, sourceListing);

                // Assert.
                destinationListing.Pricing.SalePrice.ShouldBe(sourceListing.Pricing.SalePrice);
                destinationListing.Pricing.ModifiedData.IsModified.ShouldBe(true);
                destinationListing.Estate.Name.ShouldBe(sourceListing.Estate.Name);
                destinationListing.Estate.ModifiedData.IsModified.ShouldBe(true);
                destinationListing.StatusType.ShouldBe(sourceListing.StatusType);
                destinationListing.CouncilRates.ShouldBe(sourceListing.CouncilRates);
                destinationListing.ModifiedData.IsModified.ShouldBe(true);
            }

            [Fact]
            public void GivenTwoRuralListingsWhereOneIsEmpty_Copy_CopiesTheData()
            {
                // Arrange.
                var destinationListing = new RuralListing();
                var sourceListing = TestHelperUtilities.RuralListing(false);

                // Act.
                ListingHelpers.Copy(destinationListing, sourceListing);

                // Assert.
                TestHelperUtilities.AssertRuralListing(destinationListing, sourceListing);
            }

            [Fact]
            public void GivenTwoRuralListings_Copy_CopiesTheData()
            {
                // Arrange.
                var destinationListing = TestHelperUtilities.RuralListing();
                var sourceListing = TestHelperUtilities.RuralListing(false);
                sourceListing.StatusType = StatusType.Leased;
                sourceListing.Pricing.SalePrice = 100;

                // Act.
                ListingHelpers.Copy(destinationListing, sourceListing);

                // Assert.
                destinationListing.StatusType.ShouldBe(sourceListing.StatusType);
                destinationListing.Pricing.SalePrice.ShouldBe(sourceListing.Pricing.SalePrice);
            }

            [Fact]
            public void GivenTwoResidentialListingsWhereOneHasAnAuctionTimeAndTheNewOneDoesnt_Copy_CopiesTheDataWithNoAuctionTime()
            {
                // Arrange.
                var destinationListing = TestHelperUtilities.ResidentialListingFromFile(false);
                destinationListing.AuctionOn = DateTime.UtcNow.AddDays(-10);
                destinationListing.ClearAllIsModified();

                var sourceListing = TestHelperUtilities.ResidentialListingFromFile(false);
                sourceListing.AuctionOn = null;
                sourceListing.ClearAllIsModified();

                // Act.
                ListingHelpers.Copy(destinationListing, sourceListing, CopyDataOptions.CopyAllData);

                // Assert.
                destinationListing.AuctionOn.ShouldBeNull();
            }
        }
    }
}