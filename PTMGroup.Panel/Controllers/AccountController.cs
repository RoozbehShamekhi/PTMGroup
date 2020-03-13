using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Text;
using System.Security.Cryptography;
using PTMGroup.DataLayer.Domain;
using PTMGroup.DataLayer.ViewModels;
using PTMGroup.Panel.Models.ViewModels;
using PTMGroup.Common.Plugins;
using PTMGroup.Services.BaseRepository;


namespace PTMGroup.Panel.Controllers
{
    public class AccountController : Controller
    {
        PTMEntities db = new PTMEntities();

        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "Dashboard");

            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(Model_Login model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "Dashboard");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.State = "Error";

                return View("Login", model);

            }


            model.Password = model.Password.PersianToEnglish();
            model.Username = model.Username.PersianToEnglish();

            var q = db.Tbl_Login.Where(a => a.Login_Email == model.Username || a.Login_Mobile == model.Username).SingleOrDefault();

            if (q == null)
            {
                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "کاربر یافت نشد !";

                return View();
            }


            var SaltPassword = model.Password + q.Login_PasswordSalt;
            var SaltPasswordBytes = Encoding.UTF8.GetBytes(SaltPassword);
            var SaltPasswordHush = Convert.ToBase64String(SHA512.Create().ComputeHash(SaltPasswordBytes));


            if (q.Login_PasswordHash == SaltPasswordHush)
            {
                string s = string.Empty;

                s = Rep_UserRole.Get_RoleNameWithID(q.Login_BaseRoleID);

                var Ticket = new FormsAuthenticationTicket(0, q.Login_Guid.ToString(), DateTime.Now, model.RemenberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddDays(1), true, s);
                var EncryptedTicket = FormsAuthentication.Encrypt(Ticket);
                var Cookie = new HttpCookie(FormsAuthentication.FormsCookieName, EncryptedTicket)
                {
                    Expires = Ticket.Expiration
                };
                Response.Cookies.Add(Cookie);

                TempData["TosterState"] = "success";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "خوش آمدید";

                return RedirectToAction("index", "Dashboard");
            }
            else
            {
                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "پسورد نادرست است !";

                return View();
            }

        }



        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            var Cookie = new HttpCookie(FormsAuthentication.FormsCookieName)
            {
                Expires = DateTime.Now.AddDays(-1)
            };

            Response.Cookies.Add(Cookie);
            Session.RemoveAll();

            return RedirectToAction("Login", "Account", new { area = "Dashboard" });
        }

        [HttpPost]
        public ActionResult _Register(Model_Register model)
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

                return RedirectToAction("Login");
            }
            else
            {
                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "ثبت نام با موفقیت انجام نشد";

                return View();
            }
        }
    }
}