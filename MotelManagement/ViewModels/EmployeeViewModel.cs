using Microsoft.AspNetCore.Mvc.Rendering;
using MotelManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.ViewModels
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "نام کامل الزامی است")]
        [Display(Name = "نام کامل")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "شماره مبابل الزامی است")]
        [Display(Name = "شماره مبابل")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "لطفا نوعیت قرار داد را انتخاب کنید")]
        [Display(Name = "نوعیت قرار داد")]
        public int EmployeeTypeId { get; set; }
        public int EmployeeTypeString { get; set; }
        [Required(ErrorMessage = "لطفا وظیفه را انتخاب کنید")]
        [Display(Name = "وظیفه")]
        public int DutyId { get; set; }
        public int DutyString { get; set; }
        [Required(ErrorMessage = "آدرس کامل الزامی است")]
        [Display(Name = "آدرس کامل")]
        public string Address { get; set; }
        [Required(ErrorMessage = "لطفا حساب فعلی را درج کنید")]
        [Display(Name = "حساب فعلی")]
        public int Balance { get; set; }
        [Required(ErrorMessage = "معاش به افغانی را درج کنید")]
        [Display(Name = "معاش")]
        public int MonthlySalary { get; set; }
        [Display(Name = "امتیازات دیگر")]
        public string OtherBenefit { get; set; }
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "تاریخ قرار داد را درج کنید")]
        [Display(Name = "تاریخ")]
        public string DateString { get; set; }
        public IEnumerable<SelectListItem> Duty { get; set; }
        public IEnumerable<SelectListItem> EmployeeType { get; set; }


    }
}
