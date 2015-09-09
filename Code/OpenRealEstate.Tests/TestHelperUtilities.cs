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
            var communications = new List<Communication>
            {
                new Communication
                {
                    CommunicationType = CommunicationType.Email,
                    Details = "a@b.c.d.e"
                }
            };
            var agent = new ListingAgent
            {
                Name = "Princess Leia",
                Order = 1
            };
            agent.AddCommunications(communications);

            var agents = new List<ListingAgent>
            {
                agent
            };

            var listing = new ResidentialListing
            {
                AuctionOn = new DateTime(2015, 5, 23),

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
            listing.AddAgents(agents);

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

            var agent1 = new ListingAgent
            {
                Name = "Princess Leia",
                Order = 1
            };
            agent1.AddCommunications(new List<Communication>
            {
                new Communication
                {
                    CommunicationType = CommunicationType.Email,
                    Details = "i-am-a-princess@rebel-alliance.org"
                }
            });
            
            var agent2 = new ListingAgent
            {
                Name = "Han Solo",
                Order = 1
            };
            agent2.AddCommunications(new List<Communication>
            {
                new Communication
                {
                    CommunicationType = CommunicationType.Email,
                    Details = "scruffy-nerf-herder@galacticmail.com"
                }
            });

            listing.AddAgents(new List<ListingAgent>
            {
                agent1,
                agent2
            });

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
                CarParking = new CarParking
                {
                    Garages = 5,
                    Carports = 3,
                    OpenSpaces = 7
                },
                Ensuites = 4,
                LivingAreas = 6,

                Toilets = 8,
            };
            listing.Features.AddTags(new HashSet<string>(new[] {"z", "y", "x", "w"}));
            listing.AddFloorPlans(new List<Media>
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
            });
            listing.AddImages(new List<Media>
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
            });
            listing.AddInspections(new List<Inspection>
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
            });
            listing.LandDetails = new LandDetails
            {
                Area = new Depth
                {
                    Value = 1.234m,
                    Type = "some type",
                    Side = "some side"
                },
                CrossOver = "some cross over",
                Frontage = new Depth
                {
                    Value = 1234.11m,
                    Type = "some type 1",
                    Side = "some side 1"
                }
            };
            listing.LandDetails.AddDepths(new List<Depth>
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
            });
            listing.AddLinks(new List<string>(new[] {"link 1", "link 2"}));
            listing.StatusType = StatusType.Current;
            listing.Title = "some title";
            listing.AddVideos(new List<Media>
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
            });
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

        #region Asserts

        public static void AssertAggregateRoot(AggregateRoot destination, AggregateRoot source)
        {
            if (destination == null &&
                source == null)
            {
                return;
            } 
            
            destination.Id.ShouldBe(source.Id);
            destination.UpdatedOn.ShouldBe(source.UpdatedOn);
        }

        public static void AssertAddress(Address destination, Address source)
        {
            if (destination == null &&
                source == null)
            {
                return;
            }

            destination.CountryIsoCode.ShouldBe(source.CountryIsoCode);
            destination.IsStreetDisplayed.ShouldBe(source.IsStreetDisplayed);
            destination.Latitude.ShouldBe(source.Latitude);
            destination.Longitude.ShouldBe(source.Longitude);
            destination.Municipality.ShouldBe(source.Municipality);
            destination.Postcode.ShouldBe(source.Postcode);
            destination.State.ShouldBe(destination.State);
            destination.Street.ShouldBe(source.Street);
            destination.StreetNumber.ShouldBe(source.StreetNumber);
            destination.Suburb.ShouldBe(source.Suburb);
            destination.IsModified.ShouldBe(true);
        }

        public static void AssertBuildingDetails(BuildingDetails destination, BuildingDetails source)
        {
            if (destination == null &&
                source == null)
            {
                return;
            }

            destination.EnergyRating.ShouldBe(source.EnergyRating);
            AssertUnitOfMeasure(destination.Area, source.Area);
            destination.ModifiedData.IsModified.ShouldBe(true);
        }

        public static void AssertSalePricing(SalePricing destination, SalePricing source)
        {
            if (destination == null &&
                source == null)
            {
                return;
            }

            destination.IsUnderOffer.ShouldBe(source.IsUnderOffer);
            destination.SalePrice.ShouldBe(source.SalePrice);
            destination.SalePriceText.ShouldBe(source.SalePriceText);
            destination.SoldPrice.ShouldBe(source.SoldPrice);
            destination.SoldPriceText.ShouldBe(source.SoldPriceText);
            destination.SoldOn.ShouldBe(source.SoldOn);
            destination.ModifiedData.IsModified.ShouldBe(true);
        }

        public static void AssertCommunication(Communication destination, Communication source)
        {
            if (destination == null &&
                   source == null)
            {
                return;
            }

            destination.CommunicationType.ShouldBe(source.CommunicationType);
            destination.Details.ShouldBe(source.Details);
            destination.ModifiedData.IsModified.ShouldBe(true);
        }

        public static void AssertListingAgent(ListingAgent destination, ListingAgent source)
        {
            if (destination == null &&
                source == null)
            {
                return;
            }

            if (destination.Communications != null)
            {
                for(var i = 0; i < destination.Communications.Count; i++)
                {
                    destination.Communications.Count.ShouldBe(source.Communications.Count);
                    AssertCommunication(destination.Communications[i], source.Communications[i]);
                }
            }
            destination.Name.ShouldBe(source.Name);
            destination.Order.ShouldBe(source.Order);
            destination.ModifiedData.IsModified.ShouldBe(true);
        }

        public static void AssertListingAgents(IList<ListingAgent> destination, IList<ListingAgent> source)
        {
            if (destination != null &&
                source != null)
            {
                destination.Count.ShouldBe(source.Count);
                for (var i = 0; i < destination.Count; i++)
                {
                    AssertListingAgent(destination[i], source[i]);
                }
            }
        }

        public static void AssertCarparking(CarParking destination, CarParking source)
        {
            if (destination == null &&
                source == null)
            {
                return;
            }

            destination.Carports.ShouldBe(source.Carports);
            destination.Garages.ShouldBe(source.Garages);
            destination.OpenSpaces.ShouldBe(source.OpenSpaces);
            destination.ModifiedData.IsModified.ShouldBe(true);
        }

        public static void AssertStringCollection(IList<string> destination, IList<string> source)
        {
            if (destination == null &&
                source == null)
            {
                return;
            }

            destination.Count.ShouldBe(source.Count);
            for (var i = 0; i < destination.Count; i++)
            {
                destination[i].ShouldBe(source[i]);
            }
        }

        public static void AssertFeatures(Features destination, Features source)
        {
            if (destination == null &&
                source == null)
            {
                return;
            }

            destination.Bathrooms.ShouldBe(source.Bathrooms);
            destination.Bedrooms.ShouldBe(source.Bedrooms);
            AssertCarparking(destination.CarParking, source.CarParking);
            destination.Ensuites.ShouldBe(source.Ensuites);
            destination.LivingAreas.ShouldBe(source.LivingAreas);
            AssertStringCollection(destination.Tags, source.Tags);
            destination.Toilets.ShouldBe(source.Toilets);
            destination.ModifiedData.IsModified.ShouldBe(true);
        }

        public static void AssertMedia(Media destination, Media source)
        {
            if (destination == null &&
                source == null)
            {
                return;
            }

            destination.Order.ShouldBe(source.Order);
            destination.Tag.ShouldBe(source.Tag);
            destination.Url.ShouldBe(source.Url);
            destination.ModifiedData.IsModified.ShouldBe(true);
        }

        public static void AssertMedias(IList<Media> destination, IList<Media> source)
        {
            if (destination != null &&
                source != null)
            {
                destination.Count.ShouldBe(source.Count);
                for (var i = 0; i < destination.Count; i++)
                {
                    AssertMedia(destination[i], source[i]);
                }
            }
        }

        public static void AssertInspection(Inspection destination, Inspection source)
        {
            if (destination == null &&
                source == null)
            {
                return;
            }

            destination.ClosesOn.ShouldBe(source.ClosesOn);
            destination.OpensOn.ShouldBe(source.OpensOn);
            destination.ModifiedData.IsModified.ShouldBe(true);
        }

        public static void AssertInspections(IList<Inspection> destination, IList<Inspection> source)
        {
            if (destination == null &&
                source == null)
            {
                return;
            }

            destination.Count.ShouldBe(source.Count);
            for (var i = 0; i < destination.Count; i++)
            {
                AssertInspection(destination[i], source[i]);
            }
        }

        public static void AssertUnitOfMeasure(UnitOfMeasure destination, UnitOfMeasure source)
        {
            if (destination == null &&
                source == null)
            {
                return;
            }
            
            destination.Type.ShouldBe(source.Type);
            destination.Value.ShouldBe(source.Value);
            destination.ModifiedData.IsModified.ShouldBe(true);
        }

        public static void AssertDepth(Depth destination, Depth source)
        {
            if (destination == null &&
                source == null)
            {
                return;
            }

            destination.Side.ShouldBe(source.Side);
            AssertUnitOfMeasure(destination, source);
        }

        public static void AssertDepths(IList<Depth> destination, IList<Depth> source)
        {
            if (destination == null &&
                source == null)
            {
                return;
            }

            destination.Count.ShouldBe(source.Count);
            for (var i = 0; i < destination.Count; i++)
            {
                AssertDepth(destination[i], source[i]);
            }
        }

        public static void AssertLandDetails(LandDetails destination, LandDetails source)
        {
            if (destination == null &&
                source == null)
            {
                return;
            }

            AssertUnitOfMeasure(destination.Area, source.Area);
            AssertDepths(destination.Depths, source.Depths);
            destination.CrossOver.ShouldBe(source.CrossOver);
            AssertUnitOfMeasure(destination.Frontage, source.Frontage);
        }

        public static void AssertListing(Listing destination, Listing source)
        {
            if (destination == null &&
                source == null)
            {
                return;
            }

            AssertAggregateRoot(destination, source);
            AssertAddress(destination.Address, source.Address);
            AssertListingAgents(destination.Agents, source.Agents);
            destination.AgencyId.ShouldBe(source.AgencyId);
            destination.CreatedOn.ShouldBe(source.CreatedOn);
            destination.Description.ShouldBe(source.Description);
            AssertFeatures(destination.Features, source.Features);
            AssertMedias(destination.Images, source.Images);
            AssertMedias(destination.FloorPlans, source.FloorPlans);
            AssertInspections(destination.Inspections, source.Inspections);
            destination.StatusType.ShouldBe(source.StatusType);
            destination.Title.ShouldBe(source.Title);
            AssertStringCollection(destination.Links, source.Links);
            AssertMedias(destination.Videos, source.Videos);
            destination.IsModified.ShouldBe(true);
        }

        public static void AssertResidentialListing(ResidentialListing destination, ResidentialListing source)
        {
            if (destination == null &&
                source == null)
            {
                return;
            }

            AssertListing(destination, source);
            AssertBuildingDetails(destination.BuildingDetails, source.BuildingDetails);
            destination.CouncilRates.ShouldBe(source.CouncilRates);
            destination.PropertyType.ShouldBe(source.PropertyType);
            AssertSalePricing(destination.Pricing, source.Pricing);
            destination.IsModified.ShouldBe(true);
        }

        #endregion
    }
}