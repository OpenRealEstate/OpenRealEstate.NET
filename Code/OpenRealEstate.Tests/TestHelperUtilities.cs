using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Land;
using OpenRealEstate.Core.Models.Rental;
using OpenRealEstate.Core.Models.Residential;
using OpenRealEstate.Core.Models.Rural;
using OpenRealEstate.Services.RealEstateComAu;
using Shouldly;
using LandListingCategoryType = OpenRealEstate.Core.Models.Land.CategoryType;
using RuralListingCategoryType = OpenRealEstate.Core.Models.Rural.CategoryType;

namespace OpenRealEstate.Tests
{
    public static class TestHelperUtilities
    {
        public static ResidentialListing ResidentialListingFromFile(bool isClearAllIsModified = true,
            bool includeDataForCustomDataFields = true)
        {
            var reaXml =
                File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            var listing = (ResidentialListing)reaXmlTransmorgrifier
                .ConvertTo(reaXml)
                .Listings
                .First()
                .Listing;

            if (includeDataForCustomDataFields)
            {
                listing.Address.Latitude = 1m;
                listing.Address.Longitude = 2.4m;
                for (var i = 0; i < listing.FloorPlans.Count; i++)
                {
                    listing.FloorPlans[i].Tag = "Tag_" + i + 1;
                }

                for (var i = 0; i < listing.Images.Count; i++)
                {
                    listing.Images[i].Tag = "Tag_" + i + 1;
                }

                for (var i = 0; i < listing.Videos.Count; i++)
                {
                    listing.Videos[i].Tag = "Tag_" + i + 1;
                }
            }

            if (isClearAllIsModified)
            {
                listing.ClearAllIsModified();
            }

            return listing;
        }

        public static RentalListing RentalListingFromFile(bool isClearAllIsModified = true)
        {
            var reaXml =
                File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rental\\REA-Rental-Current.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            var listing = (RentalListing)reaXmlTransmorgrifier
                .ConvertTo(reaXml)
                .Listings
                .First()
                .Listing;

            if (isClearAllIsModified)
            {
                listing.ClearAllIsModified();
            }

            return listing;
        }

        public static LandListing LandListingFromFile(bool isClearAllIsModified = true)
        {
            var reaXml =
                File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-Current.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            var listing = (LandListing)reaXmlTransmorgrifier
                .ConvertTo(reaXml)
                .Listings
                .First()
                .Listing;

            if (isClearAllIsModified)
            {
                listing.ClearAllIsModified();
            }

            return listing;
        }

        public static RuralListing RuralListingFromFile(bool isClearAllIsModified = true)
        {
            var reaXml =
                File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rural\\REA-Rural-Current.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            var listing = (RuralListing)reaXmlTransmorgrifier
                .ConvertTo(reaXml)
                .Listings
                .First()
                .Listing;

            if (isClearAllIsModified)
            {
                listing.ClearAllIsModified();
            }

            return listing;
        }

        public static ResidentialListing ResidentialListing(bool isClearAllIsModified = true)
        {
            var listing = new ResidentialListing
                {
                    AuctionOn = new DateTime(2015, 5, 23),
                    Agents = new List<ListingAgent>
                    {
                        new ListingAgent
                        {
                            Name = "Princess Leia",
                            Order = 1,
                            Communications = new List<Communication>
                            {
                                new Communication
                                {
                                    CommunicationType = CommunicationType.Email,
                                    Details = "a@b.c.d.e"
                                }
                            }
                        }
                    },
                    BuildingDetails = new BuildingDetails
                    {
                        Area = new UnitOfMeasure
                        {
                            Type = "Some type",
                            Value = 1.2345m
                        },
                        EnergyRating = 111.222m,
                    },
                    CouncilRates = "some council rates",
                    PropertyType = PropertyType.Townhouse,
                    Pricing = new SalePricing
                    {
                        IsUnderOffer = true,
                        SalePrice = 12345.66m,
                        SalePriceText = "house for sale",
                        SoldOn = new DateTime(2015, 6, 1),
                        SoldPrice = 45432.99m,
                        SoldPriceText = "just sold woot!"
                    }
                };

            if (isClearAllIsModified)
            {
                listing.ClearAllIsModified();
            }

            return listing;
        }

