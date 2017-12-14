using System;
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

        /// <summary>
        /// This is the address the agent wants to display to the public. It might not have the street number or appartment numbers or even the suburb!
        /// </summary>
        public string DisplayAddress { get; set; }

        public override string ToString()
        {
            return ToString(true);
        }

        public string ToString(bool isLatLongIncluded)
        {
            var address = new StringBuilder();
            
            address.Append(StreetNumber);
            address.PrependWithDelimeter(Street, " ");
            address.PrependWithDelimeter(Suburb);
            address.PrependWithDelimeter(State);
            address.PrependWithDelimeter(CountryIsoCode);

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

        
    }
}