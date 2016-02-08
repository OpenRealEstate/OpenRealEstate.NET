using System;
using System.Collections.Generic;
using System.Linq;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Land;
using OpenRealEstate.Core.Models.Rental;
using OpenRealEstate.Core.Models.Residential;
using OpenRealEstate.Core.Models.Rural;
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
            [InlineData(typeof (ResidentialListing))]
            [InlineData(typeof(RentalListing))]
            [InlineData(typeof(LandListing))]
            [InlineData(typeof(RuralListing))]
            public void GivenThatWeNeedAListing_CreateAFakeListing_ReturnsAListing(Type type)
            {
                // Arrange & Act.
                var listing = type == typeof (ResidentialListing)
                    ? (Listing) FakeListings.CreateAFakeListing<ResidentialListing>()
                    : type == typeof (RentalListing)
                        ? (Listing) FakeListings.CreateAFakeListing<RentalListing>()
                        : type == typeof (LandListing)
                            ? (Listing) FakeListings.CreateAFakeListing<LandListing>()
                            : (Listing) FakeListings.CreateAFakeListing<RuralListing>();

                // Assert.
                listing.ShouldNotBeNull();
                listing.GetType().ShouldBe(type);
            }
        }

        public class CreateFakeListingsTests
        {
            [Theory]
            [InlineData(typeof(ResidentialListing))]
            [InlineData(typeof(RentalListing))]
            [InlineData(typeof(LandListing))]
            [InlineData(typeof(RuralListing))]
            public void GivenThatWeNeedASomeListings_CreateFakeListings_ReturnsASomeListings(Type type)
            {
                // Arrange.
                const int numberOfListings = 100;

                // Act.
                var listings = ExecuteCreateFakeListings(type, numberOfListings);

                // Assert.
                listings.ShouldNotBeNull();
                listings.Count.ShouldBe(numberOfListings);
                listings.ShouldAllBe(x => x.GetType() == type);
            }

            private static IList<Listing> ExecuteCreateFakeListings(Type type, int numberOfListings)
            {
                if (type == typeof(ResidentialListing))
                {
                    return
                        FakeListings.CreateFakeListings<ResidentialListing>(numberOfListings).Cast<Listing>().ToList();
                }

                if (type == typeof(RentalListing))
                {
                    return FakeListings.CreateFakeListings<RentalListing>(numberOfListings).Cast<Listing>().ToList();
                }

                if (type == typeof(LandListing))
                {
                    return FakeListings.CreateFakeListings<LandListing>(numberOfListings).Cast<Listing>().ToList();
                }

                if (type == typeof(RuralListing))
                {
                    return FakeListings.CreateFakeListings<RuralListing>(numberOfListings).Cast<Listing>().ToList();
                }

                throw new Exception($"Type '{type}' is not handled.");
            }
        }

        public class CreateFixedListingsTests
        {
            [Theory]
            [InlineData(typeof(ResidentialListing))]
            [InlineData(typeof(RentalListing))]
            [InlineData(typeof(LandListing))]
            [InlineData(typeof(RuralListing))]
            public void GivenThatWeNeedASomeFixedListings_CreateFixedListings_ReturnsASomeListings(Type type)
            {
                // Arrange & Act.
                var listings = ExecuteCreateFixedListings(type);

                // Assert.
                listings.ShouldNotBeNull();
                listings.Count.ShouldBe(5);
                listings.ShouldAllBe(x => x.GetType() == type);
            }

            private static IList<Listing> ExecuteCreateFixedListings(Type type)
            {
                if (type == typeof(ResidentialListing))
                {
                    return
                        FakeListings.CreateFixedListings<ResidentialListing>().Cast<Listing>().ToList();
                }

                if (type == typeof(RentalListing))
                {
                    return FakeListings.CreateFixedListings<RentalListing>().Cast<Listing>().ToList();
                }

                if (type == typeof(LandListing))
                {
                    return FakeListings.CreateFixedListings<LandListing>().Cast<Listing>().ToList();
                }

                if (type == typeof(RuralListing))
                {
                    return FakeListings.CreateFixedListings<RuralListing>().Cast<Listing>().ToList();
                }

                throw new Exception($"Type '{type}' is not handled.");
            }
        }
    }
}