//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace merge_EIP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Funding
    {
        public int applicationNumber { get; set; }
        [Display(Name = "員工編號")]
        public string employeeID { get; set; }
        [Display(Name = "姓名")]
        public string employeeName { get; set; }
        [Display(Name = "申請金額")]
        public int applicationAmount { get; set; }
        [Display(Name = "類別")]
        public string Type { get; set; }
        [Display(Name = "狀態")]
        public string State { get; set; }
        [Display(Name = "原因")]
        public string Reason { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> submitDate { get; set; }
        public Nullable<System.DateTime> Auditdate { get; set; }
        public Nullable<bool> fcheck { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
