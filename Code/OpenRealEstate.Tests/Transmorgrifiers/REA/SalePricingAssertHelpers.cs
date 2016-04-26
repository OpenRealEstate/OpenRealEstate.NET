using OpenRealEstate.Core;
using Shouldly;

namespace OpenRealEstate.Tests.Transmorgrifiers.REA
{
    public static class SalePricingAssertHelpers
    {
        public static void AssertSalePrice(SalePricing source, SalePricing destination)
        {
            if (source == null &&
                destination == null)
            {
                return;
            }

            source.SalePrice.ShouldBe(destination.SalePrice);
            source.SalePriceText.ShouldBe(destination.SalePriceText);
            source.IsUnderOffer.ShouldBe(destination.IsUnderOffer);
            source.SoldOn.ShouldBe(destination.SoldOn);
            source.SoldPrice.ShouldBe(destination.SoldPrice);
            source.SoldPriceText.ShouldBe(destination.SoldPriceText);
        }
    }
}