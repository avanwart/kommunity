using System.Web.Http;

namespace DasKlub.Web.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            configuration.Routes.MapHttpRoute("DefaultAPI", "api/v1/{controller}/{id}",
                new {id = RouteParameter.Optional});
        }
    }
}