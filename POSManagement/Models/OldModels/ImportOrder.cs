using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace POSManagement.Models
{
    public class ImportOrder
    {
        public ImportOrder()
        {
            this.ImportOrderItems = new HashSet<ImportOrderItem>();
        }
    
        [Key]
        public string Id { get; set; }
        public int LineCount { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
        public virtual ICollection<ImportOrderItem> ImportOrderItems { get; set; }
        public virtual User User { get; set; }
    }

    public class ImportOrderItem
    {
        [Key]
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string UnitPrice { get; set; }
        public virtual ImportOrder ImportOrder { get; set; }
        public virtual Product Product { get; set; }
    }
}
