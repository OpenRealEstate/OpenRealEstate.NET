using System;
using System.Diagnostics;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models.Land
{
    public class LandListing : Listing
    {
        private const string AuctionOnName = "AuctionOn";
        private const string CategoryTypeName = "CategoryType";
        private const string CouncilRatesName = "CouncilRates";
        private const string EstateName = "Estate";
        private const string SalePricingName = "Pricing";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DateTimeNullableNotified _auctionOn;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly EnumNotified<CategoryType> _categoryType;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _councilRates;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly InstanceObjectNotified<LandEstate> _estate;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly InstanceObjectNotified<SalePricing> _pricing;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [Obsolete]
        private bool _isEstateModified;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [Obsolete]
        private bool _isPricingModified;

        public LandListing()
        {
            _auctionOn = new DateTimeNullableNotified(AuctionOnName);
            _auctionOn.PropertyChanged += ModifiedData.OnPropertyChanged;

            _categoryType = new EnumNotified<CategoryType>(CategoryTypeName);
            _categoryType.PropertyChanged += ModifiedData.OnPropertyChanged;

            _councilRates = new StringNotified(CouncilRatesName);
            _councilRates.PropertyChanged += ModifiedData.OnPropertyChanged;

            _estate = new InstanceObjectNotified<LandEstate>(EstateName);
            _estate.PropertyChanged += ModifiedData.OnPropertyChanged;

            _pricing = new InstanceObjectNotified<SalePricing>(SalePricingName);
            _pricing.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public CategoryType CategoryType
        {
            get { return _categoryType.Value; }
            set { _categoryType.Value = value; }
        }

        [Obsolete]
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

        public LandEstate Estate
        {
            get { return _estate.Value; }
            set { _estate.Value = value; }
        }

        [Obsolete]
        public bool IsEstateModified
        {
            get
            {
                return _isEstateModified ||
                       (Estate != null &&
                        Estate.IsModified);
            }
            set { _isEstateModified = value; }
        }

        public string CouncilRates
        {
            get { return _councilRates.Value; }
            set { _councilRates.Value = value; }
        }

        [Obsolete]
        public bool IsCouncilRatesModified { get; set; }

        public override bool IsModified
        {
            get
            {
                return base.IsModified ||
                       IsCategoryTypeModified ||
                       IsPricingModified ||
                       IsAuctionOnModified ||
                       IsEstateModified ||
                       IsCouncilRatesModified;
            }
        }

        public override string ToString()
        {
            return string.Format("Land >> {0}", base.ToString());
        }

        public void Copy(LandListing newLandListing)
        {
            if (newLandListing == null)
            {
                throw new ArgumentNullException("newLandListing");
            }

            ModifiedData.Copy(newLandListing, this);

            if (_estate != null &&
                _estate.Value.ModifiedData.IsModified)
            {
                _estate.Value.ModifiedData.Copy(newLandListing.Estate, Estate);
            }

            if (_pricing != null &&
                _pricing.Value.ModifiedData.IsModified)
            {
                _pricing.Value.ModifiedData.Copy(newLandListing.Pricing, Pricing);
            }
        }

        public override void ClearAllIsModified()
        {
            base.ClearAllIsModified();

            ModifiedData.ClearModifiedProperties(new[]
            {
                AuctionOnName,
                CategoryTypeName,
                CouncilRatesName,
                EstateName,
                SalePricingName
            });
        }
    }
}