using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LiveWebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ico/{*pathInfo}");

            routes.MapHubs();

            routes.MapRoute(
                name: "RunServer",
                url: "server/{*identifier}",
                defaults: new { controller = "Home", action = "RunServer", identifier = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "RunClient",
                url: "client/{*identifier}",
                defaults: new { controller = "Home", action = "RunClient", identifier = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{*identifier}",
                defaults: new { controller = "Home", action = "Index", identifier = UrlParameter.Optional }
            );
        }
    }
}