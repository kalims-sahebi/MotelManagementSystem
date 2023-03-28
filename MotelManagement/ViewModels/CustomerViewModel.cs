using Microsoft.AspNetCore.Mvc.Rendering;
using MotelManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.ViewModels
{
    public class CustomerViewModel
    {
        public IEnumerable<CustomerModel> CustomerList { get; set; }

        //////////
        /////Order Attribute
        /////////
       
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
        /// <summary>
        /// ///New Food Attribute
        /// </summary>
        /// 
        [Required(ErrorMessage = "نام غذا ضروریست")]
        [Display(Name = "نام غذا")]
        public string Food { get; set; }
        [Required(ErrorMessage = "قیمت غذا ضروریست")]
        [Display(Name = "قیمت غذا")]
        public int Price { get; set; }

        /// <summary>
        /// ///New Drink Attribute
        /// </summary>
        /// 
        [Required(ErrorMessage = "نام نوشیدنی ضروریست")]
        [Display(Name = "نام نوشیدنی")]
        public string Drink { get; set; }

        [Required(ErrorMessage = "قیمت نوشیدنی ضروریست")]
        [Display(Name = "قیمت نوشیدنی")]
        public int DPrice { get; set; }

        public IEnumerable<SelectListItem> DrinkList { get; set; }
        public IEnumerable<SelectListItem> FoodList { get; set; }
    }
}
