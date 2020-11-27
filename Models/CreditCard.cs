//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TPA_Desktop_NT20_2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CreditCard
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CreditCard()
        {
            this.CreditCardTransactions = new HashSet<CreditCardTransaction>();
        }
    
        public string CreditCardId { get; set; }
        public string AccountId { get; set; }
        public string CompanyId { get; set; }
        public string FamilyCard { get; set; }
        public string IdentityCard { get; set; }
        public decimal Limit { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual CreditCardCompany CreditCardCompany { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CreditCardTransaction> CreditCardTransactions { get; set; }
    }
}