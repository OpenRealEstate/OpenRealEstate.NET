using System;
using FluentValidation;
using OpenRealEstate.Core;

namespace OpenRealEstate.Validation
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public static string RuleLocationNames = "LocationNames";

        public AddressValidator()
        {
            // Cannot have a street number/lot and no street name.
            RuleFor(address => address.Street)
                .NotEmpty()
                .When(address => !string.IsNullOrWhiteSpace(address.StreetNumber))
                .WithMessage("A street name is required when a street number has been provided. Eg. Smith Street.");

            RuleFor(address => address.Suburb)
                .NotEmpty()
                .WithMessage("A Suburb is required. Eg. Ivanhoe or Pott's Point.");

            RuleFor(address => address.State)
                .NotEmpty()
                .WithMessage("A State is required. Eg. Victoria or New South Wales.");

            RuleFor(address => address.CountryIsoCode)
                .NotEmpty()
                .WithMessage("A Country ISO code is required. Eg. AU, NZ, etc.")
                .Must(BeAValidCountryIsoCode)
                .WithMessage(
                    "A valid ISO 1366-1 two letter iso code is required. Eg. AU, NZ. etc. Reference: https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2");

            RuleFor(address => address.Postcode)
                .NotEmpty()
                .WithMessage("A Postcode is required. Eg. 3000 or 4566.")
                .Must((anAddress,
                       postcode,
                       context) =>
                {
                    if (!(context.ParentContext.InstanceToValidate is Address address))
                    {
                        // No idea what instance we have .. so just step out :/
                        return true;
                    }

                    if (string.IsNullOrWhiteSpace(address.CountryIsoCode) ||
                            !string.Equals(address.CountryIsoCode, "au", StringComparison.OrdinalIgnoreCase))
                    {
                        // Not Australia - so don't check for any specifics postcode values.
                        return true;
                    }


                    // We have an Australian postcode.
                    if (!int.TryParse(address.Postcode, out var numberPostcode))
                    {
                        // But it is not a number :(
                        return false;
                    }

                    if (numberPostcode < 200 ||
                             numberPostcode > 9999)
                    {
                        // .. but it is a number, just not in the valid range :(
                        return false;
                    }

                    return true;
                });

                // NOTE: Custom is used in FV 7.x +
                //       We had to downgrade to 6.fuckyou cause 7.fuckyou was strong-fucked which means
                //       we cannot do binding redirects from 6 -> 7.

                //.Custom((postcode,
                //         context) =>
                //{
                //    if (!(context.ParentContext.InstanceToValidate is Address address))
                //    {
                //        return;
                //    }

                //    if (string.IsNullOrWhiteSpace(address.CountryIsoCode) ||
                //        !string.Equals(address.CountryIsoCode, "au", StringComparison.OrdinalIgnoreCase))
                //    {
                //        return;
                //    }

                //    // We have an Australian postcode.
                //    if (!int.TryParse(address.Postcode, out var numberPostcode))
                //    {
                //        context.AddFailure("Postcode", "Postcode's in Australia need to be numbers only. Eg. 3000, 4566, etc.");
                //    }
                //    else if (numberPostcode < 200 ||
                //             numberPostcode > 9999)
                //    {
                //        context.AddFailure("Postcode",
                //                           "The (Australian) Postcode's is not in the valid range of postcodes. It should be between 200 and 9999. Eg. 3000, 4566, etc. Reference: https://en.wikipedia.org/wiki/Postcodes_in_Australia");
                //    }
                //});

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
        }

        private static bool BeAValidCountryIsoCode(string countryCode)
        {
            return !string.IsNullOrWhiteSpace(countryCode) &&
                   CountryData.TwoLetterIsoCodes.Contains(countryCode);
        }
    }
}