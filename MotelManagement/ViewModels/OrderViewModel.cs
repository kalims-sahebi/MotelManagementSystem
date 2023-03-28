using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.ViewModels
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "تعداد نوشیدنی الزامیست")]
        [Display(Name = "تعداد نوشیدنی")]
        public int DrinkCount { get; set; }
        [Required(ErrorMessage = "تعداد غذا الزامیست")]
        [Display(Name = "تعداد غذا")]
        public int FoodCount { get; set; }

        [Display(Name = "غذا")]
        public int? FoodId { get; set; }
        [Display(Name = "نوشیدنی")]
        public int? DrinkId { get; set; }

        public IEnumerable<SelectListItem> DrinkList { get; set; }
        public IEnumerable<SelectListItem> FoodListList { get; set; }
    }
}
