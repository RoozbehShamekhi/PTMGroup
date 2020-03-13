using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PTMGroup.Admin.Models.ViewModels
{
    public class Model_PartnerAdd
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        [StringLength(200, ErrorMessage = "مقدار وارد شده بیش 200 کارکتراست")]
        public string Tital { get; set; }
        [Display(Name = "ادرس")]
        public string Link { get; set; }

        [Display(Name = " ")]
        public bool IsActive { get; set; }

        [Display(Name = "تصویر")]
        [Required(ErrorMessage = "لطفا مقدار را وارد نمایید")]
        public HttpPostedFileBase Document { get; set; }
    }
}