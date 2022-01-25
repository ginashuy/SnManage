using System;
using System.Web;

namespace Byte48
{
    public class Tools
    {
        public string ip;
        public string sessionId;

        public Tools()
        {
            HttpContext context = HttpContext.Current;
            string sIPAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(sIPAddress))
            {
                ip = context.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                string[] ipArray = sIPAddress.Split(new Char[] { ',' });
                ip = ipArray[0];
            }
        }

        /// <summary>
        /// 取得用戶IP資訊
        ///  2016-11-22 Create by Jason
        /// </summary>
        /// <returns>IP</returns>
        public static string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string sIPAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(sIPAddress))
            {
                return context.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                string[] ipArray = sIPAddress.Split(new Char[] { ',' });
                return ipArray[0];
            }
        }

        /// <summary>
        /// 取得IP資訊(非靜態)
        ///  2017-01-13 Create by Jason
        /// </summary>
        /// <returns>IP</returns>
        public string GetIP()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string sIPAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(sIPAddress))
            {
                return context.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                string[] ipArray = sIPAddress.Split(new Char[] { ',' });
                return ipArray[0];
            }
        }

        /// <summary>
        /// 全形自動轉半形工具(DBCcase)
        /// 2016-12-14 Create by Jason
        /// </summary>
        /// <param name="input">任意字元串</param>
        /// <returns>半形字元串</returns>
        /// <remarks>
        /// 全形空格為12288，半形空格為32
        /// 其他字元半形(33-126)與全形(65281-65374)的對應關係是：均相差65248
        /// </remarks>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
    }
}
