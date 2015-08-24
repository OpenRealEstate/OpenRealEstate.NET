using System;
using System.Text;

namespace OpenRealEstate.Core.Models
{
    public class Address
    {
        private string _countryIsoCode;
        private bool _isStreetDisplayed;
        private decimal? _latitude;
        private decimal? _longitude;
        private string _municipality;
        private string _postcode;
        private string _state;
        private string _street;
        private string _streetNumber;
        private string _suburb;

        public string StreetNumber
        {
            get { return _streetNumber; }
            set
            {
                _streetNumber = value;
                IsStreetNumberModified = true;
            }
        }

        public bool IsStreetNumberModified { get; private set; }

        public string Street
        {
            get { return _street; }
            set
            {
                _street = value;
                IsStreetModified = true;
            }
        }

        public bool IsStreetModified { get; private set; }

        public string Suburb
        {
            get { return _suburb; }
            set
            {
                _suburb = value;
                IsSuburbModified = true;
            }
        }

        public bool IsSuburbModified { get; private set; }

        public string Municipality
        {
            get { return _municipality; }
            set
            {
                _municipality = value;
                IsMunicipalityModified = true;
            }
        }

        public bool IsMunicipalityModified { get; private set; }

        public string State
        {
            get { return _state; }
            set
            {
                _state = value;
                IsStateModified = true;
            }
        }

        public bool IsStateModified { get; private set; }

        /// <remarks>More Info: http://en.wikipedia.org/wiki/ISO_3166-1</remarks>
        public string CountryIsoCode
        {
            get { return _countryIsoCode; }
            set
            {
                _countryIsoCode = value;
                IsCountryIsoCodeModified = true;
            }
        }

        public bool IsCountryIsoCodeModified { get; private set; }

        public string Postcode
        {
            get { return _postcode; }
            set
            {
                _postcode = value;
                IsPostcodeModified = true;
            }
        }

        public bool IsPostcodeModified { get; private set; }

        public decimal? Latitude
        {
            get { return _latitude; }
            set
            {
                _latitude = value;
                IsLatitudeModified = true;
            }
        }

        public bool IsLatitudeModified { get; private set; }

        public decimal? Longitude
        {
            get { return _longitude; }
            set
            {
                _longitude = value;
                IsLongitudeModified = true;
            }
        }

        public bool IsLongitudeModified { get; private set; }

        public bool IsStreetDisplayed
        {
            get { return _isStreetDisplayed; }
            set
            {
                _isStreetDisplayed = value;
                IsIsStreetDisplayedModified = true;
            }
        }

        public bool IsIsStreetDisplayedModified { get; private set; }

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
                        ? Latitude.Value.ToString("n3")
                        : "-",
                    Longitude.HasValue
                        ? Longitude.Value.ToString("n3")
                        : "-");
            }

            return address.ToString();
        }

        public bool IsModified
        {
            get
            {
                return IsStreetNumberModified ||
                       IsStreetModified ||
                       IsSuburbModified ||
                       IsMunicipalityModified ||
                       IsStateModified ||
                       IsCountryIsoCodeModified ||
                       IsPostcodeModified ||
                       IsLatitudeModified ||
                       IsLongitudeModified ||
                       IsIsStreetDisplayedModified;
            }
        }

        public void Copy(Address newAddress)
        {
            if (newAddress == null)
            {
                throw new ArgumentNullException("newAddress");
            }

            if (newAddress.IsStreetNumberModified)
            {
                StreetNumber = newAddress.StreetNumber;
            }

            if (newAddress.IsStreetModified)
            {
                Street = newAddress.Street;
            }

            if (newAddress.IsSuburbModified)
            {
                Suburb = newAddress.Suburb;
            }

            if (newAddress.IsMunicipalityModified)
            {
                Municipality = newAddress.Municipality;
            }

            if (newAddress.IsStateModified)
            {
                State = newAddress.State;
            }

            if (newAddress.IsCountryIsoCodeModified)
            {
                CountryIsoCode = newAddress.CountryIsoCode;
            }

            if (newAddress.IsPostcodeModified)
            {
                Postcode = newAddress.Postcode;
            }

            if (newAddress.IsLatitudeModified)
            {
                Latitude = newAddress.Latitude;
            }

            if (newAddress.IsLongitudeModified)
            {
                Longitude = newAddress.Longitude;
            }

            if (newAddress.IsIsStreetDisplayedModified)
            {
                IsStreetDisplayed = newAddress.IsStreetDisplayed;
            }
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

        public void ClearAllIsModified()
        {
            IsStreetNumberModified = false;
            IsStreetModified = false;
            IsSuburbModified = false;
            IsMunicipalityModified = false;
            IsStateModified = false;
            IsCountryIsoCodeModified = false;
            IsPostcodeModified = false;
            IsLatitudeModified = false;
            IsLongitudeModified = false;
            IsIsStreetDisplayedModified = false;
        }
    }
}