using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ByteStandard2
{
    public class CodeRoleAuth
    {
        public CodeRoleAuth()
        { }

        #region 變數

        public string uSystem { get; set; }
        public string Role { get; set; }
        public string FuncGrpId { get; set; }
        public string FuncGrpName { get; set; }
        public string Func { get; set; }
        public string FuncName { get; set; }
        public string FuncGrpSeq { get; set; }
        public string FuncSeq { get; set; }
        public string Lang { get; set; }
        public string Active { get; set; }

        #endregion 變數

        #region 取得角色功能權限

        /// 取得角色功能權限
        /// 2021-09-07 v1.0
        /// Created by Yiling
        public IEnumerable<CodeRoleAuth> GetRoleFunc(CodeRoleAuth roleAuth, string conn)
        {
            string condition = string.Empty;
            string conditionLang = string.Empty;

            switch (roleAuth.Lang)
            {
                case "CHT":
                    conditionLang = ", fg.ChtName as 'FuncGrpName', f.ChtName as 'FuncName'";
                    break;

                case "CHS":
                    conditionLang = ", fg.ChsName as 'FuncGrpName', f.ChsName as 'FuncName'";
                    break;

                case "ENG":
                    conditionLang = ", fg.EngName as 'FuncGrpName', f.EngName as 'FuncName'";
                    break;
            }

            if (!string.IsNullOrEmpty(roleAuth.FuncGrpId))
                condition += " and r.r.FuncGrpId = @FuncGrpId";

            if (!string.IsNullOrEmpty(roleAuth.Func))
                condition += " and r.[Function] = @Func";

            string cmd = $@"select r.Role
                                 , r.FuncGrpId
	                             , r.[Function] as 'Func'
	                             {conditionLang}
	                             , r.FuncGrpSeq
	                             , r.FuncSeq
	                             from CodeRoleAuth r with(nolock)
                            left join CodeFunctionGrp fg with(nolock) on r.System = fg.System and r.FuncGrpId = fg.GrpId
                            left join CodeFunction f with(nolock) on r.System = f.System and r.[Function] = f.[Function]
                            where r.System = @uSystem
                              and r.Role = @Role
                              and r.Active = @Active
                              {condition}
                            order by CONVERT(INT, FuncGrpSeq), CONVERT(INT, FuncSeq)";
            try
            {
                using (SqlConnection Conn = new SqlConnection(conn))
                    return Conn.Query<CodeRoleAuth>(cmd, roleAuth);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //throw;
                return null;
            }
        }

        #endregion 取得角色功能權限
    }
}