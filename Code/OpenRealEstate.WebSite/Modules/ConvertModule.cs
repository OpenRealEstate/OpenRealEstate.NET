using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
using Shouldly;

namespace OpenRealEstate.WebSite.Modules
{
    public class ConvertModule : NancyModule
    {
        private readonly ITransmorgrifier _reaXmlTransmorgrifier;

        public ConvertModule(ITransmorgrifier reaXmlTransmorgrifier) : base("/convert")
        {
            _reaXmlTransmorgrifier = reaXmlTransmorgrifier;

            Post["/ReaToORE"] = parameters => PostConvertReaToOre();
            Post["/Files"] = parameters => PostConvertFiles();
        }

        private dynamic PostConvertReaToOre()
        {
            var reaXml = Request.Form.reaXml;

            return string.IsNullOrWhiteSpace(reaXml)
                ? Response.AsJson("Please provide an ReaXml value to convert.", HttpStatusCode.BadRequest)
                : ConvertReaXmlToJson(new Dictionary<string, string> {{"no file name", reaXml}});
        }

        private dynamic PostConvertFiles()
        {
            if (Request.Files == null ||
                !Request.Files.Any())
            {
                return Response.AsJson("Please provide one or more files to convert.", HttpStatusCode.BadRequest);
            }

            var reaXmls = new Dictionary<string, string>();
            foreach (var file in Request.Files)
            {
                var memoryStream = new MemoryStream();
                file.Value.CopyTo(memoryStream);
                reaXmls.Add(file.Name, Encoding.UTF8.GetString(memoryStream.ToArray()));
            }

            return ConvertReaXmlToJson(reaXmls);
        }

        private dynamic ConvertReaXmlToJson(IEnumerable<KeyValuePair<string, string>> contents)
        {
            string lastFile = null;

            try
            {
                var results = new Dictionary<string, ConvertToResult>();

                foreach (var content in contents)
                {
                    lastFile = content.Key;
                    var convertToResult = _reaXmlTransmorgrifier.ConvertTo(content.Value, true);
                    if (convertToResult != null)
                    {
                        results.Add(content.Key, convertToResult);
                    }
                }

                var listings = new List<Listing>();
                var validationErrors = new Dictionary<string, string>();
                foreach (var convertToResult in results)
                {
                    ExtractData(convertToResult, listings, validationErrors);
                }

                var viewModel = new ConvertViewModel
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
                    $"Failed to convert the ReaXml to OpenRealEstate json. File: {(string.IsNullOrEmpty(lastFile) ? "--no filename--" : lastFile)}. Error message: {exception.InnerException?.Message ?? exception.Message}.")
                    .WithStatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private static void ExtractData(KeyValuePair<string, ConvertToResult> convertToResultKeyValuePair,
            IList<Listing> listingsResult, 
            IDictionary<string, string> validationErrorsResult)
        {
            convertToResultKeyValuePair.ShouldNotBeNull();
            listingsResult.ShouldNotBeNull();
            validationErrorsResult.ShouldNotBeNull();

            var errors = new List<ValidationError>();

            var listings = convertToResultKeyValuePair.Value.Listings?.Select(x => x.Listing).ToList();
            var invalidData = convertToResultKeyValuePair.Value.Errors?.Select(x => x.ExceptionMessage).ToList();
            var unhandledData = convertToResultKeyValuePair.Value.UnhandledData?.Select(x => x).ToList();

            if (listings != null &&
                listings.Any())
            {
                foreach (var listing in listings)
                {
                    // We only do strict validation (the bare minimum needed to store a listing) if this is current.
                    var validationResults = ValidatorMediator.Validate(listing, listing.StatusType == StatusType.Available);
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
                CreateError(validationErrorsResult, convertToResultKeyValuePair.Key, invalidData);
            }

            if (unhandledData != null &&
                unhandledData.Any())
            {
                var errorList = new[] {$"Found: {unhandledData.Count} unhandled data segments"};
                CreateError(validationErrorsResult, convertToResultKeyValuePair.Key, errorList);
            }

            if (errors.Any())
            {
                var convertedErrors = ConvertErrorsToDictionary(convertToResultKeyValuePair.Key, errors);
                foreach (var convertedError in convertedErrors)
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

        private static IEnumerable<KeyValuePair<string, string>> ConvertErrorsToDictionary(string fileName,
            IList<ValidationError> errors)
        {
            var result = new Dictionary<string, string>();
            for (int i = 0; i < errors.Count; i++)
            {
                result.Add(string.Format("{0} | {1} :: {2} - {3}",
                    fileName,
                    errors[i].Id,
                    i + 1,
                    errors[i].ValidationFailure.PropertyName),
                    errors[i].ValidationFailure.ErrorMessage);
            }

            return result;
        }
    }
}