using System;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Exceptionless;
using Exceptionless.Plugins;

namespace ExceptionDrivenDemo {
    public class ExceptionlessExceptionLogger : ExceptionLogger {
        public override void Log(ExceptionLoggerContext context) {
            var contextData = new ContextData();
            contextData.MarkAsUnhandledError();
            contextData.SetSubmissionMethod("ExceptionLogger");
            contextData.Add("HttpActionContext", context.ExceptionContext.ActionContext);

            context.Exception.ToExceptionless(contextData).Submit();
        }
    }
    
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            ExceptionlessClient.Default.RegisterWebApi(config);
            config.Services.Add(typeof(IExceptionLogger), new ExceptionlessExceptionLogger());

            config.Routes.MapHttpRoute(name: "DefaultApi", routeTemplate: "api/{controller}/{id}", defaults: new {
                id = RouteParameter.Optional
            });
        }
    }
}