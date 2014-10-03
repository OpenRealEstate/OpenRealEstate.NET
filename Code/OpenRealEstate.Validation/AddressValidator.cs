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

                RuleFor(address => address.Latitude)
                    .GreaterThanOrEqualTo(-90M)
                    .LessThanOrEqualTo(90M)
                    .When(address => address.Latitude.HasValue)
                        .WithMessage("Latitude value has to be between -90 and 90.");

                RuleFor(address => address.Longitude)
                        .GreaterThanOrEqualTo(-180M)
                        .LessThanOrEqualTo(180M)
                        .When(address => address.Longitude.HasValue)
                            .WithMessage("Longitude value has to be between -180 and 180.");
            //});
        }
    }
}