        public static RentalListing RentalListing(bool isClearAllIsModified = true)
        {
                var listing = new RentalListing
                {
                    AvailableOn = new DateTime(2015, 5, 23),
                    BuildingDetails = new BuildingDetails
                    {
                        Area = new UnitOfMeasure
                        {
                            Type = "Some type",
                            Value = 1.2345m
                        },
                        EnergyRating = 111.222m,
                    },
                    PropertyType = PropertyType.Townhouse,
                    Pricing = new RentalPricing
                    {
                        RentalPrice = 567.88m,
                        RentalPriceText = "house for rent",
                        Bond = 1000m,
                        PaymentFrequencyType = PaymentFrequencyType.Monthly
                    }
                };

            if (isClearAllIsModified)
            {
                listing.ClearAllIsModified();
            }

            return listing;
        }

        public static LandListing LandListing(bool isClearAllIsModified = true)
        {
            var listing = new LandListing
            {
                CategoryType = LandListingCategoryType.Residential,
                AuctionOn = new DateTime(2015, 5, 23),
                CouncilRates = "some council rates",
                Estate = new LandEstate
                {
                    Name = "some land estate",
                    Stage = "1st stage"
                },
                Pricing = new SalePricing
                {
                    IsUnderOffer = true,
                    SalePrice = 12345.66m,
                    SalePriceText = "house for sale",
                    SoldOn = new DateTime(2015, 6, 1),
                    SoldPrice = 45432.99m,
                    SoldPriceText = "just sold woot!"
                }
            };

            if (isClearAllIsModified)
            {
                listing.ClearAllIsModified();
            }

            return listing;
        }

        public static RuralListing RuralListing(bool isClearAllIsModified = true)
        {
            var listing = new RuralListing
            {
                CategoryType = RuralListingCategoryType.Horticulture,
                AuctionOn = new DateTime(2015, 5, 23),
                CouncilRates = "some council rates",
                RuralFeatures = new RuralFeatures
                {
                    AnnualRainfall = "some rain",
                    CarryingCapacity = "some carrying capacity",
                    Fencing = "some fencing",
                    Improvements = "lots of improvements",
                    Irrigation = "some irrigation",
                    Services = "a number of services",
                    SoilTypes = "dirty soil"
                },
                Pricing = new SalePricing
                {
                    IsUnderOffer = true,
                    SalePrice = 12345.66m,
                    SalePriceText = "house for sale",
                    SoldOn = new DateTime(2015, 6, 1),
                    SoldPrice = 45432.99m,
                    SoldPriceText = "just sold woot!"
                },
                BuildingDetails = new BuildingDetails
                {
                    Area = new UnitOfMeasure
                    {
                        Type = "Some type",
                        Value = 1.2345m
                    },
                    EnergyRating = 111.222m,
                }
            };
         
            if (isClearAllIsModified)
            {
                listing.ClearAllIsModified();
            }
            return listing;
        }

