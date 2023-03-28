using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.Models
{
    public class PrintByRoomModel
    {

        [Required(ErrorMessage = "شماره اتاق را وارید کنید!")]
        [Display(Name = "شماره اتاق")]
        public string Room { get; set; }
    }
}
