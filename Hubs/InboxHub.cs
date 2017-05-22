using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Diagnostics;

namespace webApp_SignalRLab.Hubs
{
    public class InboxHub : Hub
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly InboxTicker _inboxTicker;

        public InboxHub() :
            this(InboxTicker.Instance)
        {

        }

        public InboxHub(InboxTicker inboxTicker)
        {
            _inboxTicker = inboxTicker;
            _inboxTicker.OpenInbox();
        }
        
        public void RegisterUser(string idName)
        {
            _inboxTicker.RegisterUser(idName, Context.ConnectionId);
            logger.Info("RegisterUser(idName:{0})", idName);
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            logger.Info("OnConnected → {{ User.Identity.Name:{0}, ConnectionId:{1} }}", Context.User.Identity.Name, Context.ConnectionId);
            return base.OnConnected();            
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            logger.Info("OnDisconnected → {{ User.Identity.Name:{0}, ConnectionId:{1} }}", Context.User.Identity.Name, Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public override System.Threading.Tasks.Task OnReconnected()
        {
            logger.Info("OnReconnected → {{ User.Identity.Name:{0}, ConnectionId:{1} }}", Context.User.Identity.Name, Context.ConnectionId);
            return base.OnReconnected();
        }

    }
}
