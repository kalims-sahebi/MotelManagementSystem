using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.Models
{
    public class OrderRemoveModel
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        /// <summary>
        /// //////////
        /// </summary>
        public int FoodId { get; set; }
        public int FoodCount { get; set; }
        public string Food { get; set; }
        public int Price { get; set; }
        /// <summary>
        /// //////
        /// </summary>
        public int DrinkId { get; set; }
        public string Drink { get; set; }
        public int DrinkCount { get; set; }
        public int DPrice { get; set; }
        
    }
}
