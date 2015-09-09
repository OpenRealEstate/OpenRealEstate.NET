using System;
using System.Diagnostics;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models.Rental
{
    public class RentalPricing : IModifiedData
    {
        private const string BondName = "Bond";
        private const string PaymentFrequencyTypeName = "PaymentFrequencyType";
        private const string RentalPriceName = "RentalPrice";
        private const string RentalPriceTextName = "RentalPriceText";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DecimalNullableNotified _bond;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly EnumNotified<PaymentFrequencyType> _paymentFrequencyType;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DecimalNotified _rentalPrice;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _rentalPriceText;

        public RentalPricing()
        {
            ModifiedData = new ModifiedData(GetType());

            _bond = new DecimalNullableNotified(BondName);
            _bond.PropertyChanged += ModifiedData.OnPropertyChanged;

            _paymentFrequencyType = new EnumNotified<PaymentFrequencyType>(PaymentFrequencyTypeName);
            _paymentFrequencyType.PropertyChanged += ModifiedData.OnPropertyChanged;

            _rentalPrice = new DecimalNotified(RentalPriceName);
            _rentalPrice.PropertyChanged += ModifiedData.OnPropertyChanged;

            _rentalPriceText = new StringNotified(RentalPriceTextName);
            _rentalPriceText.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public ModifiedData ModifiedData { get; private set; }

        public decimal RentalPrice
        {
            get { return _rentalPrice.Value; }
            set { _rentalPrice.Value = value; }
        }

        public PaymentFrequencyType PaymentFrequencyType
        {
            get { return _paymentFrequencyType.Value; }
            set { _paymentFrequencyType.Value = value; }
        }

        public string RentalPriceText
        {
            get { return _rentalPriceText.Value; }
            set { _rentalPriceText.Value = value; }
        }

        public decimal? Bond
        {
            get { return _bond.Value; }
            set { _bond.Value = value; }
        }

        public void Copy(RentalPricing newRentalPricing)
        {
            if (newRentalPricing == null)
            {
                throw new ArgumentNullException("newRentalPricing");
            }

            ModifiedData.Copy(newRentalPricing, this);
        }

        public void ClearAllIsModified()
        {
            ModifiedData.ClearModifiedPropertiesAndCollections();
        }
    }
}