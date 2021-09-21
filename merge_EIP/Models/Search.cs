using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace merge_EIP.Models
{
    public class Search
    {
        public List<dayOff> leave { get; set; }
        public List<Funding> budget { get; set; }
        public List<workOvertime> OT { get; set; }
        public List<rePunchin> repunch { get; set; }
    }
}