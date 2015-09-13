using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace POSManagement.Models
{
    public class User
    {
        public User()
        {
            this.ImportOrders = new HashSet<ImportOrder>();
            this.SaleOrders = new HashSet<SaleOrder>();
        }

        [Key]
        [Column("user_id")]
        public string Id { get; set; }
        [Required]
        [Column("user_name")]
        public string Name { get; set; }
        [Column("user_pwd")]
        public string Password { get; set; }
        [Column("user_role")]
        public string Role { get; set; }

        public virtual ICollection<ImportOrder> ImportOrders { get; set; }
        public virtual ICollection<SaleOrder> SaleOrders { get; set; }
    }
}
