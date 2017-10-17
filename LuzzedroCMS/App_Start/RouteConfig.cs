using LuzzedroCMS.WebUI.Properties;
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
                url: Resources.RoutingFavs + "/" + Resources.RoutingPage + "-{page}",
                defaults: new { controller = "User", action = "Bookmarks" }
            );

            routes.MapRoute(
                name: "Favs",
                url: Resources.RoutingFavs + "/",
                defaults: new { controller = "User", action = "Bookmarks" }
            );

            routes.MapRoute(
                name: "ArticlePaged",
                url: "{url}-art/" + Resources.RoutingPage + "-{page}",
                defaults: new { controller = "Article", action = "Article" }
            );

            routes.MapRoute(
                name: "Article",
                url: "{url}-art",
                defaults: new { controller = "Article", action = "Article" }
            );


            routes.MapRoute(
                name: "CategoryPaged",
                url: "{category}/" + Resources.RoutingPage + "-{page}",
                defaults: new { controller = "Article", action = "ArticlesByCategory" }
            );

            routes.MapRoute(
                name: "TagPaged",
                url: Resources.RoutingTags + "/{tag}/" + Resources.RoutingPage + "-{page}",
                defaults: new { controller = "Article", action = "ArticlesByTag" }
            );

            routes.MapRoute(
                name: "Tag",
                url: Resources.RoutingTags + "/{tag}/",
                defaults: new { controller = "Article", action = "ArticlesByTag" }
            );

            routes.MapRoute(
                name: "SearchPaged",
                url: Resources.RoutingSearch + "/{Key}/" + Resources.RoutingPage + "-{page}",
                defaults: new { controller = "Search", action = "Result" }
            );

            routes.MapRoute(
                name: "Search",
                url: Resources.RoutingSearch + "/{Key}",
                defaults: new { controller = "Search", action = "Result" }
            );

            routes.MapRoute(
                name: "Account",
                url: Resources.RoutingAccount + "/",
                defaults: new { controller = "User", action = "EditAccount" }
            );

            routes.MapRoute(
                name: "AccountLogout",
                url: Resources.RoutingAccount + "/" + Resources.RoutingLogout,
                defaults: new { controller = "Account", action = "Logout" }
            );

            routes.MapRoute(
                name: "AccountLogoutWithReturn",
                url: Resources.RoutingAccount + "/" + Resources.RoutingLogout + "/{returnUrl}",
                defaults: new { controller = "Account", action = "Logout" }
            );

            routes.MapRoute(
                name: "UserComments",
                url: Resources.RoutingComments + "/",
                defaults: new { controller = "User", action = "Comments" }
            );

            routes.MapRoute(
                name: "UserCommentsPaged",
                url: Resources.RoutingComments + "/" + Resources.RoutingPage + "-{page}",
                defaults: new { controller = "User", action = "Comments" }
            );

            routes.MapRoute(
                name: "AdminCommentsPaged",
                url: "Admin/" + Resources.RoutingComments + "/" + Resources.RoutingPage + "-{page}",
                defaults: new { controller = "Admin", action = "Comments" }
            );

            routes.MapRoute(
                name: "AdminComments",
                url: "Admin/" + Resources.RoutingComments + "/",
                defaults: new { controller = "Admin", action = "Comments" }
            );


            routes.MapRoute(
                name: "Category",
                url: "{category}/",
                defaults: new { controller = "Article", action = "ArticlesByCategory" }
            );

            routes.MapRoute(
                name: "defaultPaged",
                url: "{controller}/{action}/" + Resources.RoutingPage + "-{page}",
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
