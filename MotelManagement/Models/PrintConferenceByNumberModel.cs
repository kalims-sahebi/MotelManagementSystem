using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.Models
{
    public class PrintConferenceByNumberModel
    {
        [Required(ErrorMessage = "شماره را وارید کنید!")]
        [Display(Name = "شماره تماس")]
        public string Contact { get; set; }
    }
}
