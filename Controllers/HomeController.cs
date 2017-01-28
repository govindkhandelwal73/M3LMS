using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using M3LMS.Models;
using System.Web.Security;

namespace M3LMS.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        M3LMS.Models.DB_A14BC3_M3LMSEntities db = new Models.DB_A14BC3_M3LMSEntities();
        string RedirectUrl;
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection col)
        {

            int LoginType = int.Parse(col[0].ToString());
            string UserEmail = col[1];
            string Password = col[2];
            //get Id & Role from UserEMail
            int? Id = db.ref_Users.Where(a => a.Email == UserEmail && a.Password==Password && a.UserRole==LoginType).Select(a => a.Id).FirstOrDefault();
            //check users role in role table
            //List<int>Roles = db.ref_UserRoles.Where(a => a.Id == Id).Select(a=>a.RoleId).ToList();
            
            if(Id!=null && Id!=0)
            {
                Session["UserName"] = db.ref_Users.Where(a => a.Id == Id).Select(a => a.Email).FirstOrDefault();
                if (LoginType == 0)
                {
                    RedirectUrl = db.ref_Users.Where(a => a.Id == Id).Count() > 0 ? "Admin/Index" : "Home/Index";
                    Session["UserType"] = "Super Admin";
                }

                else if (LoginType == 1)
                {
                    RedirectUrl = db.ref_Users.Where(a => a.Id == Id ).Count() > 0 ? "Admin/Index" : "Home/Index";
                    Session["UserType"] = "Admin";
                }
                else if (LoginType == 2)
                {
                    Session["UserType"] = "Manager";
                    RedirectUrl = db.ref_Users.Where(a => a.Id == Id ).Count() > 0 ? "Manager/Index" : "Home/Index";
                }
                else
                {
                    Session["UserType"] = "Learner";
                    RedirectUrl = db.ref_Users.Where(a => a.Id == Id ).Count() > 0 ? "Learner/Index" : "Home/Index";
                }
                string[] Url = RedirectUrl.Split('/');
                ref_loginhistory lgn = new ref_loginhistory();
                lgn.Id = (int) Id;
                lgn.Login_Date = DateTime.Now.Date.Date;
                lgn.Login_Time = Convert.ToDateTime(DateTime.Now.ToString("HH:mm:ss"));// DateTime.Now.Date.Date; 
                lgn.Login_Type = int.Parse(col[0]);
                db.ref_loginhistory.Add(lgn);
                db.SaveChanges();
                return RedirectToAction(Url[1], Url[0]);
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid Login ...Try again...";
                return RedirectToAction("Index", "Home");
            }

            
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}
