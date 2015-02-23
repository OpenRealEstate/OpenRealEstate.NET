using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Nancy;
using OpenRealEstate.Core.Models;
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
                    var convertToResult = _reaXmlTransmorgrifier.ConvertTo(content.Value);
                    if (convertToResult != null)
                    {
                        results.Add(content.Key, convertToResult);
                    }
                }

                var viewModel = new ConvertViewModel();
                foreach (var convertToResult in results)
                {
                    CopyToViewModel(convertToResult, viewModel);
                }

                return Response.AsJson(viewModel);
            }
            catch (Exception exception)
            {
                return Response.AsText(
                    string.Format("Failed to convert the ReaXml to OpenRealEstate json. File: {0}. Error message: {1}.",
                        string.IsNullOrEmpty(lastFile)
                            ? "--no filename--"
                            : lastFile,
                        exception.InnerException != null
                            ? exception.InnerException.Message
                            : exception.Message))
                    .WithStatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private static void CopyToViewModel(KeyValuePair<string, ConvertToResult> convertToResultKeyValuePair,
            ConvertViewModel viewModel)
        {
            if (convertToResultKeyValuePair.Value == null)
            {
                throw new ArgumentNullException("convertToResultKeyValuePair");
            }

            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }

            var errors = new List<ValidationError>();

            var listings = convertToResultKeyValuePair.Value.Listings != null
                ? convertToResultKeyValuePair.Value.Listings.Select(x => x.Listing).ToList()
                : null;
            var invalidData = convertToResultKeyValuePair.Value.Errors != null
                ? convertToResultKeyValuePair.Value.Errors.Select(x => x.ExceptionMessage).ToList()
                : null;
            var unhandledData = convertToResultKeyValuePair.Value.UnhandledData != null
                ? convertToResultKeyValuePair.Value.UnhandledData.Select(x => x).ToList()
                : null;

            if (listings != null &&
                listings.Any())
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

                if (viewModel.Listings == null)
                {
                    viewModel.Listings = new List<Listing>();
                }

                viewModel.Listings.AddRange(listings);
            }

            if (viewModel.ValidationErrors == null)
            {
                viewModel.ValidationErrors = new Dictionary<string, string>();
            }

            if (invalidData != null &&
                invalidData.Any())
            {
                CreateError(viewModel.ValidationErrors, convertToResultKeyValuePair.Key, invalidData);
            }

            if (unhandledData != null && 
                unhandledData.Any())
            {
                var errorList = new [] {string.Format("Found: {0} unhandled data segments", unhandledData.Count)};
                CreateError(viewModel.ValidationErrors, convertToResultKeyValuePair.Key,
                    errorList);
            }

            if (errors.Any())
            {
                var convertedErrors = ConvertErrorsToDictionary(convertToResultKeyValuePair.Key, errors);
                foreach (var convertedError in convertedErrors)
                {
                    viewModel.ValidationErrors.Add(convertedError);
                }
            }
        }

        private static void CreateError(IDictionary<string, string> validationErrors, string key, IEnumerable<string> values)
        {
            foreach (var value in values)
            {
                var uniqueKey = string.Format("{0}_{1}", key, Guid.NewGuid());
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