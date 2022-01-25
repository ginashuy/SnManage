using ByteStandard2;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebByteFlow
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        #region 變數

        public string conn = WebConfigurationManager.ConnectionStrings["EvisionConnectionString"].ConnectionString.ToString();
        public string lang;
        public cLoginState oStatus;
        public Language lanng = new Language();

        #endregion 變數

        protected void Page_Load(object sender, EventArgs e)
        {
            oStatus = Session["LoginObject"] as cLoginState;
            lblRole.Text = oStatus.Role;
            lang = Session["Lang"].ToString();

            if (!IsPostBack)
            {
                oStatus = Session["LoginObject"] as cLoginState;
                //使用者語系
                lang = Session["Lang"].ToString();
                Dynamic_Header();
            }
        }

        #region 依照使用者判斷使用語系

        public void changelang(List<Control> controllist, string userlang, string condition)
        {
            foreach (Control control in controllist)
            {
                if (control is Label)
                    ((Label)control).Text = lanng.changelang[userlang][control.ID + condition];
                else
                if (control is Button)
                    ((Button)control).Text = lanng.changelang[userlang][control.ID + condition];
            }
        }

        #endregion 依照使用者判斷使用語系

        protected void ChangeLang_Click(object sender, EventArgs e)
        {
            Session["Lang"] = ((Control)sender).ClientID.Substring(3);
            Dynamic_Header();
            Response.Redirect(Session["NowWeb"].ToString());
        }

        private void Dynamic_Header()
        {
            rptNavBar.DataSource = null;
            rptNavBar.DataBind();
            lang = Session["Lang"].ToString();
            switch (lang)
            {
                case "CHT":
                    lblChooseLang.Text = "繁中";
                    break;

                case "CHS":
                    lblChooseLang.Text = "简中";
                    break;

                default:
                    lblChooseLang.Text = "English";
                    break;
            }

            //讀取使用者角色的權限功能明細
            var getRoleAuth
               = new CodeRoleAuth()
                .GetRoleFunc(new CodeRoleAuth
                {
                    uSystem = "WebSnManage",
                    Role = lblRole.Text,
                    Active = "1",
                    Lang = lang
                }, conn).ToList();

            #region 動態產生導覽列選項

            string navbar = string.Empty;

            for (int i = 0; i < getRoleAuth.Count; i++)
            {
                string funcGrp = getRoleAuth[i].FuncGrpSeq;

                //計算同一個群組有幾筆功能
                var funcGrpCount = getRoleAuth.Where(p => p.FuncGrpSeq == funcGrp).LongCount();

                string navbarChild = string.Empty;
                if (funcGrpCount == 1 && string.IsNullOrEmpty(getRoleAuth[i].FuncGrpId))
                    navbar += $@"<li class='nav-list'><a href='{getRoleAuth[i].Func}'>
                                     <span>{getRoleAuth[i].FuncName}</span></a></li>";
                else if (funcGrpCount > 1 || !string.IsNullOrEmpty(getRoleAuth[i].FuncGrpId))
                {
                    int listIndex = 0;
                    for (int j = 0; j < funcGrpCount; j++)
                    {
                        listIndex = i + j;
                        navbarChild += $@"<li class='nav-list'><a href='{getRoleAuth[listIndex].Func}'>
                                              <span>{getRoleAuth[listIndex].FuncName}</span></a></li>";
                    }
                    navbar += $@"<li class='nav-list drop-down-list'>
                                         <a href='javascript:;'>
                                             <span>{getRoleAuth[i].FuncGrpName}</span>
                                             <svg width='16' height='10' xmlns='http://www.w3.org/2000/svg'>
                                                 <g>
                                                     <rect fill='none' id='canvas_background1' height='402' width='582' y='-1' x='-1' />
                                                 </g>
                                                 <g>
                                                     <polygon id='svg_1' points='8,7.328 4.275,2.672 11.725,2.672 ' fill='#73A3B8' />
                                                 </g>
                                             </svg>
                                         </a>
                                         <ul class='drop-down-menu'>
                                             {navbarChild}
                                         </ul>
                                     </li>";
                    i = listIndex;
                }
                else
                    navbar += string.Empty;
            }

            List<string> navbarshow = new List<string>();
            navbarshow.Add(navbar);

            rptNavBar.DataSource = navbarshow;
            rptNavBar.DataBind();

            #endregion 動態產生導覽列選項

            //讀取網頁輸入的網址
            StringBuilder sb = new StringBuilder().Append(System.IO.Path.GetFileName(Request.PhysicalPath));

            string url = sb.ToString();

            //確認URL的使用權限
            var getRoleUrl = getRoleAuth.Where(p => p.Func == url).ToList();

            if (getRoleUrl.Count == 0 && url != "ErrorPage")
            {
                Response.Write($@"<script>alert('此帳號未授權{url}權限');</script>
                                 <script type='text/javascript'>location.href='{oStatus.Redirect}';</script>");
            }

            #region 依照使用者判斷使用語系

            changelang(new List<Control>() { null }, lang, "");

            var userInfo
                = new UserInfo()
                 .GetUserInfo(new UserInfo
                 {
                     UserId = oStatus.UserId.Trim(),
                     Lang = lang,
                     Active = "Y"
                 }, conn).FirstOrDefault();

            lblUser.Text = $"{userInfo.UserId} {userInfo.Name}";

            #endregion 依照使用者判斷使用語系
        }
    }
}