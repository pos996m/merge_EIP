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
    
    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.Backlog = new HashSet<Backlog>();
            this.BusinessTrip = new HashSet<BusinessTrip>();
            this.Calendar = new HashSet<Calendar>();
            this.dayOff = new HashSet<dayOff>();
            this.Funding = new HashSet<Funding>();
            this.gameRecord = new HashSet<gameRecord>();
            this.messageBoard = new HashSet<messageBoard>();
            this.Order = new HashSet<Order>();
            this.Order1 = new HashSet<Order>();
            this.punchIn = new HashSet<punchIn>();
            this.rePunchin = new HashSet<rePunchin>();
            this.workOvertime = new HashSet<workOvertime>();
        }
    
        public string employeeID { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string departmentID { get; set; }
        public string positionID { get; set; }
        public System.DateTime onBoard { get; set; }
        public Nullable<System.DateTime> terminationDate { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Telephone { get; set; }
        public string cellPhone { get; set; }
        public string Address { get; set; }
        public string emergencyContact { get; set; }
        public string emergencyContactphone { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Backlog> Backlog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BusinessTrip> BusinessTrip { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Calendar> Calendar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<dayOff> dayOff { get; set; }
        public virtual Department Department { get; set; }
        public virtual Position Position { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Funding> Funding { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<gameRecord> gameRecord { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<messageBoard> messageBoard { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Order { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Order1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<punchIn> punchIn { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rePunchin> rePunchin { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<workOvertime> workOvertime { get; set; }
    }
}
