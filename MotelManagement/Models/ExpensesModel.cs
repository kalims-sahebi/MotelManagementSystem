using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.Models
{
    public class ExpensesModel
    {
        public int ExpensesId { get; set; }
        [Display(Name = "تفصیلات")]
        public string Description { get; set; }
        [Required(ErrorMessage ="مقدار الزامی است")]
        [Display(Name ="مقدار")]
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "تاریخ الزامی است")]
        [Display(Name = "تاریخ")]
        public string DateString { get; set; }
        public string user { get; set; }
    }
}
