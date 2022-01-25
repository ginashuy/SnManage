using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ByteStandard2
{
    public class UserInfo
    {
        public UserInfo()
        { }

        #region 變數

        public string UserId { get; set; }
        public string EngName { get; set; }
        public string ChtName { get; set; }
        public string ChsName { get; set; }
        public string Company { get; set; }
        public string Lang { get; set; }
        public string MainUser { get; set; }
        public string Email { get; set; }
        public string Active { get; set; }
        public string Remark { get; set; }
        public string exeStatus { get; set; }

        public string Name { get; set; }

        #endregion 變數

        #region 取得使用者資訊

        /// 2021-09-07 v1.0
        /// Created by Yiling
        public IEnumerable<UserInfo> GetUserInfo(UserInfo userInfo, string conn)
        {
            string condition = string.Empty;
            string conditionWhere = string.Empty;

            conditionWhere += string.IsNullOrEmpty(userInfo.Active) ? "" : " and Active = @Active ";

            switch (userInfo.Lang)
            {
                case "CHT":
                    condition += ", ChtName as 'Name'";
                    break;

                case "CHS":
                    condition += ", ChsName as 'Name'";
                    break;

                case "ENG":
                    condition += ", EngName as 'Name'";
                    break;

                default:
                    break;
            }

            string cmd = $@"select UserId
                                 {condition}
                                 , Lang
                                 , Company
                                 , Email
                                 , MainUser
                                 , Active
                           from UserId
                           where UserId = @UserId {conditionWhere}";

            try
            {
                using (SqlConnection Conn = new SqlConnection(conn))
                    return Conn.Query<UserInfo>(cmd, userInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                //throw;
                return null;
            }
        }

        #endregion 取得使用者資訊
    }
}