using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTMGroup.Admin.Models.ViewModels
{
    public class Model_UserAdd
    {
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [StringLength(100, ErrorMessage = "مقدار وارد شده بیش 100 کارکتراست")]
        public string Name { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [StringLength(100, ErrorMessage = "مقدار وارد شده بیش 100 کارکتراست")]
        public string Family { get; set; }

        [Display(Name = "نقش")]
        public int Role { get; set; }

        [Display(Name = "جنسیت")]
        [Required(ErrorMessage = "لطفا مقداری را وارد نمایید")]
        public string Gender { get; set; }

        [Display(Name = "موبایل")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [MaxLength(11, ErrorMessage = "مقدار وارد شده بیش 11 کارکتراست")]
        [MinLength(11, ErrorMessage = "مقدار وارد شده کمتر 11 کارکتراست")]
        [Remote("MobileValid", "Validator", HttpMethod = "Post", ErrorMessage = "این شماره قبلا ثبت شده است")]
        public string Mobile { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [StringLength(200, ErrorMessage = "مقدار وارد شده بیش 200 کارکتراست")]
        [EmailAddress(ErrorMessage = "ایمیل را به درستی وارد نمایید")]
        [Remote("EmailValid", "Validator", HttpMethod = "Post", ErrorMessage = "این ایمیل قبلا ثبت شده است")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [StringLength(100, ErrorMessage = "مقدار وارد شده بیش 100 کارکتراست")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [StringLength(100, ErrorMessage = "مقدار وارد شده بیش 100 کارکتراست")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "پسورد ها برابر نیست")]
        public string PasswordVerify { get; set; }
    }
}