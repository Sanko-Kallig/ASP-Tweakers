﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Tweakers
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Overview",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Product", action = "Overview", id = UrlParameter.Optional }
                );
            routes.MapRoute(
                name: "Article",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Article", action = "Index", id = UrlParameter.Optional }
                );
            routes.MapRoute(
                name: "Review",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "REview", action = "Review", id = UrlParameter.Optional }
     );

        }
    }
}
