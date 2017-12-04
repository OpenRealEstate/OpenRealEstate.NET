using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenRealEstate.Core;
using OpenRealEstate.Core.Land;
using OpenRealEstate.Core.Rental;
using OpenRealEstate.Core.Residential;
using OpenRealEstate.Core.Rural;
using OpenRealEstate.Services;
using OpenRealEstate.Validation;
using OpenRealEstate.WebSite.Models;
using OpenRealEstate.WebSite.ViewModels;

namespace OpenRealEstate.WebSite.Controllers
{
    [Route("parse")]
    public class ParseController : Controller
    {
        private readonly ITransmorgrifier _reaXmlTransmorgrifier;

        public ParseController(ITransmorgrifier reaXmlTransmorgrifier)
        {
            _reaXmlTransmorgrifier = reaXmlTransmorgrifier ?? throw new System.ArgumentNullException(nameof(reaXmlTransmorgrifier));
        }

        [HttpPost("rea")]
        public IActionResult PostParseRea([FromForm]string reaXml)
        {
            return string.IsNullOrWhiteSpace(reaXml)
                       ?  StatusCode((int) HttpStatusCode.BadRequest, Json("Please provide an ReaXml value to parse."))
                       : ParseReaXmlToJson(new Dictionary<string, string> { { "no file name", reaXml } });
        }

        [HttpPost("files")]
        public async Task<IActionResult> PostParseFilesAsync()
        {
            var files = Request?.Form?.Files;
            if (files == null ||
                !files.Any())
            {
                return StatusCode((int)HttpStatusCode.BadRequest, Json("Please provide one or more files to parse."));
            }

            var reaXmls = new Dictionary<string, string>();
            foreach (var file in files)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    reaXmls.Add(file.Name, Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }

            return ParseReaXmlToJson(reaXmls);
        }

        private dynamic ParseReaXmlToJson(IEnumerable<KeyValuePair<string, string>> contents)
        {
            if (contents == null)
            {
                throw new ArgumentNullException(nameof(contents));
            }

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

                return Json(viewModel);
            }
            catch (Exception exception)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError,
                                  $"Failed to parse the ReaXml to OpenRealEstate json. File: {(string.IsNullOrEmpty(lastFile) ? "--no filename--" : lastFile)}. Error message: {exception.InnerException?.Message ?? exception.Message}.");
            }
        }

        private static void ExtractData(KeyValuePair<string, ParsedResult> parseResultKeyValuePair,
            IList<Listing> listingsResult,
            IDictionary<string, string> validationErrorsResult)
        {
            if (listingsResult == null)
            {
                throw new ArgumentNullException(nameof(listingsResult));
            }

            if (validationErrorsResult == null)
            {
                throw new ArgumentNullException(nameof(validationErrorsResult));
            }

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
                var errorList = new[] { $"Found: {unhandledData.Count} unhandled data segments" };
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