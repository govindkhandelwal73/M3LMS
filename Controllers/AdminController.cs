using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using M3LMS.DTO;
using M3LMS.Helper;
using M3LMS.ViewModels;
using M3LMS.Models;

namespace M3LMS.Controllers
{
    [HandleError]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        M3LMS.Models.DB_A14BC3_M3LMSEntities db = new M3LMS.Models.DB_A14BC3_M3LMSEntities();

        public ActionResult Index()
        {
            M3LMS.MvcApplication.LoadPageSections();
           //check for login user status if inactive then show change password and active 
            var URL = string.Empty;
           
            if(Session["UserName"]!=null && Session["UserType"].ToString()=="Admin")
            {
                 var UserName=Session["UserName"].ToString();
                  URL = db.ref_Users.Where(a => a.Email ==UserName &&  a.ActiveStatus==false).Count()>0? "ChangePassword":"Index";
                 ViewBag.Email = Session["UserName"].ToString();
                
            }

            return View(URL);
        }
        
        public ActionResult PageSettings()
        {
            return View();
        
        }
        [HttpPost]
        public ActionResult PageSettings(ref_pagesection page)
        {
            if(ModelState.IsValid)
            {

            
                db.ref_pagesection.Add(page);
                db.SaveChanges();

            }

            return View("Index");
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]

        public ActionResult ChangePassword(string Email,string ConfirmPassword)
        {
            List<ref_Users> Users = db.ref_Users.Where(a => a.Email == Email && a.ActiveStatus==false).ToList();
            foreach (ref_Users usr in Users)
            {
                usr.Password = ConfirmPassword;
                usr.ActiveStatus = true;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
