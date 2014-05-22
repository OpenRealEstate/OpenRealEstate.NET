using System;

namespace OpenRealEstate.Core.Models
{
    public class RentalPricing
    {
        public decimal RentalPrice { get; set; }
        public PaymentFrequencyType PaymentFrequencyType { get; set; }
        public string RentalPriceText { get; set; }
        public decimal Bond { get; set; }
    }
}