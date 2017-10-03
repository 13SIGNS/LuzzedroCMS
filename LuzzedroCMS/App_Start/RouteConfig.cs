using System.Web.Mvc;
using System.Web.Routing;

namespace LuzzedroCMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Content/*");

            routes.MapRoute(
                null,
                string.Empty,
                new
                {
                    controller = "Article",
                    action = "Index"
                }
            );

            routes.MapRoute(
                name: "defaultHome",
                url: "Home",
                defaults: new { controller = "Article", action = "Index" }
            );

            routes.MapRoute(
                name: "FavsPaged",
                url: "Favs/strona-{page}",
                defaults: new { controller = "User", action = "Bookmarks" }
            );

            routes.MapRoute(
                name: "Favs",
                url: "Favs/",
                defaults: new { controller = "User", action = "Bookmarks" }
            );

            routes.MapRoute(
                name: "ArticlePaged",
                url: "{category}/{url}-art/strona-{page}",
                defaults: new { controller = "Article", action = "Article" }
            );

            routes.MapRoute(
                name: "Article",
                url: "{category}/{url}-art",
                defaults: new { controller = "Article", action = "Article" }
            );


            routes.MapRoute(
                name: "CategoryPaged",
                url: "{category}/strona-{page}",
                defaults: new { controller = "Article", action = "ArticlesByCategory" }
            );

            routes.MapRoute(
                name: "TagPaged",
                url: "Tags/{tag}/strona-{page}",
                defaults: new { controller = "Article", action = "ArticlesByTag" }
            );

            routes.MapRoute(
                name: "Tag",
                url: "Tags/{tag}/",
                defaults: new { controller = "Article", action = "ArticlesByTag" }
            );

            routes.MapRoute(
                name: "SearchPaged",
                url: "Search/strona-{page}",
                defaults: new { controller = "Search", action = "Result" }
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
                name: "UserComments",
                url: "Comments/",
                defaults: new { controller = "User", action = "Comments" }
            );

            routes.MapRoute(
                name: "UserCommentsPaged",
                url: "Comments/strona-{page}",
                defaults: new { controller = "User", action = "Comments" }
            );

            routes.MapRoute(
                name: "AdminCommentsPaged",
                url: "Admin/Comments/strona-{page}",
                defaults: new { controller = "Admin", action = "Comments" }
            );

            routes.MapRoute(
                name: "AdminComments",
                url: "Admin/Comments/",
                defaults: new { controller = "Admin", action = "Comments" }
            );


            routes.MapRoute(
                name: "Category",
                url: "{category}/",
                defaults: new { controller = "Article", action = "ArticlesByCategory" }
            );

            routes.MapRoute(
                name: "defaultPaged",
                url: "{controller}/{action}/strona-{page}",
                defaults: new { controller = "Article", action = "Index" }
            );

            routes.MapRoute(
                name: "default",
                url: "{controller}/{action}",
                defaults: new { controller = "Article", action = "Index" }
            );

        }
    }
}
