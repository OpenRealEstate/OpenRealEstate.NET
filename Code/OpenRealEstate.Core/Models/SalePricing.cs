using System;

namespace OpenRealEstate.Core.Models
{
    public class SalePricing
    {
        private bool _isUnderOffer;
        private decimal _salePrice;
        private string _salePriceText;
        private DateTime? _soldOn;
        private decimal? _soldPrice;
        private string _soldPriceText;

        public decimal SalePrice
        {
            get { return _salePrice; }
            set
            {
                _salePrice = value;
                IsSalePriceModified = true;
            }
        }

        public bool IsSalePriceModified { get; private set; }

        public string SalePriceText
        {
            get { return _salePriceText; }
            set
            {
                _salePriceText = value;
                IsSalePriceTextModified = true;
            }
        }

        public bool IsSalePriceTextModified { get; private set; }

        public decimal? SoldPrice
        {
            get { return _soldPrice; }
            set
            {
                _soldPrice = value;
                IsSoldPriceModified = true;
            }
        }

        public bool IsSoldPriceModified { get; private set; }

        public string SoldPriceText
        {
            get { return _soldPriceText; }
            set
            {
                _soldPriceText = value;
                IsSoldPriceTextModified = true;
            }
        }

        public bool IsSoldPriceTextModified { get; private set; }

        public DateTime? SoldOn
        {
            get { return _soldOn; }
            set
            {
                _soldOn = value;
                IsSoldOnModified = true;
            }
        }

        public bool IsSoldOnModified { get; private set; }

        public bool IsUnderOffer
        {
            get { return _isUnderOffer; }
            set
            {
                _isUnderOffer = value;
                IsUnderOfferModified = true;
            }
        }

        public bool IsUnderOfferModified { get; private set; }

        public bool IsModifed
        {
            get
            {
                return IsUnderOfferModified ||
                       IsSalePriceModified ||
                       IsSalePriceTextModified ||
                       IsSoldOnModified ||
                       IsSoldPriceModified ||
                       IsSoldPriceTextModified;
            }
        }

        public void Copy(SalePricing newSalePricing)
        {
            if (newSalePricing == null)
            {
                throw new ArgumentNullException();
            }

            if (newSalePricing.IsSalePriceModified)
            {
                SalePrice = newSalePricing.SalePrice;
            }

            if (newSalePricing.IsSalePriceTextModified)
            {
                SalePriceText = newSalePricing.SalePriceText;
            }

            if (newSalePricing.IsSoldPriceModified)
            {
                SoldPrice = newSalePricing.SoldPrice;
            }

            if (newSalePricing.IsSoldPriceTextModified)
            {
                SoldPriceText = newSalePricing.SoldPriceText;
            }

            if (newSalePricing.IsSoldOnModified)
            {
                SoldOn = newSalePricing.SoldOn;
            }

            if (newSalePricing.IsUnderOffer)
            {
                IsUnderOffer = newSalePricing.IsUnderOffer;
            }
        }

        public void ClearAllIsModified()
        {
            IsSalePriceModified = false;
            IsSalePriceTextModified = false;
            IsSoldPriceModified = false;
            IsSoldPriceTextModified = false;
            IsSoldOnModified = false;
            IsUnderOfferModified = false;
        }
    }
}