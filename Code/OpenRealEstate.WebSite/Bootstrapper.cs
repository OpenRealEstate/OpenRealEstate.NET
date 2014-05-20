using Nancy;
using Nancy.TinyIoc;
using OpenRealEstate.Services;
using OpenRealEstate.Services.RealEstate.com.au;

namespace OpenRealEstate.WebSite
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            RegisterServices(container);

            Nancy.Json.JsonSettings.MaxJsonLength = int.MaxValue;
        }

        private static void RegisterServices(TinyIoCContainer container)
        {
            // NOTE: Use a specific constructor, which is why we have to use the delayed registration.
            container.Register<ITransmorgrifier, ReaXmlTransmorgrifier>();
        }
    }
}