        private static void UpdateListingWithFakeData(Listing listing)
        {
            if (listing == null)
            {
                throw new ArgumentNullException("listing");
            }

            UpdateAggregateRootWithFakeData(listing);

            listing.Agents = new List<ListingAgent>
            {
                new ListingAgent
                {
                    Name = "Princess Leia",
                    Order = 1,
                    Communications = new List<Communication>
                    {
                        new Communication
                        {
                            CommunicationType = CommunicationType.Email,
                            Details = "i-am-a-princess@rebel-alliance.org"
                        }
                    }
                },
                new ListingAgent
                {
                    Name = "Han Solo",
                    Order = 1,
                    Communications = new List<Communication>
                    {
                        new Communication
                        {
                            CommunicationType = CommunicationType.Email,
                            Details = "scruffy-nerf-herder@galacticmail.com"
                        }
                    }
                }
            };
            listing.Address = new Address
            {
                CountryIsoCode = "AU",
                IsStreetDisplayed = true,
                Latitude = 1.23m,
                Longitude = 3.45m,
                Municipality = "some municipality",
                Postcode = "1234a",
                State = "VIC",
                Street = "Some Street",
                StreetNumber = "69",
                Suburb = "Some Suburb"
            };
            listing.AgencyId = "ABCD-1234";
            listing.CreatedOn = new DateTime(2015, 5, 1);
            listing.Description = "Some description";
            listing.Features = new Features
            {
                Bathrooms = 1,
                Bedrooms = 2,
                Carports = 3,
                Ensuites = 4,
                Garages = 5,
                LivingAreas = 6,
                OpenSpaces = 7,
                Toilets = 8,
                Tags = new HashSet<string>(new[] {"z", "y", "x", "w"})
            };
            listing.FloorPlans = new List<Media>
            {
                new Media
                {
                    Url = "http://a.b.c/floorplan1",
                    Order = 1,
                    Tag = "fp1"
                },
                new Media
                {
                    Url = "http://a.b.c/floorplan2",
                    Order = 2,
                    Tag = "fp2"
                }
            };
            listing.Images = new List<Media>
            {
                new Media
                {
                    Url = "http://a.b.c/image1",
                    Order = 1,
                    Tag = "img1"
                },
                new Media
                {
                    Url = "http://a.b.c/image2",
                    Order = 2,
                    Tag = "img2"
                },
                new Media
                {
                    Url = "http://a.b.c/image3",
                    Order = 3,
                    Tag = "img3"
                }
            };
            listing.Inspections = new List<Inspection>
            {
                new Inspection
                {
                    OpensOn = new DateTime(2015, 5, 5, 11, 55, 00),
                    ClosesOn = new DateTime(2015, 5, 5, 13, 00, 00)
                },
                new Inspection
                {
                    OpensOn = new DateTime(2015, 5, 6, 11, 55, 00),
                    ClosesOn = new DateTime(2015, 5, 6, 13, 00, 00)
                },
                new Inspection
                {
                    OpensOn = new DateTime(2015, 5, 7, 11, 55, 00),
                    ClosesOn = new DateTime(2015, 5, 7, 13, 00, 00)
                }
            };
            listing.LandDetails = new LandDetails
            {
                Area = new Depth
                {
                    Value = 1.234m,
                    Type = "some type",
                    Side = "some side"
                },
                CrossOver = "some cross over",
                Depths = new List<Depth>
                {
                    new Depth
                    {
                        Value = 1234.11m,
                        Type = "some type 1",
                        Side = "some side 1"
                    },
                    new Depth
                    {
                        Value = 333.44m,
                        Type = "some type 2",
                        Side = "some side 2"
                    }
                },
                Frontage = new Depth
                {
                    Value = 1234.11m,
                    Type = "some type 1",
                    Side = "some side 1"
                }
            };
            listing.Links = new List<string>(new[] {"link 1", "link 2"});
            listing.StatusType = StatusType.Current;
            listing.Title = "some title";
            listing.Videos = new List<Media>
            {
                new Media
                {
                    Url = "http://a.b.c/video1",
                    Order = 1,
                    Tag = "v1"
                },
                new Media
                {
                    Url = "http://a.b.c/video2",
                    Order = 2,
                    Tag = "v2"
                }
            };
        }

        public static void UpdateAggregateRootWithFakeData(AggregateRoot aggregateRoot)
        {
            if (aggregateRoot == null)
            {
                throw new ArgumentNullException("aggregateRoot");
            }

            aggregateRoot.Id = "1234";
            aggregateRoot.UpdatedOn = new DateTime(2015, 5, 2);
        }

        public static void AssertMediaItems(IList<Media> destinationMedia,
            IList<Media> sourceMedia)
        {
            if ((destinationMedia == null &&
                 sourceMedia != null) ||
                (destinationMedia != null &&
                 sourceMedia == null))
            {
                throw new ArgumentException("Both ICollection<'s need to be null or need to be instantiated.");
            }

            for (int i = 0; i < destinationMedia.Count; i++)
            {
                destinationMedia[i].Url.ShouldBe(sourceMedia[i].Url);
                destinationMedia[i].IsUrlModified.ShouldBe(true);
                destinationMedia[i].Order.ShouldBe(sourceMedia[i].Order);
                destinationMedia[i].IsOrderModified.ShouldBe(true);
                destinationMedia[i].Tag.ShouldBe(sourceMedia[i].Tag);
                destinationMedia[i].IsTagModified.ShouldBe(true);
            }
        }
    }
}