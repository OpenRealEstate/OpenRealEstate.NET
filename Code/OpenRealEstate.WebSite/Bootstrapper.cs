using Nancy;
using Nancy.Json;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using OpenRealEstate.Services;
using OpenRealEstate.Services.RealEstateComAu;
using OpenRealEstate.WebSite.Models;

namespace OpenRealEstate.WebSite
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            RegisterServices(container);

            StaticConfiguration.DisableErrorTraces = false;

            container.Register<JsonSerializer, CustomJsonSerializer>();

            //Nancy.Json.JsonSettings.Converters
            //JsonSettings.MaxJsonLength = int.MaxValue;
            //JsonSettings.RetainCasing = false;
        }

        private static void RegisterServices(TinyIoCContainer container)
        {
            // NOTE: Use a specific constructor, which is why we have to use the delayed registration.
            container.Register<ITransmorgrifier, ReaXmlTransmorgrifier>();
        }
    }
}