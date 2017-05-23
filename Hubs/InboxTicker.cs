using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR;

namespace webApp_SignalRLab.Hubs
{
    public class InboxTicker
    {
        #region Singleton instance

        private readonly static Lazy<InboxTicker> _instance = new Lazy<InboxTicker>(
            () => new InboxTicker(GlobalHost.ConnectionManager.GetHubContext<InboxHub>().Clients));

        public static InboxTicker Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        #endregion

        #region properties
        public InboxStateEnum InboxState
        {
            get;
            private set;
        }

        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }
        #endregion

        #region attributes
        private Timer _timer;
        private readonly object _thisLock = new object();
        private readonly object _timerLock = new object();

        private Dictionary<string, string> mapUserConn = new Dictionary<string, string>();

        #endregion attributes

        public enum InboxStateEnum
        {
            Closed,
            Open
        }

        private InboxTicker(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
        }

        public void OpenInbox()
        {
            lock (_thisLock)
            {
#if DEBUG
                TimeSpan updateInterval = TimeSpan.FromMilliseconds(2000);
#else
                TimeSpan updateInterval = TimeSpan.FromMilliseconds(250);
#endif
                _timer = new System.Threading.Timer(TimerElapsed, null, updateInterval, updateInterval);
                this.InboxState = InboxStateEnum.Open;
            }
        }

        public void CloseInbox()
        {
            lock (_thisLock)
            {
                if (_timer != null)
                {
                    _timer.Dispose();
                    _timer = null;
                }

                this.InboxState = InboxStateEnum.Closed;
            }
        }

        public void RegisterUser(string idName, string connId)
        {
            lock(_thisLock)
            {
                //# replace: idName, connId
                if(mapUserConn.ContainsKey(idName))
                {
                    mapUserConn[idName] = connId;
                }
                else
                {
                    mapUserConn.Add(idName, connId);
                }
            }
        }

        private void TimerElapsed(object state)
        {
            // This function must be re-entrant as it's running as a timer interval handler
            lock (_timerLock)
            {
                // notify message to registed user
                foreach(var c in mapUserConn)
                {
                    string idName = c.Key;
                    string connId = c.Value;

                    // Broadcast notification.
                    NotifyMessage msg = new NotifyMessage()
                    {
                        notifyType = new string[] { "success","info","warning","danger" }[DateTime.Now.Ticks % 4],
                        notifyMsg = string.Format("伺服器時間 {0:HH:mm:ss} for {1}", DateTime.Now, idName)
                    };

                    Clients.Client(connId).NotifyMessage(msg);
                }
            }
        }

    }
}