using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace POSManagement.Models
{
    public class Promotion
    {
        [Key]
        public string Id { get; set; }
        public string Description { get; set; }
        public string ProductId { get; set; }
        public DateTime AddedDay { get; set; }
        public DateTime ExpiredDay { get; set; }
        public int Status { get; set; }

    }
}
