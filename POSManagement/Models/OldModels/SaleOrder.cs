using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSManagement.Models
{
    public class SaleOrder
    {
        public SaleOrder()
        {
            this.SaleOrderItems = new HashSet<SaleOrderItem>();
        }

        [Key]
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("cust_id")]
        public string CustomerId { get; set; }
        [Column("user_id")]
        public string UserId { get; set; }
        //[Column(TypeName = "DateTime2")]
        [Column("date_ship", TypeName = "DateTime2")]
        public DateTime ShipDate { get; set; }
        [Column("total_price")]
        public decimal TotalPrice { get; set; }

        [Column("received_money")]
        public decimal TakenMoney { get; set; }
        [Column("order_status")]
        public string Status { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<SaleOrderItem> SaleOrderItems { get; set; }

        public string CustomerName { get { return Customer.Name; } }

        public string SalerName { get { return User.Name; } }

        public SaleOrder DeepCopy()
        {
            SaleOrder other = (SaleOrder)this.MemberwiseClone();
            return other;
        }
    }

    public class SaleOrderItem
    {
        [Key]
        [Column("order_id")]
        public int OrderId { get; set; }
        [Key]
        [Column("order_item_id")]
        public int OrderItemId { get; set; }
        [Column("prod_id")]
        public string ProductId { get; set; }
        [Column("item_price")]
        public decimal ItemPrice { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("quantity_type")]
        public string QuantityType { get; set; }

        public virtual Product Product { get; set; }
        public virtual SaleOrder SaleOrder { get; set; }

        public string ProductName { get { return Product.Name; } }
        public decimal BasePrice { get { return Product.BasePrice; } }
        public decimal FirstSalePrice { get { return Product.SalePrice; } }

    }
}
