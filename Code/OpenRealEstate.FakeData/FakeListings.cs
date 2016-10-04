using System;
using System.Collections.Generic;
using System.Linq;
using OpenRealEstate.Core;
using OpenRealEstate.Core.Land;
using OpenRealEstate.Core.Rental;
using OpenRealEstate.Core.Residential;
using OpenRealEstate.Core.Rural;
using OpenRealEstate.FakeData.Customizations;
using Ploeh.AutoFixture;

namespace OpenRealEstate.FakeData
{
    public class FakeListings
    {
        private static readonly string[] Streets =
        {
            "Smith Street",
            "Park Street",
            "Holiday Lane",
            "Collins Street",
            "Sydney Road",
            "Stinky Place",
            "Albert Road",
            "Dead-Man's Alley",
            "Sunshine Court",
            "Little Bobby Tables Lane",
            ";DROP TABLE dbo.USERS;"
        };

        private static readonly string[] Suburbs =
        {
            "Richmond",
            "Ivanhoe",
            "Collingwood",
            "Hawthorn",
            "Kew",
            "Abbotsford",
            "Fairfield",
            "Ivanhoe East",
            "South Yarra",
            "Brighton",
            "Eaglemont"
        };

        private static readonly string[] Municipalities =
        {
            "City of Melbourne",
            "City of Port Phillip",
            "City of Stonnington",
            "City of Yarra",
            "City of Banyule",
            "City of Bayside",
            "City of Boroondara",
            "City of Brimbank",
            "City of Darebin",
            "City of Glen Eira",
            "City of Hobsons Bay"
        };

        public static T CreateAFakeListing<T>() where T : Listing, new()
        {
            var random = new Random();

            var fixture = new Fixture();
            
            fixture.Customize<Address>(c => c
                .With(a => a.IsStreetDisplayed, true)
                .With(a => a.StreetNumber, random.Next(1, 200).ToString())
                .With(a => a.Street, Streets[random.Next(0, Streets.Length)])
                .With(a => a.Suburb, Suburbs[random.Next(0, Suburbs.Length)])
                .With(a => a.Municipality, Municipalities[random.Next(0, Municipalities.Length)])
                .With(a => a.CountryIsoCode, "AU")
                .With(a => a.State, "VIC")
                .With(a => a.Latitude, 10)
                .With(a => a.Longitude, 10)
                .With(a => a.Postcode, random.Next(3000, 3999).ToString()));
            var address = fixture.Create<Address>();

            fixture.Customize<T>(c => c
                .With(x => x.Id, $"listing-{fixture.Create<int>()}")
                .With(x => x.AgencyId, $"Agency-{fixture.Create<string>().Substring(0, 6)}")
                .With(x => x.StatusType, StatusType.Available)
                .With(x => x.Address, address)
                //.Do(x =>
                //{
                //    ResidentialListing residentialListing;
                //    RentalListing rentalListing;
                //    RuralListing ruralListing;
                //    LandListing landListing;
                //    if ((residentialListing = x as ResidentialListing) != null)
                //    {
                //        var index = random.Next(1, Enum.GetValues(typeof(PropertyType)).Length);
                //        residentialListing.PropertyType =
                //            (PropertyType) Enum.GetValues(typeof(PropertyType)).GetValue(index);
                //    }
                //    else if ((rentalListing = x as RentalListing) != null)
                //    {
                //        var index = random.Next(1, Enum.GetValues(typeof(PropertyType)).Length);
                //        rentalListing.PropertyType = (PropertyType) Enum.GetValues(typeof(PropertyType)).GetValue(index);
                //    }
                //    else if ((ruralListing = x as RuralListing) != null)
                //    {
                //        var index = random.Next(1, Enum.GetValues(typeof(PropertyType)).Length);
                //        ruralListing.
                //    }
                //})
                .With(x => x.CreatedOn, new DateTime((DateTime.Now - new TimeSpan(0, random.Next(24, 24 * 7), 0, 0)).Ticks, DateTimeKind.Unspecified))
                .With(x => x.UpdatedOn, new DateTime((DateTime.Now - new TimeSpan(0, random.Next(1, 23), 0, 0)).Ticks, DateTimeKind.Unspecified)));

            return fixture.Create<T>();
        }

        public static IList<T> CreateFakeListings<T>(int numberOfFakeListings = 20) where T : Listing, new()
        {
            if (numberOfFakeListings <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfFakeListings),
                    "You need at least ONE fake listing to be requested.");
            }

            var listings = new List<T> {CreateFixedListing<T>()};

            // Start at 1 because the first listing should be the hard-coded one.
            for (var i = 1; i < numberOfFakeListings; i++)
            {
                listings.Add(CreateAFakeListing<T>());
            }

