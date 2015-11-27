using System.Web.Mvc;
using System.Web.Routing;

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
                defaults: new
                {
                    controller = "Reservations",
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );

            /** Library Items **/
            routes.MapRoute(
                name: "LibraryItem",
                url: "library/index",
                defaults: new { controller = "LibraryItems", action = "Index" }
            );

            /** Books **/
            routes.MapRoute(
                name: "Book",
                url: "library/book/{action}/{id}",
                defaults: new { controller = "Books", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "LibraryItemActions",
                url: "library/{type}/{action}/{id}",
                defaults: new { controller = "LibraryItems", id = UrlParameter.Optional }
            );

            /** Customers **/
            routes.MapRoute(
                name: "Customer",
                url: "customers/{action}/{id}",
                defaults: new { controller = "Customers", action = "Index", id = UrlParameter.Optional }
            );

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
