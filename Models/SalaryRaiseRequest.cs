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
    
    public partial class SalaryRaiseRequest
    {
        public string RequestId { get; set; }
        public string EmployeeId { get; set; }
        public decimal Amount { get; set; }
        public bool IsApproved { get; set; }
        public System.DateTime RequestDate { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}