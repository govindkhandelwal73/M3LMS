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
    public class DepController : Controller
    {
        //
        // GET: /Dep/
        DB_A14BC3_M3LMSEntities db = new Models.DB_A14BC3_M3LMSEntities();
        public ActionResult Index()
        {
            

            return View();
        }

        public ActionResult CreateDepartment()
        {
             
                
                ViewBag.Deptlist = db.ref_department.ToList();
                return PartialView("CreateDepartment");
            
            
        }
        [HttpPost]
        public ActionResult CreateDepartment(ref_department obj)
        {
            if (ModelState.IsValid)
            {
                if (db.ref_department.Count(a => a.DeptName == obj.DeptName) == 0)
                {
                    db.ref_department.Add(obj);
                    db.SaveChanges();
                    ViewBag.Deptlist = db.ref_department.ToList();
                    return RedirectToAction("CreateDepartment");
                }
                else
                {
                    TempData["ErrMsg"] = obj.DeptName + " Department already exists";
                    return RedirectToAction("CreateDepartment");
                }
            }
            else
            {
                return RedirectToAction("CreateDepartment");
            }
        }
        public ActionResult EditDepartment(int id)
        {
            
            ref_department objdept = db.ref_department.Where(a => a.DeptId == id).FirstOrDefault();

            return PartialView("EditDepartment", objdept);
        }
        [HttpPost]
        public ActionResult EditDepartment(ref_department obj)
        {
             

            int deptid = obj.DeptId;
            ref_department  _dept = db.ref_department.Where(a => a.DeptId == obj.DeptId).FirstOrDefault();
            _dept.DeptName = obj.DeptName;
            _dept.Description = obj.Description;
            
            _dept.PhoneNo = obj.PhoneNo;
            _dept.Strength = obj.Strength;
            _dept.Location = obj.Location;
            db.SaveChanges();
            ViewBag.Deptlist = db.ref_department.ToList();
            ViewBag.IsEmptyModel = true;
            return PartialView("CreateDepartment");
        }
        public ActionResult DeleteDepartment(int id)
        {
            
            ref_department objdept = db.ref_department.Where(a => a.DeptId == id).FirstOrDefault();
            return PartialView("DeleteDepartment",objdept);
        }
        [HttpPost, ActionName("DeleteDepartment")]
        public ActionResult Delete(int Id)
        {
           
            ref_department dept = db.ref_department.Find(Id);
            db.ref_department.Remove(dept);
            
            
            
            db.SaveChanges();
            ViewBag.Deptlist = db.ref_department.ToList();
           // return RedirectToAction("Index","Admin");
            //return Content("Data Deleted");
            return PartialView("CreateDepartment");
        }

    }
}
