using System;
using OpenRealEstate.Core;
using OpenRealEstate.Core.Residential;
using Shouldly;

namespace OpenRealEstate.Tests.Transmorgrifiers.REA
{
    public static class ResidentialListingAssertHelpers
    {
        public static void AssertResidentialListing(ResidentialListing source,
            ResidentialListing destination)
        {
            ListingAssertHelpers.AssertCommonData(source, destination);

            source.AuctionOn.ShouldBe(destination.AuctionOn);
            source.PropertyType.ShouldBe(destination.PropertyType);
            source.CouncilRates.ShouldBe(destination.CouncilRates);

            SalePricingAssertHelpers.AssertSalePrice(source.Pricing, destination.Pricing);
            BuildingDetailsAssertHelpers.AssertBuildingDetails(source.BuildingDetails, destination.BuildingDetails);
        }

        public static void AssertResidentialSoldListing(ResidentialListing listing,
            bool isSoldPriceVisibile = true)
        {
            listing.AgencyId.ShouldBe("XNWXNW");
            listing.Id.ShouldBe("Residential-Sold-ABCD1234");
            listing.StatusType.ShouldBe(StatusType.Sold);

            decimal? soldPrice = 580000m;
            listing.Pricing.SoldPrice.ShouldBe(soldPrice);
            listing.Pricing.SoldPriceText.ShouldBe(isSoldPriceVisibile
                ? soldPrice.Value.ToString("C0")
                : null);
            listing.Pricing.SoldOn.ShouldBe(new DateTime(2009, 01, 10, 12, 30, 00));
        }
    }
}