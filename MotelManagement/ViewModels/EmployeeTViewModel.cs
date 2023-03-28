using Microsoft.AspNetCore.Mvc.Rendering;
using MotelManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.ViewModels
{
    public class EmployeeTViewModel
    {
        [Display(Name ="نام کارمند")]
        public IEnumerable<SelectListItem> Employee { get; set; }
        [Display(Name = "بردگی/رسیدگی")]
        public short inOut { get; set; }
        [Required(ErrorMessage = " مبلغ به افغانی درج شود")]
        [Display(Name = "مبلغ به افغانی")]
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "تاریخ درج شود")]
        [Display(Name = "تاریخ")]
        public string DateString { get; set; }
        public string FullName { get; set; }
        
        [Display(Name = "توضیح")]
        public string Description { get; set; }
        public int EmployeeId { get; set; }
        public int EmployeeTransactionId { get; set; }
        public int inmoney { get; set; }
        public int Balance { get; set; }
        public int outmoney { get; set; }
        public string user { get; set; }
        //TransactionList
        public IEnumerable<EmployeeTViewModel> TList { get; set; }
    }
}
