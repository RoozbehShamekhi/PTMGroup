using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PTMGroup.DataLayer.Domain;
using PTMGroup.DataLayer.ViewModels;
using PTMGroup.Admin.Models.ViewModels;
using PTMGroup.Services.BaseRepository;
using PTMGroup.Common.Plugins;
using System.Text;
using System.Security.Cryptography;
using System.Data.Entity;
using System.Net;
using System.IO;

namespace PTMGroup.Admin.Controllers
{
    public class SettingController : Controller
    {
        readonly PTMEntities db = new PTMEntities();

        public object ServerPath { get; private set; }

        // GET: Setting
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        #region Main

        #region Slider

        [HttpGet]
        public ActionResult Slider_List()
        {
            List<Model_SliderList> models = new List<Model_SliderList>();


            foreach (var item in db.Tbl_Slider.Where(x => x.Slider_IsDelete == false).ToList())
            {
                Model_SliderList model = new Model_SliderList();

                model.ID = item.Slider_ID;
                model.Tital = item.Slider_Tital;
                model.Login = item.Tbl_Login.Tbl_User.User_Name + " " + item.Tbl_Login.Tbl_User.User_Family;
                model.Description = item.Slider_Description;
                model.Subject = item.Slider_Subject;
                model.Link = item.Slider_Link;
                model.Document = item.Tbl_Document.Document_Guid.ToString();
                model.IsActive = item.Slider_IsActive;

                models.Add(model);
            }




            return View(models);

        }

