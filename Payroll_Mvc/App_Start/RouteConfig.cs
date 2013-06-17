using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Payroll_Mvc.Areas.Admin.Controllers;

namespace Payroll_Mvc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Auth",
                "{action}",
                new { controller = "Application", action = "Login" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Application", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}