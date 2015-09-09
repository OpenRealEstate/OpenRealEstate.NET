using System;
using System.Text;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class Address
    {
        private readonly ModifiedData _modifiedData;
        private readonly StringNotified _countryIsoCode;
        private readonly BooleanNotified _isStreetDisplayed;
        private readonly DecimalNullableNotified _latitude;
        private readonly DecimalNullableNotified _longitude;
        private readonly StringNotified _municipality;
        private readonly StringNotified _postcode;
        private readonly StringNotified _state;
        private readonly StringNotified _street;
        private readonly StringNotified _streetNumber;
        private readonly StringNotified _suburb;
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

        public Address()
        {
            _modifiedData = new ModifiedData(GetType());
            
            _countryIsoCode = new StringNotified(CountryIsoCodeName);
            _countryIsoCode.PropertyChanged += _modifiedData.OnPropertyChanged;

            _isStreetDisplayed = new BooleanNotified(IsStreetDisplayedName);
            _isStreetDisplayed.PropertyChanged += _modifiedData.OnPropertyChanged;

            _latitude = new DecimalNullableNotified(LatitudeName);
            _latitude.PropertyChanged += _modifiedData.OnPropertyChanged;

            _longitude = new DecimalNullableNotified(LongitudeName);
            _longitude.PropertyChanged += _modifiedData.OnPropertyChanged;

            _municipality = new StringNotified(MunicipalityName);
            _municipality.PropertyChanged += _modifiedData.OnPropertyChanged;

            _postcode = new StringNotified(PostcodeName);
            _postcode.PropertyChanged += _modifiedData.OnPropertyChanged;

            _state = new StringNotified(StateName);
            _state.PropertyChanged += _modifiedData.OnPropertyChanged;

            _street = new StringNotified(StreetName);
            _street.PropertyChanged += _modifiedData.OnPropertyChanged;

            _streetNumber = new StringNotified(StreetNumberName);
            _streetNumber.PropertyChanged += _modifiedData.OnPropertyChanged;

            _suburb = new StringNotified(SuburbName);
            _suburb.PropertyChanged += _modifiedData.OnPropertyChanged;
        }

        public string StreetNumber
        {
            get { return _streetNumber.Value; }
            set { _streetNumber.Value = value; }
        }

        [Obsolete]
        public bool IsStreetNumberModified { get; private set; }

        public string Street
        {
            get { return _street.Value; }
            set { _street.Value = value; }
        }
        [Obsolete]
        public bool IsStreetModified { get; private set; }

        public string Suburb
        {
            get { return _suburb.Value; }
            set { _suburb.Value = value; }
        }
        [Obsolete]
        public bool IsSuburbModified { get; private set; }

        public string Municipality
        {
            get { return _municipality.Value; }
            set { _municipality.Value = value; }
        }
        [Obsolete]
        public bool IsMunicipalityModified { get; private set; }

        public string State
        {
            get { return _state.Value; }
            set { _state.Value = value; }
        }
        [Obsolete]
        public bool IsStateModified { get; private set; }

        /// <remarks>More Info: http://en.wikipedia.org/wiki/ISO_3166-1</remarks>
        public string CountryIsoCode
        {
            get { return _countryIsoCode.Value; }
            set { _countryIsoCode.Value = value; }
        }
        [Obsolete]
        public bool IsCountryIsoCodeModified { get; private set; }

        public string Postcode
        {
            get { return _postcode.Value; }
            set { _postcode.Value = value; }
        }
        [Obsolete]
        public bool IsPostcodeModified { get; private set; }

        public decimal? Latitude
        {
            get { return _latitude.Value; }
            set { _latitude.Value = value; }
        }
        [Obsolete]
        public bool IsLatitudeModified { get; private set; }

        public decimal? Longitude
        {
            get { return _longitude.Value; }
            set { _longitude.Value = value; }
        }
        [Obsolete]
        public bool IsLongitudeModified { get; private set; }

        public bool IsStreetDisplayed
        {
            get { return _isStreetDisplayed.Value; }
            set { _isStreetDisplayed.Value = value; }
        }
        [Obsolete]
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
            get { return _modifiedData.IsModified; }
        }

        public void Copy(Address newAddress)
        {
            _modifiedData.Copy(newAddress, this);
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
            _modifiedData.ClearModifiedProperties(new[]
            {
                CountryIsoCodeName,
                IsStreetDisplayedName,
                LatitudeName,
                LongitudeName,
                MunicipalityName,
                PostcodeName,
                StateName,
                StreetName,
                StreetNumberName,
                SuburbName
            });
        }
    }
}