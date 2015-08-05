using System;

namespace OpenRealEstate.Core.Models.Rental
{
    public class RentalPricing
    {
        private decimal? _bond;
        private PaymentFrequencyType _paymentFrequencyType;
        private decimal _rentalPrice;
        private string _rentalPriceText;

        public decimal RentalPrice
        {
            get { return _rentalPrice; }
            set
            {
                _rentalPrice = value;
                IsRentalPriceModified = true;
            }
        }

        public bool IsRentalPriceModified { get; set; }

        public PaymentFrequencyType PaymentFrequencyType
        {
            get { return _paymentFrequencyType; }
            set
            {
                _paymentFrequencyType = value;
                IsPaymentFrequencyTypeModified = true;
            }
        }

        public bool IsPaymentFrequencyTypeModified { get; set; }

        public string RentalPriceText
        {
            get { return _rentalPriceText; }
            set
            {
                _rentalPriceText = value;
                IsRentalPriceTextModified = true;
            }
        }

        public bool IsRentalPriceTextModified { get; set; }

        public decimal? Bond
        {
            get { return _bond; }
            set
            {
                _bond = value;
                IsBondModified = true;
            }
        }

        public bool IsBondModified { get; set; }

        public void CopyOverNewData(RentalPricing newRentalPricing)
        {
            if (newRentalPricing == null)
            {
                throw new ArgumentNullException("newRentalPricing");
            }

            if (newRentalPricing.IsRentalPriceModified)
            {
                RentalPrice = newRentalPricing.RentalPrice;
            }

            if (newRentalPricing.IsPaymentFrequencyTypeModified)
            {
                PaymentFrequencyType = newRentalPricing.PaymentFrequencyType;
            }

            if (newRentalPricing.IsRentalPriceTextModified)
            {
                RentalPriceText = newRentalPricing.RentalPriceText;
            }

            if (newRentalPricing.IsBondModified)
            {
                Bond = newRentalPricing.Bond;
            }
        }

        public void ClearAllIsModified()
        {
            IsRentalPriceModified = false;
            IsPaymentFrequencyTypeModified = false;
            IsRentalPriceTextModified = false;
            IsBondModified = false;
        }
    }
}