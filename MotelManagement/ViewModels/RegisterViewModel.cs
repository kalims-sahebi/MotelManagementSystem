﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.ViewModels
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "لطفا نام کامل را وارد نماید")]
        [Display(Name = "نام")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "لطفا نام یوزر  را وارد نماید")]
        [Display(Name = "نام یوزر")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "لطفا رول  را انتخاب نماید")]
        [Display(Name = "رول")]
        public int Role { get; set; }

        [Required(ErrorMessage = "لطفا ایمیل را وارد نماید")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا رمز را وارد نماید")]
        [StringLength(50, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} کاراکتر باشد.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "رمز")]
        public string Password { get; set; }
        [Required(ErrorMessage = "لطفا تکرار رمز را وارد نماید")]
        [DataType(DataType.Password)]
        [Display(Name = "تکرار رمز")]
        [Compare("Password",ErrorMessage ="تکرار رمز اشتباه است")]
        public string ConfirmPassword { get; set; }
    }
}
