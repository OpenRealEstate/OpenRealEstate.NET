using System;
using System.Collections.Generic;
using System.Linq;
using OpenRealEstate.Core;

namespace OpenRealEstate.FakeData
{
    internal static class FakeCommonListingHelpers
    {
        internal static readonly HashSet<string> DefaultTags = new HashSet<string>
        {
            "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other",
            "balcony", "shed", "courtyard", "isANewConstruction"
        };

        internal static void SetCommonListingData(Listing listing,
            string agencyId = "XNWXNW",
            StatusType statusType = StatusType.Available,
            string description =
                "Don't pass up an opportunity like this! First to inspect will buy! Close to local amenities and schools. Features lavishly appointed bathrooms, modern kitchen, rustic outhouse.Don't pass up an opportunity like this! First to inspect will buy! Close to local amenities and schools. Features lavishly appointed bathrooms, modern kitchen, rustic outhouse.",
            string title = "SHOW STOPPER!!!")
        {
            SetFeatures(listing, tags: DefaultTags);
            SetAddress(listing);
            SetAgents(listing);
            SetFloorPlans(listing);
            SetImages(listing);
            SetInspections(listing);
            SetLandDetails(listing);
            SetLinks(listing);
            SetVideos(listing);

            listing.AgencyId = "XNWXNW";
            listing.CreatedOn = new DateTime(2009, 1, 1, 12, 30, 00);
            listing.StatusType = statusType;
            listing.Description = description;
            listing.Title = title;
        }

        internal static void SetFeatures(Listing listing,
            byte bedrooms = 4,
            byte bathrooms = 2,
            byte ensuite = 2,
            byte livingAreas = 0,
            byte carports = 2,
            byte garages = 3,
            byte openSpaces = 0,
            byte toilets = 0,
            HashSet<string> tags = null)
        {
            listing.Features = new Features
            {
                Bedrooms = bedrooms,
                Bathrooms = bathrooms,
                Ensuites = ensuite,
                LivingAreas = livingAreas,
                CarParking = new CarParking
                {
                    Carports = carports,
                    Garages = garages,
                    OpenSpaces = openSpaces
                },
                Toilets = toilets,
                Tags = tags
            };
        }

        internal static void SetAddress(Listing listing,
            string streetNumber = "2/39",
            string street = "Main Road",
            string suburb = "RICHMOND",
            string municipality = "Yarra",
            string state = "vic",
            string countryIsoCode = "AU",
            string postcode = "3121",
            bool isStreetDisplayed = true,
            decimal? latitude = null,
            decimal? longitude = null)
        {
            listing.Address = new Address
            {
                StreetNumber = streetNumber,
                Street = street,
                Suburb = suburb,
                Municipality = municipality,
                State = state,
                CountryIsoCode = countryIsoCode,
                Postcode = postcode,
                IsStreetDisplayed = isStreetDisplayed,
                Latitude = latitude,
                Longitude = longitude
            };
        }

        internal static void SetAgents(Listing listing)
        {
            listing.Agents = new List<ListingAgent> {CreateAFakeListingAgent()};
        }

        internal static ListingAgent CreateAFakeListingAgent(string name = "Mr. John Doe",
            string email = "jdoe@somedomain.com.au", 
            string mobilePhone = "0418 123 456",
            string workPhone = "05 1234 5678",
            int order = 1)
        {
            var emailCommunocation = new Communication
            {
                CommunicationType = CommunicationType.Email,
                Details = email
            };

            var mobileCommunication = new Communication
            {
                CommunicationType = CommunicationType.Mobile,
                Details = mobilePhone
            };

            var workCommunication = new Communication
            {
                CommunicationType = CommunicationType.Landline,
                Details = workPhone
            };

            return new ListingAgent
            {
                Name = name,
                Order = order,
                Communications = new List<Communication>
                {
                    emailCommunocation,
                    mobileCommunication,
                    workCommunication
                }
                    .ToList()
            };
        }

        internal static void SetFloorPlans(Listing listing)
        {
            listing.FloorPlans = new List<Media>
            {
                new Media
                {
                    Url = "http://www.realestate.com.au/tmp/floorplan1.gif",
                    CreatedOn = new DateTime(2009, 1, 1, 12, 30, 0),
                    Order = 1
                },
                new Media
                {
                    Url = "http://www.realestate.com.au/tmp/floorplan2.gif",
                    CreatedOn = new DateTime(2009, 1, 1, 12, 30, 0),
                    Order = 2
                }
            };
        }

        internal static void SetImages(Listing listing)
        {
            listing.Images = new List<Media>
            {
                new Media
                {
                    Url = "http://www.realestate.com.au/tmp/imageM.jpg",
                    CreatedOn = new DateTime(2009, 1, 1, 12, 30, 0),
                    Order = 1
                },
                new Media
                {
                    Url = "http://www.realestate.com.au/tmp/imageA.jpg",
                    CreatedOn = new DateTime(2009, 1, 1, 12, 30, 0),
                    Order = 2
                }
            };
        }

        internal static void SetInspections(Listing listing)
        {
            listing.Inspections = new List<Inspection>
            {
                new Inspection
                {
                    OpensOn = new DateTime(2009, 1, 21, 11, 00, 00),
                    ClosesOn = new DateTime(2009, 1, 21, 13, 00, 00)
                },
                new Inspection
                {
                    OpensOn = new DateTime(2009, 1, 22, 15, 00, 00),
                    ClosesOn = new DateTime(2009, 1, 22, 15, 30, 00)
                }
            };
        }

        internal static void SetLandDetails(Listing listing)
        {
            var depths = new List<Depth>
            {
                new Depth {Side = "rear", Type = "meter", Value = 40},
                new Depth {Side = "left", Type = "meter", Value = 60},
                new Depth {Side = "right", Type = "meter", Value = 20}
            };

            listing.LandDetails = new LandDetails
            {
                Area = new UnitOfMeasure {Type = "square", Value = 80},
                CrossOver = "left",
                Depths = depths,
                Frontage = new UnitOfMeasure {Type = "meter", Value = 20}
            };
        }

        internal static void SetLinks(Listing listing)
        {
            listing.Links = new List<string>
            {
                "http://www.au.open2view.com/properties/314244/tour#floorplan",
                "http://www.google.com/hello"
            };
        }

        internal static void SetVideos(Listing listing)
        {
            listing.Videos = new List<Media>
            {
                new Media
                {
                    Url = "http://www.foo.tv/abcd.html",
                    Order = 1,
                }
            };
        }

        internal static void SetBuildingDetails(IBuildingDetails listing )
        {
            listing.BuildingDetails = new BuildingDetails
            {
                Area = new UnitOfMeasure {Type = "square", Value = 40},
                EnergyRating = 4.5M
            };
        }

        internal static void SetSalePrice(ISalePricing listing,
            decimal salePrice = 500000,
            string salePriceText = "Between $400,000 and $600,000",
            bool isUnderOffer = false,
            DateTime? soldOn = null,
            decimal? soldPrice = null,
            string soldPriceText = null)
        {
            listing.Pricing = new SalePricing
            {
                SalePrice = salePrice,
                SalePriceText = salePriceText,
                IsUnderOffer = isUnderOffer,
                SoldOn = soldOn,
                SoldPrice = soldPrice,
                SoldPriceText = soldPriceText
            };
        }
    }
}