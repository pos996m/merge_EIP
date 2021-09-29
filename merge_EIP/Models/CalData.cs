using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace merge_EIP.Models
{
    public class CalData
    {
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string color { get; set; }
        // 判斷哪來的，留言板、代辦事項、顯示誰請假
        public string state { get; set; }
        // 編號
        public int num { get; set; }
    }
}