using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace webApp_SignalRLab
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            ////// <li>@Html.ActionLink("SignalR.Sample", "FirstSample", "SignalR")</li>
            //routes.MapPageRoute(
            //    "SignalR_Demo",
            //    "{controller}/{action}",
            //    "~/SignalR.Sample/StockTicker.html",
            //    true,
            //    new RouteValueDictionary { { "controller", "SignalR" }, { "action", "FirstSample" } },
            //    new RouteValueDictionary { { "controller", "SignalR" }, { "action", "FirstSample" } }
            //    );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
