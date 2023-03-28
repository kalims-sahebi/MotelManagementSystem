using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.Models
{
    public class RoomBill
    {
        public string RoomNumber { get; set; }
        public int Price { get; set; }
        public int Days { get; set; }
        public int TotalRent { get; set; }
       
    }
}
