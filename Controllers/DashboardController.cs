using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using M3LMS.Models;
using System.Data;


namespace M3LMS.Controllers
{
    [HandleError]
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/
        M3LMS.Models.DB_A14BC3_M3LMSEntities db = new M3LMS.Models.DB_A14BC3_M3LMSEntities();
        //ICharts _ICharts;
        public DashboardController()
        {
            //_ICharts = new ChartsConcrete();
        }  
        
        public ActionResult Index()
        {

            return View();
           
            
        }

        public ActionResult Index1()
        {

            return PartialView("Index2");


        }
        public JsonResult Piechart()
        {


            var AdminDeptList = db.ref_Users.Select(a => a.DepartmentId).ToList();
            var userlist = db.ref_Users.Where(a => AdminDeptList.Contains(a.DepartmentId)).ToList();
            var DeptUsers = db.ref_Users.Where(a => AdminDeptList.Contains(a.DepartmentId)).ToList();
            var data = DeptUsers.GroupBy(a => a.ref_department.DeptName).Select(y => new { Department = y.Key.ToString(), TotalEmployees = y.Count() }); // calling method Listdata

            return Json(data, JsonRequestBehavior.AllowGet); // returning list from here.
        }

        public JsonResult LoginHistoryChart()
        {


            var data = db.ref_loginhistory.GroupBy(a => a.Login_Date).Select(y => new { Date = y.Key.ToString(), TotalLogins = y.Count() });

            return Json(data, JsonRequestBehavior.AllowGet); // returning list from here.
        }
    }
}
