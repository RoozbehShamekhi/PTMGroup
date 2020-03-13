using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTMGroup.Admin.Models.ViewModels
{
    public class Model_Message
    {
        public int ID { get; set; }
        public string Guid { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}