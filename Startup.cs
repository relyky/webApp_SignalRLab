using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(webApp_SignalRLab.Startup))]
namespace webApp_SignalRLab
{
    /// <summary>
    /// OWIN startup class, OWIN架構下必備啟動類別
    /// </summary>
    /// <see cref="https://dotblogs.com.tw/jgame2012/2016/07/27/010210"/>
    public class Startup
    {
        public void Configuration(Owin.IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}
