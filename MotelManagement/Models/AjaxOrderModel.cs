using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.Models
{
    public class AjaxOrderModel
    {
        public int FoodId { get; set; }
        public int FoodCount { get; set; }
        public int DrinkCount { get; set; }
        public int DrinkId { get; set; }
        public int CustomerId { get; set; }
    }
}
