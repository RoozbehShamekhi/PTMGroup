using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTMGroup.DataLayer.Domain;

namespace PTMGroup.Services.BaseRepository
{
    public static class Rep_UserRole
    {
        private static readonly PTMEntities db = new PTMEntities();
        public static string Get_RoleNameWithID(int id)
        {
            return db.Tbl_BaseRole.Where(a => a.BR_ID == id).SingleOrDefault().BR_Name;
        }
    }
}
