using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Land;
using OpenRealEstate.Core.Models.Rental;
using OpenRealEstate.Core.Models.Residential;
using OpenRealEstate.Core.Models.Rural;
using OpenRealEstate.FakeData.Customizations;
using OpenRealEstate.FakeData.Properties;
using OpenRealEstate.Services.Json;
using Ploeh.AutoFixture;

namespace OpenRealEstate.FakeData
{
    public class FakeListings
    {
        public static T CreateAFakeListing<T>() where T : Listing, new()
        {
            var fixture = new Fixture()
                .Customize(new ListingCompositeCustomization());

            return fixture.Build<T>()
                .With(x => x.Id, $"listing-{fixture.Create<int>()}")
                .With(x => x.AgencyId, $"Agency-{fixture.Create<string>().Substring(0, 6)}")
                .With(x => x.StatusType, StatusType.Available)
                .Create();
        }

        public static IList<T> CreateFakeListings<T>(int numberOfFakeListings = 20) where T : Listing, new()
        {
            var fixedListings = CreateFixedListings<T>();
            if (numberOfFakeListings < fixedListings.Count)
            {
                throw new Exception(
                    $"You requested {numberOfFakeListings} listings, but there are {numberOfFakeListings} created. The requested _total_ number of fake listings has to be equal or greater then the number of fixed, fake listings. In this case, you need to request at least {numberOfFakeListings} listings.");
            }

            var listings = new List<T>();
            listings.AddRange(fixedListings);

            for (var i = 0; i < numberOfFakeListings - fixedListings.Count; i++)
            {
                listings.Add(CreateAFakeListing<T>());
            }

            return listings;
        }

        public static IEnumerable<ResidentialListing> CreateFixedResidentialListings()
        {
            return CreateFixedListings<ResidentialListing>(Resources.ResidentialListingsJson);
        }

        public static IEnumerable<RentalListing> CreateFixedRentalListings()
        {
            return CreateFixedListings<RentalListing>(Resources.RentalListingsJson);
        }

        public static IEnumerable<LandListing> CreateFixedLandListings()
        {
            return CreateFixedListings<LandListing>(Resources.LandListingsJson);
        }

        public static IEnumerable<RuralListing> CreateFixedRuralListings()
        {
            return CreateFixedListings<RuralListing>(Resources.RuralListingsJson);
        }

        public static IList<T> CreateFixedListings<T>() where T : Listing, new()
        {
            if (typeof (T) == typeof (ResidentialListing))
            {
                return CreateFixedResidentialListings().Cast<T>().ToList();
            }

            if (typeof (T) == typeof (RentalListing))
            {
                return CreateFixedRentalListings().Cast<T>().ToList();
            }

            if (typeof (T) == typeof (LandListing))
            {
                return CreateFixedLandListings().Cast<T>().ToList();
            }

            if (typeof (T) == typeof (RuralListing))
            {
                return CreateFixedRuralListings().Cast<T>().ToList();
            }

            throw new Exception($"The type '{typeof (T)}' was not handled.");
        }

        private static IEnumerable<T> CreateFixedListings<T>(byte[] resourceData) where T : Listing, new()
        {
            var json = new StreamReader(new MemoryStream(resourceData), true).ReadToEnd();

            var jsonTransmorgrifier = new JsonTransmorgrifier();
            var listings = jsonTransmorgrifier.ConvertTo(json);
            return listings.Listings.Select(x => x.Listing).Cast<T>();
        }
    }
}