using System;
using System.Collections.Generic;

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
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsStreetDisplayed { get; set; }

        public void Validate(Dictionary<string, string> errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException("errors");
            }

            if (string.IsNullOrWhiteSpace(Suburb))
            {
                errors.Add("Suburb", "A Suburb is required. Eg. Ivanhoe or Pott's Point.");
            }

            if (string.IsNullOrWhiteSpace(State))
            {
                errors.Add("State", "A State is required. Eg. Victoria or New South Wales.");
            }

            if (string.IsNullOrWhiteSpace(CountryIsoCode))
            {
                errors.Add("CountryIsoCode", "A Country ISO code is required. eg. AU, NZ, etc.");
            }
        }
    }
}