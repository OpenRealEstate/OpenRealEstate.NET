﻿using System;
using System.Collections.Generic;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using OpenRealEstate.Core;
using OpenRealEstate.Core.Land;
using OpenRealEstate.Core.Rental;
using OpenRealEstate.Core.Residential;
using OpenRealEstate.Core.Rural;
using CategoryType = OpenRealEstate.Core.Land.CategoryType;

namespace OpenRealEstate.FakeData
{
    public class FakeListings
    {
        public static T CreateAFakeListing<T>() where T : Listing, new()
        {
            var listing = Builder<T>.CreateNew()
                                    .With(x => x.Id, $"listing-{GetRandom.Int()}")
                                    .With(x => x.AgencyId, $"Agency-{GetRandom.String(6)}")
                                    .With(x => x.StatusType, StatusType.Available)
                                    .With(x => x.Address, FakeAddress.CreateAFakeAddress())
                                    .With(x => x.Agents, FakeListingAgent.CreateFakeListingAgents())
                                    .With(x => x.CreatedOn, GetRandom.DateTime(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow.AddDays(-1)))
                                    .With(x => x.UpdatedOn, GetRandom.DateTimeFrom(DateTime.UtcNow.AddDays(-1)))

                                    .Do(x =>
                                    {
                                        if (x is ResidentialListing residentialListing)
                                        {
                                            // Skip first enumeration -> Unknown.
                                            var index = GetRandom.Int(1, Enum.GetValues(typeof(PropertyType)).Length);
                                            residentialListing.PropertyType = (PropertyType)Enum.GetValues(typeof(PropertyType)).GetValue(index);
                                        }
                                        else if (x is RentalListing rentalListing)
                                        {
                                            var index = GetRandom.Int(1, Enum.GetValues(typeof(PropertyType)).Length);
                                            rentalListing.PropertyType = (PropertyType)Enum.GetValues(typeof(PropertyType)).GetValue(index);
                                        }
                                    });

            return listing.Build();
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
                                                                       PropertyType propertyType = PropertyType.House)
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
            listing.CouncilRates = "$2000 per month";

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
            listing.CategoryType = CategoryType.Residential;
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