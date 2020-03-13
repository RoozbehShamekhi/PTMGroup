using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using PTMGroup.DataLayer.Domain;

namespace PTMGroup.Services.BaseRepository
{
    public class Rep_SelectItems
    {
        private readonly PTMEntities db = new PTMEntities();

        //public IEnumerable<SelectListItem> Get_AllUnivercity()
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();


        //    foreach (var item in db.Tbl_University)
        //    {
        //        list.Add(new SelectListItem() { Value = item.University_ID.ToString(), Text = item.University_Display });
        //    }

        //    return list.AsEnumerable();
        //}

    }
}
