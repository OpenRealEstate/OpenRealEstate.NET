using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            bool includeDataForCustomDataFields = true,
            string fileName = "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current.xml")
        {
            fileName.ShouldNotBeNullOrWhiteSpace();

            var reaXml = File.ReadAllText(fileName);
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
                Id = "Residential-Current-ABCD1234",
                CouncilRates = "$2000 per month",
                AuctionOn = DateTime.Parse("2009-02-04T18:30:00"),
                PropertyType = PropertyType.Townhouse,
                Pricing = CreateSalePricing(),

                BuildingDetails = CreateBuildingDetails()
            };

            UpdateCommonListingData(listing, isClearAllIsModified);

            return listing;
        }

        public static RentalListing RentalListing(bool isClearAllIsModified = true)
        {
                var listing = new RentalListing
                {
                    Id = "Rental-Current-ABCD1234",
                    AvailableOn = new DateTime(2015, 5, 23),
                    Pricing = CreateRentalPricing(),
                    BuildingDetails = CreateBuildingDetails()
                };

            UpdateCommonListingData(listing, isClearAllIsModified);

            return listing;
        }

        public static LandListing LandListing(bool isClearAllIsModified = true)
        {
            var listing = new LandListing
            {
                Id = "Land-Current-ABCD1234",
                CategoryType = LandListingCategoryType.Residential,
                AuctionOn = new DateTime(2015, 5, 23),
                CouncilRates = "some council rates",
                Estate = CreateLandEstate(),
                Pricing = CreateSalePricing()
            };

            UpdateCommonListingData(listing, isClearAllIsModified);
            
            return listing;
        }

        public static RuralListing RuralListing(bool isClearAllIsModified = true)
        {
            var listing = new RuralListing
            {
                CategoryType = RuralListingCategoryType.Horticulture,
                AuctionOn = new DateTime(2015, 5, 23),
                CouncilRates = "some council rates",
                RuralFeatures = CreateRuralFeatures(),
                Pricing = CreateSalePricing(),
                BuildingDetails = CreateBuildingDetails()
            };
         
            UpdateCommonListingData(listing, isClearAllIsModified);

            return listing;
        }

        #region Fake data for listing parts

        private static void UpdateCommonListingData(Listing listing, bool isClearAllIsModified)
        {
            listing.ShouldNotBeNull();
            
            listing.AgencyId = "XNWXNW";
            listing.CreatedOn = DateTime.Parse("2009-01-01T12:30:00");
            listing.UpdatedOn = DateTime.Parse("2009-01-01T12:30:00");
            
            listing.Title = "SHOW STOPPER!!!";
            listing.Description =
                "Don't pass up an opportunity like this! First to inspect will buy! Close to local amenities and schools. Features lavishly appointed bathrooms, modern kitchen, rustic outhouse.Don't pass up an opportunity like this! First to inspect will buy! Close to local amenities and schools. Features lavishly appointed bathrooms, modern kitchen, rustic outhouse.";
            listing.StatusType = StatusType.Current;

            listing.Address = CreateAddress();
            listing.Features = CreateFeatures();
            listing.AddAgents(CreateAgents());
            listing.AddImages(CreateImages());
            listing.AddFloorPlans(CreateFloorplans());
            listing.AddInspections(CreateInspections());
            listing.AddLinks(CreateLinks());
            listing.AddVideos(CreateVideos());

            if (isClearAllIsModified)
            {
                listing.ClearAllIsModified();
            }

        }

        private static Address CreateAddress()
        {
            return new Address
            {
                CountryIsoCode = "AU",
                IsStreetDisplayed = true,
                Latitude = (decimal?)12.34,
                Longitude = (decimal?)56.78901,
                Municipality = "Yarra",
                Postcode = "3121",
                State = "vic",
                Street = "Main Road",
                StreetNumber = "2/39",
                Suburb = "RICHMOND"
            };
        }

        private static IList<ListingAgent> CreateAgents()
        {
            var communications1 = new List<Communication>
            {
                new Communication
                {
                    CommunicationType = CommunicationType.Email,
                    Details = "I.am.a.Princess@rebel.alliance"
                },
                new Communication
                {
                    CommunicationType = CommunicationType.Mobile,
                    Details = "0418 123 456"
                },
                new Communication
                {
                    CommunicationType = CommunicationType.Landline,
                    Details = "05 1234 5678"
                }
            };
            var agent1 = new ListingAgent
            {
                Name = "Princess Leia",
                Order = 1
            };
            agent1.AddCommunications(communications1);

            var communications2 = new List<Communication>
            {
                new Communication
                {
                    CommunicationType = CommunicationType.Email,
                    Details = "scoundrel@rebel.alliance"
                },
                new Communication
                {
                    CommunicationType = CommunicationType.Mobile,
                    Details = "0418 555 666"
                },
                new Communication
                {
                    CommunicationType = CommunicationType.Landline,
                    Details = "05 2222 3333"
                }
            };
            var agent2 = new ListingAgent
            {
                Name = "Han Solo",
                Order = 2
            };
            agent2.AddCommunications(communications2);

            return new List<ListingAgent>
            {
                agent1,
                agent2
            };
        }

        private static BuildingDetails CreateBuildingDetails()
        {
            return new BuildingDetails
            {
                Area = new UnitOfMeasure {Type = "square", Value = 40},
                EnergyRating = 4.5m
            };
        }

        private static Features CreateFeatures()
        {
            var tags = new List<string>
            {
                "heating-other",
                "hotWaterService-gas",
                "poolinground",
                "spainground",
                "balcony",
                "courtyard",
                "shed",
                "tennisCourt",
                "secureParking",
                "remoteGarage",
                "pool",
                "outdoorEnt",
                "openFirePlace",
                "fullyFenced",
                "deck",
                "alarmSystem",
                "airConditioning",
                "solarPanels",
                "waterTank",
                "houseAndLandPackage",
                "isANewConstruction"
            };
            return new Features
            {
                CarParking = new CarParking
                {
                    Carports = 1,
                    Garages = 2,
                    OpenSpaces = 3,
                },
                Bedrooms = 3,
                Bathrooms = 2,
                Ensuites = 1,
                LivingAreas = 4,
                Toilets = 5,
                Tags = new ReadOnlyCollection<string>(tags)
            };
        }

        private static SalePricing CreateSalePricing()
        {
            return new SalePricing
            {
                IsUnderOffer = true,
                SalePrice = 500000,
                SalePriceText = "Between $400,000 and $600,000",
                SoldOn = DateTime.Parse("2010-02-03T14:30:00"),
                SoldPrice = 45432.99m,
                SoldPriceText = "Nice Sale!"
            };
        }

        private static IList<Media> CreateImages()
        {
            return new List<Media>
            {
                CreateMedia("http://www.photos.com.au/tmp/imageM.jpg", 1),
                CreateMedia("http://www.photos.com.au/tmp/imageA.jpg", 2)
            };
        }

        private static IList<Media> CreateFloorplans()
        {
            return new List<Media>
            {
                CreateMedia("http://www.photos.com.au/tmp/floorplan1.gif", 1),
                CreateMedia("http://www.photos.com.au/tmp/floorplan2.gif", 2)
            };
        }

        private static IList<Media> CreateVideos()
        {
            return new List<Media>
            {
                CreateMedia("http://www.foo.tv/abcd.html", 1),
                CreateMedia("http://www.foo.tv/qqqq.html", 2)
            };
        } 

        private static Media CreateMedia(string url, int order)
        {
            return new Media
            {
                CreatedOn = DateTime.Parse("2009-01-01T12:30:00"),
                Url = url,
                Order = order,
                Tag = "aaa bbb"
            };
        }

        private static IList<Inspection> CreateInspections()
        {
            return new List<Inspection>
            {
                new Inspection
                {
                    OpensOn = DateTime.Parse("2009-01-21T11:00:00"),
                    ClosesOn = DateTime.Parse("2009-01-21T13:00:00")
                },
                new Inspection
                {
                    OpensOn = DateTime.Parse("2009-01-22T15:00:00"),
                    ClosesOn = DateTime.Parse("2009-01-22T15:30:00")
                }
            };
        }

        private static IList<string> CreateLinks()
        {
            return new[]
            {
                "http://www.au.open2view.com/properties/314244/tour#floorplan",
                "http://www.google.com/hello"
            };
        }

        private static RentalPricing CreateRentalPricing()
        {
            return new RentalPricing
            {
                RentalPrice = 567.88m,
                RentalPriceText = "house for rent",
                Bond = 1000m,
                PaymentFrequencyType = PaymentFrequencyType.Monthly
            };
        }

        private static LandEstate CreateLandEstate()
        {
            return new LandEstate
            {
                Name = "some land estate",
                Stage = "1st stage"
            };
        }

        private static RuralFeatures CreateRuralFeatures()
        {
            return new RuralFeatures
            {
                AnnualRainfall = "some rain",
                CarryingCapacity = "some carrying capacity",
                Fencing = "some fencing",
                Improvements = "lots of improvements",
                Irrigation = "some irrigation",
                Services = "a number of services",
                SoilTypes = "dirty soil"
            };
        }

        #endregion

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
            destination.ModifiedData.IsModified.ShouldBe(true);
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

        public static void AssertRentalPricing(RentalPricing destination, RentalPricing source)
        {
            if (destination == null &&
                source == null)
            {
                return;
            }

            destination.Bond.ShouldBe(source.Bond);
            destination.PaymentFrequencyType.ShouldBe(source.PaymentFrequencyType);
            destination.RentalPrice.ShouldBe(source.RentalPrice);
            destination.RentalPriceText.ShouldBe(source.RentalPriceText);
            destination.ModifiedData.IsModified.ShouldBe(true);
        }

        public static void AssertLandEstate(LandEstate destination, LandEstate source)
        {
            if (destination == null &&
                   source == null)
            {
                return;
            }

            destination.Name.ShouldBe(source.Name);
            destination.Stage.ShouldBe(source.Stage);
            destination.ModifiedData.IsModified.ShouldBe(true);
        }

        public static void AssertRuralFeatures(RuralFeatures destination, RuralFeatures source)
        {
            if (destination == null &&
                   source == null)
            {
                return;
            }

            destination.AnnualRainfall.ShouldBe(source.AnnualRainfall);
            destination.CarryingCapacity.ShouldBe(source.CarryingCapacity);
            destination.Fencing.ShouldBe(source.Fencing);
            destination.Improvements.ShouldBe(source.Improvements);
            destination.Irrigation.ShouldBe(source.Irrigation);
            destination.Services.ShouldBe(source.Services);
            destination.SoilTypes.ShouldBe(source.SoilTypes);
            destination.ModifiedData.IsModified.ShouldBe(true);
        }

        public static void AssertListing(Listing destination, Listing source)
        {
            if (destination == null &&
                source == null)
            {
                return;
            }

            AssertAggregateRoot(destination, source);

            destination.AgencyId.ShouldBe(source.AgencyId);
            destination.CreatedOn.ShouldBe(source.CreatedOn);
            destination.Description.ShouldBe(source.Description);
            destination.StatusType.ShouldBe(source.StatusType);
            destination.Title.ShouldBe(source.Title);

            AssertAddress(destination.Address, source.Address);
            AssertListingAgents(destination.Agents, source.Agents);
            AssertFeatures(destination.Features, source.Features);
            AssertMedias(destination.Images, source.Images);
            AssertMedias(destination.FloorPlans, source.FloorPlans);
            AssertInspections(destination.Inspections, source.Inspections);
            AssertStringCollection(destination.Links, source.Links);
            AssertMedias(destination.Videos, source.Videos);
            destination.ModifiedData.IsModified.ShouldBe(true);
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
            destination.AuctionOn.ShouldBe(source.AuctionOn);
            destination.CouncilRates.ShouldBe(source.CouncilRates);
            destination.PropertyType.ShouldBe(source.PropertyType);
            AssertSalePricing(destination.Pricing, source.Pricing);
            destination.ModifiedData.IsModified.ShouldBe(true);
        }

        public static void AssertRentalListing(RentalListing destination, RentalListing source)
        {
            if (destination == null &&
                   source == null)
            {
                return;
            }

            AssertListing(destination, source);
            destination.AvailableOn.ShouldBe(source.AvailableOn);
            AssertBuildingDetails(destination.BuildingDetails, source.BuildingDetails);
            destination.PropertyType.ShouldBe(source.PropertyType);
            AssertRentalPricing(destination.Pricing, source.Pricing);
            destination.ModifiedData.IsModified.ShouldBe(true);
        }

        public static void AssertLandListing(LandListing destination, LandListing source)
        {
            if (destination == null &&
                    source == null)
            {
                return;
            }

            AssertListing(destination, source);
            destination.AuctionOn.ShouldBe(source.AuctionOn);
            destination.CategoryType.ShouldBe(source.CategoryType);
            AssertLandEstate(destination.Estate, source.Estate);
            AssertSalePricing(destination.Pricing, source.Pricing);
            destination.ModifiedData.IsModified.ShouldBe(true);
        }

        public static void AssertRuralListing(RuralListing destination, RuralListing source)
        {
            if (destination == null &&
                    source == null)
            {
                return;
            }

            AssertListing(destination, source);
            destination.AuctionOn.ShouldBe(source.AuctionOn);
            AssertBuildingDetails(destination.BuildingDetails, source.BuildingDetails);
            destination.CategoryType.ShouldBe(source.CategoryType);
            destination.CouncilRates.ShouldBe(source.CouncilRates);
            AssertSalePricing(destination.Pricing, source.Pricing);
            AssertRuralFeatures(destination.RuralFeatures, source.RuralFeatures);
        }

        public static void AssertUnitOfMeasureIsModified(UnitOfMeasure unitOfMeasure, bool isModified)
        {
            unitOfMeasure.ModifiedData.IsModified.ShouldBe(isModified);
        }

        public static void AssertDepthIsModified(Depth depth, bool isModified)
        {
            depth.ModifiedData.IsModified.ShouldBe(isModified);
        }

        public static void AssertDepthsAreModified(IList<Depth> depths, bool isModified)
        {
            if (depths == null ||
                !depths.Any())
            {
                return;
            }

            foreach (var depth in depths)
            {
                AssertDepthIsModified(depth, isModified);
            }
        }

        public static void AssertBuildingDetailsIsModified(BuildingDetails buildingDetails, bool isModified)
        {
            AssertUnitOfMeasureIsModified(buildingDetails.Area, isModified);
            buildingDetails.ModifiedData.IsModified.ShouldBe(isModified);
        }

        public static void AssertSalePricingIsModified(SalePricing salePricing, bool isModified)
        {
            salePricing.ModifiedData.IsModified.ShouldBe(isModified);
        }

        public static void AssertAddressIsModified(Address address, bool isModified)
        {
            address.ModifiedData.IsModified.ShouldBe(isModified);
        }

        public static void AssertListingAgentIsModified(ListingAgent listingAgent, bool isModified)
        {
            listingAgent.ModifiedData.IsModified.ShouldBe(isModified);
        }

        public static void AssertListingAgentsAreModified(IList<ListingAgent> listingAgents, bool isModified)
        {
            if (listingAgents == null ||
                !listingAgents.Any())
            {
                return;
            }

            foreach (var listingAgent in listingAgents)
            {
                AssertListingAgentIsModified(listingAgent, isModified);
            }
        }

        public static void AssertCarParkingIsModified(CarParking carParking, bool isModified)
        {
            carParking.ModifiedData.IsModified.ShouldBe(isModified);
        }

        public static void AssertFeaturesIsModified(Features features, bool isModified)
        {
            AssertCarParkingIsModified(features.CarParking, isModified);
            features.ModifiedData.IsModified.ShouldBe(isModified);
        }

        public static void AssertLandDetailsIsModified(LandDetails landDetails, bool isModified)
        {
            AssertUnitOfMeasureIsModified(landDetails.Area, isModified);
            AssertDepthsAreModified(landDetails.Depths, isModified);
            AssertUnitOfMeasureIsModified(landDetails.Frontage, isModified);
            landDetails.ModifiedData.IsModified.ShouldBe(isModified);
        }

        public static void AssertInspectionIsModified(Inspection inspection, bool isModified)
        {
            inspection.ModifiedData.IsModified.ShouldBe(isModified);
        }

        public static void AssertInspectionsAreModified(IList<Inspection> inspections, bool isModified)
        {
            if (inspections == null ||
                !inspections.Any())
            {
                return;
            }

            foreach (var inspection in inspections)
            {
                AssertInspectionIsModified(inspection, isModified);
            }
        }

        public static void AssertMediaIsModified(Media media, bool isModified)
        {
            media.ModifiedData.IsModified.ShouldBe(isModified);
        }

        public static void AssertMediasAreModified(IList<Media> medias, bool isModified)
        {
            if (medias == null ||
                !medias.Any())
            {
                return;
            }

            foreach (var media in medias)
            {
                AssertMediaIsModified(media, isModified);
            }
        }


        public static void AssertListingIsModified(Listing listing, bool isModified)
        {
            AssertAddressIsModified(listing.Address, isModified);
            AssertListingAgentsAreModified(listing.Agents, isModified);
            AssertFeaturesIsModified(listing.Features, isModified);
            AssertMediasAreModified(listing.FloorPlans, isModified);
            AssertMediasAreModified(listing.Images, isModified);
            AssertInspectionsAreModified(listing.Inspections, isModified); 
            AssertLandDetailsIsModified(listing.LandDetails, isModified);
            AssertMediasAreModified(listing.Videos, isModified);
            listing.ModifiedData.IsModified.ShouldBe(isModified);
        }

        public static void AssertResidentialListingIsModified(ResidentialListing residentialListing, bool isModified)
        {
            AssertBuildingDetailsIsModified(residentialListing.BuildingDetails, isModified);
            AssertSalePricingIsModified(residentialListing.Pricing, isModified);
            AssertListingIsModified(residentialListing, isModified);
            residentialListing.ModifiedData.IsModified.ShouldBe(isModified);
        }

        #endregion
    }
}