        [HttpGet]
        public ActionResult Slider_Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Slider_Add(Model_SliderAdd model)
        {
            if (model.Document != null && model.Document.ContentLength > 0)
            {
                Tbl_Document _Document = new Tbl_Document();
                Tbl_Slider _Slider = new Tbl_Slider();
                Tbl_Login _Login = db.Tbl_Login.Where(a => a.Login_Email == User.Identity.Name || a.Login_Mobile == User.Identity.Name).FirstOrDefault();

                _Slider.Slider_Tital = model.Tital;
                _Slider.Slider_Guid = Guid.NewGuid();
                _Slider.Slider_Subject = model.Subject;
                _Slider.Slider_Link = model.Link;
                _Slider.Slider_Description = model.Description;
                _Slider.Slider_IsActive = model.IsActive;
                _Slider.Slider_CreateDate = DateTime.Now;
                _Slider.Tbl_Login = _Login;

                _Document.Document_FileName = model.Document.FileName;
                _Document.Document_FolderName = "Slider";
                _Document.Document_TypeCodeID = 5;
                _Document.Tbl_Login = _Login;
                _Document.Document_CreateDate = DateTime.Now;
                _Document.Document_Guid = Guid.NewGuid();
                _Document.Document_Path = Guid.NewGuid().ToString();

                model.Document.SaveAs(Path.Combine(Server.MapPath("~/App_Data/Slider/"), _Document.Document_Path));

                db.Tbl_Document.Add(_Document);

                _Slider.Tbl_Document = _Document;

                db.Tbl_Slider.Add(_Slider);

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["TosterState"] = "success";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                    return RedirectToAction("Slider_List");
                }
                else
                {
                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                    return RedirectToAction("Slider_List");
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult Slider_Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Slider_Edit(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Slider_Delete(int? id)
        {

            if (id.HasValue)
            {
                Model_Message model = new Model_Message();

                var _Login = db.Tbl_Slider.Where(x => x.Slider_ID == id).SingleOrDefault();

                if (_Login != null)
                {
                    model.ID = id.Value;
                    
                    model.Description = "آیا از حذف اسلایدر مورد نظر اطمینان دارید ؟";

                    return PartialView(model);
                }

                return HttpNotFound();
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult Slider_delete(Model_Message model)
        {
            Tbl_Slider _Slider = db.Tbl_Slider.Where(a => a.Slider_ID == model.ID).SingleOrDefault();

            FileManagement fileManagement = new FileManagement();

            string path = Path.Combine(Server.MapPath("~/App_Data/"), _Slider.Tbl_Document.Document_FolderName + "\\" + _Slider.Tbl_Document.Document_Path);

            fileManagement.DeleteFileWithPath(path);

            db.Tbl_Document.Remove(_Slider.Tbl_Document);
            db.Tbl_Slider.Remove(_Slider);

            if (Convert.ToBoolean(db.SaveChanges() > 0))
            {
                TempData["TosterState"] = "success";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                return RedirectToAction("Slider_List");
            }
            else
            {
                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                return RedirectToAction("Slider_List");
            }

        }
        #endregion

        #region Partner
        [HttpGet]
        public ActionResult Partner_List()
        {
            var model = db.Tbl_Partner.Select(x => new Model_PartnerList
            {
                ID = x.Partner_ID,
                Tital = x.Partner_Titel,
                Login = x.Tbl_Login.Tbl_User.User_Name + " " + x.Tbl_Login.Tbl_User.User_Family,
                Document = x.Tbl_Document.Document_Guid.ToString(),
                IsActive = x.Partner_IsActive,
                Link = x.Partner_Link

            }).ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult Partner_Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Partner_Add(Model_PartnerAdd model)
        {
            if (model.Document != null && model.Document.ContentLength > 0)
            {
                Tbl_Document _Document = new Tbl_Document();
                Tbl_Partner _Partner = new Tbl_Partner();
                Tbl_Login _Login = db.Tbl_Login.Where(a => a.Login_Email == User.Identity.Name || a.Login_Mobile == User.Identity.Name).FirstOrDefault();

                _Partner.Partner_Titel = model.Tital;
                _Partner.Partner_Guid = Guid.NewGuid();
                _Partner.Partner_Link = model.Link;
                _Partner.Partner_IsActive = model.IsActive;
                _Partner.Partner_CreateDate = DateTime.Now;
                _Partner.Tbl_Login = _Login;

                _Document.Document_FileName = model.Document.FileName;
                _Document.Document_FolderName = "Partner";
                _Document.Document_TypeCodeID = 5;
                _Document.Tbl_Login = _Login;
                _Document.Document_CreateDate = DateTime.Now;
                _Document.Document_Guid = Guid.NewGuid();
                _Document.Document_Path = Guid.NewGuid().ToString();

                model.Document.SaveAs(Path.Combine(Server.MapPath("~/App_Data/Partner/"), _Document.Document_Path));

                db.Tbl_Document.Add(_Document);

                _Partner.Tbl_Document = _Document;

                db.Tbl_Partner.Add(_Partner);

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["TosterState"] = "success";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                    return RedirectToAction("Partner_List");
                }
                else
                {
                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                    return RedirectToAction("Partner_List");
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult Partner_Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Partner_Edit(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Partner_Delete(int? id)
        {
            if (id.HasValue)
            {
                Model_Message model = new Model_Message();

                var _Login = db.Tbl_Partner.Where(x => x.Partner_ID == id).SingleOrDefault();

                if (_Login != null)
                {
                    model.ID = id.Value;

                    model.Description = "آیا از حذف شریک مورد نظر اطمینان دارید ؟";

                    return PartialView(model);
                }

                return HttpNotFound();
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult Partner_delete(Model_Message model)
        {
            Tbl_Partner _Partner = db.Tbl_Partner.Where(a => a.Partner_ID == model.ID).SingleOrDefault();

            FileManagement fileManagement = new FileManagement();

            string path = Path.Combine(Server.MapPath("~/App_Data/"), _Partner.Tbl_Document.Document_FolderName + "\\" + _Partner.Tbl_Document.Document_Path);

            fileManagement.DeleteFileWithPath(path);

            db.Tbl_Document.Remove(_Partner.Tbl_Document);
            db.Tbl_Partner.Remove(_Partner);

            if (Convert.ToBoolean(db.SaveChanges() > 0))
            {
                TempData["TosterState"] = "success";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                return RedirectToAction("Partner_List");
            }
            else
            {
                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                return RedirectToAction("Partner_List");
            }
        }
        #endregion

        #region Client
        [HttpGet]
        public ActionResult Client_List()
        {
            var model = db.Tbl_Client.Select(x => new Model_ClientList
            {
                ID = x.Client_ID,
                Tital = x.Client_Titel,
                Login = x.Tbl_Login.Tbl_User.User_Name + " " + x.Tbl_Login.Tbl_User.User_Family,
                Document = x.Tbl_Document.Document_Guid.ToString(),
                IsActive = x.Client_IsActive,
                Link = x.Client_Link

            }).ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult Client_Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Client_Add(Model_ClientAdd model)
        {
            if (model.Document != null && model.Document.ContentLength > 0)
            {
                Tbl_Document _Document = new Tbl_Document();
                Tbl_Client _Client = new Tbl_Client();
                Tbl_Login _Login = db.Tbl_Login.Where(a => a.Login_Email == User.Identity.Name || a.Login_Mobile == User.Identity.Name).FirstOrDefault();

                _Client.Client_Titel = model.Tital;
                _Client.Client_Guid = Guid.NewGuid();
                _Client.Client_Link = model.Link;
                _Client.Client_IsActive = model.IsActive;
                _Client.Client_CreateDate = DateTime.Now;
                _Client.Tbl_Login = _Login;

                _Document.Document_FileName = model.Document.FileName;
                _Document.Document_FolderName = "Client";
                _Document.Document_TypeCodeID = 5;
                _Document.Tbl_Login = _Login;
                _Document.Document_CreateDate = DateTime.Now;
                _Document.Document_Guid = Guid.NewGuid();
                _Document.Document_Path = Guid.NewGuid().ToString();

                model.Document.SaveAs(Path.Combine(Server.MapPath("~/App_Data/Client/"), _Document.Document_Path));

                db.Tbl_Document.Add(_Document);

                _Client.Tbl_Document = _Document;

                db.Tbl_Client.Add(_Client);

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["TosterState"] = "success";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                    return RedirectToAction("Client_List");
                }
                else
                {
                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                    return RedirectToAction("Client_List");
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult Client_Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Client_Edit(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Client_Delete(int? id)
        {
            if (id.HasValue)
            {
                Model_Message model = new Model_Message();

                var _Login = db.Tbl_Client.Where(x => x.Client_ID == id).SingleOrDefault();

                if (_Login != null)
                {
                    model.ID = id.Value;

                    model.Description = "آیا از حذف مشتری مورد نظر اطمینان دارید ؟";

                    return PartialView(model);
                }

                return HttpNotFound();
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult Client_Delete(Model_Message model)
        {
            Tbl_Client _Client = db.Tbl_Client.Where(a => a.Client_ID == model.ID).SingleOrDefault();

            FileManagement fileManagement = new FileManagement();

            string path = Path.Combine(Server.MapPath("~/App_Data/"), _Client.Tbl_Document.Document_FolderName + "\\" + _Client.Tbl_Document.Document_Path);

            fileManagement.DeleteFileWithPath(path);

            db.Tbl_Document.Remove(_Client.Tbl_Document);
            db.Tbl_Client.Remove(_Client);

            if (Convert.ToBoolean(db.SaveChanges() > 0))
            {
                TempData["TosterState"] = "success";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                return RedirectToAction("Client_List");
            }
            else
            {
                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                return RedirectToAction("Client_List");
            }
        }
        #endregion

        #region Video
        [HttpGet]
        public ActionResult Video_List()
        {
            var model = db.Tbl_Video.Select(x => new Model_VideoList
            {
                ID = x.Video_ID,
                Tital = x.Video_Titel,
                Login = x.Tbl_Login.Tbl_User.User_Name + " " + x.Tbl_Login.Tbl_User.User_Family,
                Document = x.Tbl_Document.Document_Guid.ToString(),
                IsActive = x.Video_IsActive

            }).ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult Video_Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Video_Add(Model_VideoAdd model)
        {
            if (model.Document != null && model.Document.ContentLength > 0)
            {
                Tbl_Document _Document = new Tbl_Document();
                Tbl_Video _Video = new Tbl_Video();
                Tbl_Login _Login = db.Tbl_Login.Where(a => a.Login_Email == User.Identity.Name || a.Login_Mobile == User.Identity.Name).FirstOrDefault();

                _Video.Video_Titel = model.Tital;
                _Video.Video_Guid = Guid.NewGuid();
                _Video.Video_IsActive = model.IsActive;
                _Video.Video_CreateDate = DateTime.Now;
                _Video.Tbl_Login = _Login;

                _Document.Document_FileName = model.Document.FileName;
                _Document.Document_FolderName = "Video";
                _Document.Document_TypeCodeID = 7;
                _Document.Tbl_Login = _Login;
                _Document.Document_CreateDate = DateTime.Now;
                _Document.Document_Guid = Guid.NewGuid();
                _Document.Document_Path = Guid.NewGuid().ToString();

                model.Document.SaveAs(Path.Combine(Server.MapPath("~/App_Data/Video/"), _Document.Document_Path));

                db.Tbl_Document.Add(_Document);

                _Video.Tbl_Document = _Document;

                db.Tbl_Video.Add(_Video);

                if (Convert.ToBoolean(db.SaveChanges() > 0))
                {
                    TempData["TosterState"] = "success";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                    return RedirectToAction("Video_List");
                }
                else
                {
                    TempData["TosterState"] = "error";
                    TempData["TosterType"] = TosterType.Maseage;
                    TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                    return RedirectToAction("Video_List");
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult Video_Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Video_Edit(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Video_Delete(int? id)
        {
            if (id.HasValue)
            {
                Model_Message model = new Model_Message();

                var _Login = db.Tbl_Video.Where(x => x.Video_ID == id).SingleOrDefault();

                if (_Login != null)
                {
                    model.ID = id.Value;

                    model.Description = "آیا از حذف ویدیو مورد نظر اطمینان دارید ؟";

                    return PartialView(model);
                }

                return HttpNotFound();
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult Video_Delete(Model_Message model)
        {
            Tbl_Video _Video = db.Tbl_Video.Where(a => a.Video_ID == model.ID).SingleOrDefault();

            FileManagement fileManagement = new FileManagement();

            string path = Path.Combine(Server.MapPath("~/App_Data/"), _Video.Tbl_Document.Document_FolderName + "\\" + _Video.Tbl_Document.Document_Path);

            fileManagement.DeleteFileWithPath(path);

            db.Tbl_Document.Remove(_Video.Tbl_Document);
            db.Tbl_Video.Remove(_Video);

            if (Convert.ToBoolean(db.SaveChanges() > 0))
            {
                TempData["TosterState"] = "success";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "عملیات با موفقیت انجام شد";

                return RedirectToAction("Video_List");
            }
            else
            {
                TempData["TosterState"] = "error";
                TempData["TosterType"] = TosterType.Maseage;
                TempData["TosterMassage"] = "عملیات با موفقیت انجام نشد";

                return RedirectToAction("Video_List");
            }
        }
        #endregion

        #endregion

        #region UserManagment

        #region Role
        [HttpGet]
        public ActionResult Role_List()
        {
            var model = db.Tbl_BaseRole.Select(x => new Model_RoleList
            {
                ID = x.BR_ID,
                Display = x.BR_Display,
                Name = x.BR_Name,

            }).ToList();

            return View(model);
        }
        #endregion

        #region User
        [HttpGet]
        public ActionResult User_List()
        {
            var model = db.Tbl_Login.Where(x => x.Login_IsDelete == false && x.Login_BaseRoleID != 2).Select(x => new Model_UserList
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
        #endregion

        #endregion
    }
}