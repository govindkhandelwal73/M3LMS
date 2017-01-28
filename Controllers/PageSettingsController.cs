using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using M3LMS.Models;

namespace M3LMS.Views.Shared
{
    [HandleError]
    public class PageSettingsController : Controller
    {
        //
        // GET: /PageSettings/
        M3LMS.Models.DB_A14BC3_M3LMSEntities db = new M3LMS.Models.DB_A14BC3_M3LMSEntities();
        public ActionResult Index()
        {
            return PartialView("Settings");
        }
        [HttpPost]
        public ActionResult Index(FormCollection col)
        {
            List<ref_pagesection> pages=db.ref_pagesection.Where(a=>a.PageId==1).ToList();// && a.Section=="BackColor"
            foreach(ref_pagesection p in pages)
            {
                if(p.Section=="Color")
                {
                    p.Text = col[0].ToString();
                }
                if(p.Section=="Name1")
                {
                    p.Text = col[1].ToString();

                }
                if(p.Section=="Name2")
                {
                    p.Text = col[2].ToString();
                }
                db.SaveChanges();
            }
            
            return RedirectToAction("Index", "Admin");
        }
    }
}
