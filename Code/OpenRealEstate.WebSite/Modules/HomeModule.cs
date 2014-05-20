using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace OpenRealEstate.WebSite.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => GetHome();
        }

        private dynamic GetHome()
        {
            return View["index"];
        }
    }
}