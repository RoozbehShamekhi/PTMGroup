using PTMGroup.DataLayer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTMGroup.Admin.Controllers
{
    public class ValidatorController : Controller
    {
        readonly PTMEntities db = new PTMEntities();
        // GET: Validator
        [HttpPost]
        public JsonResult EmailValid(string Email)
        {
            try
            {

                var q = db.Tbl_Login.Where(a => a.Login_Email == Email && a.Login_IsDelete != true).SingleOrDefault();

                if (q == null)
                {
                    return Json(true, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.DenyGet);
                }

            }
            catch
            {

                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult MobileValid(string Mobile)
        {
            try
            {

                var q = db.Tbl_Login.Where(a => a.Login_Mobile == Mobile && a.Login_IsDelete != true).SingleOrDefault();

                if (q == null)
                {
                    return Json(true, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.DenyGet);
                }

            }
            catch
            {

                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }
    }
}