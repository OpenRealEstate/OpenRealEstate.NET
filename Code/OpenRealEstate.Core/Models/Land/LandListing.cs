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

        public override string ListingType
        {
            get { return "Land"; }
        }

        public CategoryType CategoryType
        {
            get { return _categoryType.Value; }
            set { _categoryType.Value = value; }
        }

        public SalePricing Pricing
        {
            get { return _pricing.Value; }
            set { _pricing.Value = value; }
        }

        public DateTime? AuctionOn
        {
            get { return _auctionOn.Value; }
            set { _auctionOn.Value = value; }
        }

        public LandEstate Estate
        {
            get { return _estate.Value; }
            set { _estate.Value = value; }
        }

        public string CouncilRates
        {
            get { return _councilRates.Value; }
            set { _councilRates.Value = value; }
        }

        public override string ToString()
        {
            return string.Format("Land >> {0}", base.ToString());
        }

        public void Copy(LandListing newLandListing, bool isModifiedPropertiesOnly = true)
        {
            if (newLandListing == null)
            {
                throw new ArgumentNullException("newLandListing");
            }

            ModifiedData.Copy(newLandListing, this, isModifiedPropertiesOnly);
        }

        public override void ClearAllIsModified()
        {
            base.ClearAllIsModified();

            if (_estate.Value != null &&
                _estate.Value.ModifiedData.IsModified)
            {
                _estate.Value.ClearAllIsModified();
            }

            if (_pricing.Value != null &&
                _pricing.Value.ModifiedData.IsModified)
            {
                _pricing.Value.ClearAllIsModified();
            }
        }
    }
}