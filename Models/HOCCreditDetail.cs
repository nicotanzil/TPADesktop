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
    
    public partial class HOCCreditDetail
    {
        public string HOCId { get; set; }
        public string PaymentTypeId { get; set; }
        public System.DateTime DueDate { get; set; }
        public System.DateTime PaidDate { get; set; }
        public int PeriodMonth { get; set; }
        public decimal Amount { get; set; }
        public decimal AdditionalCharge { get; set; }
        public decimal LatePenalty { get; set; }
        public decimal TotalAmount { get; set; }
    
        public virtual HOCCredit HOCCredit { get; set; }
        public virtual PaymentType PaymentType { get; set; }
    }
}
