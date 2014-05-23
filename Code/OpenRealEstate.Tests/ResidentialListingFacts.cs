using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using OpenRealEstate.Core.Models;
using Shouldly;
using Xunit;

namespace OpenRealEstate.Tests
{
    public class ResidentialListingFacts
    {
        public class DeserializationFacts
        {
            [Fact]
            public void GivenAFileResidentialCurrentSampleAgency1Ref1111_Deserialization_ValidatesAListing()
            {
                // Arrange.
                string json =
                    File.ReadAllText("Sample Data\\Listings\\Residential-Current-Sample.Agency.1-Ref1111.json");

                // Act.
                var listing = JsonConvert.DeserializeObject<ResidentialListing>(json);

                // Assert.
                listing.ShouldNotBe(null);

                listing.AgencyId.ShouldBe("Sample.Agency.1");
                listing.Id.ShouldBe("Ref1111");

                listing.Address.StreetNumber.ShouldBe("1");
                listing.Address.Street.ShouldBe("Upper Heidelberg Road");
                listing.Address.Suburb.ShouldBe("Ivanhoe");
                listing.Address.State.ShouldBe("VIC");
                listing.Address.CountryIsoCode.ShouldBe("AU");
                listing.Address.Postcode.ShouldBe("3079");
                listing.Address.IsStreetDisplayed.ShouldBe(true);
                listing.Address.Latitude.ShouldBe(-11.22m);
                listing.Address.Longitude.ShouldBe(33.44m);

                listing.Pricing.SalePrice.ShouldBe(500100.99m);
                listing.Pricing.SalePriceText.ShouldBe("Contact Agent");
                listing.Pricing.SoldOn.ShouldBe(null);
                listing.Pricing.SoldPrice.ShouldBe(null);
                listing.Pricing.SoldPriceText.ShouldBe(null);

                listing.Features.Bedrooms.ShouldBe(1);
                listing.Features.Bathrooms.ShouldBe(2);
                listing.Features.CarSpaces.ShouldBe(3);

                listing.Agents.Count.ShouldBe(2);

                listing.Images.Count.ShouldBe(5);
                listing.FloorPlans.Count.ShouldBe(2);
                listing.Videos.ShouldBe(null);
            }
        }
    }

    public class ValidationFacts
    {
        [Fact]
        public void GivenAListingWithNoErrors_Validation_Validates()
        {
            // Arrange.
            string json =
                File.ReadAllText("Sample Data\\Listings\\Residential-Current-Sample.Agency.1-Ref1111.json");
            var listing = JsonConvert.DeserializeObject<ResidentialListing>(json);
            var errors = new Dictionary<string, string>();

            // Act.
            listing.Validate(errors);

            // Assert.
            errors.Count.ShouldBe(0);
        }
    }
}