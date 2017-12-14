using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using OpenRealEstate.Core;

namespace OpenRealEstate.FakeData
{
    public class FakeAddress
    {
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

        public static Address CreateAFakeAddress(string streetNumber = "2/39",
                                                 string street = "Main Road",
                                                 string suburb = "RICHMOND",
                                                 string municipality = "Yarra",
                                                 string state = "vic",
                                                 string countryIsoCode = "AU",
                                                 string postcode = "3121",
                                                 bool isStreetDisplayed = true,
                                                 decimal? latitude = null,
                                                 decimal? longitude = null)
        {
            if (string.IsNullOrWhiteSpace(streetNumber))
            {
                streetNumber = GetRandom.Int(1, 200).ToString();
            }

            if (string.IsNullOrWhiteSpace(street))
            {
                street = Streets[GetRandom.Int(0, Streets.Length - 1)];
            }

            if (string.IsNullOrWhiteSpace(suburb))
            {
                suburb = Suburbs[GetRandom.Int(0, Suburbs.Length - 1)];
            }

            if (string.IsNullOrWhiteSpace(municipality))
            {
                municipality = Municipalities[GetRandom.Int(0, Municipalities.Length - 1)];
            }

            if (string.IsNullOrWhiteSpace(countryIsoCode))
            {
                countryIsoCode = "AU";
            }

            if (string.IsNullOrWhiteSpace(state))
            {
                state = "VIC";
            }

            if (string.IsNullOrWhiteSpace(postcode))
            {
                postcode = GetRandom.Int(3000, 3999).ToString();
            }

            return Builder<Address>.CreateNew()
                                   .With(a => a.StreetNumber, streetNumber)
                                   .With(a => a.Street, street)
                                   .With(a => a.Suburb, suburb)
                                   .With(a => a.Municipality, municipality)
                                   .With(a => a.CountryIsoCode, countryIsoCode)
                                   .With(a => a.State, state)
                                   .With(a => a.Latitude, latitude)
                                   .With(a => a.Longitude, longitude)
                                   .With(a => a.Postcode, postcode)
                                   .With(a => a.DisplayAddress, null)
                                   .Build();
        }
    }
}
