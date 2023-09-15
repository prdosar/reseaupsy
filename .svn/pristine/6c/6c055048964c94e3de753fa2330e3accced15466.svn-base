using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ReseauPsy.Models
{
    public class TrailingSlashRoute : Route
    {
        public TrailingSlashRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler) { }

        public TrailingSlashRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler) { }

        public TrailingSlashRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints,
                              IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler) { }

        public TrailingSlashRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints,
                              RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler) { }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            VirtualPathData path = base.GetVirtualPath(requestContext, values);

            if (path != null)
                path.VirtualPath = path.VirtualPath + "/";
            return path;
        }
    }

    public static class RouteCollectionExtensions
    {
        public static void MapRouteTrailingSlash(this RouteCollection routes, string name, string url, object defaults)
        {
            routes.MapRouteTrailingSlash(name, url, defaults, null);
        }

        public static void MapRouteTrailingSlash(this RouteCollection routes, string name, string url, object defaults,
                                             object constraints)
        {
            if (routes == null)
                throw new ArgumentNullException("routes");

            if (url == null)
                throw new ArgumentNullException("url");

            var route = new TrailingSlashRoute(url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints)
            };

            if (String.IsNullOrEmpty(name))
                routes.Add(route);
            else
                routes.Add(name, route);
        }
    }
}