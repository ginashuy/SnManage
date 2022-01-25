using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Byte48;

namespace ByteStandard2
{
    public class cLoginState
    {
        public cLoginState()
        { }

        #region 變數

        public string UserId { get; set; }
        public string Pwd { get; set; }
        public string EngName { get; set; }
        public string System { get; set; }
        public string Role { get; set; }
        public string Region { get; set; }
        public string Redirect { get; set; }
        public string LoginStatus { get; set; }
        public string IpAddress { get; set; }
        public string Lang { get; set; }
        public List<string> Systems { get; set; }

        #endregion 變數

        #region 確認登入資訊

        /// 2021-09-07 v1.0
        /// Created by Yiling
        public IEnumerable<cLoginState> ChkLoginState(cLoginState loginInfo, string conn)
        {
            string cmd = @"select UserId
                                , Password as 'Pwd'
                                , EngName
                                , Lang
                           from UserId
                           where UserId = @UserId
                             and Password = @Pwd ";

            try
            {
                using (SqlConnection Conn = new SqlConnection(conn))
                {
                    var result = Conn.Query<cLoginState>(cmd, loginInfo).AsList();

                    if (result.Count != 0)
                    {
                        //讀取使用者帳號已開通的系統別清單 (by Yiling)
                        loginInfo.Systems = new List<string>();
                        string cmd1
                            = @"select System from UserSystem where UserId = @UserId and System = @System";
                        var uSystems = Conn.Query<cLoginState>(cmd1, loginInfo);

                        foreach (var item in uSystems)
                        { loginInfo.Systems.Add(item.System.ToString()); }

                        result[0].LoginStatus = "OK";
                        result[0].Systems = loginInfo.Systems;
                        result[0].Role = "Admin";
                    }
                    else
                        result.Add(new cLoginState { LoginStatus = "Fail", UserId = loginInfo.UserId, Pwd = loginInfo.Pwd });

                    #region 寫入Log

                    Tools tool = new Tools();
                    IpAddress = tool.GetIP();

                    string cmdLog
                        = $@"insert into LoginLog
                                        (UserID
                                       , UserName
                                       , Password
                                       , System
                                       , Role
                                       , Region
                                       , Message
                                       , IP
                                       , Date)
                             values(@UserId
                                  , '{result[0].EngName}'
                                  , @Pwd
                                  , '{loginInfo.System}'
                                  , '{result[0].Role}'
                                  , '{result[0].Region}'
                                  , 'Login {result[0].LoginStatus}'
                                  , '{IpAddress}'
                                  , GETDATE())";

                    Conn.Execute(cmdLog, result);

                    #endregion 寫入Log

                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                //throw;
                return null;
            }
        }

        #endregion 確認登入資訊

        #region 登入元件for SSO

        /// 登入元件for SSO
        /// 2021-10-24 v1.0
        /// Created by Yiling
        public IEnumerable<cLoginState> ChkSsoLoginState(cLoginState loginInfo, string conn)
        {
            string cmd = @"select UserId
                                , EngName
                                , Role
                                , System
                                , Lang
                                , Region
                                , Redirect
                           from VrmaLoginCheck
                           where UserId = @UserId
                             and System = @System";

            try
            {
                using (SqlConnection Conn = new SqlConnection(conn))
                {
                    var result = Conn.Query<cLoginState>(cmd, loginInfo).AsList();

                    if (result.Count != 0)
                    {
                        //讀取使用者帳號已開通的系統別清單 (by Yiling)
                        loginInfo.Systems = new List<string>();
                        string cmd1
                            = @"select System from UserSystem where UserId = @UserId";
                        var uSystems = Conn.Query<cLoginState>(cmd1, loginInfo);

                        foreach (var item in uSystems)
                        { loginInfo.Systems.Add(item.System.ToString()); }

                        result[0].LoginStatus = "OK";
                        result[0].Systems = loginInfo.Systems;
                    }
                    else
                        result.Add(new cLoginState { LoginStatus = "Fail", UserId = loginInfo.UserId, Pwd = loginInfo.Pwd });

                    #region 寫入Log

                    Tools tool = new Tools();
                    IpAddress = tool.GetIP();

                    string cmdLog
                        = $@"insert into LoginLog
                                        (UserID
                                       , UserName
                                       , Password
                                       , System
                                       , Role
                                       , Region
                                       , Message
                                       , IP
                                       , Date)
                             values(@UserId
                                  , '{result[0].EngName}'
                                  , 'FromSingleSignOn'
                                  , '{loginInfo.System}'
                                  , '{result[0].Role}'
                                  , '{result[0].Region}'
                                  , 'Login {result[0].LoginStatus}'
                                  , '{IpAddress}'
                                  , GETDATE())";

                    Conn.Execute(cmdLog, result);

                    #endregion 寫入Log

                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                //throw;
                return null;
            }
        }

        #endregion 登入元件for SSO
    }
}