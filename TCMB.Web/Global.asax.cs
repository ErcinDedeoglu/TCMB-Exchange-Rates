using System;
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
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            WebApp.Start<Startup>("http://localhost:8080");
        }

        public class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                //app.UseCors(CorsOptions.AllowAll);
                //app.MapSignalR(new HubConfiguration()
                //{
                //    EnableJSONP = true,

                //});
                // Branch the pipeline here for requests that start with "/signalr"
                app.Map("/signalr", map =>
                {
                    // Setup the CORS middleware to run before SignalR.
                    // By default this will allow all origins. You can 
                    // configure the set of origins and/or http verbs by
                    // providing a cors options with a different policy.
                    map.UseCors(CorsOptions.AllowAll);

                    HubConfiguration hubConfiguration = new HubConfiguration
                    {
                        // You can enable JSONP by uncommenting line below.
                        // JSONP requests are insecure but some older browsers (and some
                        // versions of IE) require JSONP to work cross domain
                        EnableJSONP = true,
                        EnableDetailedErrors = true
                    };

                    // Run the SignalR pipeline. We're not using MapSignalR
                    // since this branch already runs under the "/signalr"
                    // path.
                    map.RunSignalR(hubConfiguration);
                });

                GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(6);
                GlobalHost.Configuration.MaxIncomingWebSocketMessageSize = int.MaxValue;
            }
        }

    }
}