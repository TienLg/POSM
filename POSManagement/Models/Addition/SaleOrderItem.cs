using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSManagement.Models
{
    public partial class SaleOrderItem
    {
        // Manual add
        public string ProductName { get; set; }
        public int QuantityStocksOnHand { get; set;}
        public int QuantityUnitsOnHand { get; set; }
        public decimal TotalItemPrice { get; set; }

    }
}
