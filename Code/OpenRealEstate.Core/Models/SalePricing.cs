using System;
using System.Diagnostics;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public class SalePricing
    {
        private const string IsUnderOfferName = "IsUnderOffer";
        private const string SalePriceName = "SalePrice";
        private const string SalePriceTextName = "SalePriceText";
        private const string SoldOnName = "SoldOn";
        private const string SoldPriceName = "SoldPrice";
        private const string SoldPriceTextName = "SoldPriceText";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly BooleanNotified _isUnderOffer;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DecimalNotified _salePrice;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _salePriceText;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DateTimeNullableNotified _soldOn;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DecimalNullableNotified _soldPrice;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _soldPriceText;

        public SalePricing()
        {
            ModifiedData = new ModifiedData(GetType());

            _isUnderOffer = new BooleanNotified(IsUnderOfferName);
            _isUnderOffer.PropertyChanged += ModifiedData.OnPropertyChanged;

            _salePrice = new DecimalNotified(SalePriceName);
            _salePrice.PropertyChanged += ModifiedData.OnPropertyChanged;

            _salePriceText = new StringNotified(SalePriceTextName);
            _salePriceText.PropertyChanged += ModifiedData.OnPropertyChanged;

            _soldOn = new DateTimeNullableNotified(SoldOnName);
            _soldOn.PropertyChanged += ModifiedData.OnPropertyChanged;

            _soldPrice = new DecimalNullableNotified(SoldPriceName);
            _soldPrice.PropertyChanged += ModifiedData.OnPropertyChanged;

            _soldPriceText = new StringNotified(SoldPriceTextName);
            _soldPriceText.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public ModifiedData ModifiedData { get; private set; }

        public decimal SalePrice
        {
            get { return _salePrice.Value; }
            set { _salePrice.Value = value; }
        }

        public string SalePriceText
        {
            get { return _salePriceText.Value; }
            set { _salePriceText.Value = value; }
        }

        public decimal? SoldPrice
        {
            get { return _soldPrice.Value; }
            set { _soldPrice.Value = value; }
        }

        public string SoldPriceText
        {
            get { return _soldPriceText.Value; }
            set { _soldPriceText.Value = value; }
        }

        public DateTime? SoldOn
        {
            get { return _soldOn.Value; }
            set { _soldOn.Value = value; }
        }

        public bool IsUnderOffer
        {
            get { return _isUnderOffer.Value; }
            set { _isUnderOffer.Value = value; }
        }

        public void Copy(SalePricing newSalePricing)
        {
            ModifiedData.Copy(newSalePricing, this);
        }

        public void ClearAllIsModified()
        {
            ModifiedData.ClearModifiedPropertiesAndCollections();
        }
    }
}