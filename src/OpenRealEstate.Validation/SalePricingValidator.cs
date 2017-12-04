using System;
using FluentValidation;
using OpenRealEstate.Core;

namespace OpenRealEstate.Validation
{
    public class SalePricingValidator : AbstractValidator<SalePricing>
    {
        public SalePricingValidator()
        {
            // Required.
            RuleFor(salePricing => salePricing.SalePrice)
                .NotNull()
                .GreaterThanOrEqualTo(0);

            // Optional.
            RuleFor(salePricing => salePricing.SoldPrice).GreaterThanOrEqualTo(0);
            RuleFor(salePricing => salePricing.SoldOn).NotEqual(DateTime.MinValue);
        }
    }
}