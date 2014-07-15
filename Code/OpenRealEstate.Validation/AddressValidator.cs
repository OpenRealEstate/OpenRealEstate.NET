using FluentValidation;
using OpenRealEstate.Core.Models;

namespace OpenRealEstate.Validation
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public static string RuleLocationNames = "LocationNames";

        public AddressValidator()
        {
            //RuleSet(RuleLocationNames, () =>
            //{
                // Cannot have a street number/lot and no street name.
                RuleFor(address => address.Street).NotEmpty()
                    .When(address => !string.IsNullOrWhiteSpace(address.StreetNumber))
                    .WithMessage("A street name is required when a street number has been provided. Eg. Smith Street.");

                RuleFor(address => address.Suburb).NotEmpty()
                    .WithMessage("A Suburb is required. Eg. Ivanhoe or Pott's Point.");

                RuleFor(address => address.State).NotEmpty()
                    .WithMessage("A State is required. Eg. Victoria or New South Wales.");

                RuleFor(address => address.CountryIsoCode).NotEmpty()
                    .WithMessage("A Country ISO code is required. eg. AU, NZ, etc.");
            //});
        }
    }
}