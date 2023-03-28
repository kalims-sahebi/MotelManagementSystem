using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.Models
{
    public class FoodDrinkModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "نام غذا/نوشیدنی ضروریست")]
        [Display(Name = "نام غذا/نوشیدنی")]
        public string Name { get; set; }
        [Required(ErrorMessage = "قیمت غذا/نوشیدنی ضروریست")]
        [Display(Name = "قیمت غذا/نوشیدنی")]
        public int Price { get; set; }
        [Required(ErrorMessage = "نوعیت غذا/نوشیدنی ضروریست")]
        [Display(Name = "نوعیت غذا/نوشیدنی")]
        public int Type { get; set; }

        public IEnumerable<FoodDrinkModel> List { get; set; }

    }
}
