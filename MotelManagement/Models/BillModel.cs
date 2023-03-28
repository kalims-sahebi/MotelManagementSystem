using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.Models
{
    public class BillModel
    {
        public string Description { get; set; }
        public int Price { get; set; }
        public int qty { get; set; }
        public int total { get; set; }
        public int OrderId { get; set; }
    }
}
