using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using OpenRealEstate.Core;
using OpenRealEstate.Core.Residential;
using OpenRealEstate.FakeData;
using OpenRealEstate.Services;
using OpenRealEstate.Services.Json;
using OpenRealEstate.Services.RealEstateComAu;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests.Transmorgrifiers.REA
{
    public class ParseResidentialTests
    {
        private static ResidentialListing CreateAFakeEmptyResidentialListing(string id)
        {
            id.ShouldNotBeNullOrWhiteSpace();

            return new ResidentialListing
            {
                AgencyId = "XNWXNW",
                Id = id,
                CreatedOn = new DateTime(2009, 1, 1, 12, 30, 0),
                UpdatedOn = new DateTime(2009, 1, 1, 12, 30, 0)
            };
        }

        private static void AssertResidentialListing(ParsedResult result, 
            ResidentialListing expectedListing)
        {
            result.ShouldNotBeNull();
            result.Listings.Count.ShouldBe(1);
            result.Errors.Count.ShouldBe(0);
            result.UnhandledData.Count.ShouldBe(0);
            ResidentialListingAssertHelpers.AssertResidentialListing(result.Listings.First().Listing as ResidentialListing,
               expectedListing);
        }

        private static void AssertParsingError(ParsedResult result, ParsedError error)
        {
            result.ShouldNotBeNull();
            result.Listings.Count.ShouldBe(0);
            result.UnhandledData.Count.ShouldBe(0);

            error.ShouldNotBeNull();
            result.Errors.First().ExceptionMessage.ShouldBe(error.ExceptionMessage);
            result.Errors.First().InvalidData.ShouldNotBeNullOrWhiteSpace();
        }

        private const string FakeDataFolder = "Sample Data\\Transmorgrifiers\\REA\\Residential\\";

        [Theory]
        [InlineData("REA-Residential-Current.xml")]
        [InlineData("REA-Segment-Residential-Current.xml")]
        [InlineData("REA-Residential-Current-WithAllFeatures.xml")]
        [InlineData("REA-Residential-Current-WithAStreetNumberAndASingleSubNumber.xml")]
        [InlineData("REA-Residential-Current-WithA4PointZeroZeroBedroomNumber.xml")]
        public void GivenTheFileREAResidentialCurrent_Parse_ReturnsAResidentialAvailableListing(string fileName)
        {
            // Arrange.
            var expectedListing = FakeListings.CreateAFakeResidentialListing();
            var reaXml = File.ReadAllText(FakeDataFolder + fileName);
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertResidentialListing(result, expectedListing);
        }

        [Fact]
        public void GivenTheFileREAResidentialCurrentMinimumAndAnExistingListing_Parse_ReturnsAResidentialAvailableListing()
        {
            // Arrange.
            var source = FakeListings.CreateAFakeResidentialListing();
            var destination = FakeListings.CreateAFakeResidentialListing();

            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();
            var reaXml = File.ReadAllText(FakeDataFolder + "REA-Residential-Current-Minimum.xml");

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml, source);

            // Assert.
            AssertResidentialListing(result, destination);
        }

        [Fact]
        public void GivenTheFileREAResidentialSold_Parse_ReturnsAResidentialSoldListing()
        {
            // Arrange.
            var expectedListing = CreateAFakeEmptyResidentialListing("Residential-Sold-ABCD1234");
            expectedListing.StatusType = StatusType.Sold;
            expectedListing.Pricing = new SalePricing
            {
                SoldOn = new DateTime(2009, 1, 10, 12, 30, 00),
                SoldPrice = 580000M,
                SoldPriceText = "$580,000"
            };
            var reaXml = File.ReadAllText(FakeDataFolder + "REA-Residential-Sold.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertResidentialListing(result, expectedListing);
        }

        // NOTE: no display attribute for the sold-data element means a 'yes', please show the value.
        [Fact]
        public void GivenTheFileREAResidentialSoldWithMissingDisplayPrice_Parse_ReturnsAResidentialSoldListing()
        {
            // Arrange.
            var expectedListing = CreateAFakeEmptyResidentialListing("Residential-Sold-ABCD1234");
            expectedListing.StatusType = StatusType.Sold;
            expectedListing.Pricing = new SalePricing
            {
                SoldOn = new DateTime(2009, 1, 10, 12, 30, 00),
                SoldPrice = 580000M,
                SoldPriceText = "$580,000"
            };

            var reaXml = File.ReadAllText(FakeDataFolder + "REA-Residential-Sold-MissingDisplayPrice.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertResidentialListing(result, expectedListing);
        }

        [Fact]
        public void GivenTheFileREAResidentialSoldWithDisplayPriceIsNo_Parse_ReturnsAResidentialSoldListingWithNoSoldPriceText()
        {
            // Arrange.
            var expectedListing = CreateAFakeEmptyResidentialListing("Residential-Sold-ABCD1234");
            expectedListing.StatusType = StatusType.Sold;
            expectedListing.Pricing = new SalePricing
            {
                SoldOn = new DateTime(2009, 1, 10, 12, 30, 00),
                SoldPrice = 580000M
            };

            var reaXml = File.ReadAllText(FakeDataFolder + "REA-Residential-Sold-DisplayPriceIsNo.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertResidentialListing(result, expectedListing);
        }

        [Theory]
        [InlineData("REA-Residential-Withdrawn.xml", "Residential-Withdrawn-ABCD1234")]
        [InlineData("REA-Residential-OffMarket.xml", "Residential-OffMarket-ABCD1234")]
        [InlineData("REA-Residential-Deleted.xml", "Residential-Deleted-ABCD1234")]
        public void GivenAnReaFileThatRepresentsARemovedListing_Parse_ReturnsAResidentialRemovedListing(string fileName,
            string id)
        {
            // Arrange.
            var expectedListing = CreateAFakeEmptyResidentialListing(id);
            expectedListing.StatusType = StatusType.Removed;

            var reaXml = File.ReadAllText(FakeDataFolder + fileName);
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertResidentialListing(result, expectedListing);
        }

        [Theory]
        [InlineData("REA-Residential-Current-WithBadInspectionTime.xml", "Inspection element has an invald Date/Time value. Element: <inspection> 12:00AM to 12:00AM</inspection>")]
        [InlineData("REA-Residential-Current-BadSalePrice.xml", "Failed to parse the value '550000600000550000600000550000600000' into a decimal.")]
        [InlineData("REA-Residential-Sold-DisplayAttributeIsRange.xml", "Value 'range' is out of range. It should only be 0/1/yes/no.\r\nParameter name: value")]
        [InlineData("REA-Residential-Current-WithABadBedroomNumber.xml", "Failed to parse the value '4.5' into an int. Is it a valid number? Does it contain decimal point values?")]
        [InlineData("REA-Residential-Current-WithTooManyBedrooms.xml", "Failed to parse the value '3334' into a byte.")]
        [InlineData("REA-Residential-Current-WithBadModTimeInImagesAndFloorPlans.xml", "Invalid date/time trying to be parsed. Attempted the value: '2016-02-1112:50:05' but that format is invalid. Element/Attribute: <img modTime='..'/>")]
        public void GivenAnReaFileWithSomeTypeOfBadData_Parse_ReturnsAParsedError(string fileName,
            string errorMessage)
        {
            // Arrange.
            var reaXml = File.ReadAllText(FakeDataFolder + fileName);
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            var error = new ParsedError(errorMessage,reaXml);
            AssertParsingError(result, error);
        }
        
        [Fact]
        public void GivenTheFileREAResidentialCurrentBedroomIsStudio_Parse_ReturnsAResidentialAvailableListing()
        {
            // Arrange.
            var expectedListing = FakeListings.CreateAFakeResidentialListing();
            expectedListing.PropertyType = PropertyType.Studio;
            expectedListing.Features.Bedrooms = 0;
            var reaXml = File.ReadAllText(FakeDataFolder + "REA-Residential-Current-BedroomIsStudio.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertResidentialListing(result, expectedListing);
        }

        [Fact]
        public void GivenTheFileREAResidentialCurrentWithPriceAndDisplayYesButNoPriceView_Parse_ReturnsAListing()
        {
            // Arrange.
            var expectedListing = FakeListings.CreateAFakeResidentialListing();
            expectedListing.Pricing.SalePriceText = "$500,000";
            var reaXml = File.ReadAllText(FakeDataFolder + "REA-Residential-Current-WithPriceAndDisplayYesButNoPriceView.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertResidentialListing(result, expectedListing);
        }

        [Fact]
        public void GivenTheFileREAResidentialCurrentWithPriceAndDisplayNoAndNoPriceView_Parse_ReturnsAListing()
        {
            // Arrange.
            var expectedListing = FakeListings.CreateAFakeResidentialListing();
            expectedListing.Pricing.SalePriceText = null;
            var reaXml =
                File.ReadAllText(FakeDataFolder + "REA-Residential-Current-WithPriceAndDisplayNoAndNoPriceView.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertResidentialListing(result, expectedListing);
        }

        [Fact]
        public void GivenTheFileREAResidentialCurrentWithPriceAndDisplayNoAndAPriceView_Parse_ReturnsAListing()
        {
            // Arrange.
            var expectedListing = FakeListings.CreateAFakeResidentialListing();
            expectedListing.Pricing.SalePriceText = null;
            var reaXml = File.ReadAllText(FakeDataFolder + "REA-Residential-Current-WithPriceAndDisplayNoAndAPriceView.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertResidentialListing(result, expectedListing);
        }

        [Fact]
        public void GivenTheFileREAResidentialCurrentWithLocalFilesForImages_Parse_ReturnsAListing()
        {
            // Arrange.
            var expectedListing = FakeListings.CreateAFakeResidentialListing();
            expectedListing.Images[0].Url = "imageM.jpg";
            expectedListing.Images[1].Url = "imageA.jpg";
            expectedListing.FloorPlans[0].Url = "floorplan1.gif";
            expectedListing.FloorPlans[1].Url = "floorplan2.gif";
            var reaXml = File.ReadAllText(FakeDataFolder + "REA-Residential-Current-WithLocalFilesForImages.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertResidentialListing(result, expectedListing);
        }

        [Fact]
        public void
            GivenTheFileREAResidentialCurrentWithNoStreetNumberButASubNumber_Parse_ReturnsAResidentialAvailableListing
            ()
        {
            // Arrange.
            var expectedListing = FakeListings.CreateAFakeResidentialListing();
            expectedListing.Address.StreetNumber = "2/77a";
            var reaXml = File.ReadAllText(FakeDataFolder + "REA-Residential-Current-WithNoStreetNumberButASubNumber.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertResidentialListing(result, expectedListing);
        }

        [Fact]
        public void
            GivenTheFileREAResidentialCurrentWithAStreetNumberAndASubNumber_Parse_ReturnsAResidentialAvailableListing
            ()
        {
            // Arrange.
            var expectedListing = FakeListings.CreateAFakeResidentialListing();
            expectedListing.Address.StreetNumber = "2/77a 39";
            var reaXml =
                File.ReadAllText(FakeDataFolder + "REA-Residential-Current-WithAStreetNumberAndASubNumber.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertResidentialListing(result, expectedListing);
        }

        [Fact]
        public void
            GivenTheFileREAResidentialCurrentWithAStreetNumberAndASingleSubNumberWithACustomDelimeter_Parse_ReturnsAResidentialAvailableListing
            ()
        {
            // Arrange.
            var expectedListing = FakeListings.CreateAFakeResidentialListing();
            expectedListing.Address.StreetNumber = "2-39";
            var reaXml = File.ReadAllText(FakeDataFolder + "REA-Residential-Current-WithAStreetNumberAndASingleSubNumber.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier
            {
                AddressDelimeter = "-"
            };

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertResidentialListing(result, expectedListing);
        }

        [Fact]
        public void GivenTheFileREAResidentialCurrentWithASingleAgentName_Parse_ReturnsAResidentialAvailableListing()
        {
            // Arrange.
            var expectedListing = FakeListings.CreateAFakeResidentialListing();
            expectedListing.Agents[0].Communications = new List<Communication>(); // Remove all communications for this agent.
            var reaXml = File.ReadAllText(FakeDataFolder + "REA-Residential-Current-WithASingleAgentName.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertResidentialListing(result, expectedListing);
        }

        [Theory]
        [InlineData("REA-Residential-Current.xml", false, StatusType.Available)]
        [InlineData("REA-Residential-Current-WithFloorPlansMissing.xml", true, StatusType.Available)]
        [InlineData("REA-Residential-Sold.xml", false, StatusType.Sold)]
        public void
            GivenTheFileREAResidentialCurrent_ParseThenSerializeThenDeserialize_ReturnsAResidentialAvailableListing(
            string fileName,
            bool isFloorPlansCleared,
            StatusType statusType)
        {
            // Arrange.
            ResidentialListing expectedListing;
            if (statusType == StatusType.Available)
            {
                expectedListing = FakeListings.CreateAFakeResidentialListing();
            }
            else
            {
                expectedListing = CreateAFakeEmptyResidentialListing("Residential-Sold-ABCD1234");
                expectedListing.StatusType = StatusType.Sold;
                expectedListing.Pricing = new SalePricing
                {
                    SoldOn = new DateTime(2009, 1, 10, 12, 30, 00),
                    SoldPrice = 580000M,
                    SoldPriceText = "$580,000"
                };
            }

            if (isFloorPlansCleared)
            {
                expectedListing.FloorPlans = new List<Media>();
            }
            var reaXml = File.ReadAllText(FakeDataFolder + fileName);
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Parse the xml, once for the first time.
            var tempResult = reaXmlTransmorgrifier.Parse(reaXml);
            var source = tempResult.Listings.First().Listing;
            var json = JsonConvertHelpers.SerializeObject(source);

            // Act.
            var result = JsonConvert.DeserializeObject<ResidentialListing>(json);

            // Assert.
            var listingResult = new ListingResult
            {
                Listing = result,
                SourceData = "blahblah"
            };
            var parsedResult = new ParsedResult
            {
                Listings = new List<ListingResult> {listingResult},
                UnhandledData = new List<string>(),
                Errors = new List<ParsedError>()
            };
            AssertResidentialListing(parsedResult, expectedListing);
        }

        [Fact]
        public void GivenTheFileREAResidentialCurrentMinimum_Parse_ReturnsAResidentialAvailableListing()
        {
            // Arrange.
            var expectedListing = CreateAFakeEmptyResidentialListing("Residential-Current-ABCD1234");
            expectedListing.StatusType = StatusType.Available;
            expectedListing.Address = new Address
            {
                IsStreetDisplayed = true,
                StreetNumber = "2/39",
                Street = "Main Road",
                Suburb = "RICHMOND",
                State = "vic",
                Postcode = "3121",
                CountryIsoCode = "AU"
            };
            var reaXml = File.ReadAllText(FakeDataFolder + "REA-Residential-Current-Minimum.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertResidentialListing(result, expectedListing);
        }


        [Theory]
        [InlineData("REA-Residential-Current-WithEnsuiteIsTrue.xml", 1)]
        [InlineData("REA-Residential-Current-WithEnsuiteIsFalse.xml", 0)]
        public void GivenTheFileREAResidentialCurrentWithEnsuiteIsTrue_Parse_ReturnsAResidentialAvailableListing(
            string filename,
            byte ensuiteCount)
        {
            // Arrange.
            var expectedListing = FakeListings.CreateAFakeResidentialListing();
            expectedListing.Features.Ensuites = ensuiteCount;
            var reaXml = File.ReadAllText(FakeDataFolder + filename);
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertResidentialListing(result, expectedListing);
        }

        [Fact]
        public void GivenTheFileREAResidentialCurrentWithDuplicateAgents_Parse_ReturnsAResidentialAvailableListing()
        {
            // Arrange.
            var agents = new List<ListingAgent>
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
                            Details = "ImAPrincess@rebelalliance.com"
                        },
                        new Communication
                        {
                            CommunicationType = CommunicationType.Mobile,
                            Details = "1234 1234"
                        }
                    }
                },
                new ListingAgent
                {
                    Name = "Han Solo",
                    Order = 2,
                    Communications = new List<Communication>
                    {
                        new Communication
                        {
                            CommunicationType = CommunicationType.Email,
                            Details = "IShotFirst@rebelalliance.com"
                        },
                        new Communication
                        {
                            CommunicationType = CommunicationType.Mobile,
                            Details = "0987 0987"
                        }
                    }
                }
            };
            var expectedListing = FakeListings.CreateAFakeResidentialListing();
            expectedListing.Agents = agents;
            var reaXml = File.ReadAllText(FakeDataFolder + "REA-Residential-Current-WithDuplicateAgents.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertResidentialListing(result, expectedListing);
        }


        [Fact]
        public void
            GivenTheFileREAResidentialCurrentWithEmptyImagesAndFloorplans_Parse_ReturnsAResidentialAvailableListing()
        {
            // Arrange.
            var expectedListing = FakeListings.CreateAFakeResidentialListing();
            expectedListing.Images = new List<Media>();
            expectedListing.FloorPlans = new List<Media>();
            var reaXml = File.ReadAllText(FakeDataFolder + "REA-Residential-Current-WithEmptyImagesAndFloorplans.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertResidentialListing(result, expectedListing);
        }

        [Fact]
        public void
            GivenTheFileREAResidentialCurrentWithNoModTimeInImagesAndFloorPlans_Parse_ReturnsAResidentialAvailableListing
            ()
        {
            // Arrange.
            var expectedListing = FakeListings.CreateAFakeResidentialListing();
            expectedListing.Images.ToList().ForEach(x => x.CreatedOn = null);
            expectedListing.FloorPlans.ToList().ForEach(x => x.CreatedOn = null);
            var reaXml = File.ReadAllText(FakeDataFolder + "REA-Residential-Current-WithNoModTimeInImagesAndFloorPlans.xml");
            var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

            // Act.
            var result = reaXmlTransmorgrifier.Parse(reaXml);

            // Assert.
            AssertResidentialListing(result, expectedListing);
        }
    }
}
