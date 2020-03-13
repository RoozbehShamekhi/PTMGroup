using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PTMGroup.Admin.Models.ViewModels
{
    public class Model_Login
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Username { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = " ")]
        public bool RemenberMe { get; set; }
    }
}