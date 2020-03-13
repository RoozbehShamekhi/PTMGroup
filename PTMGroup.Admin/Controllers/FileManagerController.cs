using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PTMGroup.DataLayer.Domain;

namespace PTMGroup.Admin.Controllers
{
    public class FileManagerController : Controller
    {

        readonly PTMEntities db = new PTMEntities();
        // GET: FileManager
        [HttpGet]
        public FileResult Download(string Key)
        {
            Tbl_Document q = db.Tbl_Document.Where(a => a.Document_Guid.ToString() == Key).SingleOrDefault();

            if (q != null)
            {
                string path = Path.Combine(Server.MapPath("~/App_Data/"), q.Document_FolderName +"\\"+q.Document_Path);

                return File(path, "*", q.Document_FileName);
            }
            else
            {
                return null;
            }

        }

    }
}