using System;

namespace OpenRealEstate.Core.Models
{
    public class SalePricing
    {
        public decimal SalePrice { get; set; }
        public string SalePriceText { get; set; }
        public decimal? SoldPrice { get; set; }
        public string SoldPriceText { get; set; }
        public DateTime? SoldOn { get; set; }
        public bool IsUnderOffer { get; set; }
    }
}