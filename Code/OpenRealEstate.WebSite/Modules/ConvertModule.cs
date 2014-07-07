using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nancy;
using OpenRealEstate.Core.Models;
using OpenRealEstate.Core.Models.Rental;
using OpenRealEstate.Core.Models.Residential;
using OpenRealEstate.Services;
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
                var result = _reaXmlTransmorgrifier.ConvertTo(reaXml);
                var listings = result.Listings;

                var errors = new Dictionary<string, string>();
                if (listings != null)
                {
                    foreach (var listing in listings)
                    {
                        var keySuffix = string.Format("-{0}-{1}-{2}",
                            string.IsNullOrEmpty(listing.AgencyId)
                                ? "no-Agency-Id-"
                                : listing.AgencyId,
                            string.IsNullOrEmpty(listing.Id)
                                ? "no-listing-Id-"
                                : listing.Id,
                            Guid.NewGuid());
                        listing.Validate(errors, keySuffix);
                    }
                }


                var viewModel = errors.Any()
                    ? new ConvertViewModel
                    {
                        ValidationErrors = errors
                    }
                    : new ConvertViewModel
                {
                    Listings = listings,
                    ResidentialCount = listings == null
                        ? 0
                        : listings.OfType<ResidentialListing>().Count(),
                    RentalCount = listings == null
                        ? 0
                        : listings.OfType<RentalListing>().Count()
                };

                return Response.AsJson(viewModel);
            }
            catch (Exception exception)
            {
                return Response.AsJson(
                    string.Format("Failed to convert the ReaXml to OpenRealEstate json. Error message: {0}.",
                        exception.InnerException != null
                            ? exception.InnerException.Message
                            : exception.Message), HttpStatusCode.InternalServerError);
            }
        }
    }
}