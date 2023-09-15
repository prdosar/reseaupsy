using ReseauPsy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ReseauPsy
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            //routes.AppendTrailingSlash = true;
            routes.LowercaseUrls = true;


            routes.MapRoute(
                name: "DefaultLocalized",
                url: "{lang}/{controller}/{action}/{id}",
                //constraints: new { lang = @"(\w{2})|(\w{2}-\w{2})" },   // fr or fr-CA
                defaults: new { lang = "fr", id = UrlParameter.Optional }
            );

        }
    }
}
