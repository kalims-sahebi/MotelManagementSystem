using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.Models
{
    public class AjaxDrinkModel
    {
        //One property is DPrice instead of price
        //so need seperate DrinkModel

        public string Drink { get; set; }
        public int DPrice { get; set; }
    }
}
