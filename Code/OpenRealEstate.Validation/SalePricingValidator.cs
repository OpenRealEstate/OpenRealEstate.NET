using System;
using FluentValidation;
using OpenRealEstate.Core.Models;

namespace OpenRealEstate.Validation
{
    public class SalePricingValidator : AbstractValidator<SalePricing>
    {
        public SalePricingValidator()
        {
            RuleFor(salePricing => salePricing.SalePrice).GreaterThanOrEqualTo(0);
            RuleFor(salePricing => salePricing.SoldPrice).GreaterThanOrEqualTo(0)
                .When(salePricing => salePricing.SoldPrice.HasValue);
            RuleFor(salePricing => salePricing.SoldOn).NotEqual(DateTime.MinValue)
                .When(salePricing => salePricing.SoldOn.HasValue);
        }
    }
}