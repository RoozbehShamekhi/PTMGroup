using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PTMGroup.Admin.Models.ViewModels
{
    public class Model_RoleList
    {
        public int ID { get; set; }

        [Display(Name = "نام")]
        public string Name { get; set; }

        [Display(Name = "نمایش")]
        public string Display { get; set; }
    }
}