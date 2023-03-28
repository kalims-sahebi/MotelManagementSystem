using System;
using System.ComponentModel.DataAnnotations;

namespace MotelManagement.Models
{
    public class ConferenceRoomModel
    {
        public int ConferenceRoomId { get; set; }

        [Display(Name ="مصارف قهوه")]
        [Required(ErrorMessage = "مصارف قهوه الزامی است")]
        [DataType(dataType:DataType.Currency,ErrorMessage ="واحد پول وارد شود")]
        public int RefreshmentCost { get; set; }

        [Display(Name ="کرایه ساعتوار")]
        [Required(ErrorMessage = "کرایه ساعتوار الزامی است")]
        public int RentPerHour { get; set; }

        [Display(Name = "زمان شروع")]
        [Required(ErrorMessage = " زمان شروع کنفرانس الزامی است")]
        public DateTime StartTime { get; set; }
        public TimeSpan StartTimeString { get; set; }
       
        [Display(Name = "زمان ختم")]
        [Required(ErrorMessage = " زمان ختم کنفرانس الزامی است")]
        public DateTime EndTime { get; set; }
        public TimeSpan EndTimeString { get; set; }


        [Display(Name = "تاریخ")]
        [Required(ErrorMessage =" تاریخ کنفرانس الزامی است")]
        public string DateString { get; set; }

        public DateTime Date { get; set; }

        [Display(Name = "نام سازمان")]
        public string OrganizationName { get; set; }

        [Display(Name = "آدرس سازمان")]
        public string Address { get; set; }

        [Display(Name = "شماره تماس/ایمیل")]
        public string contact { get; set; }

        public short IsDone { get; set; }
        public string User { get; set; }
    }
}
