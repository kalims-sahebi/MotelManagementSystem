using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="لطفا نام یوزر را وارد کنید")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "لطفا رمز را وارد کنید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "به خاطر بسپارید؟")]
        public bool RememberMe { get; set; }
    }
}
