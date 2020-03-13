using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PTMGroup.Admin.Models.ViewModels
{
    public class Model_SliderList
    {
        public int ID { get; set; }

        [Display(Name = "کاربر")]
        public string Login { get; set; }

        [Display(Name = "عنوان")]
        public string Tital { get; set; }

        [Display(Name = "موضوع")]
        public string Subject { get; set; }

        [Display(Name = "متن")]
        public string Description { get; set; }

        [Display(Name = "ادرس")]
        public string Link { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }

        [Display(Name = "تصویر")]
        public string Document { get; set; }
    }
}