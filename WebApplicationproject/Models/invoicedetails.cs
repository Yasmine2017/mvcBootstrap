//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplicationproject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class invoicedetails
    {
        public int id { get; set; }
        public Nullable<int> productid { get; set; }
        public Nullable<int> invoiceid { get; set; }
        public Nullable<int> Quentity { get; set; }
        public Nullable<double> price { get; set; }
    
        public virtual invoice invoice { get; set; }
        public virtual product product { get; set; }
    }
}
