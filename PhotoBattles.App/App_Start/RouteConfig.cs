namespace PhotoBattles.App
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Contests", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "DefaultPhotos",
                url: "contest/{id}/photos",
                defaults: new { controller = "Photos", action = "AddPhoto", id = UrlParameter.Optional }
                );
        }
    }
}