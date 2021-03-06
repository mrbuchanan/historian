﻿using System;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace Historian.Api
{
    internal class WebApiConfig
    {
        internal static void Register(HttpConfiguration config)
        {
            //  Enable attribute based routing
            //  http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
            config.MapHttpAttributeRoutes();

            // map default route
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}