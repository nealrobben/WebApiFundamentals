using System.Web.Http;
using Microsoft.Web.Http;
using Newtonsoft.Json.Serialization;

namespace TheCodeCamp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            AutofacConfig.Register();

            config.AddApiVersioning(cfg =>
            {
               cfg.DefaultApiVersion = new ApiVersion(1,1);
               cfg.AssumeDefaultVersionWhenUnspecified = true;
               cfg.ReportApiVersions = true;
            });

            //Change case of JSON
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new {id = RouteParameter.Optional}
            //);
        }
    }
}
