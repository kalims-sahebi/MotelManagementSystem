using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.Models
{
    public class RoomModel
    {
        public int RoomId { get; set; }
        [Required(ErrorMessage ="شماره اطاق ضروریست")]
        [Display(Name = "شماره اتاق")]
        public string RoomNumber { get; set; }
        [Required(ErrorMessage = "وضعیت اطاق ضروریست")]
        [Display(Name = "وضعیت اتاق")]
        public int IsEmpty { get; set; }
        [Required(ErrorMessage = "موقعیت اطاق ضروریست")]
        [Display(Name = "موقعیت اتاق")]
        public string RoomLocation { get; set; }
        [Required(ErrorMessage = " تعداد تخت ضروریست")]
        [Display(Name = "تعداد تخت")]
        public int BedCount { get; set; }
        [Required(ErrorMessage = "کرایه برای یک شب ضروریست")]
        [Display(Name = "مبلغ کرایه")]
        public int RentPerNight { get; set; }
        [Required(ErrorMessage = "نوعیت اتاق ضروریست")]
        [Display(Name = "نوعیت اتاق")]
        public int RoomTypeId { get; set; }
        // View Related Fields
        public string RoomType { get; set; }
        public IEnumerable<SelectListItem> RoomTypeList { get; set; }
    }
}
