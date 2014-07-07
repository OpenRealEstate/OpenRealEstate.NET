using System;
using System.Collections.Generic;
using System.Text;

namespace OpenRealEstate.Core.Models
{
    public class Address : IValidate
    {
        public string StreetNumber { get; set; }
        public string Street { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }

        /// <remarks>More Info: http://en.wikipedia.org/wiki/ISO_3166-1</remarks>
        public string CountryIsoCode { get; set; }

        public string Postcode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool IsStreetDisplayed { get; set; }

        public void Validate(Dictionary<string, string> errors, string keySuffix = null)
        {
            if (errors == null)
            {
                throw new ArgumentNullException("errors");
            }

            // We can have a string.Empty keySuffix, which means do have a key to postpend.
            if (keySuffix == null)
            {
                throw new ArgumentNullException("keySuffix");
            }

            if (string.IsNullOrWhiteSpace(Suburb))
            {
                errors.Add("Suburb" + keySuffix, "A Suburb is required. Eg. Ivanhoe or Pott's Point.");
            }

            if (string.IsNullOrWhiteSpace(State))
            {
                errors.Add("State" + keySuffix, "A State is required. Eg. Victoria or New South Wales.");
            }

            if (string.IsNullOrWhiteSpace(CountryIsoCode))
            {
                errors.Add("CountryIsoCode" + keySuffix, "A Country ISO code is required. eg. AU, NZ, etc.");
            }
        }

        public override string ToString()
        {
            var address = new StringBuilder();
            if (!IsStreetDisplayed)
            {
                address.Append("(*Hidden*)");
            }
            address.Append(StreetNumber);
            AppendDelimeter(address, "/");
            address.Append(Street);
            AppendDelimeter(address);
            address.Append(Suburb);
            AppendDelimeter(address);
            address.Append(State);
            AppendDelimeter(address);
            address.Append(CountryIsoCode);
            address.AppendFormat("; Lat: {0} Long: {1}",
                Latitude.HasValue
                    ? Latitude.Value.ToString("n3")
                    : "-",
                Longitude.HasValue
                    ? Longitude.Value.ToString("n3")
                    : "-");

            return address.ToString();
        }

        private static void AppendDelimeter(StringBuilder stringBuilder, string delimeter = ", ")
        {
            if (stringBuilder == null)
            {
                throw new ArgumentNullException();
            }

            if (stringBuilder.Length > 0)
            {
                stringBuilder.Append(delimeter);
            }
        }
    }
}