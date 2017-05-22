using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR;

namespace webApp_SignalRLab.Hubs
{
    public class MonitorTicker
    {
        #region Singleton instance

        private readonly static Lazy<MonitorTicker> _instance = new Lazy<MonitorTicker>(
            () => new MonitorTicker(GlobalHost.ConnectionManager.GetHubContext<MonitorHub>().Clients));

        public static MonitorTicker Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        #endregion

        #region properties
        public MonitorStateEnum MonitorState
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
        #endregion attributes

        public enum MonitorStateEnum
        {
            Closed,
            Open
        }

        private MonitorTicker(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
        }

        public void OpenMonitor()
        {
            lock (_thisLock)
            {
#if DEBUG
                TimeSpan updateInterval = TimeSpan.FromMilliseconds(3000);
#else
                TimeSpan updateInterval = TimeSpan.FromMilliseconds(250);
#endif
                _timer = new System.Threading.Timer(TimerElapsed, null, updateInterval, updateInterval);
                this.MonitorState = MonitorStateEnum.Open;
            }
        }

        public void CloseMonitor()
        {
            lock (_thisLock)
            {
                if(_timer != null)
                {
                    _timer.Dispose();
                    _timer = null;
                }

                this.MonitorState = MonitorStateEnum.Closed;
            }
        }

        private void TimerElapsed(object state)
        {
            // This function must be re-entrant as it's running as a timer interval handler
            lock (_timerLock)
            {
                // Broadcast notification.
                NotifyMessage msg = new NotifyMessage()
                {
                    notifyType = "info",
                    notifyMsg = "伺服器時間 " + DateTime.Now.ToLongTimeString()
                };

                Clients.All.NotifyMessage(msg);
            }
        }
    }

    public class NotifyMessage
    {
        public NotifyMessage()
        {
        }

        public string notifyType;
        public string notifyMsg;
    }
}
