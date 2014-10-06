using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using Nancy;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Land;
using OpenRealEstate.Core.Models.Rental;
using OpenRealEstate.Core.Models.Residential;
using OpenRealEstate.Core.Models.Rural;
using OpenRealEstate.Services;
using OpenRealEstate.Validation;
using OpenRealEstate.WebSite.Models;
using OpenRealEstate.WebSite.ViewModels;

namespace OpenRealEstate.WebSite.Modules
{
    public class ConvertModule : NancyModule
    {
        private readonly ITransmorgrifier _reaXmlTransmorgrifier;

        public ConvertModule(ITransmorgrifier reaXmlTransmorgrifier) : base("/convert")
        {
            _reaXmlTransmorgrifier = reaXmlTransmorgrifier;

            Post["/ReaToORE"] = parameters => PostConvertReaToOre();
        }

        private dynamic PostConvertReaToOre()
        {
            var reaXml = Request.Form.reaXml;

            if (string.IsNullOrWhiteSpace(reaXml))
            {
                return Response.AsJson("Please provide an ReaXml value to convert.", HttpStatusCode.BadRequest);
            }

            try
            {
                ConvertToResult result = _reaXmlTransmorgrifier.ConvertTo(reaXml);
                var listings = result == null
                    ? null
                    : result.Listings.Select(x => x.Listing).ToList();

                var errors = new List<ValidationError>();
                var viewModel = new ConvertViewModel();

                if (listings != null)
                {
                    foreach (var listing in listings)
                    {
                        var ruleSet = listing.StatusType == StatusType.Current
                            ? ValidatorMediator.MinimumRuleSet
                            : null;
                        var validationResults = ValidatorMediator.Validate(listing, ruleSet);
                        if (validationResults.Errors != null &&
                            validationResults.Errors.Any())
                        {
                            errors.AddRange(ValidationError.ConvertToValidationErrors(listing.ToString(),
                                validationResults.Errors));
                        }
                    }


                    viewModel.Listings = listings;
                    viewModel.ResidentialCount = listings.OfType<ResidentialListing>().Count();
                    viewModel.RentalCount = listings.OfType<RentalListing>().Count();
                    viewModel.RuralCount = listings.OfType<RuralListing>().Count();
                    viewModel.LandCount = listings.OfType<LandListing>().Count();
                }

                if (errors.Any())
                {
                    viewModel.ValidationErrors = ConvertErrorsToDictionary(errors);
                }

                return Response.AsJson(viewModel);
            }
            catch (Exception exception)
            {
                return Response.AsText(
                    string.Format("Failed to convert the ReaXml to OpenRealEstate json. Error message: {0}.",
                        exception.InnerException != null
                            ? exception.InnerException.Message
                            : exception.Message))
                    .WithStatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private static IDictionary<string, string> ConvertErrorsToDictionary(IList<ValidationError> errors)
        {
            var result = new Dictionary<string, string>();
            for(int i = 0; i < errors.Count; i++)
            {
                result.Add(string.Format("{0} :: {1} - {2}", 
                    errors[i].Id,
                    i + 1, 
                    errors[i].ValidationFailure.PropertyName),
                    errors[i].ValidationFailure.ErrorMessage);
            }

            return result;
        }
    }
}