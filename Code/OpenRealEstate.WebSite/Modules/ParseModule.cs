using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Nancy;
using OpenRealEstate.Core;
using OpenRealEstate.Core.Land;
using OpenRealEstate.Core.Rental;
using OpenRealEstate.Core.Residential;
using OpenRealEstate.Core.Rural;
using OpenRealEstate.Services;
using OpenRealEstate.Validation;
using OpenRealEstate.WebSite.Models;
using OpenRealEstate.WebSite.ViewModels;
using Shouldly;

namespace OpenRealEstate.WebSite.Modules
{
    public class ParseModule : NancyModule
    {
        private readonly ITransmorgrifier _reaXmlTransmorgrifier;

        public ParseModule(ITransmorgrifier reaXmlTransmorgrifier) : base("/parse")
        {
            _reaXmlTransmorgrifier = reaXmlTransmorgrifier;

            Post["/Rea"] = parameters => PostParseRea();
            Post["/Files"] = parameters => PostParseFiles();
        }

        private dynamic PostParseRea()
        {
            var reaXml = Request.Form.reaXml;

            return string.IsNullOrWhiteSpace(reaXml)
                ? Response.AsJson("Please provide an ReaXml value to parse.", HttpStatusCode.BadRequest)
                : ParseReaXmlToJson(new Dictionary<string, string> {{"no file name", reaXml}});
        }

        private dynamic PostParseFiles()
        {
            if (Request.Files == null ||
                !Request.Files.Any())
            {
                return Response.AsJson("Please provide one or more files to parse.", HttpStatusCode.BadRequest);
            }

            var reaXmls = new Dictionary<string, string>();
            foreach (var file in Request.Files)
            {
                var memoryStream = new MemoryStream();
                file.Value.CopyTo(memoryStream);
                reaXmls.Add(file.Name, Encoding.UTF8.GetString(memoryStream.ToArray()));
            }

            return ParseReaXmlToJson(reaXmls);
        }

        private dynamic ParseReaXmlToJson(IEnumerable<KeyValuePair<string, string>> contents)
        {
            string lastFile = null;

            try
            {
                var results = new Dictionary<string, ParsedResult>();

                foreach (var content in contents)
                {
                    lastFile = content.Key;
                    var parsedResult = _reaXmlTransmorgrifier.Parse(content.Value, areBadCharactersRemoved: true);
                    if (parsedResult != null)
                    {
                        results.Add(content.Key, parsedResult);
                    }
                }

                var listings = new List<Listing>();
                var validationErrors = new Dictionary<string, string>();
                foreach (var parseResultKeyValuePair in results)
                {
                    ExtractData(parseResultKeyValuePair, listings, validationErrors);
                }

                var viewModel = new ParsedViewModel
                {
                    ListingsJson = Services.Json.JsonConvertHelpers.SerializeObject(listings),
                    ResidentialCount = listings.OfType<ResidentialListing>().Count(),
                    RentalCount = listings.OfType<RentalListing>().Count(),
                    LandCount = listings.OfType<LandListing>().Count(),
                    RuralCount = listings.OfType<RuralListing>().Count(),
                    ValidationErrors = validationErrors
                };

                return Response.AsJson(viewModel);
            }
            catch (Exception exception)
            {
                return Response.AsText(
                    $"Failed to parse the ReaXml to OpenRealEstate json. File: {(string.IsNullOrEmpty(lastFile) ? "--no filename--" : lastFile)}. Error message: {exception.InnerException?.Message ?? exception.Message}.")
                    .WithStatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private static void ExtractData(KeyValuePair<string, ParsedResult> parseResultKeyValuePair,
            IList<Listing> listingsResult, 
            IDictionary<string, string> validationErrorsResult)
        {
            parseResultKeyValuePair.ShouldNotBeNull();
            listingsResult.ShouldNotBeNull();
            validationErrorsResult.ShouldNotBeNull();

            var errors = new List<ValidationError>();

            var listings = parseResultKeyValuePair.Value.Listings?.Select(x => x.Listing).ToList();
            var invalidData = parseResultKeyValuePair.Value.Errors?.Select(x => x.ExceptionMessage).ToList();
            var unhandledData = parseResultKeyValuePair.Value.UnhandledData?.Select(x => x).ToList();

            if (listings != null &&
                listings.Any())
            {
                foreach (var listing in listings)
                {
                    var ruleSet = listing.StatusType == StatusType.Available
                        ? ListingValidatorRuleSet.Strict
                        : ListingValidatorRuleSet.Minimum;
                    
                    // We only do strict validation (the bare minimum needed to store a listing) if this is current.
                    var validationResults = ValidatorMediator.Validate(listing, ruleSet);
                    if (validationResults.Errors != null &&
                        validationResults.Errors.Any())
                    {
                        errors.AddRange(ValidationError.ConvertToValidationErrors(listing.ToString(),
                            validationResults.Errors));
                    }

                    listingsResult.Add(listing);
                }
            }

            if (invalidData != null &&
                invalidData.Any())
            {
                CreateError(validationErrorsResult, parseResultKeyValuePair.Key, invalidData);
            }

            if (unhandledData != null &&
                unhandledData.Any())
            {
                var errorList = new[] {$"Found: {unhandledData.Count} unhandled data segments"};
                CreateError(validationErrorsResult, parseResultKeyValuePair.Key, errorList);
            }

            if (errors.Any())
            {
                var parsedErrors = ParseErrorsToDictionary(parseResultKeyValuePair.Key, errors);
                foreach (var convertedError in parsedErrors)
                {
                    validationErrorsResult.Add(convertedError);
                }
            }
        }
        
        private static void CreateError(IDictionary<string, string> validationErrors, string key, IEnumerable<string> values)
        {
            foreach (var value in values)
            {
                var uniqueKey = $"{key}_{Guid.NewGuid()}";
                validationErrors.Add(uniqueKey, value);
            }
        }

        private static IEnumerable<KeyValuePair<string, string>> ParseErrorsToDictionary(string fileName,
            IList<ValidationError> errors)
        {
            var result = new Dictionary<string, string>();
            for (var i = 0; i < errors.Count; i++)
            {
                result.Add($"{fileName} | {errors[i].Id} :: {i + 1} - {errors[i].ValidationFailure.PropertyName}",
                    errors[i].ValidationFailure.ErrorMessage);
            }

            return result;
        }
    }
}