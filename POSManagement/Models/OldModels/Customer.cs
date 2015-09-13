using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace POSManagement.Models
{
    public class Customer
    {
        public Customer()
        {
            this.SaleOrders = new HashSet<SaleOrder>();
        }

        [Key]
        [Column("cust_id")]
        public string Id { get; set;}
        [Required]
        [Column("cust_name")]
        public string Name{ get; set;}
        [Column("cust_address")]
        public string Address{ get; set;}
        [Column("cust_phone")]
        public string Phone { get; set;}
        public virtual ICollection<SaleOrder> SaleOrders { get; set; }

    }
}
