using Byte48;
using ByteStandard2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSnManage
{
    public partial class Login : System.Web.UI.Page
    {
        public string conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            //新增當使用者使用IE時，直接轉頁告知「请勿使用Internet Explorer浏览器操作此系统」 (by Yiling)
            System.Web.HttpBrowserCapabilities browser = Request.Browser;
            if (browser.Browser == "InternetExplorer")
            {
                Response.Redirect("IEError.html");
            }
            else
            {
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Session.RemoveAll();

                if (!IsPostBack)
                {
                }
                //自webconfig 讀取connection string資訊
                conn = WebConfigurationManager.ConnectionStrings["EvisionConnectionString"].ConnectionString.ToString();
            }
        }

        protected void btnSummit_Click(object sender, EventArgs e)
        {
            LoginMessage.InnerText = "";
            LoginMessage.Visible = true;
            if (LoginId.Text == "")
                LoginMessage.InnerText = "User ID is required!!";
            else if (LoginPassword.Text == "")
                LoginMessage.InnerText = "Password is required!!";
            else
            {
                cLoginState oStstus
                    = new cLoginState()
                     .ChkLoginState(new cLoginState
                     {
                         UserId = Tools.ToDBC(LoginId.Text).ToUpper(),
                         Pwd = Tools.ToDBC(LoginPassword.Text),
                         System = "WebSnManage"
                     }, conn).FirstOrDefault();

                Session["LoginObject"] = oStstus;

                if (oStstus.LoginStatus.ToString() != "OK")
                    LoginMessage.InnerText = "User ID or Password is not correct!!";
                else
                {
                    Session["LANG"] = oStstus.Lang;
                    Response.Redirect("AddDate.aspx");
                }
            }
        }
    }
}