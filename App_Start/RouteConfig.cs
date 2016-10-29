using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LuzzedroCMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                null,
                "",
                new
                {
                    controller = "Article",
                    action = "Index"
                }
            );

            routes.MapRoute(
                name: "Article",
                url: "{category}/{url}-art",
                defaults: new { controller = "Article", action = "Article" }
            );

            routes.MapRoute(
                name: "Tag",
                url: "Tags/{tag}/",
                defaults: new { controller = "Article", action = "ArticlesByTag" }
            );

            routes.MapRoute(
                name: "Search",
                url: "Search/",
                defaults: new { controller = "Search", action = "Result" }
            );

            routes.MapRoute(
                name: "Account",
                url: "Account/",
                defaults: new { controller = "User", action = "EditAccount" }
            );

            routes.MapRoute(
                name: "Favs",
                url: "Favs/",
                defaults: new { controller = "User", action = "Bookmarks" }
            );

            routes.MapRoute(
                name: "Comments",
                url: "Comments/",
                defaults: new { controller = "User", action = "Comments" }
            );

            routes.MapRoute(
                name: "Category",
                url: "{category}/",
                defaults: new { controller = "Article", action = "ArticlesByCategory" }
            );

            routes.MapRoute(
                name: "default",
                url: "{controller}/{action}",
                defaults: new { controller = "Article", action = "Index" }
            );
           
        }
    }
}
