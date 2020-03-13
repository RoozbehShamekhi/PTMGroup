using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PTMGroup.DataLayer.Domain;
using PTMGroup.DataLayer.ViewModels;
using PTMGroup.Admin.Models.ViewModels;
using PTMGroup.Services.BaseRepository;
using System.Text;
using System.Security.Cryptography;
using System.Data.Entity;
using System.Net;

namespace PTMGroup.Admin.Controllers
{
    public class UserController : Controller
    {
        readonly PTMEntities db = new PTMEntities();
        // GET: User
        [HttpGet]
        public ActionResult Index()
        {
            var model = db.Tbl_Login.Where(x => x.Login_IsDelete == false && x.Login_BaseRoleID == 2 ).Select(x => new Model_UserList
            {
                ID = x.Login_ID,
                Family = x.Tbl_User.User_Family,
                Name = x.Tbl_User.User_Name,
                Role = x.Tbl_BaseRole.BR_Display,
                Email = x.Login_Email,
                Mobile = x.Login_Mobile

            }).ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult User_Profile(int? id)
        {
            var model = db.Tbl_Login.Where(x => x.Login_ID == id).Select(x => new Model_User
            {
                ID = x.Login_ID,
                Family = x.Tbl_User.User_Family,
                Name = x.Tbl_User.User_Name,
                Role = x.Tbl_BaseRole.BR_Display,
                Email = x.Login_Email,
                Mobile = x.Login_Mobile,
                Gender = x.Tbl_User.Tbl_Code.Code_Guid.ToString()

            }).SingleOrDefault();


            return View(model);

        }

        [HttpGet]
        public ActionResult User_Edit(int id)
        {
            var model = db.Tbl_Login.Where(x => x.Login_ID == id).Select(x => new Model_UserEdit
            {
                ID = x.Login_ID,
                Family = x.Tbl_User.User_Family,
                Name = x.Tbl_User.User_Name,
                Role = 2,
                Email = x.Login_Email,
                Mobile = x.Login_Mobile,
                Gender = x.Tbl_User.Tbl_Code.Code_Guid.ToString()

            }).SingleOrDefault();

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult User_Edit(Model_UserEdit model)
        {
            Tbl_Login _Login = db.Tbl_Login.Where(a => a.Login_ID == model.ID).SingleOrDefault();

            Tbl_User _User = _Login.Tbl_User;


            _Login.Login_Email = model.Email;
            _Login.Login_Mobile = model.Mobile;
            _User.User_GenderCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(Guid.Parse(model.Gender));
            _User.User_Name = model.Name;
            _User.User_Family = model.Family;

            db.Entry(_Login).State = EntityState.Modified;
            db.Entry(_User).State = EntityState.Modified;

            if (Convert.ToBoolean(db.SaveChanges() > 0))
            {
                TempData["TosterState"] = "success";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                return RedirectToAction("index");
            }
            else
            {
                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                return RedirectToAction("index");
            }
        }

        [HttpGet]
        public ActionResult User_Add() 
        {
            Model_UserAdd model = new Model_UserAdd();
            model.Role = 2;


            return PartialView(model);

        }

        [HttpPost]
        public ActionResult User_Add(Model_UserAdd model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }

            if (!ModelState.IsValid)
            {
                return View("Register", model);
            }


            Tbl_User _User = new Tbl_User();
            Tbl_Login _Login = new Tbl_Login();

            _Login.Login_Email = model.Email;
            _Login.Login_CreateDate = DateTime.Now;
            _Login.Login_ModifyDate = DateTime.Now;
            _Login.Login_Guid = Guid.NewGuid();
            _Login.Login_Mobile = model.Mobile;
            _Login.Login_BaseRoleID = 2;


            _User.User_Guid = Guid.NewGuid();
            _User.User_Name = model.Name;
            _User.User_Family = model.Family;
            _User.User_GenderCodeID = Rep_CodeGroup.Get_CodeIDWithGUID(Guid.Parse(model.Gender));

            _User.User_Birtday = DateTime.Now;

            var Salt = Guid.NewGuid().ToString("N");
            var SaltPassword = model.Password + Salt;
            var SaltPasswordBytes = Encoding.UTF8.GetBytes(SaltPassword);
            var SaltPasswordHush = Convert.ToBase64String(SHA512.Create().ComputeHash(SaltPasswordBytes));

            _Login.Login_PasswordHash = SaltPasswordHush;
            _Login.Login_PasswordSalt = Salt;

            db.Tbl_User.Add(_User);

            _Login.Tbl_User = _User;

            db.Tbl_Login.Add(_Login);



            if (Convert.ToBoolean(db.SaveChanges() > 0))
            {
                //if (new SMSPortal().SendServiceable(model.Mobile, model.Mobile, model.Password, "", model.Name + " " + model.Family, SMSTemplate.Register) != "ارسال به مخابرات")
                //{
                //    TempData["TosterState"] = "warning";
                //    TempData["TosterType"] = TosterType.Maseage;
                //    TempData["TosterMassage"] = "خطا در ارسال پیامک";

                //    return RedirectToAction("Login");
                //};

                TempData["TosterState"] = "success";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "ثبت نام با موفقیت انجام شد";

                return RedirectToAction("index");
            }
            else
            {
                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "ثبت نام با موفقیت انجام نشد";

                return RedirectToAction("index");
            }

        }

        [HttpGet]
        public ActionResult User_Delete(int? id)
        {
            if (id.HasValue)
            {
                Model_Message model = new Model_Message();

                var _Login = db.Tbl_Login.Where(x => x.Login_ID == id).SingleOrDefault();

                if (_Login != null)
                {
                    model.ID = id.Value;
                    model.Name = _Login.Tbl_User.User_Name + " " + _Login.Tbl_User.User_Family;
                    model.Description = "آیا از حذف کاربر مورد نظر اطمینان دارید ؟";

                    return PartialView(model);
                }

                return HttpNotFound();
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

        [HttpPost]
        public ActionResult User_Delete(Model_Message model)
        {
            if (ModelState.IsValid)
            {
                var _Login = db.Tbl_Login.Where(x => x.Login_ID == model.ID && !x.Login_IsDelete).SingleOrDefault();

                if (_Login != null)
                {
                    _Login.Login_IsDelete = true;
                    

                    db.Entry(_Login).State = EntityState.Modified;

                    if (Convert.ToBoolean(db.SaveChanges() > 0))
                    {
                        TempData["TosterState"] = "success";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "کاربر مورد نظر با موفقیت حذف شد.";
                    }
                    else
                    {
                        TempData["TosterState"] = "error";
                        TempData["TosterType"] = TosterType.Maseage;
                        TempData["TosterMassage"] = "کاربر مورد نظر با موفقیت حذف نشد.";
                    }

                    return RedirectToAction("index");
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        }

    }
}