using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using TCMB.Web.SignalR;

namespace TCMB.Web
{
    public class SignalRHub : Hub
    {
        public void Request(string methodName, object parameters)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                TCMB.Helper.InvokeHelper.ObjectInvoke(new RequestHandler() { ConnectionID = Context.ConnectionId }, methodName, parameters);
            });
        }

        public override async Task OnConnected()
        {
            await base.OnConnected();
        }

        public override async Task OnDisconnected(bool stopCalled)
        {
            await base.OnDisconnected(stopCalled);
        }

        public override async Task OnReconnected()
        {
            await base.OnReconnected();
        }
    }
}