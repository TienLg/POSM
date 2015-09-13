using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSManagement.Models
{
    public class Product
    {
        public Product()
        {
            this.ImportOrderItems = new HashSet<ImportOrderItem>();
            this.SaleOrderItems = new HashSet<SaleOrderItem>();
        }

        [Key]
        [Column("prod_id")]
        public string Id { get; set; }
        [Required]
        [Column("prod_name")]
        public string Name { get; set; }
        [Column("quantity_control")]
        public int QuantityControl { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("base_price")]
        public decimal BasePrice { get; set; }
        [Column("sale_price")]
        public decimal SalePrice { get; set; }
        [Column("date_added")]
        public DateTime DateAdded { get; set; }
        [Column("date_modified")]
        public DateTime DateModified { get; set; }

        public virtual ICollection<ImportOrderItem> ImportOrderItems { get; set; }
        public virtual ICollection<SaleOrderItem> SaleOrderItems { get; set; }

        public bool isValid()
        {
            if(Id == string.Empty || Name == string.Empty)
                return false;
            return true;
        }
    }
}