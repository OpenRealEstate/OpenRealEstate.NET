using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenRealEstate.Core.Filters;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Land;
using OpenRealEstate.Core.Models.Rental;
using OpenRealEstate.Core.Models.Residential;
using OpenRealEstate.Services.RealEstateComAu;
using Shouldly;
using Xunit;
using CategoryType = OpenRealEstate.Core.Models.Land.CategoryType;

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

                var residentialCurrentListing = result.Listings.Select(x => x.Listing)
                    .AsQueryable()
                    .WithId("Residential-Current-ABCD1234")
                    .OfType<ResidentialListing>()
                    .SingleOrDefault();
                AssertResidentialCurrentListing(residentialCurrentListing);
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

                var residentialSoldListing = result.Listings.Select(x => x.Listing)
                    .AsQueryable()
                    .WithId("Residential-Sold-ABCD1234")
                    .OfType<ResidentialListing>()
                    .SingleOrDefault();
                AssertResidentialSoldListing(residentialSoldListing);
            }

            [Fact]
            public void GivenTheFileREAResidentialSoldWithNoDisplayPrice_Convert_ReturnsAResidentialSoldListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Sold-NoDisplayPrice.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);

                var residentialSoldListing = result.Listings.Select(x => x.Listing)
                    .AsQueryable()
                    .WithId("Residential-Sold-ABCD1234")
                    .OfType<ResidentialListing>()
                    .SingleOrDefault();
                AssertResidentialSoldListing(residentialSoldListing);
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

                var residentialSoldListing = result.Listings.Select(x => x.Listing)
                    .AsQueryable()
                    .WithId("Residential-Sold-ABCD1234")
                    .OfType<ResidentialListing>()
                    .SingleOrDefault();
                AssertResidentialSoldListing(residentialSoldListing, false);
            }

            [Fact]
            public void GivenTheFileREAResidentialWithdawn_Convert_ReturnsAResidentialWithdawnListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA\\Residential\\REA-Residential-Withdawn.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var result = reaXmlTransmorgrifier.ConvertTo(reaXml);

                // Assert.
                result.Listings.Count.ShouldBe(1);
                result.UnhandledData.ShouldBe(null);

                var residentialWithdrawnListing = result.Listings.Select(x => x.Listing)
                    .AsQueryable()
                    .WithId("Residential-Withdrawn-ABCD1234")
                    .OfType<ResidentialListing>()
                    .SingleOrDefault();
                AssertResidentialWithdrawnListing(residentialWithdrawnListing);
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

                var residentialCurrentListing = result.Listings.Select(x => x.Listing)
                    .AsQueryable()
                    .WithId("Residential-Current-ABCD1234")
                    .OfType<ResidentialListing>()
                    .SingleOrDefault();
                AssertResidentialCurrentListing(residentialCurrentListing);
            }

            private static void AssertResidentialCurrentListing(ResidentialListing listing)
            {
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Residential-Current-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Current);
                listing.PropertyType.ShouldBe(PropertyType.House);

                listing.Address.IsStreetDisplayed.ShouldBe(true);
                listing.Address.StreetNumber.ShouldBe("2/39");
                listing.Address.Street.ShouldBe("Main Road");
                listing.Address.Suburb.ShouldBe("RICHMOND");
                listing.Address.State.ShouldBe("vic");
                listing.Address.CountryIsoCode.ShouldBe("AU");
                listing.Address.Postcode.ShouldBe("3121");

                listing.Pricing.SalePrice.ShouldBe(500000m);
                listing.Pricing.SalePriceText.ShouldBe("Between $400,000 and $600,000");
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
                listingAgent.Communications[1].CommunicationType.ShouldBe(CommunicationType.Landline);
                listingAgent.Communications[1].Details.ShouldBe("05 1234 5678");

                listing.Features.Bedrooms.ShouldBe(4);
                listing.Features.Bathrooms.ShouldBe(2);
                listing.Features.CarSpaces.ShouldBe(2);

                listing.Images.Count.ShouldBe(2);
                listing.Images[0].Order.ShouldBe(1);
                listing.Images[0].Url.ShouldBe("http://www.realestate.com.au/tmp/imageM.jpg");

                listing.FloorPlans.Count.ShouldBe(2);
                listing.FloorPlans[0].Url.ShouldBe("http://www.realestate.com.au/tmp/floorplan1.gif");
                listing.FloorPlans[0].Order.ShouldBe(1);
                listing.FloorPlans[1].Url.ShouldBe("http://www.realestate.com.au/tmp/floorplan2.gif");
                listing.FloorPlans[0].Order.ShouldBe(1);

                listing.AuctionOn.ShouldBe(new DateTime(2009, 02, 04, 18, 30, 00));

                listing.LandDetails.Area.Value.ShouldBe(80M);
                listing.LandDetails.Area.Type.ShouldBe("square");
                listing.LandDetails.Frontage.Value.ShouldBe(20M);
                listing.LandDetails.Frontage.Type.ShouldBe("meter");
                listing.LandDetails.Depth.Value.ShouldBe(40M);
                listing.LandDetails.Depth.Type.ShouldBe("meter");
                listing.LandDetails.Depth.Side.ShouldBe("rear");

                var errors = new Dictionary<string, string>();
                listing.Validate(errors);
                errors.Count.ShouldBe(0);
            }

            private static void AssertResidentialSoldListing(ResidentialListing listing,
                bool isSoldPriceVisibile = true)
            {
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Residential-Sold-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Sold);

                listing.Pricing.SoldPrice.ShouldBe(580000m);
                listing.Pricing.IsSoldPriceVisibile.ShouldBe(isSoldPriceVisibile);
                listing.Pricing.SoldOn.ShouldBe(new DateTime(2009, 01, 10, 12, 30, 00));
            }

            private static void AssertResidentialWithdrawnListing(ResidentialListing listing)
            {
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Residential-Withdrawn-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Withdrawn);
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

                var rentalCurrentListing = result.Listings.Select(x => x.Listing)
                    .AsQueryable()
                    .WithId("Rental-Current-ABCD1234")
                    .OfType<RentalListing>()
                    .SingleOrDefault();
                AssertRentalCurrentListing(rentalCurrentListing);
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

                var rentalLeasedListing = result.Listings.Select(x => x.Listing)
                    .AsQueryable()
                    .WithId("Rental-Leased-ABCD1234")
                    .OfType<RentalListing>()
                    .SingleOrDefault();
                AssertRentalLeasedListing(rentalLeasedListing);
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

                var rentalListing = result.Listings.Select(x => x.Listing)
                    .AsQueryable()
                    .WithId("Rental-Withdrawn-ABCD1234")
                    .OfType<RentalListing>()
                    .SingleOrDefault();
                AssertRentalWithdrawnListing(rentalListing);
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

                var rentalCurrentListing = result.Listings.Select(x => x.Listing)
                    .AsQueryable()
                    .WithId("Rental-Current-ABCD1234")
                    .OfType<RentalListing>()
                    .SingleOrDefault();
                AssertRentalCurrentListing(rentalCurrentListing);
            }

            private static void AssertRentalCurrentListing(RentalListing listing)
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

                listing.AvailableOn.ShouldBe(new DateTime(2009, 01, 26, 12, 30, 00));

                listing.Pricing.RentalPrice.ShouldBe(350);
                listing.Pricing.RentalPriceText.ShouldBe(null);
                listing.Pricing.Bond.ShouldBe(350);

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
                listing.Features.CarSpaces.ShouldBe(2);

                listing.Images.Count.ShouldBe(2);
                listing.Images[0].Order.ShouldBe(1);
                listing.Images[0].Url.ShouldBe("http://www.realestate.com.au/tmp/imageM.jpg");

                listing.LandDetails.Area.Value.ShouldBe(60M);
                listing.LandDetails.Area.Type.ShouldBe("square");
                listing.LandDetails.Frontage.Value.ShouldBe(15M);
                listing.LandDetails.Frontage.Type.ShouldBe("meter");
                listing.LandDetails.Depth.Value.ShouldBe(40M);
                listing.LandDetails.Depth.Type.ShouldBe("meter");
                listing.LandDetails.Depth.Side.ShouldBe("rear");

                listing.FloorPlans.Count.ShouldBe(2);

                var errors = new Dictionary<string, string>();
                listing.Validate(errors);
                errors.Count.ShouldBe(0);
            }

            private static void AssertRentalLeasedListing(RentalListing listing)
            {
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Rental-Leased-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Leased);
            }

            private static void AssertRentalWithdrawnListing(RentalListing listing)
            {
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Rental-Withdrawn-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Withdrawn);
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

                var landCurrentListing = result.Listings.Select(x => x.Listing)
                    .AsQueryable()
                    .WithId("Land-Current-ABCD1234")
                    .OfType<LandListing>()
                    .SingleOrDefault();
                AssertLandCurrentListing(landCurrentListing);
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

                var landListing = result.Listings.Select(x => x.Listing)
                    .AsQueryable()
                    .WithId("Land-Current-ABCD1234")
                    .OfType<LandListing>()
                    .SingleOrDefault();

                landListing.LandDetails.Area.ShouldBe(null);
                landListing.LandDetails.Frontage.ShouldBe(null);
                landListing.LandDetails.Depth.ShouldBe(null);
                landListing.LandDetails.CrossOver.ShouldBeNullOrEmpty();
            }

            private static void AssertLandCurrentListing(LandListing listing)
            {
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Land-Current-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Current);
                listing.CategoryType.ShouldBe(CategoryType.Residential);

                var errors = new Dictionary<string, string>();
                listing.Validate(errors);
                errors.Count.ShouldBe(0);

                listing.Agents.Count.ShouldBe(1);
                listing.Agents[0].Name.ShouldBe("Mr. John Doe");
                listing.Agents[0].Communications.Count.ShouldBe(2);

                listing.Address.StreetNumber.ShouldBe("LOT 12/39");
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
                listing.LandDetails.Depth.Type.ShouldBe("meter");
                listing.LandDetails.Depth.Value.ShouldBe(30m);
                listing.LandDetails.Depth.Side.ShouldBe("rear");
                listing.LandDetails.CrossOver.ShouldBe("left");

                listing.Images.Count.ShouldBe(2);
                listing.Images[0].Order.ShouldBe(1);
                listing.Images[0].Url.ShouldBe("http://www.realestate.com.au/tmp/imageM.jpg");

                listing.FloorPlans.Count.ShouldBe(2);
                listing.FloorPlans[0].Order.ShouldBe(1);
                listing.FloorPlans[0].Url.ShouldBe("http://www.realestate.com.au/tmp/floorplan1.gif");

                listing.AuctionOn.ShouldBe(new DateTime(2009, 1, 24, 12, 30, 00));
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
                var exception = Should.Throw<Exception>(() => reaXmlTransmorgrifier.ConvertTo(reaXml));

                // Assert.
                exception.ShouldNotBe(null);
                exception.Message.ShouldBe("Unable to parse the xml data provided. Currently, only a <propertyList/> or listing segments <residential/> / <rental/> / <land/>. Root node found: 'badContent'.");
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
                var exception = Should.Throw<Exception>(() => reaXmlTransmorgrifier.ConvertTo(reaXml));

                // Assert.
                exception.ShouldNotBe(null);
                exception.Message.ShouldBe(
                    "The REA Xml data provided contains some invalid characters. Line: 0, Position: 1661. Error: '\x16', hexadecimal value 0x16, is an invalid character. Suggested Solution: Either set the 'areBadCharactersRemoved' parameter to 'true' so invalid characters are removed automatically OR manually remove the errors from the file OR manually handle the error (eg. notify the people who sent you this data, that it contains bad data and they should clean it up.)");
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
        }
    }
}