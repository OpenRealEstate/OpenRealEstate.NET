using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenRealEstate.Core.Filters;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Services.RealEstate.com.au;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests
{
    public class TransmorgrifierFacts
    {
        public class ReaXmlTransmorgrifierFacts
        {
            [Fact]
            public void GivenAnReaXmlFile_Convert_ReturnsAListOfListings()
            {
                // Arrange.
                var reaXml = File.ReadAllText("Sample Data\\Transmorgrifiers\\Rea.xml");
                var reaXmlTransmorgrifier = new ReaXmlTransmorgrifier();

                // Act.
                var listings = reaXmlTransmorgrifier.Convert(reaXml);

                // Assert.
                listings.Count.ShouldBe(6);

                var residentialListing = listings
                    .AsQueryable()
                    .WithId("Residential-ABCD1234")
                    .OfType<ResidentialListing>()
                    .SingleOrDefault();
                residentialListing.ShouldNotBe(null);

                AssertResidentialListing(residentialListing);

                var rentalListing = listings
                    .AsQueryable()
                    .WithId("Rental-ABCD1234")
                    .OfType<RentalListing>()
                    .SingleOrDefault();

                AssertRentalListing(rentalListing);
            }

            private static void AssertResidentialListing(ResidentialListing listing)
            {
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Residential-ABCD1234");
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

            private static void AssertRentalListing(RentalListing listing)
            {
                listing.AgencyId.ShouldBe("XNWXNW");
                listing.Id.ShouldBe("Rental-ABCD1234");
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
        }
    }
}