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

    public partial class tOrderDetail
    {
        public int OrderDetailId { get; set; }
        [Display(Name = "訂單編號")]
        public int fOrderId { get; set; }
        [Display(Name = "點餐者員工編號")]
        public string DetailEID { get; set; }
        [Display(Name = "品項")]
        public string fFood { get; set; }
        [Display(Name = "單價")]
        public int fPrice { get; set; }
        [Display(Name = "數量")]
        public int fQty { get; set; }
        [Display(Name = "備註")]
        public string fNote { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual tOrder tOrder { get; set; }
    }
}
