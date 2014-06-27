using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenRealEstate.Core.Filters;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Services.RealEstateComAu;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests
{
    public class TransmorgrifierFacts
    {
        public class ReaXmlTransmorgrifierFacts
        {
            [Fact]
            public void GivenTheFileREAAllTypes_Convert_ReturnsAListOfListings()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA-AllTypes.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var listings = reaXmlTransmorgrifier.Convert(reaXml);

                // Assert.
                listings.Count.ShouldBe(6);

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
            public void GivenTheFileREAResidentialCurrent_Convert_ReturnsAResidentialCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA-Residential-Current.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var listings = reaXmlTransmorgrifier.Convert(reaXml);

                // Assert.
                listings.Count.ShouldBe(1);

                var residentialCurrentListing = listings
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
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA-Residential-Sold.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var listings = reaXmlTransmorgrifier.Convert(reaXml);

                // Assert.
                listings.Count.ShouldBe(1);

                var residentialSoldListing = listings
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
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA-Residential-Sold-NoDisplayPrice.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var listings = reaXmlTransmorgrifier.Convert(reaXml);

                // Assert.
                listings.Count.ShouldBe(1);

                var residentialSoldListing = listings
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
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA-Residential-Sold-DisplayPriceIsNo.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var listings = reaXmlTransmorgrifier.Convert(reaXml);

                // Assert.
                listings.Count.ShouldBe(1);

                var residentialSoldListing = listings
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
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA-Residential-Withdawn.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var listings = reaXmlTransmorgrifier.Convert(reaXml);

                // Assert.
                listings.Count.ShouldBe(1);

                var residentialWithdrawnListing = listings
                    .AsQueryable()
                    .WithId("Residential-Withdrawn-ABCD1234")
                    .OfType<ResidentialListing>()
                    .SingleOrDefault();
                AssertResidentialWithdrawnListing(residentialWithdrawnListing);
            }

            [Fact]
            public void GivenTheFileREARentalCurrent_Convert_ReturnsARentalCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA-Rental-Current.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var listings = reaXmlTransmorgrifier.Convert(reaXml);

                // Assert.
                listings.Count.ShouldBe(1);

                var rentalCurrentListing = listings
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
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA-Rental-Leased.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var listings = reaXmlTransmorgrifier.Convert(reaXml);

                // Assert.
                listings.Count.ShouldBe(1);

                var rentalLeasedListing = listings
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
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA-Rental-Withdrawn.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var listings = reaXmlTransmorgrifier.Convert(reaXml);

                // Assert.
                listings.Count.ShouldBe(1);

                var rentalListing = listings
                    .AsQueryable()
                    .WithId("Rental-Withdrawn-ABCD1234")
                    .OfType<RentalListing>()
                    .SingleOrDefault();
                AssertRentalWithdrawnListing(rentalListing);
            }

            [Fact]
            public void GivenTheFileREASegmentResidentialCurrent_Convert_ReturnsAResidentialCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA-Segment-Residential-Current.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var listings = reaXmlTransmorgrifier.Convert(reaXml);

                // Assert.
                listings.Count.ShouldBe(1);

                var residentialCurrentListing = listings
                    .AsQueryable()
                    .WithId("Residential-Current-ABCD1234")
                    .OfType<ResidentialListing>()
                    .SingleOrDefault();
                AssertResidentialCurrentListing(residentialCurrentListing);
            }

            [Fact]
            public void GivenTheFileREASegmentRentalCurrent_Convert_ReturnsARentalCurrentListing()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA-Segment-Rental-Current.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var listings = reaXmlTransmorgrifier.Convert(reaXml);

                // Assert.
                listings.Count.ShouldBe(1);

                var rentalCurrentListing = listings
                    .AsQueryable()
                    .WithId("Rental-Current-ABCD1234")
                    .OfType<RentalListing>()
                    .SingleOrDefault();
                AssertRentalCurrentListing(rentalCurrentListing);
            }

            [Fact]
            public void GivenTheFileREABadContent_Convert_ThrowsAnException()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA-BadContent.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var exception = Should.Throw<Exception>(() => reaXmlTransmorgrifier.Convert(reaXml));

                // Assert.
                exception.ShouldNotBe(null);
                exception.Message.ShouldBe("Unable to parse the xml data provided. Currently, only a <propertyList/> or listing segments <residential/> / <rental/>. Root node found: 'badContent'.");
            }

            [Fact]
            public void GivenTheFileREAInvalidCharacterAndNoBadCharacterCleaning_Convert_ThrowsAnException()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA-InvalidCharacter.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var exception = Should.Throw<Exception>(() => reaXmlTransmorgrifier.Convert(reaXml));

                // Assert.
                exception.ShouldNotBe(null);
                exception.Message.ShouldBe(
                    "The REA Xml data provided contains some invalid characters. Line: 0, Position: 1661. Error: '\x16', hexadecimal value 0x16, is an invalid character. Suggested Solution: Either set the 'areBadCharactersRemoved' parameter to 'true' so invalid characters are removed automatically OR manually remove the errors from the file OR manually handle the error (eg. notify the people who sent you this data, that it contains bad data and they should clean it up.)");
            }

            [Fact]
            public void GivenTheFileREAInvalidCharacterAndBadCharacterCleaning_Convert_ThrowsAnException()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\REA-InvalidCharacter.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var listings = reaXmlTransmorgrifier.Convert(reaXml, true);

                // Assert.
                listings.Count.ShouldBe(1);

                var residentialCurrentListing = listings
                    .AsQueryable()
                    .WithId("Residential-Current-ABCD1234")
                    .OfType<ResidentialListing>()
                    .SingleOrDefault();
                residentialCurrentListing.ShouldNotBe(null);
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

                var errors = new Dictionary<string, string>();
                listing.Validate(errors);
                errors.Count.ShouldBe(0);
            }

            private static void AssertResidentialWithdrawnListing(ResidentialListing listing)
            {
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Residential-Withdrawn-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Withdrawn);

                var errors = new Dictionary<string, string>();
                listing.Validate(errors);
                errors.Count.ShouldBe(0);
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

                var errors = new Dictionary<string, string>();
                listing.Validate(errors);
                errors.Count.ShouldBe(0);
            }

            private static void AssertRentalWithdrawnListing(RentalListing listing)
            {
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Rental-Withdrawn-ABCD1234");
                listing.StatusType.ShouldBe(StatusType.Withdrawn);

                var errors = new Dictionary<string, string>();
                listing.Validate(errors);
                errors.Count.ShouldBe(0);
            }
        }
    }
}