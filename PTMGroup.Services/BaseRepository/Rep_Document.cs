using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTMGroup.DataLayer.Domain;

namespace PTMGroup.Services.BaseRepository
{
    public static class Rep_Document
    {
        private static readonly PTMEntities db = new PTMEntities();
        public static string Get_LinkByID(int id , string ServerPath)
        {
            var q = db.Tbl_Document.Where(a => a.Document_ID == id).SingleOrDefault();

            string path = Path.Combine(ServerPath, string.Format("{0},{1}", q.Document_FolderName, q.Document_Path));


            return path;
        }
    }
}
