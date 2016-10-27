using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Exceptionless;

namespace ExceptionDrivenDemo {
    public class WebApiApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            // NOTE: We only set the user like this for the simplicity of this sample (there is no authentication).
            ExceptionlessClient.Default.Configuration.SetUserIdentity("123456789", "Blake");

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}