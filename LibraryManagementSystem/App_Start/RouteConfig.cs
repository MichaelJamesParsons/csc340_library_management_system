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
                "Reservation",
                "library/reservations/{action}/{id}",
                new
                {
                    controller = "Reservations",
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );

            /** Library Items **/
            routes.MapRoute(
                "LibraryItem",
                "library/index",
                new { controller = "LibraryItems", action = "Index" }
            );
            
            /** Library Item Search **/
            routes.MapRoute(
                "LibraryItemSearch",
                "library/search",
                new { controller = "LibraryItems", action = "Search" }
            );

            /** Books **/
            routes.MapRoute(
                "Book",
                "library/book/{action}/{id}",
                new { controller = "Books", action = "Index", id = UrlParameter.Optional },
                new { action = @"Index|Edit|Create|Details" }
            );

            routes.MapRoute(
                "LibraryItemActions",
                "library/{type}/{action}/{id}",
                new { controller = "LibraryItems", id = UrlParameter.Optional }
            );

            /** Customers **/
            routes.MapRoute(
                "Customer",
                "customers/{action}/{id}",
                new { controller = "Customers", action = "Index", id = UrlParameter.Optional }
            );

            /** Librarian **/
            routes.MapRoute(
                "Librarian",
                "librarian/{action}/{id}",
                new { controller = "Librarians", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
