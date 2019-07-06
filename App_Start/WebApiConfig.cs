using System.Web.Http;
using System.Web.Http.Cors;



namespace EBook
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
//            var cors = new EnableCorsAttribute("*", "*", "*");
//            config.EnableCors(cors);

            
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            // Web API 路由
            config.MapHttpAttributeRoutes();
            config.EnableCors(new EnableCorsAttribute(origins: "*", headers: "*", methods: "*") { SupportsCredentials = true });


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}