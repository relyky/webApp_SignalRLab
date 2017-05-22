using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace webApp_SignalRLab
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            #region # 取得應用層級設定
            //// 自 Web.config 取設定值，改自properties取設定
            //var appSettings = System.Web.Configuration.WebConfigurationManager.AppSettings; 
            //String ApiBaseUrl = appSettings["ApiBaseUrl"];

            //// 相依 WebAPI 網址
            //Application.Add("APIBASEURL", new Uri(ApiBaseUrl));
            //// 加密金鑰
            //Application.Add("TOKENAESKEY", new byte[32] { 11, 22, 33, 44, 55, 66, 77, 88, 99, 100, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 255, 254, 253, 252, 251, 250, 249, 248, 247, 246, 245, 244 });
            //Application.Add("TOKENAESIV", new byte[16] { 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 49, 53, 59, 61, 67 });

            #endregion
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            Session["SESSION_START_TIME"] = now;
            //Session["SESSION_EXPIRATION_TIME"] = now.AddMinutes(Session.Timeout); // session 到期時間會自動延伸，無法預估。
        }
    }
}
