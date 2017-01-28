using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using M3LMS.DTO;
using M3LMS.Helper;
using M3LMS.ViewModels;
using M3LMS.Models;
using System.Data.Entity;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;

namespace M3LMS.Controllers
{
    [HandleError]
    public class UserController : Controller
    {
        //
        // GET: /User/
        M3LMS.Models.DB_A14BC3_M3LMSEntities db = new M3LMS.Models.DB_A14BC3_M3LMSEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateUser()
        {
            if (Session["UserType"].ToString() != null && Session["UserType"].ToString() == "Super Admin")
            {

                ViewBag.Userlist = db.ref_Users.ToList();//Where(a => AdminDeptList.Contains(a.DepartmentId)).ToList() ;
                var DeptList = db.ref_department.ToList();//Where(a => AdminDeptList.Contains(a.DeptId)).ToList();
                var RoleList = db.ref_roles.ToList();
                ViewBag.DeptLists = new MultiSelectList(DeptList, "DeptId", "DeptName");
                ViewBag.RoleList = new MultiSelectList(RoleList, "RoleId", "RoleName");
            }
            if (Session["UserType"].ToString() != null && Session["UserType"].ToString() == "Admin")
            {
                string Email = Session["UserName"].ToString();
                var AdminDeptList = db.ref_Users.Where(a => a.Email == Email).Select(a => a.DepartmentId).ToList();
                //ViewBag.Userlist = db.ref_Users.Where(a => AdminDeptList.Contains(a.DepartmentId)).ToList();
                ViewBag.Userlist = from a in db.ref_Users where AdminDeptList.Contains(a.DepartmentId) select  a;
                var DeptList = db.ref_department.Where(a => AdminDeptList.Contains(a.DeptId)).ToList();
                var RoleList = db.ref_roles.Where(a => a.RoleId != 0).ToList();
                ViewBag.DeptLists = new MultiSelectList(DeptList, "DeptId", "DeptName");
                ViewBag.RoleList = new SelectList(RoleList, "RoleId", "RoleName");
            }
            var model = new User();
            return PartialView("CreateUser2",model);
        }
        [HttpPost]
        public ActionResult CreateUser(User obj)
        {
           // HttpPostedFileBase file = Request.Files["ImageData"];

            if (ModelState.IsValid)
            {
                ref_Users user = new ref_Users();
                user.DepartmentId = obj.DepartmentId;
                user.Email = obj.Email;
                user.FirstName = obj.FirstName;
                user.LastName = obj.LastName;
                user.UserRole = (int)obj.RoleId;

                var fileName = Path.GetFileName(obj.ImagePath.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/Upload/"), fileName);
                obj.ImagePath.SaveAs(path);
                user.ImagePath = fileName;
                user.Password = obj.FirstName;
                user.ActiveStatus = false;

                ViewBag.Message = "File has been uploaded successfully";
                db.ref_Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }

            else
            {
                return Content("Error");
            }
        }

        public ActionResult EditUser(int id)
        {
            string Email = Session["UserName"].ToString();
            ref_Users obj = db.ref_Users.Where(a => a.Id == id).FirstOrDefault();
            var User = new User { Id = obj.Id, DepartmentId = obj.DepartmentId, FirstName = obj.FirstName, LastName = obj.LastName, Email = obj.Email, RoleId =  obj.UserRole};
            var AdminDeptList = db.ref_Users.Where(a => a.Email == Email).Select(a => a.DepartmentId).ToList();
            ViewBag.Userlist = db.ref_Users.Where(a => AdminDeptList.Contains(a.DepartmentId)).ToList();
            var DeptList = db.ref_department.Where(a => AdminDeptList.Contains(a.DeptId)).ToList();
            var RoleList = db.ref_roles.Where(a => a.RoleId != 4).ToList();
            ViewBag.DeptLists = new SelectList(DeptList, "DeptId", "DeptName");
            ViewBag.RoleList = new SelectList(RoleList, "RoleId", "RoleName");
            ViewBag.Image = obj.ImagePath;
            User.ActiveStatus = (bool)obj.ActiveStatus;

            return PartialView("EditUser2", User);
        }
        [HttpPost]
        public ActionResult EditUser(User user)
        {
           
            if (ModelState.IsValid)
            {
                ref_Users obj = db.ref_Users.Where(a => a.Id == user.Id).FirstOrDefault();

                obj.DepartmentId = user.DepartmentId;
                obj.FirstName = user.FirstName;
                obj.LastName = user.LastName;
                obj.UserRole = (int)user.RoleId;
                obj.ActiveStatus = user.ActiveStatus;
                if (user.ImagePath != null)
                {
                    var fileName = Path.GetFileName(user.ImagePath.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Upload/"), fileName);
                    user.ImagePath.SaveAs(path);
                    obj.ImagePath = fileName;
                }

                db.SaveChanges();
                
            }
            return RedirectToAction("Index", "Admin");
            
        }

        public ActionResult DeleteUser(int id)
        {
            ref_Users objdept = db.ref_Users.Where(a =>a.Id == id).FirstOrDefault();
            return PartialView("DeleteUser", objdept);
        }
        [HttpPost, ActionName("DeleteUser")]
        public ActionResult Delete(ref_Users user)
        {

            ref_Users UserToDelete = db.ref_Users.Where(a => a.Id == user.Id && a.DepartmentId == user.DepartmentId).FirstOrDefault();
            db.ref_Users.Remove(UserToDelete);
            db.SaveChanges();
            ViewBag.UserList = db.ref_Users.ToList();
            return RedirectToAction("CreateUser");
        }
        public ActionResult GetDepartments(string Email)
        {
             List<string> DeptList = db.ref_Users.Where(a => a.Email == Email).Select(a => a.ref_department.DeptName).ToList();
             ViewBag.Departments = DeptList;
             return PartialView(ViewBag.Departments);
        }

       public ActionResult BulkUserUpload()
        {
            if (Session["UserType"].ToString() == "Admin" || Session["UserType"].ToString() == "Super Admin")
            {
                return PartialView("BulkUserUpload");
            }
            else
            {
                return Content("Sorry You are not authorized to do bulk upload operation");
            }
        }
        [HttpPost]
       public ActionResult BulkUserUpload(HttpPostedFileBase file)
       {
           DataSet ds = new DataSet();
           if (Request.Files["file"].ContentLength > 0)
           {
               string fileExtension =
                                    System.IO.Path.GetExtension(Request.Files["file"].FileName);
               if (fileExtension == ".xls" || fileExtension == ".xlsx")
               {
                   //string fileLocation = Server.MapPath("~/UserFiles/") + Request.Files["file"].FileName;
                   System.IO.Directory.CreateDirectory(Server.MapPath("~/UserFiles/" + DateTime.Now.DayOfWeek + "_" + Session["UserName"].ToString()));
                   string fileLocation = Server.MapPath("~/UserFiles/" + DateTime.Now.DayOfWeek + "_" + Session["UserName"].ToString()+"/") + Request.Files["file"].FileName;
                 
                   Request.Files["file"].SaveAs(fileLocation);
                   string excelConnectionString = string.Empty;
                   excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=No;IMEX=2\"";
                   //connection String for xls file format.
                   if (fileExtension == ".xls")
                   {
                       excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=2\"";
                   }
                   //connection String for xlsx file format.
                   else if (fileExtension == ".xlsx")
                   {
                       excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                   }
                   //Create Connection to Excel work book and add oledb namespace
                   OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                   excelConnection.Open();
                   DataTable dt = new DataTable();

                   dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                   if (dt == null)
                   {
                       return null;
                   }
                   String[] excelSheets = new String[dt.Rows.Count];
                   int t = 0;
                   //excel data saves in temp file here.
                   foreach (DataRow row in dt.Rows)
                   {
                       excelSheets[t] = row["TABLE_NAME"].ToString();
                       t++;
                   }
                   OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);
                   string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    excelConnection.Close();
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                   {
                       dataAdapter.Fill(ds);
                   }
               }
                
               for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
               {
                   ref_Users obj = new ref_Users();
                   obj.Email = ds.Tables[0].Rows[i][0].ToString();
                   if (ds.Tables[0].Rows[i][1].ToString()== "")
                   {
                       obj.DepartmentId = 2;// Set default value for blank DepartmentId
                   }
                   else
                   {
                       obj.DepartmentId = int.Parse(ds.Tables[0].Rows[i][1].ToString());
                   }
                   obj.Password = obj.Email.Substring(0, 4);// ds.Tables[0].Rows[i][2].ToString();
                   obj.FirstName = ds.Tables[0].Rows[i][2].ToString();
                   obj.LastName = ds.Tables[0].Rows[i][3].ToString();
                   obj.ImagePath = "a.jpg";

                   obj.ActiveStatus = false;// Convert.ToBoolean(ds.Tables[0].Rows[i][3].ToString());
                   switch (ds.Tables[0].Rows[i][4].ToString())
                   {
                       case "Admin":
                           obj.UserRole = 1;
                           break;
                       case "Manager":
                           obj.UserRole = 2;
                           break;
                       case "Learner":
                           obj.UserRole = 3;
                           break;
                          
                    }
                   if (db.ref_Users.Where(a => a.Email == obj.Email && a.DepartmentId == obj.DepartmentId && a.UserRole == obj.UserRole).Count() == 0)
                   {
                       //obj.UserRole = int.Parse(ds.Tables[0].Rows[i][4].ToString());
                       db.ref_Users.Add(obj);
                   }

                   
                }
               
               db.SaveChanges();
           }
             return RedirectToAction("Index","Admin");
       } 
    }
}
