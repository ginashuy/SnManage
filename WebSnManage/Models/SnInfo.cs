using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSnManage.Models
{
    public class SnInfo
    {
        public string SysId { get; set; }
        public string Sn { get; set; }
        public string StartDtm { get; set; }
        public string EndDtm { get; set; }
        public List<ItemCheck> ItemChecks { get; set; }
    }
}