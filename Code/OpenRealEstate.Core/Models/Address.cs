using System;
using System.Diagnostics;
using System.Text;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class Address : BaseModifiedData
    {
        private const string CountryIsoCodeName = "CountryIsoCode";
        private const string IsStreetDisplayedName = "IsStreetDisplayed";
        private const string LatitudeName = "Latitude";
        private const string LongitudeName = "Longitude";
        private const string MunicipalityName = "Municipality";
        private const string PostcodeName = "Postcode";
        private const string StateName = "State";
        private const string StreetName = "Street";
        private const string StreetNumberName = "StreetNumber";
        private const string SuburbName = "Suburb";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _countryIsoCode;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly BooleanNotified _isStreetDisplayed;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DecimalNullableNotified _latitude;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DecimalNullableNotified _longitude;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _municipality;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _postcode;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _state;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _street;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _streetNumber;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _suburb;

        public Address()
        {
            _countryIsoCode = new StringNotified(CountryIsoCodeName);
            _countryIsoCode.PropertyChanged += ModifiedData.OnPropertyChanged;

            _isStreetDisplayed = new BooleanNotified(IsStreetDisplayedName);
            _isStreetDisplayed.PropertyChanged += ModifiedData.OnPropertyChanged;

            _latitude = new DecimalNullableNotified(LatitudeName);
            _latitude.PropertyChanged += ModifiedData.OnPropertyChanged;

            _longitude = new DecimalNullableNotified(LongitudeName);
            _longitude.PropertyChanged += ModifiedData.OnPropertyChanged;

            _municipality = new StringNotified(MunicipalityName);
            _municipality.PropertyChanged += ModifiedData.OnPropertyChanged;

            _postcode = new StringNotified(PostcodeName);
            _postcode.PropertyChanged += ModifiedData.OnPropertyChanged;

            _state = new StringNotified(StateName);
            _state.PropertyChanged += ModifiedData.OnPropertyChanged;

            _street = new StringNotified(StreetName);
            _street.PropertyChanged += ModifiedData.OnPropertyChanged;

            _streetNumber = new StringNotified(StreetNumberName);
            _streetNumber.PropertyChanged += ModifiedData.OnPropertyChanged;

            _suburb = new StringNotified(SuburbName);
            _suburb.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public string StreetNumber
        {
            get { return _streetNumber.Value; }
            set { _streetNumber.Value = value; }
        }

        public string Street
        {
            get { return _street.Value; }
            set { _street.Value = value; }
        }

        public string Suburb
        {
            get { return _suburb.Value; }
            set { _suburb.Value = value; }
        }

        public string Municipality
        {
            get { return _municipality.Value; }
            set { _municipality.Value = value; }
        }

        public string State
        {
            get { return _state.Value; }
            set { _state.Value = value; }
        }

        /// <remarks>More Info: http://en.wikipedia.org/wiki/ISO_3166-1</remarks>
        public string CountryIsoCode
        {
            get { return _countryIsoCode.Value; }
            set { _countryIsoCode.Value = value; }
        }

        public string Postcode
        {
            get { return _postcode.Value; }
            set { _postcode.Value = value; }
        }

        public decimal? Latitude
        {
            get { return _latitude.Value; }
            set { _latitude.Value = value; }
        }

        public decimal? Longitude
        {
            get { return _longitude.Value; }
            set { _longitude.Value = value; }
        }

        public bool IsStreetDisplayed
        {
            get { return _isStreetDisplayed.Value; }
            set { _isStreetDisplayed.Value = value; }
        }

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

        public void Copy(Address newAddress, bool isModifiedPropertiesOnly = true)
        {
            ModifiedData.Copy(newAddress, this, isModifiedPropertiesOnly);
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
            ModifiedData.ClearModifiedPropertiesAndCollections();
        }
    }
}