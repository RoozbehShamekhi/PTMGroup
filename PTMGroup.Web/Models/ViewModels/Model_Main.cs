using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTMGroup.Web.Models.ViewModels
{
    public class Model_Main
    {
        public IEnumerable<Model_Slider> Sliders { get; set; }

        public IEnumerable<Model_Video> Videos { get; set; }

        public IEnumerable<Model_Partner> Partners { get; set; }

        public IEnumerable<Model_Customer> Customers { get; set; }
    }
}