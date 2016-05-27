using System;
using OpenRealEstate.Core;
using Ploeh.AutoFixture;

namespace OpenRealEstate.FakeData.Customizations
{
    public class AddressCustomization : ICustomization
    {
        private readonly string[] _streetNames = new[]
        {
            "Smith Street",
            "Park Street",
            "Holiday Lane",
            "Collins Street",
            "Sydney Road",
            "Stinky Place",
            "Albert Road",
            "Dead-Man's Alley",
            "Sunshine Court",
            "Little Bobby Tables Lane",
            ";DROP TABLE dbo.USERS;"
        };

        private readonly string[] _suburbNames = new[]
        {
            "Richmond",
            "Ivanhoe",
            "Collingwood",
            "Hawthorn",
            "Kew",
            "Abbotsford",
            "Fairfield",
            "Ivanhoe East",
            "South Yarra",
            "Brighton",
            "Eaglemont"
        };

        public void Customize(IFixture fixture)
        {
            fixture.Customize<Address>(c =>
                c.With(address => address.IsStreetDisplayed, true)
                    .With(address => address.CountryIsoCode, "AU")
                    .With(address => address.State, "VIC")
                    .With(address => address.Latitude, 10)
                    .With(address => address.Longitude, 10)
                    .With(address => address.Postcode, "12312323213123")
                    .Do(address =>
                    {
                        var random = new Random();
                        address.Postcode = "123123123";
                        //address.Postcode = random.Next(3000, 3999).ToString();
                    }));
        }
    }
}