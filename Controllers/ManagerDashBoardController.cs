using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using M3LMS.Models;

namespace M3LMS.Controllers
{
    public class ManagerDashBoardController : Controller
    {  // GET: /Dashboard/
        M3LMS.Models.DB_A14BC3_M3LMSEntities db = new Models.DB_A14BC3_M3LMSEntities();
        //ICharts _ICharts;
         
        
       

        public ActionResult Index()
        {

            return PartialView("Index");


        }
        public JsonResult Piechart()
        {
                     
            
            var data = db.ref_questions.GroupBy(a => a.ref_modules.CourseName).Select(y => new { CourseName = y.Key.ToString(), TotalQuestions = y.Count() }); // calling method Listdata

            return Json(data, JsonRequestBehavior.AllowGet); // returning list from here.
        }

    }
}
