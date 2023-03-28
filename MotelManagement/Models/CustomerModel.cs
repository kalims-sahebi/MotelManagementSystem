using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.Models
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }
        [Required(ErrorMessage ="نام کامل ضروریست")]
        [Display(Name = "نام کامل")]
        public string FullName { get; set; }
        [Display(Name = "آدرس")]
        public string Address { get; set; }
        [Required(ErrorMessage = " شماره تماس ضروریست")]
        [Display(Name = "شماره تماس")]
        public string Phone { get; set; }
        [Required(ErrorMessage = " شماره اطاق ضروریست")]
        [Display(Name = "شماره اتاق")]
        public int RoomId { get; set; }
        [Required(ErrorMessage = " تاریخ ورود ضروریست")]
        [Display(Name = "تاریخ ورود")]
        public string EntryDateString { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public string user { get; set; }
        public short  IsOut { get; set; }

       
        public IEnumerable<SelectListItem> RoomList { get; set; }
        public string RoomNumber { get; set; }

    }
}
