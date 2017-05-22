using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Diagnostics;

namespace webApp_SignalRLab.Hubs
{
    public class MonitorHub : Hub
    {
        private readonly MonitorTicker _monitorTicker;

        public MonitorHub() :
            this(MonitorTicker.Instance)
        {

        }

        public MonitorHub(MonitorTicker monitorTicker)
        {
            _monitorTicker = monitorTicker;
        }

        public void Open()
        {
            _monitorTicker.OpenMonitor();
        }

        public void Close()
        {
            _monitorTicker.CloseMonitor();
        }

        public string GetState()
        {
            return _monitorTicker.MonitorState.ToString();
        }

    }
}