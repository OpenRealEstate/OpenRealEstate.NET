using System;
using System.Diagnostics;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models.Residential
{
    public class ResidentialListing : Listing
    {
        private const string AuctionOnName = "AuctionOn";
        private const string BuildingDetailsName = "BuildingDetails";
        private const string CouncilRatesName = "CouncilRates";
        private const string PropertyTypeName = "PropertyType";
        private const string SalePricingName = "Pricing";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly DateTimeNullableNotified _auctionOn;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly InstanceObjectNotified<BuildingDetails>
            _buildingDetails;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly StringNotified _councilRates;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly EnumNotified<PropertyType> _propertyType;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly InstanceObjectNotified<SalePricing>
            _salePricing;

        [Obsolete] [DebuggerBrowsable(DebuggerBrowsableState.Never)] private bool _isBuildingDetailsModified;

        [Obsolete] [DebuggerBrowsable(DebuggerBrowsableState.Never)] private bool _isPricingModified;

        public ResidentialListing()
        {
            _auctionOn = new DateTimeNullableNotified(AuctionOnName);
            _auctionOn.PropertyChanged += ModifiedData.OnPropertyChanged;

            _buildingDetails = new InstanceObjectNotified<BuildingDetails>(BuildingDetailsName);
            _buildingDetails.PropertyChanged += ModifiedData.OnPropertyChanged;

            _councilRates = new StringNotified(CouncilRatesName);
            _councilRates.PropertyChanged += ModifiedData.OnPropertyChanged;

            _propertyType = new EnumNotified<PropertyType>(PropertyTypeName);
            _propertyType.PropertyChanged += ModifiedData.OnPropertyChanged;

            _salePricing = new InstanceObjectNotified<SalePricing>(SalePricingName);
            _salePricing.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public PropertyType PropertyType
        {
            get { return _propertyType.Value; }
            set { _propertyType.Value = value; }
        }

        [Obsolete]
        public bool IsPropertyTypeModified { get; private set; }

        public SalePricing Pricing
        {
            get { return _salePricing.Value; }
            set { _salePricing.Value = value; }
        }

        [Obsolete]
        public bool IsPricingModified
        {
            get
            {
                return _isPricingModified ||
                       (Pricing != null &&
                        Pricing.IsModified);
            }
            set { _isPricingModified = value; }
        }

        public DateTime? AuctionOn
        {
            get { return _auctionOn.Value; }
            set { _auctionOn.Value = value; }
        }

        [Obsolete]
        public bool IsAuctionOnModified { get; private set; }

        public string CouncilRates
        {
            get { return _councilRates.Value; }
            set { _councilRates.Value = value; }
        }

        [Obsolete]
        public bool IsCouncilRatesModified { get; private set; }

        public BuildingDetails BuildingDetails
        {
            get { return _buildingDetails.Value; }
            set { _buildingDetails.Value = value; }
        }

        [Obsolete]
        public bool IsBuildingDetailsModified
        {
            get
            {
                return _isBuildingDetailsModified ||
                       (BuildingDetails != null &&
                        BuildingDetails.IsModified);
            }
            set { _isBuildingDetailsModified = value; }
        }

        public override bool IsModified
        {
            get { return ModifiedData.IsModified; }
        }

        public override string ToString()
        {
            return string.Format("Residential >> {0}", base.ToString());
        }

        public override void ClearAllIsModified()
        {
            base.ClearAllIsModified();

            if (_buildingDetails != null &&
                _buildingDetails.Value.IsModified)
            {
                _buildingDetails.Value.ClearAllIsModified();
            }

            if (_salePricing != null &&
                _salePricing.Value.IsModified)
            {
                _salePricing.Value.ClearAllIsModified();
            }

            ModifiedData.ClearModifiedProperties(new[]
            {
                AuctionOnName,
                BuildingDetailsName,
                CouncilRatesName,
                PropertyTypeName,
                SalePricingName
            });
        }
    }
}