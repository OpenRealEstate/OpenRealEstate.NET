using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using FluentValidation.Results;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OpenRealEstate.Core.Filters;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Land;
using OpenRealEstate.Core.Models.Rental;
using OpenRealEstate.Core.Models.Residential;
using OpenRealEstate.Core.Models.Rural;
using OpenRealEstate.Services.RealEstateComAu;
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
                var reaXml =
                    File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);
                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    tags:
                        new[]
                        {
                            "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other",
                            "balcony", "shed", "courtyard", "isANewConstruction"
                        },
                    videoUrls: new[] {"http://www.foo.tv/abcd.html"});
            }

            [Fact]
            public void GivenTheFileREAResidentialSold_Convert_ReturnsAResidentialSoldListing()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Sold.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertResidentialSoldListing(result.Listings.First().Listing as ResidentialListing);
            }

            // NOTE: no display attribute for the sold-data element means a 'yes', please show the value.
            [Fact]
            public void GivenTheFileREAResidentialSoldWithMissingDisplayPrice_Convert_ReturnsAResidentialSoldListing()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Sold-MissingDisplayPrice.xml");
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
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Sold-DisplayPriceIsNo.xml");
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
                var reaXml =
                    File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Withdrawn.xml");
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
                var reaXml =
                    File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-OffMarket.xml");
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
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Segment-Residential-Current.xml");
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
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-CurrentBadInspectionTime.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.ShouldBe(null);
                result.UnhandledData.ShouldBe(null);
                result.Errors.Count.ShouldBe(1);
                result.Errors[0].ExceptionMessage.ShouldBe(
                    "Inspection element has an invald Date/Time value. Element: <inspection> 12:00AM to 12:00AM</inspection>");
                result.Errors[0].InvalidData.ShouldNotBeNullOrEmpty();
            }

            [Fact]
            public void GivenTheFileReaResidentialCurrentBadSalePrice_Convert_ReturnsSomeInvalidData()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-BadSalePrice.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.ShouldBe(null);
                result.UnhandledData.ShouldBe(null);
                result.Errors[0].ExceptionMessage.ShouldBe(
                    "Failed to parse the value '550000600000550000600000550000600000' into a decimal.");
                result.Errors[0].InvalidData.ShouldNotBeNullOrEmpty();
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentBedroomIsStudio_Convert_ReturnsAResidentialCurrentListing()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-BedroomIsStudio.xml");
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
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithAllFeatures.xml");
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
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Sold-DisplayAttributeIsRange.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.ShouldBe(null);
                result.UnhandledData.ShouldBe(null);
                result.Errors.Count.ShouldBe(1);
                result.Errors.First()
                    .ExceptionMessage.ShouldBe(
                        "Value 'range' is out of range. It should only be 0/1/yes/no.\r\nParameter name: value");
                result.Errors.First()
                    .InvalidData.ShouldStartWith("<residential modTime=\"2009-01-01-12:30:00\" status=\"sold\">");
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentWithPriceAndDisplayYesButNoPriceView_Convert_ReturnsAListing()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithPriceAndDisplayYesButNoPriceView.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);

                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    tags:
                        new[]
                        {
                            "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other",
                            "balcony", "shed", "courtyard", "isANewConstruction"
                        },
                    salePriceText: "$500,000");
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentWithPriceAndDisplayNoAndNoPriceView_Convert_ReturnsAListing()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithPriceAndDisplayNoAndNoPriceView.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);

                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    tags:
                        new[]
                        {
                            "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other",
                            "balcony", "shed", "courtyard", "isANewConstruction"
                        },
                    salePriceText: null);
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentWithPriceAndDisplayNoAndAPriceView_Convert_ReturnsAListing()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithPriceAndDisplayNoAndAPriceView.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);

                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    tags:
                        new[]
                        {
                            "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other",
                            "balcony", "shed", "courtyard", "isANewConstruction"
                        },
                    salePriceText: null);
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentWithLocalFilesForImages_Convert_ReturnsAListing()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithLocalFilesForImages.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);

                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    tags:
                        new[]
                        {
                            "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other",
                            "balcony", "shed", "courtyard", "isANewConstruction"
                        },
                    imageUrls: new[] {"imageM.jpg", "imageA.jpg"},
                    floorplanUrls: new[] {"floorplan1.gif", "floorplan2.gif"});
            }

            [Fact]
            public void
                GivenTheFileREAResidentialCurrentWithNoStreetNumberButASubNumber_Convert_ReturnsAResidentialCurrentListing
                ()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithNoStreetNumberButASubNumber.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);
                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    tags:
                        new[]
                        {
                            "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other",
                            "balcony", "shed", "courtyard", "isANewConstruction"
                        },
                    streetNumber: "2/77a");
            }

            [Fact]
            public void
                GivenTheFileREAResidentialCurrentWithAStreetNumberAndASubNumber_Convert_ReturnsAResidentialCurrentListing
                ()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithAStreetNumberAndASubNumber.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);
                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    tags:
                        new[]
                        {
                            "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other",
                            "balcony", "shed", "courtyard", "isANewConstruction"
                        },
                    streetNumber: "2/77a 39");
            }

            [Fact]
            public void
                GivenTheFileREAResidentialCurrentWithAStreetNumberAndASingleSubNumber_Convert_ReturnsAResidentialCurrentListing
                ()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithAStreetNumberAndASingleSubNumber.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);
                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    tags:
                        new[]
                        {
                            "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other",
                            "balcony", "shed", "courtyard", "isANewConstruction"
                        });
            }

            [Fact]
            public void
                GivenTheFileREAResidentialCurrentWithAStreetNumberAndASingleSubNumberWithACustomDelimeter_Convert_ReturnsAResidentialCurrentListing
                ()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithAStreetNumberAndASingleSubNumber.xml");
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
                    tags:
                        new[]
                        {
                            "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other",
                            "balcony", "shed", "courtyard", "isANewConstruction"
                        },
                    streetNumber: "2-39");
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentWithA4PointZeroZeroBedroomNumber_Convert_ReturnsAListing()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithA4PointZeroZeroBedroomNumber.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);

                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    tags:
                        new[]
                        {
                            "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other",
                            "balcony", "shed", "courtyard", "isANewConstruction"
                        },
                    videoUrls: new[] {"http://www.foo.tv/abcd.html"});
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentWithABadBedroomNumber_Convert_ReturnsAnError()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithABadBedroomNumber.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.UnhandledData.ShouldBe(null);
                result.Errors.Count.ShouldBe(1);
                result.Errors.First()
                    .ExceptionMessage.ShouldBe(
                        "Failed to parse the value '4.5' into an int. Is it a valid number? Does it contain decimal point values?");
            }

            [Fact]
            public void
                GivenTheFileREAResidentialCurrent_ConvertWithIsClearAllIsModified_ReturnsAResidentialCurrentListing()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml, isClearAllIsModified: true);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);
                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    tags:
                        new[]
                        {
                            "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other",
                            "balcony", "shed", "courtyard", "isANewConstruction"
                        },
                    videoUrls: new[] {"http://www.foo.tv/abcd.html"},
                    isModified: false);
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentWithASingleAgentName_Convert_ReturnsAResidentialCurrentListing()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithASingleAgentName.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);
                var listing = (ResidentialListing) result.Listings.First().Listing;
                Action<IList<ListingAgent>, bool> assertAgents =
                    delegate(IList<ListingAgent> listingAgents, bool isModified)
                    {
                        listingAgents.Count.ShouldBe(1);
                        listingAgents.First().Name.ShouldBe("Mr. John Doe");
                        listingAgents.First().Order.ShouldBe(1);
                    };
                AssertResidentialCurrentListing(listing,
                    tags:
                        new[]
                        {
                            "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other",
                            "balcony", "shed", "courtyard", "isANewConstruction"
                        },
                    videoUrls: new[] { "http://www.foo.tv/abcd.html" },
                    assertAgents: assertAgents);
            }

            [Theory,
            InlineData("REA-Residential-Current.xml"),
            InlineData("REA-Residential-Current-WithFloorPlansMissing.xml"),
            InlineData("REA-Residential-Sold.xml")]
            public void GivenTheFileREAResidentialCurrent_ConvertThenSaveThenCovertAgain_ReturnsAResidentialCurrentListing(string fileName)
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\" + fileName);
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var tempResult = reaXmlTransmorgrifier.ConvertTo(reaXml);

                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new ModifiedDataContractResolver(),
                    Formatting = Formatting.Indented
                };

                var source = tempResult.Listings.First().Listing;

                // Act.
                var json = JsonConvert.SerializeObject(source, settings);
                var result = JsonConvert.DeserializeObject<ResidentialListing>(json);

                // Assert.
                result.Agents.Count.ShouldBe(source.Agents.Count);
                result.Images.Count.ShouldBe(source.Images.Count);
                result.FloorPlans.Count.ShouldBe(source.FloorPlans.Count);
                result.Videos.Count.ShouldBe(source.Videos.Count);
                result.Inspections.Count.ShouldBe(source.Inspections.Count);
                result.Links.Count.ShouldBe(source.Links.Count);
                if (result.Features != null)
                {
                    result.Features.Tags.Count.ShouldBe(source.Features.Tags.Count);
                }
                if (result.LandDetails != null)
                {
                    result.LandDetails.Depths.Count.ShouldBe(source.LandDetails.Depths.Count);
                }
                for (var i = 0; i < result.Agents.Count; i++)
                {
                    result.Agents[i].Communications.Count.ShouldBe(source.Agents[i].Communications.Count);
                }

                // If we don't have a count, then we need to make sure the collection hasn't been modified.
                // Basically, the collection hasn't been touched.
                result.ModifiedData.ModifiedCollections.Contains("Agents").ShouldBe(result.Agents.Count > 0);
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentMinimum_Convert_ReturnsAResidentialCurrentListing()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-Minimum.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);

                // We have the minimum listing data, so lets make sure barelly anytything
                // has been set.
                var listing = (ResidentialListing)result.Listings.First().Listing;
                listing.Pricing.SalePrice.ShouldBe(0);
                listing.Pricing.SalePriceText.ShouldBeNullOrEmpty();
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentWithEnsuiteIsTrue_Convert_ReturnsAResidentialCurrentListing()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithEnsuiteIsTrue.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);
                result.Listings.First().Listing.Features.Ensuites.ShouldBe(1);
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentWithEnsuiteIsFalse_Convert_ReturnsAResidentialCurrentListing()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithEnsuiteIsFalse.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);
                result.Listings.First().Listing.Features.Ensuites.ShouldBe(0);
            }

            [Fact]
            public void GivenTheFileREAResidentialCurrentWithDuplicateAgents_Convert_ReturnsAResidentialCurrentListing()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Current-WithDuplicateAgents.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.ShouldNotBe(null);
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                result.Errors.ShouldBe(null);

                Action<IList<ListingAgent>, bool> assertAgents =
                    delegate (IList<ListingAgent> listingAgents, bool isModified)
                    {
                        listingAgents.Count.ShouldBe(2);
                        listingAgents[0].Name.ShouldBe("Princess Leia");
                        listingAgents[0].Order.ShouldBe(1);
                        listingAgents[0].Communications.Count.ShouldBe(2);

                        listingAgents[1].Name.ShouldBe("Han Solo");
                        listingAgents[1].Order.ShouldBe(2);
                        listingAgents[1].Communications.Count.ShouldBe(2);
                    };

                AssertResidentialCurrentListing(result.Listings.First().Listing as ResidentialListing,
                    tags:
                        new[]
                        {
                            "houseAndLandPackage", "solarPanels", "waterTank", "hotWaterService-gas", "heating-other",
                            "balcony", "shed", "courtyard", "isANewConstruction"
                        },
                    videoUrls: new[] { "http://www.foo.tv/abcd.html" },
                    assertAgents: assertAgents);
            }

            private static void AssertResidentialCurrentListing(ResidentialListing listing,
                PropertyType expectedPropertyType = PropertyType.House,
                int expectedBedroomsCount = 4,
                IList<string> tags = null,
                string salePriceText = "Between $400,000 and $600,000",
                IList<string> imageUrls = null,
                IList<string> floorplanUrls = null,
                IList<string> videoUrls = null,
                string streetNumber = "2/39",
                bool isModified = true,
                Action<IList<ListingAgent>, bool> assertAgents = null)
            {
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Residential-Current-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Current);
                listing.PropertyType.ShouldBe(expectedPropertyType);
                listing.CouncilRates.ShouldBe("$2000 per month");
                listing.Links[0].ShouldBe("http://www.au.open2view.com/properties/314244/tour#floorplan");
                listing.Links[1].ShouldBe("http://www.google.com/hello");
                AssertAddress(listing.Address, streetNumber);
                AssertSalePrice(listing.Pricing, salePriceText);
                AssertInspections(listing.Inspections);

                if (assertAgents != null)
                {
                    assertAgents(listing.Agents, isModified);
                }
                else
                {
                    AssertAgents(listing.Agents);    
                }

                AssertFeatures(listing.Features, 
                    tags, 
                    expectedBedroomsCount,
                    bathroomCount: 2,
                    ensuitesCount: 2);

                AssertImages(listing.Images, imageUrls);

                AssertFloorPlans(listing.FloorPlans, floorplanUrls);

                if (videoUrls != null)
                {
                    AssertVideos(listing.Videos, videoUrls);
                }
                
                listing.AuctionOn.ShouldBe(new DateTime(2009, 02, 04, 18, 30, 00));

                AssertBuildingDetails(listing.BuildingDetails);

                AssertLandDetails(listing.LandDetails);
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
                    new[]
                    {
                        "hotWaterService-gas", "heating-other", "balcony", "shed", "courtyard", "isANewConstruction",
                        "fullyFenced", "outdoorEnt", "courtyard", "deck", "tennisCourt"
                    },
                    4);
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
                var reaXml =
                    File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rental\\REA-Segment-Rental-Current.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertRentalCurrentListing(result.Listings.First().Listing as RentalListing,
                    bedroomsCount: 4);
            }

            [Fact]
            public void GivenTheFileREARentalCurrentWithNoBond_Convert_ReturnsARentalCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rental\\REA-Rental-Current-WithNoBond.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertRentalCurrentListing(result.Listings.First().Listing as RentalListing,
                    new[]
                    {
                        "hotWaterService-gas", "heating-other", "balcony", "shed", "courtyard", "isANewConstruction",
                        "fullyFenced", "outdoorEnt", "courtyard", "deck", "tennisCourt"
                    },
                    4,
                    null);
            }

            private static void AssertRentalCurrentListing(RentalListing listing,
                IList<string> tags = null,
                int bedroomsCount = 0,
                decimal? bond = 999)
            {
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Rental-Current-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Current);
                listing.PropertyType.ShouldBe(PropertyType.House);

                AssertAddress(listing.Address, "39");

                listing.AvailableOn.ShouldBe(new DateTime(2009, 01, 26, 12, 30, 00));

                AssertRentalPricing(listing.Pricing, bond);

                listing.Inspections.Count.ShouldBe(2);

                listing.Agents.Count.ShouldBe(1);
                var listingAgent = listing.Agents[0];
                listingAgent.Name.ShouldBe("Mr. John Doe");
                listingAgent.Order.ShouldBe(1);
                listingAgent.Communications[0].CommunicationType.ShouldBe(CommunicationType.Email);
                listingAgent.Communications[0].Details.ShouldBe("jdoe@somedomain.com.au");
                listingAgent.Communications[1].CommunicationType.ShouldBe(CommunicationType.Landline);
                listingAgent.Communications[1].Details.ShouldBe("05 1234 5678");

                AssertFeatures(listing.Features, 
                    tags, 
                    bedroomsCount:bedroomsCount,
                    bathroomCount: 2,
                    ensuitesCount: 2,
                    carParking: new CarParking
                    {
                        Garages = 3,
                        Carports = 2
                    });

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
                    tags: new[] {"fullyFenced"},
                    carParking: new CarParking());
            }

            [Fact]
            public void GivenTheFileREALandCurrentIncompleteLandDetails_Convert_ReturnsALandCurrentListing()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-Current-IncompleteLandDetails.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);

                var listing = result.Listings.First().Listing;
                listing.LandDetails.Area.ShouldBe(null);
                listing.LandDetails.Frontage.ShouldBe(null);
                listing.LandDetails.Depths.Count.ShouldBe(0);
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
                var reaXml =
                    File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-Sold-DisplayPriceisNo.xml");
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
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-Current-MissingLandCategory.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertLandCurrentListing(result.Listings.First().Listing as LandListing, 
                    LandCategoryType.Unknown,
                    carParking: new CarParking());
            }

            [Fact]
            public void GivenTheFileREALandCurrentWithASubNumberButNoStreetNumber_Convert_ReturnsALandCurrentListing()
            {
                // Arrange.
                var reaXml =
                    File.ReadAllText(
                        "Sample Data\\Transmorgrifiers\\REA\\Land\\REA-Land-Current-WithASubNumberButNoStreetNumber.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);
                AssertLandCurrentListing(result.Listings.First().Listing as LandListing,
                    tags: new[] {"fullyFenced"},
                    streetNumber: "12",
                    carParking: new CarParking());
            }

            private static void AssertLandCurrentListing(LandListing listing,
                LandCategoryType landCategoryType = LandCategoryType.Residential,
                IList<string> tags = null,
                string streetNumber = "LOT 12/39",
                CarParking carParking = null,
                bool isModified = true)
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

                AssertFeatures(listing.Features, 
                    tags, 
                    carParking:carParking);
            }

            private static void AssertLandSoldListing(LandListing listing,
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
                    new[]
                    {
                        "hotWaterService-gas", "heating-other", "balcony", "shed", "courtyard", "fullyFenced", "outdoorEnt",
                        "courtyard", "deck", "tennisCourt"
                    },
                    true,
                    4);
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
                var reaXml =
                    File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Rural\\REA-Rural-Sold-DisplayPriceisNo.xml");
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
                IList<string> tags = null,
                bool isModified = true,
                int bedroomsCount = 0)
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
                listing.Description.ShouldStartWith(
                    "Don't pass up an opportunity like this! First to inspect will buy! Close to local amen");

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

                AssertFeatures(listing.Features, 
                    tags, 
                    bedroomsCount,
                    2,
                    2);
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
                result.Errors.First()
                    .ExceptionMessage.ShouldBe(
                        "Unable to parse the xml data provided. Currently, only a <propertyList/> or listing segments <residential/> / <rental/> / <land/> / <rural/>. Root node found: 'badContent'.");
                result.Errors.First()
                    .InvalidData.ShouldBe(
                        "Failed to parse the provided xml data because it contains some invalid data. Pro Tip: This is usually because a character is not encoded. Like an ampersand.");
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
                result.Errors.First()
                    .ExceptionMessage.ShouldBe(
                        "The REA Xml data provided contains some invalid characters. Line: 0, Position: 1661. Error: '\x16', hexadecimal value 0x16, is an invalid character. Suggested Solution: Either set the 'areBadCharactersRemoved' parameter to 'true' so invalid characters are removed automatically OR manually remove the errors from the file OR manually handle the error (eg. notify the people who sent you this data, that it contains bad data and they should clean it up.)");
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

            private static void AssertAddress(Address address,
                string streetNumber)
            {
                address.IsStreetDisplayed.ShouldBe(true);
                address.StreetNumber.ShouldBe(streetNumber);
                address.Street.ShouldBe("Main Road");
                address.Suburb.ShouldBe("RICHMOND");
                address.Municipality.ShouldBe("Yarra");
                address.State.ShouldBe("vic");
                address.CountryIsoCode.ShouldBe("AU");
                address.Postcode.ShouldBe("3121");
            }

            private static void AssertSalePrice(SalePricing pricing,
                string salePriceText)
            {
                pricing.SalePrice.ShouldBe(500000m);
                pricing.SalePriceText.ShouldBe(salePriceText);
                pricing.IsUnderOffer.ShouldBe(false);
            }

            private static void AssertInspections(IList<Inspection> inspections)
            {
                inspections.Count.ShouldBe(2);
                inspections.First().OpensOn.ShouldBe(new DateTime(2009, 1, 21, 11, 00, 0));
                inspections.First().ClosesOn.ShouldBe(new DateTime(2009, 1, 21, 13, 00, 0));
            }

            private static void AssertAgents(IList<ListingAgent> agents)
            {
                agents.Count.ShouldBe(1);

                var listingAgent = agents[0];
                listingAgent.Name.ShouldBe("Mr. John Doe");
                listingAgent.Order.ShouldBe(1);
                listingAgent.Communications[0].CommunicationType.ShouldBe(CommunicationType.Email);
                listingAgent.Communications[0].Details.ShouldBe("jdoe@somedomain.com.au");
                listingAgent.Communications[1].CommunicationType.ShouldBe(CommunicationType.Mobile);
                listingAgent.Communications[1].Details.ShouldBe("0418 123 456");
                listingAgent.Communications[2].CommunicationType.ShouldBe(CommunicationType.Landline);
                listingAgent.Communications[2].Details.ShouldBe("05 1234 5678");
            }

            private static void AssertFeatures(Features features,
                IList<string> tags,
                int bedroomsCount = 0,
                int bathroomCount = 0,
                int ensuitesCount = 0,
                int toiletsCount = 0,
                int livingAreasCount = 0,
                CarParking carParking = null)
            {
                features.Bedrooms.ShouldBe(bedroomsCount);
                features.Bathrooms.ShouldBe(bathroomCount);
                features.Ensuites.ShouldBe(ensuitesCount);
                features.Toilets.ShouldBe(toiletsCount);
                features.LivingAreas.ShouldBe(livingAreasCount);

                if (tags != null)
                {
                    AssertTags(features.Tags, tags);
                }

                if (features.CarParking != null)
                {
                    AssertCarParking(features.CarParking, carParking);
                }
            }


            private static void AssertCarParking(CarParking carParking, 
                CarParking optionalCarParkingData)
            {
                carParking.Garages.ShouldBe(optionalCarParkingData != null
                    ? optionalCarParkingData.Garages
                    : 3);

                carParking.Carports.ShouldBe(optionalCarParkingData != null
                    ? optionalCarParkingData.Carports
                    : 2);

                carParking.OpenSpaces.ShouldBe(optionalCarParkingData != null
                    ? optionalCarParkingData.OpenSpaces
                    : 0);
            }

            private static void AssertTags(ICollection<string> featureTags, IEnumerable<string> tags)
            {
                featureTags.Count.ShouldBeGreaterThan(0);
                var missingTags = tags.Except(featureTags, StringComparer.OrdinalIgnoreCase).ToList();
                if (missingTags.Any())
                {
                    var errorMessage = string.Format("Failed to parse - the following tags haven't been handled: {0}.",
                        string.Join(", ", missingTags));
                    throw new Exception(errorMessage);
                }
            }

            private static void AssertImages(IList<Media> images,
                IList<string> imageUrls)
            {
                images.Count.ShouldBe(2);
                images[0].CreatedOn.ShouldBe(new DateTime(2009, 1, 1, 12, 30, 0));
                images[0].Order.ShouldBe(1);
                images[0].Url.ShouldBe(imageUrls == null
                    ? "http://www.realestate.com.au/tmp/imageM.jpg"
                    : imageUrls[0]);
                images[1].Order.ShouldBe(2);
                images[1].Url.ShouldBe(imageUrls == null
                    ? "http://www.realestate.com.au/tmp/imageA.jpg"
                    : imageUrls[1]);
            }

            private static void AssertFloorPlans(IList<Media> floorPlans,
                IList<string> floorplanUrls)
            {
                floorPlans.Count.ShouldBe(2);
                floorPlans[0].CreatedOn.ShouldBe(new DateTime(2009, 1, 1, 12, 30, 0));
                floorPlans[0].Url.ShouldBe(floorplanUrls == null
                    ? "http://www.realestate.com.au/tmp/floorplan1.gif"
                    : floorplanUrls[0]);
                floorPlans[0].Order.ShouldBe(1);
                floorPlans[1].Url.ShouldBe(floorplanUrls == null
                    ? "http://www.realestate.com.au/tmp/floorplan2.gif"
                    : floorplanUrls[1]);
                floorPlans[1].Order.ShouldBe(2);
            }

            private static void AssertVideos(IList<Media> videos,
                IList<string> videoUrls)
            {
                for (var i = 0; i < videos.Count; i++)
                {
                    videos[i].Url.ShouldBe(videoUrls[i]);
                }
            }

            private static void AssertBuildingDetails(BuildingDetails buildingDetails)
            {
                buildingDetails.Area.Value.ShouldBe(40);
                buildingDetails.Area.Type.ShouldBe("square");
                buildingDetails.EnergyRating.ShouldBe(4.5m);
            }

            private static void AssertLandDetails(LandDetails landDetails)
            {
                landDetails.Area.Value.ShouldBe(80M);
                landDetails.Area.Type.ShouldBe("square");
                landDetails.Frontage.Value.ShouldBe(20M);
                landDetails.Frontage.Type.ShouldBe("meter");
                landDetails.Depths[0].Value.ShouldBe(40M);
                landDetails.Depths[0].Type.ShouldBe("meter");
                landDetails.Depths[0].Side.ShouldBe("rear");
                landDetails.Depths[1].Value.ShouldBe(60M);
                landDetails.Depths[1].Type.ShouldBe("meter");
                landDetails.Depths[1].Side.ShouldBe("left");
                landDetails.Depths[2].Value.ShouldBe(20M);
                landDetails.Depths[2].Type.ShouldBe("meter");
                landDetails.Depths[2].Side.ShouldBe("right");
            }

            private static void AssertRentalPricing(RentalPricing rentalPricing,
                decimal? bond)
            {
                rentalPricing.RentalPrice.ShouldBe(350);
                rentalPricing.RentalPriceText.ShouldBe("$350");
                rentalPricing.PaymentFrequencyType.ShouldBe(PaymentFrequencyType.Weekly);
                rentalPricing.Bond.ShouldBe(bond);
            }
        }
    }
}