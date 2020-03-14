using PTMGroup.DataLayer.Domain;
using PTMGroup.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTMGroup.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly PTMEntities db = new PTMEntities();

        [HttpGet]
        public ActionResult Index()
        {
            var sliders = db.Tbl_Slider.Where(x => x.Slider_IsActive && !x.Slider_IsDelete).Select(x => new Model_Slider
            {
                Slider_Tital = x.Slider_Tital,
                Slider_Subject = x.Slider_Subject,
                Slider_Description = x.Slider_Description,
                Slider_DocumentGuid = x.Tbl_Document.Document_Guid.ToString()

            }).ToList();

            var videos = db.Tbl_Video.Where(x => x.Video_IsActive).Select(x => new Model_Video
            {
                Video_Guid = x.Video_Guid,
                Video_DocumentGuid = x.Tbl_Document.Document_Guid.ToString()

            }).ToList();

            var partners = db.Tbl_Partner.Where(x => x.Partner_IsActive).Select(x => new Model_Partner
            {
                Partner_DocumentGuid = x.Tbl_Document.Document_Guid.ToString()

            }).ToList();

            var customers = db.Tbl_Client.Where(x => x.Client_IsActive).Select(x => new Model_Customer
            {
                Client_DocumentGuid = x.Tbl_Document.Document_Guid.ToString(),
                Client_Title = x.Client_Titel
            });

            Model_Main model = new Model_Main
            {
                Sliders = sliders,
                Videos = videos,
                Partners = partners,
                Customers = customers
            };

            return View(model);
        }
    }
}