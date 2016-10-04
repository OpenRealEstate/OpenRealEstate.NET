using System;
using OpenRealEstate.Core;
using Ploeh.AutoFixture;

namespace OpenRealEstate.FakeData.Customizations
{
    public class AddressCustomization : ICustomization
    {
        private static readonly Random Randomizer = new Random();

        private static readonly string[] Streets =
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

        private static readonly string[] Suburbs =
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

        private static readonly string[] Municipalities =
        {
            "City of Melbourne",
            "City of Port Phillip",
            "City of Stonnington",
            "City of Yarra",
            "City of Banyule",
            "City of Bayside",
            "City of Boroondara",
            "City of Brimbank",
            "City of Darebin",
            "City of Glen Eira",
            "City of Hobsons Bay"
        };

        public void Customize(IFixture fixture)
        {
            fixture.Customize<Address>(c =>
                    c.With(address => address.IsStreetDisplayed, true)
                        .With(address => address.StreetNumber, Randomizer.Next(1, 200).ToString())
                        .With(address => address.Street, Streets[Randomizer.Next(0, Streets.Length)])
                        .With(address => address.Suburb, Suburbs[Randomizer.Next(0, Suburbs.Length)])
                        .With(address => address.Municipality, Municipalities[Randomizer.Next(0, Municipalities.Length)])
                        .With(address => address.CountryIsoCode, "AU")
                        .With(address => address.State, "VIC")
                        .With(address => address.Latitude, 10)
                        .With(address => address.Longitude, 10)
                        .With(address => address.Postcode, Randomizer.Next(3000, 3999).ToString())
            );
        }
    }
}