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
    
    public partial class rePunchin
    {
        public int repunchID { get; set; }
        public string employeeID { get; set; }
        public string employeeName { get; set; }
        public System.DateTime repunchdate { get; set; }
        public Nullable<System.TimeSpan> repunchTimeIn { get; set; }
        public Nullable<System.TimeSpan> repunchTimeOut { get; set; }
        public string Reason { get; set; }
        public string State { get; set; }
        public Nullable<System.DateTime> submitDate { get; set; }
        public Nullable<System.DateTime> Auditdate { get; set; }
        public Nullable<bool> fcheck { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
