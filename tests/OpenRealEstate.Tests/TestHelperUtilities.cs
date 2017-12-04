using System;
using System.Collections.Generic;
using System.Linq;
using OpenRealEstate.Core;
using OpenRealEstate.Core.Land;
using OpenRealEstate.Core.Rental;
using OpenRealEstate.Core.Residential;
using OpenRealEstate.Core.Rural;
using OpenRealEstate.FakeData;

namespace OpenRealEstate.Tests
{
    public abstract class TestHelperUtilities
    {
        protected IList<Listing> CreateListings(Type listingType,
                                                int numberOfFakeListings)
        {
            if (listingType == typeof(ResidentialListing))
            {
                return FakeListings.CreateFakeListings<ResidentialListing>(numberOfFakeListings)
                                   .Cast<Listing>()
                                   .ToList();
            }

            if (listingType == typeof(RentalListing))
            {
                return FakeListings.CreateFakeListings<RentalListing>(numberOfFakeListings)
                                   .Cast<Listing>()
                                   .ToList();
            }

            if (listingType == typeof(LandListing))
            {
                return FakeListings.CreateFakeListings<LandListing>(numberOfFakeListings)
                                   .Cast<Listing>()
                                   .ToList();
            }

            if (listingType == typeof(RuralListing))
            {
                return FakeListings.CreateFakeListings<RuralListing>(numberOfFakeListings)
                                   .Cast<Listing>()
                                   .ToList();
            }

            throw new Exception($"Failed to assert the suggested type: '{listingType}'.");
        }
    }
}