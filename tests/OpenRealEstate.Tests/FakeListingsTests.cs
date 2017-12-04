using System;
using OpenRealEstate.Core;
using OpenRealEstate.Core.Land;
using OpenRealEstate.Core.Rental;
using OpenRealEstate.Core.Residential;
using OpenRealEstate.Core.Rural;
using OpenRealEstate.FakeData;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests
{
    public class FakeListingsTests
    {
        public class CreateAFakeListingTests
        {
            [Theory]
            [InlineData(typeof(ResidentialListing))]
            [InlineData(typeof(RentalListing))]
            [InlineData(typeof(LandListing))]
            [InlineData(typeof(RuralListing))]
            public void GivenThatWeNeedAListing_CreateAFakeListing_ReturnsAListing(Type type)
            {
                // Arrange & Act.
                var listing = type == typeof(ResidentialListing)
                                  ? (Listing) FakeListings.CreateAFakeListing<ResidentialListing>()
                                  : type == typeof(RentalListing)
                                      ? (Listing) FakeListings.CreateAFakeListing<RentalListing>()
                                      : type == typeof(LandListing)
                                          ? (Listing) FakeListings.CreateAFakeListing<LandListing>()
                                          : (Listing) FakeListings.CreateAFakeListing<RuralListing>();

                // Assert.
                listing.ShouldNotBeNull();
                listing.GetType().ShouldBe(type);
            }
        }

        public class CreateFakeListingsTests : TestHelperUtilities
        {
            [Theory]
            [InlineData(typeof(ResidentialListing))]
            [InlineData(typeof(RentalListing))]
            [InlineData(typeof(LandListing))]
            [InlineData(typeof(RuralListing))]
            public void GivenThatWeNeedASomeListings_CreateFakeListings_ReturnsASomeListings(Type type)
            {
                // Arrange.
                const int numberOfListings = 30;

                // Act.
                var listings = CreateListings(type, numberOfListings);

                // Assert.
                listings.ShouldNotBeNull();
                listings.Count.ShouldBe(numberOfListings);
                listings.ShouldAllBe(x => x.GetType() == type);
            }
        }
    }
}