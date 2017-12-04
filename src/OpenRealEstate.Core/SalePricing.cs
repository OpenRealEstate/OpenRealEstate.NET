using System;

namespace OpenRealEstate.Core
{
    public class SalePricing
    {
        public decimal SalePrice { get; set; }

        public string SalePriceText { get; set; }

        public decimal? SoldPrice { get; set; }

        public string SoldPriceText { get; set; }

        public DateTime? SoldOn { get; set; }

        public bool IsUnderOffer { get; set; }

        public override string ToString()
        {
            return
                $"Sale: {SalePrice:C0}/{(string.IsNullOrWhiteSpace(SalePriceText) ? "-no sale price text-" : SalePriceText)} | Sold: {(SoldPrice.HasValue ? SoldPrice.Value.ToString("C0") : "-not sold-")}/{(string.IsNullOrWhiteSpace(SoldPriceText) ? "-no sold price text-" : SoldPriceText)}";
        }
    }
}