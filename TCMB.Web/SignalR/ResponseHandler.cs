using Microsoft.AspNet.SignalR;

namespace TCMB.Web.SignalR
{
    public class ResponseHandler
    {
        public static void ExchangeRates(string connectionID, string json)
        {
            GlobalHost.ConnectionManager.GetHubContext<SignalRHub>().Clients.Client(connectionID).response("ExchangeRates", json);
        }
    }
}