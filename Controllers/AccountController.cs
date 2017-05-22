using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using webApp_SignalRLab.ViewModels;

namespace webApp_SignalRLab.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            // 已登入
            if (!Request.IsAuthenticated)
            {
                using (AccountApi api = new AccountApi())
                {
                    ViewBag.vcode = api.GetVcode();
                }
            }

            ViewBag.Trace = new StringBuilder("GET Account/Login; ");
            return View();
        }

        // POST: Account
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string idnamepwd, string rememberMe, string ReturnUrl)
        {
            try
            {
                // for tracing
                ViewBag.Trace = new StringBuilder("[v4] ");

                //// prefix validation
                //if (idname == "" || idpwd == "")
                //{
                //    // 失敗處理…
                //    ViewBag.lastErr = new LastErrMsg("ERROR", "帳號或密碼空白。");
                //    return View();
                //}

                // resource
                AccountApi api = new AccountApi();

                //# 登入驗證, 並取回　authToken, loginUser
                String authToken;
                LoginUserInfo loginUser;
                LastErrMsg err;
                if (!api.Authenticate(idnamepwd, out authToken, out loginUser, out err))
                {
                    ViewBag.lastErr = err;
                    ViewBag.vcode = api.GetVcode(); // 重取驗證碼
                    return View();
                }

                ViewBag.Trace.Append("通過Authenticate; ");

                //# 註記入 Session
                Session["LoginAuthToken"] = authToken;
                Session["LoginUserInfo"] = loginUser;

                ViewBag.Trace.Append("註記入Session; ");

                #region ## 寫入 cookie --- 客制化
                /// 將使得 Request.IsAuthenticated == true ;
                /// 將使得 [Authorize] 有作用

                string userData = JsonConvert.SerializeObject(loginUser);
                bool isPersistent = (rememberMe == "yes"); // 記住我/永續性
                DateTime issueDate = DateTime.Now;
                DateTime expiresDate = issueDate.Add(FormsAuthentication.Timeout);

                ViewBag.Trace.Append("備好auth cookie材料; ");

                //# auth ticket
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    2,
                    loginUser.userId,
                    issueDate,
                    expiresDate,  // ticket 到期日
                    isPersistent, //永續性
                    userData, // 加入客制化資料
                    FormsAuthentication.FormsCookiePath
                    );

                ViewBag.Trace.Append("產生Forms ticket; ");

                //# new auth cookie
                string encTicket = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket)
                {
                    Expires = isPersistent ? expiresDate : DateTime.MinValue, // cookie 到期日
                    HttpOnly = true
                };

                ViewBag.Trace.Append("產生auth cookie; ");

                //# 寫入auth cookie，客制化用
                Response.Cookies.Add(cookie);

                ViewBag.Trace.Append("寫入auth cookie; ");

                #endregion

                ////## 寫入 cookie --- 預設
                ///// 寫入cookie，將使得 Request.IsAuthenticated == true 與 [Authorize] 有作用
                //FormsAuthentication.SetAuthCookie(idname, isPersistent: rememberMe == "yes");

                // success
                if (ReturnUrl != "")
                    return Redirect(ReturnUrl); // return the referred page
                // return home page; 
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                // 失敗處理…
                ViewBag.lastErr = new LastErrMsg("EXCEPTION", "發生不可預期的例外失敗！請重啟登入程序。" + ex.Message, ex.GetType().Name);
                return View();
            }

        }

    }

    internal class AccountApi : IDisposable
    {
        // resource
        HttpClient http = null;

        public AccountApi()
        {
            //http = new HttpClient();
            //http.BaseAddress = (Uri)HttpContext.Current.Application["APIBASEURL"]; // new Uri(@"http://localhost/AppPortalApi/");
            ////http.DefaultRequestHeaders.Add("Authorization", @"Basic xxxx");
        }

        #region Implement IDisposable.

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                    if (http != null)
                    {
                        http.Dispose();
                        http = null;
                    }
                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AccountApi()
        {
            Dispose(false);
        }

        #endregion

        /// <summary>
        /// 測試資料庫連線
        /// </summary>
        public Boolean TestDbConnection()
        {
            //HttpResponseMessage response = http.PostAsJsonAsync(@"api/Account/TestDbConnection", new StringContent("")).Result;
            //
            //LastErrMsg err = new LastErrMsg("WARNNING", "預設失敗。");
            //if (response.IsSuccessStatusCode)
            //{
            //    err = response.Content.ReadAsAsync<LastErrMsg>().Result;
            //    return err.errType == "SUCCESS";
            //}

            return false; // 預設失敗
        }

        public Boolean Authenticate(string idnamepwd, out String authToken, out LoginUserInfo loginUser, out LastErrMsg err)
        {
            //# 未實作。假裝成功
            // 執行成功
            err = new LastErrMsg();
            loginUser = new LoginUserInfo()
            {
                userId = "nonmae",
                name = "noname",
                cName = "無名氏",
                deptId = "nodept",
                deptName = "nodeptname"
            };
            authToken = "This Is fake AuthToken.";
            return true;


            //// 預設失敗;
            //authToken = string.Empty;
            //loginUser = null;
            //err = new LastErrMsg("WARNNING", "預設失敗。");
            //
            //// GO
            ////string vcode = "abcd1234";
            ////var objectIdnamepwd = new { idpwd, vcode, idname };
            ////string jsonIdnamepwd = JsonConvert.SerializeObject(objectIdnamepwd);
            ////string idnamepwd = Convert.ToBase64String(Encoding.GetEncoding("utf-8").GetBytes(jsonIdnamepwd)); // Base64
            //HttpResponseMessage response = http.PostAsync(@"api/Authenticate/" + idnamepwd, new StringContent("")).Result;
            //if (!response.IsSuccessStatusCode)
            //{
            //    err = new LastErrMsg("FAIL", "執行失敗！HTTP Status Code:" + response.StatusCode.ToString());
            //    return false;
            //}
            //
            //// 執行成功
            //err = response.Content.ReadAsAsync<LastErrMsg>().Result;
            //if (err.errType == "SUCCESS")
            //{
            //    authToken = err.errMsg; // b64UserInfo
            //    byte[] encUserInfo = Convert.FromBase64String(authToken);
            //    string jsonUserInfo = Utilities.DecryptStringFromBytes_Aes(encUserInfo, (byte[])HttpContext.Current.Application["TOKENAESKEY"], (byte[])HttpContext.Current.Application["TOKENAESIV"]);
            //    loginUser = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginUserInfo>(jsonUserInfo);
            //    return true;
            //}
            //
            //// 預設失敗
            //return false;
        }

        public string GetVcode()
        {
            return "Vcode"; // 未實作

            //HttpResponseMessage response = http.PostAsync(@"api/Account/vcode", new StringContent("")).Result;
            //
            //String vcode = String.Empty;
            //if (response.IsSuccessStatusCode)
            //{
            //    vcode = response.Content.ReadAsAsync<String>().Result;
            //    return vcode;
            //}
            //
            //return String.Empty; // 預設失敗
        }
    }

}