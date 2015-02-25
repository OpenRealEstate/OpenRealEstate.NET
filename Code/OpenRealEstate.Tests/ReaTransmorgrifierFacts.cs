using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentValidation.Results;
using OpenRealEstate.Core.Filters;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Land;
using OpenRealEstate.Core.Models.Rental;
using OpenRealEstate.Core.Models.Residential;
using OpenRealEstate.Core.Models.Rural;
using OpenRealEstate.Services.RealEstateComAu;
using OpenRealEstate.Validation.Residential;
using Shouldly;
using Xunit;
using CategoryType = OpenRealEstate.Core.Models.Rural.CategoryType;
using LandCategoryType = OpenRealEstate.Core.Models.Land.CategoryType;

namespace OpenRealEstate.Tests
{
    public class ReaXmlTransmorgrifierFacts
    {
        public class ConvertFacts
        {
            #region Residential

            [Fact]
            public void GivenTheFileREAResidentialCurrent_Convert_ReturnsAResidentialCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);
                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    tags: new[] { "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other", "balcony", "shed", "courtyard", "isANewConstruction" });
            }

            [Fact]
            public void GivenTheFileREAResidentialSold_Convert_ReturnsAResidentialSoldListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Sold.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertResidentialSoldListing(result.Listings.First().Listing as ResidentialListing);
            }

            [Fact]
            public void GivenTheFileREAResidentialSoldWithMissingDisplayPrice_Convert_ReturnsAResidentialSoldListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Sold-MissingDisplayPrice.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertResidentialSoldListing(result.Listings.First().Listing as ResidentialListing);
            }

            [Fact]
            public void GivenTheFileREAResidentialSoldWithDisplayPriceIsNo_Convert_ReturnsAResidentialSoldListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Sold-DisplayPriceIsNo.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertResidentialSoldListing(result.Listings.First().Listing as ResidentialListing, false);
            }

            [Fact]
            public void GivenTheFileREAResidentialWithdawn_Convert_ReturnsAResidentialWithdawnListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Withdrawn.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);

                var listing = result.Listings.First().Listing;
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Residential-Withdrawn-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Withdrawn);
            }

            [Fact]
            public void GivenTheFileREAResidentialOffMarket_Convert_ReturnsAResidentialOffMarketListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-OffMarket.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();
                
                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);

                var listing = result.Listings.First().Listing;
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Residential-OffMarket-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.OffMarket);
            }

            [Fact]
            public void GivenTheFileREASegmentResidentialCurrent_Convert_ReturnsAResidentialCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Segment-Residential-Current.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing);
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentBadInspectionTime_Convert_ReturnsSomeInvalidData()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-CurrentBadInspectionTime.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                 var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.ShouldBe(null);
                result.UnhandledData.ShouldBe(null);
                result.Errors.Count.ShouldBe(1);
                result.Errors[0].ExceptionMessage.ShouldBe("Inspection element has an invald Date/Time value. Element: <inspection> 12:00AM to 12:00AM</inspection>");
                result.Errors[0].InvalidData.ShouldNotBeNullOrEmpty();
            }

            [Fact]
            public void GivenTheFileReaResidentialCurrentBadSalePrice_Convert_ReturnsSomeInvalidData()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-BadSalePrice.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.ShouldBe(null);
                result.UnhandledData.ShouldBe(null);
                result.Errors[0].ExceptionMessage.ShouldBe("Failed to parse the value '550000600000550000600000550000600000' into a decimal.");
                result.Errors[0].InvalidData.ShouldNotBeNullOrEmpty();
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentBedroomIsStudio_Convert_ReturnsAResidentialCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-BedroomIsStudio.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    PropertyType.Studio,
                    0);
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentWithAllFeatures_Convert_ReturnsAResidentialCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithAllFeatures.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    PropertyType.House,
                    4,
                    XmlFeatureHelpers.OtherFeatureNames);
            }

            [Fact]
            public void GivenTheFileREAResidentialSold_DisplayAttributeIsRange_Convert_ReturnsAnInvalidListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Sold-DisplayAttributeIsRange.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.ShouldBe(null);
                result.UnhandledData.ShouldBe(null);
                result.Errors.Count.ShouldBe(1);
                result.Errors.First().ExceptionMessage.ShouldBe("Value 'range' is out of range. It should only be 0/1/yes/no.\r\nParameter name: value");
                result.Errors.First().InvalidData.ShouldStartWith("<residential modTime=\"2009-01-01-12:30:00\" status=\"sold\">");
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentWithPriceAndDisplayYesButNoPriceView_Convert_ReturnsAListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithPriceAndDisplayYesButNoPriceView.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);

                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    tags: new[] { "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other", "balcony", "shed", "courtyard", "isANewConstruction" },
                    salePriceText: "$500,000");
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentWithPriceAndDisplayNoAndNoPriceView_Convert_ReturnsAListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithPriceAndDisplayNoAndNoPriceView.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);

                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    tags: new[] { "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other", "balcony", "shed", "courtyard", "isANewConstruction" },
                    salePriceText: null);
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentWithPriceAndDisplayNoAndAPriceView_Convert_ReturnsAListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithPriceAndDisplayNoAndAPriceView.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);

                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    tags: new[] { "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other", "balcony", "shed", "courtyard", "isANewConstruction" },
                    salePriceText: null);
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentWithLocalFilesForImages_Convert_ReturnsAListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithLocalFilesForImages.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);

                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    tags: new[] { "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other", "balcony", "shed", "courtyard", "isANewConstruction" },
                    images: new[] { "imageM.jpg", "imageA.jpg" },
                    floorplans: new[] { "floorplan1.gif", "floorplan2.gif" });
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentWithNoStreetNumberButASubNumber_Convert_ReturnsAResidentialCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithNoStreetNumberButASubNumber.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);
                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    tags: new[] { "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other", "balcony", "shed", "courtyard", "isANewConstruction" },
                    streetNumber: "2/77a");
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentWithAStreetNumberAndASubNumber_Convert_ReturnsAResidentialCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithAStreetNumberAndASubNumber.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);
                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    tags: new[] { "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other", "balcony", "shed", "courtyard", "isANewConstruction" },
                    streetNumber: "2/77a 39");
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentWithAStreetNumberAndASingleSubNumber_Convert_ReturnsAResidentialCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithAStreetNumberAndASingleSubNumber.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);
                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    tags: new[] { "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other", "balcony", "shed", "courtyard", "isANewConstruction" });
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentWithAStreetNumberAndASingleSubNumberWithACustomDelimeter_Convert_ReturnsAResidentialCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithAStreetNumberAndASingleSubNumber.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier
                {
                    AddressDelimeter = "-"
                };

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);
                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    tags: new[] { "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other", "balcony", "shed", "courtyard", "isANewConstruction" },
                    streetNumber: "2-39");
            }

            private static void AssertResidentialCurrentListing(ResidentialListing listing,
                PropertyType expectedPropertyType = PropertyType.House,
                int expectedBedroomsCount = 4,
                IList<string> tags = null,
                string salePriceText = "Between $400,000 and $600,000",
                IList<string> images = null,
                IList<string> floorplans = null,
                string streetNumber = "2/39")
            {
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Residential-Current-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Current);
                listing.PropertyType.ShouldBe(expectedPropertyType);
                listing.CouncilRates.ShouldBe("$2000 per month");
                listing.Links[0].ShouldBe("http://www.au.open2view.com/properties/314244/tour#floorplan");
                listing.Links[1].ShouldBe("http://www.google.com/hello");

                listing.Address.IsStreetDisplayed.ShouldBe(true);
                listing.Address.StreetNumber.ShouldBe(streetNumber);
                listing.Address.Street.ShouldBe("Main Road");
                listing.Address.Suburb.ShouldBe("RICHMOND");
                listing.Address.Municipality.ShouldBe("Yarra");
                listing.Address.State.ShouldBe("vic");
                listing.Address.CountryIsoCode.ShouldBe("AU");
                listing.Address.Postcode.ShouldBe("3121");

                listing.Pricing.SalePrice.ShouldBe(500000m);
                listing.Pricing.SalePriceText.ShouldBe(salePriceText);
                listing.Pricing.IsUnderOffer.ShouldBe(false);

                listing.Inspections.Count.ShouldBe(2);
                listing.Inspections.First().OpensOn.ShouldBe(new DateTime(2009, 1, 21, 11, 00, 0));
                listing.Inspections.First().ClosesOn.ShouldBe(new DateTime(2009, 1, 21, 13, 00, 0));

                listing.Agents.Count.ShouldBe(1);
                var listingAgent = listing.Agents[0];
                listingAgent.Name.ShouldBe("Mr. John Doe");
                listingAgent.Order.ShouldBe(1);
                listingAgent.Communications[0].CommunicationType.ShouldBe(CommunicationType.Email);
                listingAgent.Communications[0].Details.ShouldBe("jdoe@somedomain.com.au");
                listingAgent.Communications[1].CommunicationType.ShouldBe(CommunicationType.Mobile);
                listingAgent.Communications[1].Details.ShouldBe("0418 123 456");
                listingAgent.Communications[2].CommunicationType.ShouldBe(CommunicationType.Landline);
                listingAgent.Communications[2].Details.ShouldBe("05 1234 5678");

                listing.Features.Bedrooms.ShouldBe(expectedBedroomsCount);
                listing.Features.Bathrooms.ShouldBe(2);
                listing.Features.Garages.ShouldBe(3);
                listing.Features.Carports.ShouldBe(2);
                listing.Features.Ensuites.ShouldBe(2);
                listing.Features.Toilets.ShouldBe(0);
                listing.Features.LivingAreas.ShouldBe(0);
                listing.Features.OpenSpaces.ShouldBe(0);
                if (tags != null)
                {
                    AssertFeatures(listing.Features, tags);
                }
                

                listing.Images.Count.ShouldBe(2);
                listing.Images[0].Order.ShouldBe(1);
                listing.Images[0].Url.ShouldBe(images == null
                    ? "http://www.realestate.com.au/tmp/imageM.jpg"
                    : images[0]);
                listing.Images[1].Order.ShouldBe(2);
                listing.Images[1].Url.ShouldBe(images == null
                    ? "http://www.realestate.com.au/tmp/imageA.jpg"
                    : images[1]);

                listing.FloorPlans.Count.ShouldBe(2);
                listing.FloorPlans[0].Url.ShouldBe(floorplans == null
                    ? "http://www.realestate.com.au/tmp/floorplan1.gif"
                    : floorplans[0]);
                listing.FloorPlans[0].Order.ShouldBe(1);
                listing.FloorPlans[1].Url.ShouldBe(floorplans == null
                    ? "http://www.realestate.com.au/tmp/floorplan2.gif"
                    : floorplans[1]);
                listing.FloorPlans[0].Order.ShouldBe(1);

                listing.AuctionOn.ShouldBe(new DateTime(2009, 02, 04, 18, 30, 00));

                listing.BuildingDetails.Area.Value.ShouldBe(40);
                listing.BuildingDetails.Area.Type.ShouldBe("square");
                listing.BuildingDetails.EnergyRating.HasValue.ShouldBe(true);
                listing.BuildingDetails.EnergyRating.ShouldBe(4.5m);

                listing.LandDetails.Area.Value.ShouldBe(80M);
                listing.LandDetails.Area.Type.ShouldBe("square");
                listing.LandDetails.Frontage.Value.ShouldBe(20M);
                listing.LandDetails.Frontage.Type.ShouldBe("meter");
                listing.LandDetails.Depths[0].Value.ShouldBe(40M);
                listing.LandDetails.Depths[0].Type.ShouldBe("meter");
                listing.LandDetails.Depths[0].Side.ShouldBe("rear");
                listing.LandDetails.Depths[1].Value.ShouldBe(60M);
                listing.LandDetails.Depths[1].Type.ShouldBe("meter");
                listing.LandDetails.Depths[1].Side.ShouldBe("left");
                listing.LandDetails.Depths[2].Value.ShouldBe(20M);
                listing.LandDetails.Depths[2].Type.ShouldBe("meter");
                listing.LandDetails.Depths[2].Side.ShouldBe("right");
            }

            private static void AssertResidentialSoldListing(ResidentialListing listing,
                bool isSoldPriceVisibile = true)
            {
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Residential-Sold-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Sold);

                decimal? soldPrice = 580000m;
                listing.Pricing.SoldPrice.ShouldBe(soldPrice);
                listing.Pricing.SoldPriceText.ShouldBe(isSoldPriceVisibile
                    ? soldPrice.Value.ToString("C0")
                    : null);
                listing.Pricing.SoldOn.ShouldBe(new DateTime(2009, 01, 10, 12, 30, 00));
            }

            #endregion

            #region Rental

            [Fact]
            public void GivenTheFileREARentalCurrent_Convert_ReturnsARentalCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rental\\REA-Rental-Current.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertRentalCurrentListing(result.Listings.First().Listing as RentalListing,
                    new[] { "hotWaterService-gas", "heating-other", "balcony", "shed", "courtyard", "isANewConstruction", "fullyFenced", "outdoorEnt", "courtyard", "deck", "tennisCourt" });
            }

            [Fact]
            public void GivenTheFileREARentalLeased_Convert_ReturnsALeasedListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rental\\REA-Rental-Leased.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                var listing = result.Listings.First().Listing;
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Rental-Leased-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Leased);
            }

            [Fact]
            public void GivenTheFileREARentalWithdrawn_Convert_ReturnsARentalWithdrawnListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rental\\REA-Rental-Withdrawn.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                var listing = result.Listings.First().Listing;
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Rental-Withdrawn-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Withdrawn);
            }

            [Fact]
            public void GivenTheFileREARentalOffMarket_Convert_ReturnsARentalOffMarketListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rental\\REA-Rental-OffMarket.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                var listing = result.Listings.First().Listing;
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Rental-OffMarket-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.OffMarket);
            }

            [Fact]
            public void GivenTheFileREASegmentRentalCurrent_Convert_ReturnsARentalCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rental\\REA-Segment-Rental-Current.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertRentalCurrentListing(result.Listings.First().Listing as RentalListing);
            }

            [Fact]
            public void GivenTheFileREARentalCurrentWithAnAddressLatLong_Convert_ReturnsARentalCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rental\\REA-Rental-Current-AddressLatLong.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertRentalCurrentListing(result.Listings.First().Listing as RentalListing,
                    new[] { "hotWaterService-gas", "heating-other", "balcony", "shed", "courtyard", "isANewConstruction", "fullyFenced", "outdoorEnt", "courtyard", "deck", "tennisCourt" },
                    12.345m,
                    -54.678m);
            }

            [Fact]
            public void GivenTheFileREARentalCurrentWithARayWhiteLatLong_Convert_ReturnsARentalCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rental\\REA-Rental-Current-RayWhiteLatLong.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertRentalCurrentListing(result.Listings.First().Listing as RentalListing,
                    new[] { "hotWaterService-gas", "heating-other", "balcony", "shed", "courtyard", "isANewConstruction", "fullyFenced", "outdoorEnt", "courtyard", "deck", "tennisCourt" },
                    -12.422818m,
                    130.867747m);
            }

            private static void AssertRentalCurrentListing(RentalListing listing,
                IList<string> tags = null,
                decimal? latitude = null,
                decimal? longitude = null)
            {
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Rental-Current-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Current);
                listing.PropertyType.ShouldBe(PropertyType.House);

                listing.Address.IsStreetDisplayed.ShouldBe(true);
                listing.Address.StreetNumber.ShouldBe("39");
                listing.Address.Street.ShouldBe("Main Road");
                listing.Address.Suburb.ShouldBe("RICHMOND");
                listing.Address.State.ShouldBe("vic");
                listing.Address.CountryIsoCode.ShouldBe("AU");
                listing.Address.Postcode.ShouldBe("3121");
                listing.Address.Latitude.ShouldBe(latitude.HasValue ? latitude.Value : (decimal?)null);
                listing.Address.Longitude.ShouldBe(longitude.HasValue ? longitude.Value : (decimal?)null);

                if (latitude.HasValue)
                {
                    
                }
                if (longitude.HasValue)
                {
                    
                }

                listing.AvailableOn.ShouldBe(new DateTime(2009, 01, 26, 12, 30, 00));

                listing.Pricing.RentalPrice.ShouldBe(350);
                listing.Pricing.RentalPriceText.ShouldBe("350");
                listing.Pricing.PaymentFrequencyType.ShouldBe(PaymentFrequencyType.Weekly);
                listing.Pricing.Bond.ShouldBe(999);

                listing.Inspections.Count.ShouldBe(2);

                listing.Agents.Count.ShouldBe(1);
                var listingAgent = listing.Agents[0];
                listingAgent.Name.ShouldBe("Mr. John Doe");
                listingAgent.Order.ShouldBe(1);
                listingAgent.Communications[0].CommunicationType.ShouldBe(CommunicationType.Email);
                listingAgent.Communications[0].Details.ShouldBe("jdoe@somedomain.com.au");
                listingAgent.Communications[1].CommunicationType.ShouldBe(CommunicationType.Landline);
                listingAgent.Communications[1].Details.ShouldBe("05 1234 5678");

                listing.Features.Bedrooms.ShouldBe(4);
                listing.Features.Bathrooms.ShouldBe(2);
                listing.Features.Garages.ShouldBe(3);
                listing.Features.Carports.ShouldBe(2);
                listing.Features.Ensuites.ShouldBe(2);
                listing.Features.Toilets.ShouldBe(0);
                listing.Features.LivingAreas.ShouldBe(0);
                listing.Features.OpenSpaces.ShouldBe(0);
                if (tags != null)
                {
                    AssertFeatures(listing.Features, tags);
                }

                listing.Images.Count.ShouldBe(2);
                listing.Images[0].Order.ShouldBe(1);
                listing.Images[0].Url.ShouldBe("http://www.realestate.com.au/tmp/imageM.jpg");

                listing.LandDetails.Area.Value.ShouldBe(60M);
                listing.LandDetails.Area.Type.ShouldBe("square");
                listing.LandDetails.Frontage.Value.ShouldBe(15M);
                listing.LandDetails.Frontage.Type.ShouldBe("meter");
                listing.LandDetails.Depths[0].Value.ShouldBe(40M);
                listing.LandDetails.Depths[0].Type.ShouldBe("meter");
                listing.LandDetails.Depths[0].Side.ShouldBe("rear");

                listing.FloorPlans.Count.ShouldBe(2);
            }

            #endregion

            #region Land

            [Fact]
            public void GivenTheFileREALandCurrent_Convert_ReturnsALandCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-Current.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertLandCurrentListing(result.Listings.First().Listing as LandListing,
                    tags: new[] { "fullyFenced" });
            }

            [Fact]
            public void GivenTheFileREALandCurrentIncompleteLandDetails_Convert_ReturnsALandCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-Current-IncompleteLandDetails.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);

                var listing = result.Listings.First().Listing;
                listing.LandDetails.Area.ShouldBe(null);
                listing.LandDetails.Frontage.ShouldBe(null);
                listing.LandDetails.Depths.ShouldBe(null);
                listing.LandDetails.CrossOver.ShouldBeNullOrEmpty();
            }

            [Fact]
            public void GivenTheFileREALandSold_Convert_ReturnsALandSoldListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-Sold.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();
                
                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertLandSoldListing(result.Listings.First().Listing as LandListing);
            }

            [Fact]
            public void GivenTheFileREALandSoldDisplayPriceisNo_Convert_ReturnsALandSoldListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-Sold-DisplayPriceisNo.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertLandSoldListing(result.Listings.First().Listing as LandListing, false);
            }

            [Fact]
            public void GivenTheFileREALandWithdawn_Convert_ReturnsALandWithdawnListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-Withdrawn.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);

                var listing = result.Listings.First().Listing;
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Land-Withdrawn-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Withdrawn);
            }

            [Fact]
            public void GivenTheFileREALandOffMarket_Convert_ReturnsALandOffMarketListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-OffMarket.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);

                var listing = result.Listings.First().Listing;
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Land-OffMarket-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.OffMarket);
            }

            [Fact]
            public void GivenTheFileReaLandCurrentMissingLandCategory_Convert_ReturnsALandCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-Current-MissingLandCategory.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertLandCurrentListing(result.Listings.First().Listing as LandListing, LandCategoryType.Unknown);
            }

            [Fact]
            public void GivenTheFileREALandCurrentWithASubNumberButNoStreetNumber_Convert_ReturnsALandCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-Current-WithASubNumberButNoStreetNumber.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertLandCurrentListing(result.Listings.First().Listing as LandListing,
                    tags: new[] { "fullyFenced" },
                    streetNumber: "12");
            }

            private static void AssertLandCurrentListing(LandListing listing,
                LandCategoryType landCategoryType = LandCategoryType.Residential,
                IList<string> tags = null,
                string streetNumber = "LOT 12/39")
            {
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Land-Current-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Current);
                listing.CategoryType.ShouldBe(landCategoryType);

                listing.Agents.Count.ShouldBe(1);
                listing.Agents[0].Name.ShouldBe("Mr. John Doe");
                listing.Agents[0].Communications.Count.ShouldBe(3);

                listing.Address.StreetNumber.ShouldBe(streetNumber);
                listing.Address.Street.ShouldBe("Main Road");
                listing.Address.Suburb.ShouldBe("RICHMOND");
                listing.Address.IsStreetDisplayed.ShouldBe(true);

                listing.Pricing.IsUnderOffer.ShouldBe(false);
                listing.Pricing.SalePrice.ShouldBe(80000);
                listing.Pricing.SalePriceText.ShouldBe("To suit buyers 60K+");

                listing.Estate.Name.ShouldBe("Panorama");
                listing.Estate.Stage.ShouldBe("5");

                listing.LandDetails.Area.Type.ShouldBe("square");
                listing.LandDetails.Area.Value.ShouldBe(60m);
                listing.LandDetails.Frontage.Type.ShouldBe("meter");
                listing.LandDetails.Frontage.Value.ShouldBe(20m);
                listing.LandDetails.Depths[0].Type.ShouldBe("meter");
                listing.LandDetails.Depths[0].Value.ShouldBe(30m);
                listing.LandDetails.Depths[0].Side.ShouldBe("rear");
                listing.LandDetails.CrossOver.ShouldBe("left");

                listing.Images.Count.ShouldBe(2);
                listing.Images[0].Order.ShouldBe(1);
                listing.Images[0].Url.ShouldBe("http://www.realestate.com.au/tmp/imageM.jpg");

                listing.FloorPlans.Count.ShouldBe(2);
                listing.FloorPlans[0].Order.ShouldBe(1);
                listing.FloorPlans[0].Url.ShouldBe("http://www.realestate.com.au/tmp/floorplan1.gif");

                listing.AuctionOn.ShouldBe(new DateTime(2009, 1, 24, 12, 30, 00));

                if (tags != null)
                {
                    AssertFeatures(listing.Features, tags);
                }
            }

            private static void  AssertLandSoldListing(LandListing listing,
                bool isSoldPriceVisibile = true)
            {
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Land-Sold-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Sold);

                decimal? soldPrice = 85000m;
                listing.Pricing.SoldPrice.ShouldBe(soldPrice);
                listing.Pricing.SoldPriceText.ShouldBe(isSoldPriceVisibile
                    ? soldPrice.Value.ToString("C0")
                    : null);
                listing.Pricing.SoldOn.ShouldBe(new DateTime(2009, 01, 10, 12, 30, 00));
            }

            #endregion

            #region Rural

            [Fact]
            public void GivenTheFileREARuralCurrent_Convert_ReturnsARurualCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rural\\REA-Rural-Current.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertRuralCurrentListing(result.Listings.First().Listing as RuralListing,
                    new[] { "hotWaterService-gas", "heating-other", "balcony", "shed", "courtyard", "fullyFenced", "outdoorEnt", "courtyard", "deck", "tennisCourt" });
            }

            [Fact]
            public void GivenTheFileREARuralSold_Convert_ReturnsARuralSoldListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rural\\REA-Rural-Sold.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertRuralSoldListing(result.Listings.First().Listing as RuralListing);
            }

            [Fact]
            public void GivenTheFileREARuralSoldDisplayPriceisNo_Convert_ReturnsARuralSoldListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rural\\REA-Rural-Sold-DisplayPriceisNo.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertRuralSoldListing(result.Listings.First().Listing as RuralListing, false);
            }

            [Fact]
            public void GivenTheFileREARuralWithdawn_Convert_ReturnsARuralWithdawnListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rural\\REA-Rural-Withdrawn.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);

                var listing = result.Listings.First().Listing;
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Rural-Withdrawn-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Withdrawn);
            }

            [Fact]
            public void GivenTheFileREARuralOffMarket_Convert_ReturnsARuralOffMarketListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rural\\REA-Rural-OffMarket.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);

                var listing = result.Listings.First().Listing;
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Rural-OffMarket-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.OffMarket);
            }

            private static void AssertRuralCurrentListing(RuralListing listing,
                IList<string> tags = null)
            {
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Rural-Current-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Current);
                listing.CategoryType.ShouldBe(CategoryType.Cropping);
                listing.AuctionOn.ShouldBe(new DateTime(2009, 01, 24, 14, 30, 00));
                listing.CouncilRates.ShouldBe("$2,200 per annum");

                listing.Agents.Count.ShouldBe(1);
                listing.Agents[0].Name.ShouldBe("Mr. John Doe");
                listing.Agents[0].Communications.Count.ShouldBe(2);

                listing.Address.StreetNumber.ShouldBe("39");
                listing.Address.Street.ShouldBe("Main Road");
                listing.Address.Suburb.ShouldBe("RICHMOND");
                listing.Address.IsStreetDisplayed.ShouldBe(true);

                listing.Title.ShouldBe("SHOW STOPPER!!!");
                listing.Description.ShouldStartWith("Don't pass up an opportunity like this! First to inspect will buy! Close to local amen");

                listing.Pricing.IsUnderOffer.ShouldBe(false);
                listing.Pricing.SalePrice.ShouldBe(400000);
                listing.Pricing.SalePriceText.ShouldBe(null);

                listing.LandDetails.Area.Value.ShouldBe(50);
                listing.LandDetails.Area.Type.ShouldBe("acre");
                listing.LandDetails.Frontage.Value.ShouldBe(500);
                listing.LandDetails.Frontage.Type.ShouldBe("meter");
                listing.LandDetails.Depths[0].Value.ShouldBe(400);
                listing.LandDetails.Depths[0].Type.ShouldBe("meter");
                listing.LandDetails.Depths[0].Side.ShouldBe("rear");

                listing.Inspections.Count.ShouldBe(2);

                listing.Images.Count.ShouldBe(2);
                listing.FloorPlans.Count.ShouldBe(2);

                listing.RuralFeatures.AnnualRainfall.ShouldBe("250 mm per annum");
                listing.RuralFeatures.CarryingCapacity.ShouldBe("400 Deer or 100 head of breeding Cattle");
                listing.RuralFeatures.Fencing.ShouldBe("Boundary and internal fencing all in good condition");
                listing.RuralFeatures.Improvements.ShouldBe("Shearing shed, barn and machinery shed.");
                listing.RuralFeatures.Irrigation.ShouldBe("Electric pump from dam and bore.");
                listing.RuralFeatures.Services.ShouldBe("Power, telephone, airstrip, school bus, mail.");
                listing.RuralFeatures.SoilTypes.ShouldBe("red basalt");

                if (tags != null)
                {
                    AssertFeatures(listing.Features, tags);
                }
            }

            private static void AssertRuralSoldListing(RuralListing listing, bool isSoldPriceVisibile = true)
            {
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Rural-Sold-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Sold);

                decimal? soldPrice = 85000m;
                listing.Pricing.SoldPrice.ShouldBe(soldPrice);
                listing.Pricing.SoldPriceText.ShouldBe(isSoldPriceVisibile
                    ? soldPrice.Value.ToString("C0")
                    : null);
                listing.Pricing.SoldOn.ShouldBe(new DateTime(2009, 01, 10, 12, 30, 00));
            }

            #endregion

            [Fact]
            public void GivenTheFileREAAllTypes_Convert_ReturnsAListOfListings()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\REA-AllTypes.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(6);
                result.UnhandledData.ShouldBe(null);
                var listings = result.Listings.Select(x => x.Listing).ToList();

                var residentialCurrentListing = listings
                    .AsQueryable()
                    .WithId("Residential-Current-ABCD1234")
                    .OfType<ResidentialListing>()
                    .SingleOrDefault();
                residentialCurrentListing.ShouldNotBe(null);

                var residentialSoldListing = listings
                    .AsQueryable()
                    .WithId("Residential-Sold-ABCD1234")
                    .OfType<ResidentialListing>()
                    .SingleOrDefault();
                residentialSoldListing.ShouldNotBe(null);

                var residentialWithdrawnListing = listings
                    .AsQueryable()
                    .WithId("Residential-Withdrawn-ABCD1234")
                    .OfType<ResidentialListing>()
                    .SingleOrDefault();
                residentialWithdrawnListing.ShouldNotBe(null);

                var rentalCurrentListing = listings
                    .AsQueryable()
                    .WithId("Rental-Current-ABCD1234")
                    .OfType<RentalListing>()
                    .SingleOrDefault();
                rentalCurrentListing.ShouldNotBe(null);

                var rentalLeasedListing = listings
                    .AsQueryable()
                    .WithId("Rental-Leased-ABCD1234")
                    .OfType<RentalListing>()
                    .SingleOrDefault();
                rentalLeasedListing.ShouldNotBe(null);

                var rentalListing = listings
                    .AsQueryable()
                    .WithId("Rental-Withdrawn-ABCD1234")
                    .OfType<RentalListing>()
                    .SingleOrDefault();
                rentalListing.ShouldNotBe(null);
            }

            [Fact]
            public void GivenTheFileREABadContent_Convert_ThrowsAnException()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\REA-BadContent.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.ShouldBe(null);
                result.UnhandledData.ShouldBe(null);
                result.Errors.Count.ShouldBe(1);
                result.Errors.First().ExceptionMessage.ShouldBe("Unable to parse the xml data provided. Currently, only a <propertyList/> or listing segments <residential/> / <rental/> / <land/> / <rural/>. Root node found: 'badContent'.");
                result.Errors.First().InvalidData.ShouldBe("Failed to parse the provided xml data because it contains some invalid data. Pro Tip: This is usually because a character is not encoded. Like an ampersand.");
            }

            [Fact]
            public void GivenTheFileREAMixedContent_Convert_ReturnsAConvertResultWithListingsAndUnhandedData()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\REA-MixedContent.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.Count.ShouldBe(2);
                result.UnhandledData.Count.ShouldBe(3);
                result.UnhandledData[0].StartsWith("<pewPew1").ShouldBe(true);
            }

            [Fact]
            public void GivenTheFileREAInvalidCharacterAndNoBadCharacterCleaning_Convert_ThrowsAnException()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\REA-InvalidCharacter.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.ShouldBe(null);
                result.UnhandledData.ShouldBe(null);
                result.Errors.Count.ShouldBe(1);
                result.Errors.First().ExceptionMessage.ShouldBe("The REA Xml data provided contains some invalid characters. Line: 0, Position: 1661. Error: '\x16', hexadecimal value 0x16, is an invalid character. Suggested Solution: Either set the 'areBadCharactersRemoved' parameter to 'true' so invalid characters are removed automatically OR manually remove the errors from the file OR manually handle the error (eg. notify the people who sent you this data, that it contains bad data and they should clean it up.)");
                result.Errors.First().InvalidData.ShouldBe("The entire data source.");
            }

            [Fact]
            public void GivenTheFileREAInvalidCharacterAndBadCharacterCleaning_Convert_ThrowsAnException()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\REA-InvalidCharacter.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml, true);

                // Assert.
                result.Listings.Count.ShouldBe(1);

                var residentialCurrentListing = result.Listings.Select(x => x.Listing)
                    .AsQueryable()
                    .WithId("Residential-Current-ABCD1234")
                    .OfType<ResidentialListing>()
                    .SingleOrDefault();
                residentialCurrentListing.ShouldNotBe(null);
            }

            private static void AssertFeatures(Features features, IList<string> tags)
            {
                features.ShouldNotBe(null);
                tags.ShouldNotBe(null);
                    
                features.Tags.Count.ShouldBeGreaterThan(0);
                var missingTags = tags.Except(features.Tags, StringComparer.OrdinalIgnoreCase).ToList();
                if (missingTags.Any())
                {
                    var errorMessage = string.Format("Failed to parse - the following tags haven't been handled: {0}.",
                        string.Join(", ", missingTags));
                    throw new Exception(errorMessage);
                }
            }
        }
    }
}