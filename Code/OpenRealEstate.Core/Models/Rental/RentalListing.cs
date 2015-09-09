using System;
using System.Diagnostics;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models.Rental
{
    public class RentalListing : Listing
    {
        private const string AvailableOnName = "AvailableOn";
        private const string BuildingDetailsName = "BuildingDetails";
        private const string RentalPricingName = "Pricing";
        private const string PropertyTypeName = "PropertyType";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DateTimeNullableNotified _availableOn;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly InstanceObjectNotified<BuildingDetails> _buildingDetails;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly InstanceObjectNotified<RentalPricing> _pricing;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly EnumNotified<PropertyType> _propertyType;

        public RentalListing()
        {
            _availableOn= new DateTimeNullableNotified(AvailableOnName);
            _availableOn.PropertyChanged += ModifiedData.OnPropertyChanged;

            _buildingDetails = new InstanceObjectNotified<BuildingDetails>(BuildingDetailsName);
            _buildingDetails.PropertyChanged += ModifiedData.OnPropertyChanged;

            _pricing = new InstanceObjectNotified<RentalPricing>(RentalPricingName);
            _pricing.PropertyChanged += ModifiedData.OnPropertyChanged;

            _propertyType = new EnumNotified<PropertyType>(PropertyTypeName);
            _propertyType.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public PropertyType PropertyType
        {
            get { return _propertyType.Value; }
            set { _propertyType.Value = value; }
        }

        public DateTime? AvailableOn
        {
            get { return _availableOn.Value; }
            set { _availableOn.Value = value; }
        }

        public RentalPricing Pricing
        {
            get { return _pricing.Value; }
            set { _pricing.Value = value; }
        }

        public BuildingDetails BuildingDetails
        {
            get { return _buildingDetails.Value; }
            set { _buildingDetails.Value = value; }
        }

        public override string ToString()
        {
            return string.Format("Rental >> {0}", base.ToString());
        }

        public override void ClearAllIsModified()
        {
            base.ClearAllIsModified();

            if (_buildingDetails.Value != null &&
                _buildingDetails.Value.ModifiedData.IsModified)
            {
                _buildingDetails.Value.ClearAllIsModified();
            }

            if (_pricing.Value != null && 
                _pricing.Value.ModifiedData.IsModified)
            {
                _pricing.Value.ClearAllIsModified();
            }
        }
    }
}