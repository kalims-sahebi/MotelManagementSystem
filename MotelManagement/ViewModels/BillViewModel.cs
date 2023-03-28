using MotelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.ViewModels
{
    public class BillViewModel
    {
        public IEnumerable<BillModel> DrinkBill { get; set; }
        public IEnumerable<BillModel> FoodBill { get; set; }
        public RoomBill Room { get; set; }
    }
}
