using System;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;

namespace TCMB.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            DTO.Global.SQLConnectionString = Helper.DatabaseHelper.BuildConnectionString("185.40.86.73", false, "TcmbExchangeRates", "test", "Test123456!");
            WebApp.Start<Startup>("http://localhost:8080");
        }

        public class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                app.Map("/signalr", map =>
                {
                    map.UseCors(CorsOptions.AllowAll);

                    HubConfiguration hubConfiguration = new HubConfiguration
                    {
                        EnableJSONP = true,
                        EnableDetailedErrors = true
                    };
                    
                    map.RunSignalR(hubConfiguration);
                });

                GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(6);
                GlobalHost.Configuration.MaxIncomingWebSocketMessageSize = int.MaxValue;
            }
        }
    }
}