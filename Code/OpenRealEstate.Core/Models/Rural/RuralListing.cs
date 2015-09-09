using System;
using System.Diagnostics;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models.Rural
{
    public class RuralListing : Listing
    {
        private const string AuctionOnName = "AuctionOn";
        private const string BuildingDetailsName = "BuildingDetails";
        private const string CategoryTypeName = "CategoryType";
        private const string CouncilRatesName = "CouncilRates";
        private const string PricingName = "Pricing";
        private const string RuralFeaturesName = "RuralFeatures";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DateTimeNullableNotified _auctionOn;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly InstanceObjectNotified<BuildingDetails> _buildingDetails;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly EnumNotified<CategoryType> _categoryType;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _councilRates;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly InstanceObjectNotified<SalePricing> _pricing;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly InstanceObjectNotified<RuralFeatures> _ruralFeatures;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _isBuildingDetailsModified;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [Obsolete]
        private bool _isPricingModified;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [Obsolete]
        private bool _isRuralFeaturesModified;

        public RuralListing()
        {
            _auctionOn = new DateTimeNullableNotified(AuctionOnName);
            _auctionOn.PropertyChanged += ModifiedData.OnPropertyChanged;

            _buildingDetails = new InstanceObjectNotified<BuildingDetails>(BuildingDetailsName);
            _buildingDetails.PropertyChanged += ModifiedData.OnPropertyChanged;

            _categoryType = new EnumNotified<CategoryType>(CategoryTypeName);
            _categoryType.PropertyChanged += ModifiedData.OnPropertyChanged;

            _councilRates = new StringNotified(CouncilRatesName);
            _councilRates.PropertyChanged += ModifiedData.OnPropertyChanged;

            _pricing = new InstanceObjectNotified<SalePricing>(PricingName);
            _pricing.PropertyChanged += ModifiedData.OnPropertyChanged;

            _ruralFeatures = new InstanceObjectNotified<RuralFeatures>(RuralFeaturesName);
            _ruralFeatures.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public CategoryType CategoryType
        {
            get { return _categoryType.Value; }
            set { _categoryType.Value = value; }
        }

        public bool IsCategoryTypeModified { get; set; }

        public SalePricing Pricing
        {
            get { return _pricing.Value; }
            set { _pricing.Value = value; }
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
        public bool IsAuctionOnModified { get; set; }

        public RuralFeatures RuralFeatures
        {
            get { return _ruralFeatures.Value; }
            set { _ruralFeatures.Value = value; }
        }

        [Obsolete]
        public bool IsRuralFeaturesModified
        {
            get
            {
                return _isRuralFeaturesModified ||
                       (RuralFeatures != null &&
                        RuralFeatures.IsModified);
            }
            set { _isRuralFeaturesModified = value; }
        }

        public string CouncilRates
        {
            get { return _councilRates.Value; }
            set { _councilRates.Value = value; }
        }

        [Obsolete]
        public bool IsCouncilRatesModified { get; set; }

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
            return string.Format("Rural >> {0}", base.ToString());
        }

        public override void ClearAllIsModified()
        {
            base.ClearAllIsModified();

            if (_buildingDetails != null &&
                _buildingDetails.Value.ModifiedData.IsModified)
            {
                _buildingDetails.Value.ClearAllIsModified();
            }

            if (_pricing != null &&
                _pricing.Value.ModifiedData.IsModified)
            {
                _pricing.Value.ClearAllIsModified();
            }

            if (_ruralFeatures != null &&
                _ruralFeatures.Value.ModifiedData.IsModified)
            {
                _ruralFeatures.Value.ClearAllIsModified();
            }

            ModifiedData.ClearModifiedProperties(new[]
            {
                AuctionOnName,
                BuildingDetailsName,
                CategoryTypeName,
                CouncilRatesName,
                PricingName,
                RuralFeaturesName
            });
        }
    }
}