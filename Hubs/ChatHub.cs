using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Diagnostics;

namespace webApp_SignalRLab.Hubs
{
    public class ChatHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void SendMessage(string message)
        {
            Clients.All.AddMessage(message);
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            Debug.WriteLine("ON : OnConnected");
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnReconnected()
        {
            Debug.WriteLine("ON : OnReconnected");
            return base.OnReconnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            Debug.WriteLine("ON : OnDisconnected");
            return base.OnDisconnected(stopCalled);
        }

    }
}