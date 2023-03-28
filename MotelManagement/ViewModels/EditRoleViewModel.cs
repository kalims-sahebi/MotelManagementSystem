using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        [Display(Name ="آی دی ")]
        public string Id { get; set; }
        [Required(ErrorMessage ="نام رول ضروری است")]
        [Display(Name ="نام رول")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}
