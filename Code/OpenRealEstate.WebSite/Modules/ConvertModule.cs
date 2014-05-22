using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using OpenRealEstate.Core.Models;
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
                IList<Listing> listings = _reaXmlTransmorgrifier.Convert(reaXml);

                var viewModel = new ConvertViewModel
                {
                    Listings = listings,
                    ResidentialCount = listings.OfType<ResidentialListing>().Count(),
                    RentalCount = listings.OfType<RentalListing>().Count()
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