using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /** Customer Items **/
            routes.MapRoute(
                name: "Reservation",
                url: "library/reservations/{action}/{id}",
                defaults: new {
                    controller = "Reservations",
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );

            /** Books **/
            routes.MapRoute(
                name: "Book",
                url: "library/books/{action}/{id}",
                defaults: new { controller = "Books", action = "Index", id = UrlParameter.Optional }
            );

            /** Library Items **/
            routes.MapRoute(
                name: "LibraryItem",
                url: "library",
                defaults: new { controller = "LibraryItem", action = "Index" }
            );

            /** Customers **/
            /*routes.MapRoute(
                name: "Customer",
                url: "customers/{action}/{id}",
                defaults: new { controller = "Customers", action = "Index", id = UrlParameter.Optional }
            );*/

            /** Librarian **/
            routes.MapRoute(
                name: "Librarian",
                url: "librarian/{action}/{id}",
                defaults: new { controller = "Librarians", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
