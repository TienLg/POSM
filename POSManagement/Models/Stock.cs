//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace POSManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Stock
    {
        public int stock_id { get; set; }
        public string prod_id { get; set; }
        public decimal base_price_by_unit { get; set; }
        public decimal base_price_by_stock { get; set; }
        public int quantity_by_unit { get; set; }
        public int quantity_by_stock { get; set; }
        public int quantity_control { get; set; }
        public Nullable<System.DateTime> date_expired { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