            return listings;
        }

        public static ResidentialListing CreateAFakeResidentialListing(string id = "Residential-Current-ABCD1234",
            PropertyType propertyType = PropertyType.House,
            string councilRates = "$2000 per month")
        {
            var listing = new ResidentialListing
            {
                Id = id
            };

            FakeCommonListingHelpers.SetCommonListingData(listing);
            FakeCommonListingHelpers.SetBuildingDetails(listing);
            FakeCommonListingHelpers.SetSalePrice(listing);

            listing.PropertyType = propertyType;
            listing.AuctionOn = new DateTime(2009, 2, 4, 18, 30, 0);
            listing.CouncilRates = councilRates;

            return listing;
        }

        public static RentalListing CreateAFakeRentalListing(string id = "Rental-Current-ABCD1234",
            PropertyType propertyType = PropertyType.House)
        {
            var listing = new RentalListing()
            {
                Id = id
            };

            FakeCommonListingHelpers.SetCommonListingData(listing);
            listing.Features.Tags.Remove("houseAndLandPackage");

            FakeCommonListingHelpers.SetBuildingDetails(listing);
            SetRentalPricing(listing);

            listing.AvailableOn = new DateTime(2009, 1, 26, 12, 30, 00);
            listing.PropertyType = propertyType;

            return listing;
        }

        public static LandListing CreateAFakeLandListing(string id = "Land-Current-ABCD1234",
            PropertyType propertyType = PropertyType.House)
        {
            var listing = new LandListing
            {
                Id = id
            };

            FakeCommonListingHelpers.SetCommonListingData(listing);
            listing.Address.StreetNumber = "LOT 12/39";
            listing.Features.Bedrooms = 0;
            listing.Features.Bathrooms = 0;
            listing.Features.Ensuites = 0;
            listing.Features.CarParking = new CarParking();

            FakeCommonListingHelpers.SetSalePrice(listing);
            SetLandEstate(listing);
            listing.AuctionOn = new DateTime(2009, 1, 24, 12, 30, 00);
            listing.CategoryType = Core.Land.CategoryType.Residential;
            listing.CouncilRates = "$2000 per month";

            listing.Features.Tags.Remove("houseAndLandPackage");
            listing.Features.Tags.Remove("isANewConstruction");
            listing.Features.Tags.Remove("hotWaterService-gas");
            listing.Features.Tags.Remove("heating-other");
            listing.Features.Tags.Remove("balcony");
            listing.Features.Tags.Remove("shed");
            listing.Features.Tags.Remove("courtyard");

            return listing;
        }

        public static RuralListing CreateAFakeRuralListing(string id = "Rural-Current-ABCD1234",
            PropertyType propertyType = PropertyType.House)
        {
            var listing = new RuralListing()
            {
                Id = id
            };

            FakeCommonListingHelpers.SetCommonListingData(listing);
            listing.Features.Tags.Remove("houseAndLandPackage");
            listing.Features.Tags.Remove("isANewConstruction");

            FakeCommonListingHelpers.SetBuildingDetails(listing);
            FakeCommonListingHelpers.SetSalePrice(listing);

            SetRuralFeatures(listing);

            listing.AuctionOn = new DateTime(2009, 1, 24, 14, 30, 00);
            listing.CategoryType = Core.Rural.CategoryType.Cropping;
            listing.CouncilRates = "$2,200 per annum";

            return listing;
        }

        private static T CreateFixedListing<T>() where T : Listing, new()
        {
            if (typeof(T) == typeof(ResidentialListing))
            {
                return CreateAFakeResidentialListing() as T;
            }

            if (typeof(T) == typeof(RentalListing))
            {
                return CreateAFakeRentalListing() as T;
            }

            if (typeof(T) == typeof(LandListing))
            {
                return CreateAFakeLandListing() as T;
            }

            if (typeof(T) == typeof(RuralListing))
            {
                return CreateAFakeRuralListing() as T;
            }

            throw new Exception($"The type '{typeof(T)}' was not handled.");
        }

        //private static ResidentialListing CreateFakeResidentialListing()
        //{
        //    var fixture = new Fixture()
        //        .Customize(new ListingCompositeCustomization());

        //    var random = new Random();

        //    var randomPropertyTypeindex = random.Next(1, Enum.GetValues(typeof(PropertyType)).Length);

        //    return fixture.Build<ResidentialListing>()
        //        .With(x => x.Id, $"listing-{fixture.Create<int>()}")
        //        .With(x => x.AgencyId, $"Agency-{fixture.Create<string>().Substring(0, 6)}")
        //        .With(x => x.StatusType, StatusType.Available)
        //        .With(x => x.PropertyType,
        //        (PropertyType)Enum.GetValues(typeof(PropertyType)).GetValue(randomPropertyTypeindex))
        //        .With(x => x.)
        //        .Create();
        //}

        //private static IList<Communication> CreateFakeCommunications(int numberOfCommunications)
        //{
        //    var results = new List<Communication>();

        //    var random = new Random();
        //    var fixture = new Fixture();
        //}

        private static void SetRentalPricing(RentalListing listing)
        {
            listing.Pricing = new RentalPricing
            {
                PaymentFrequencyType = PaymentFrequencyType.Weekly,
                RentalPrice = 350,
                RentalPriceText = "$350",
                Bond = 999
            };
        }

        private static void SetLandEstate(LandListing listing)
        {
            listing.Estate = new LandEstate
            {
                Name = "Panorama",
                Stage = "5"
            };
        }

        private static void SetRuralFeatures(RuralListing listing)
        {
            listing.RuralFeatures = new RuralFeatures
            {
                AnnualRainfall = "250 mm per annum",
                CarryingCapacity = "400 Deer or 100 head of breeding Cattle",
                Fencing = "Boundary and internal fencing all in good condition",
                Improvements = "Shearing shed, barn and machinery shed.",
                Irrigation = "Electric pump from dam and bore.",
                Services = "Power, telephone, airstrip, school bus, mail."
            };
        }
    }
}