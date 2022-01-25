using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Configuration;

namespace WebByteFlow
{
    public class Language
    {
        public class lang
        {
            public string CHS;
            public string ENG;
            public string CHT;
        };

        private string WebSiteRoot = WebConfigurationManager.AppSettings["WebSiteRoot"];

        public Dictionary<string, Dictionary<string, string>> changelang;

        public Language()
        {
            #region 繁體中文語系 CHT

            string chtJson = File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Language\\lang_zh-tw.json");

            #endregion 繁體中文語系 CHT

            #region 簡體中文語系 CHS

            string chsJson = File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Language\\lang_zh-cn.json");

            #endregion 簡體中文語系 CHS

            #region 英文中文語系 ENG

            string engJson = File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Language\\lang_en-us.json");

            #endregion 英文中文語系 ENG

            var cht = JsonConvert.DeserializeObject<Dictionary<string, string>>(chtJson);
            var chs = JsonConvert.DeserializeObject<Dictionary<string, string>>(chsJson);
            var eng = JsonConvert.DeserializeObject<Dictionary<string, string>>(engJson);

            changelang = new Dictionary<string, Dictionary<string, string>>()
             {
                {"CHT", cht},
                {"CHS", chs},
                {"ENG", eng },
             };
        }
    }
}