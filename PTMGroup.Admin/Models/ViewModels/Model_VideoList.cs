using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PTMGroup.Admin.Models.ViewModels
{
    public class Model_VideoList
    {
        public int ID { get; set; }

        [Display(Name = "کاربر")]
        public string Login { get; set; }

        [Display(Name = "عنوان")]
        public string Tital { get; set; }

        [Display(Name = "فایل")]
        public string Document { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }
    }
}