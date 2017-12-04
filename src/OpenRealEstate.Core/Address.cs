﻿using System;
using System.Text;

namespace OpenRealEstate.Core
{
    public class Address
    {
        public string StreetNumber { get; set; }

        public string Street { get; set; }

        public string Suburb { get; set; }

        public string Municipality { get; set; }

        public string State { get; set; }

        /// <remarks>More Info: http://en.wikipedia.org/wiki/ISO_3166-1</remarks>
        public string CountryIsoCode { get; set; }

        public string Postcode { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public bool IsStreetDisplayed { get; set; }

        public override string ToString()
        {
            return ToString(true);
        }

        public string ToString(bool isLatLongIncluded)
        {
            var address = new StringBuilder();
            if (!IsStreetDisplayed)
            {
                address.Append("(*Hidden*)");
            }
            address.Append(StreetNumber);
            AppendDelimeter(address, " ");
            address.Append(Street);
            AppendDelimeter(address);
            address.Append(Suburb);
            AppendDelimeter(address);
            address.Append(State);
            AppendDelimeter(address);
            address.Append(CountryIsoCode);

            if (isLatLongIncluded)
            {
                address.AppendFormat("; Lat: {0} Long: {1}",
                    Latitude.HasValue
                        ? Latitude.Value.ToString("n5")
                        : "-",
                    Longitude.HasValue
                        ? Longitude.Value.ToString("n5")
                        : "-");
            }

            return address.ToString();
        }

        private static void AppendDelimeter(StringBuilder stringBuilder, string delimeter = ", ")
        {
            if (stringBuilder == null)
            {
                throw new ArgumentNullException(nameof(stringBuilder));
            }

            if (stringBuilder.Length > 0)
            {
                stringBuilder.Append(delimeter);
            }
        }
    }
}