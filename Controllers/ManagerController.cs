using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using M3LMS.Models;
using M3LMS.ViewModels;
using M3LMS.Controllers;
using System.IO;

namespace M3LMS.Controllers
{

    [HandleError]
    public class ManagerController : Controller
    {
        //
        // GET: /Manager/
        M3LMS.Models.DB_A14BC3_M3LMSEntities db = new M3LMS.Models.DB_A14BC3_M3LMSEntities();


        public ActionResult Index()
        {

            return View();
        }
        public ActionResult CreateCourse()


        {
            string UserName = Session["UserName"].ToString();
            var MangerDepList = db.ref_Users.Where(a => a.Email == UserName && a.UserRole == 2).Select(a => a.DepartmentId).ToList();
            // List<ref_modules> CourseList = db.ref_modules.Where(a => MangerDepList.Contains(a.DepartmentId)).ToList();
            ViewBag.DeptLists = new MultiSelectList(db.ref_department.Where(a => MangerDepList.Contains(a.DeptId)).ToList(), "DeptId", "DeptName");
            ViewBag.CoursesList = db.ref_modules.Where(a => MangerDepList.Contains(a.DepartmentId)).ToList();
            return PartialView("CreateCourse");
        }

        [HttpPost]
        public ActionResult CreateCourse(Courses obj)
        {


            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                if (db.ref_modules.Count(a => a.CourseName == obj.CourseName) == 0)
                {
                    ref_modules mod = new ref_modules();
                    mod.DepartmentId = int.Parse(obj.DepartmentId);
                    mod.CourseName = obj.CourseName;
                    mod.CreatedDate = obj.CreatedDate;
                    mod.Description = obj.Description;
                    mod.Status = obj.Status;
                    db.ref_modules.Add(mod);
                    db.SaveChanges();
                }
                else
                {
                    TempData["ErrMsg"] = obj.CourseName + " Course already exists";
                    return RedirectToAction("Index");
                }


            }
            //return PartialView("CreateCourse");
            return RedirectToAction("Index");
        }

        public ActionResult Chapter()
        {
            if (Session["UserType"].ToString() != null && Session["UserType"].ToString() == "Manager")
            {
                string Email = Session["UserName"].ToString();
                var ManDeptList = db.ref_Users.Where(a => a.Email == Email).Select(a => a.DepartmentId).ToList();
                var CourseList = db.ref_modules.Where(a => ManDeptList.Contains(a.DepartmentId)).ToList();
                ViewBag.CourseLists = new MultiSelectList(CourseList, "CourseId", "CourseName");
                ViewBag.ChapterLists = db.ref_chapters.Where(a => (db.ref_modules.Where(b => ManDeptList.Contains(b.DepartmentId)).Select(b => b.CourseId)).ToList().Contains((int)a.CourseId)).ToList();
                return PartialView("Chapter");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult Chapter(Chapters obj)
        {
            if (ModelState.IsValid)
            {
                if (db.ref_chapters.Count(a => a.ChapterName == obj.ChapterName) == 0)
                {
                    ref_chapters chpt = new ref_chapters();
                    //int? chid=db.ref_chapters.Max(a => a.ChapterId);
                    //chid = chid == null ? 0 : chid;
                    //chid = chid + 1;
                    //chpt.ChapterId = (int)chid;
                    chpt.CourseId = obj.CourseId;
                    chpt.ChapterName = obj.ChapterName;
                    chpt.Content_Type = obj.Content_Type;

                    var file = obj.ChapContent;

                    var fileName = Path.GetFileName(obj.ChapContent.FileName);
                    var path = Path.Combine(Server.MapPath("~/ChapterContent/"), fileName);
                    string newPath = "";
                    switch (obj.Content_Type)
                    {
                        case "Word Document":
                            path = Path.Combine(Server.MapPath("~/ChapterContent/Word/"), fileName);
                            newPath = "ChapterContent/Word/" + fileName;
                            break;

                        case "Pdf File":

                            path = Path.Combine(Server.MapPath("~/ChapterContent/Pdf/"), fileName);
                            newPath = "ChapterContent/Pdf/" + fileName;
                            break;

                        case "Presentaion":
                            path = Path.Combine(Server.MapPath("~/ChapterContent/Ppt/"), fileName);
                            newPath = "ChapterContent/Ppt/" + fileName;
                            break;

                        case "Video File":
                            path = Path.Combine(Server.MapPath("~/ChapterContent/Video/"), fileName);
                            newPath = "ChapterContent/Video/" + fileName;
                            break;


                    }


                    file.SaveAs(path);
                    chpt.ContentPath = newPath;
                    //Stream fs = obj.ChapContent.InputStream;
                    //BinaryReader br = new BinaryReader(fs);
                    //Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    //chpt.Content = bytes;
                    chpt.Sort_Order = obj.Sort_Order;
                    db.ref_chapters.Add(chpt);
                    db.SaveChanges();
                }
                else
                {
                    TempData["ErrMsg"] = obj.ChapterName + " Chapter already exists";
                    return RedirectToAction("Index");
                }


            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult deleteNstatusCommand(IEnumerable<int> csid, string command)
        {

            foreach (int courseid in csid)
            {
                ref_modules module = db.ref_modules.Where(a => a.CourseId == courseid).SingleOrDefault();
                if (command == "Delete")
                {

                    db.ref_modules.Remove(module);
                }
                if (command == "ChangeStatus")
                {
                    if (module.Status == true)
                        module.Status = false;
                    if (module.Status == false)
                        module.Status = true;
                }
                db.SaveChanges();
            }


            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult deleteNstatusCommand_chapter(IEnumerable<int> chid, string command)
        {
            foreach (int chapterid in chid)
            {
                ref_chapters chapter = db.ref_chapters.Where(a => a.ChapterId == chapterid).SingleOrDefault();


                if (command == "Delete")
                {

                    db.ref_chapters.Remove(chapter);
                    if (System.IO.File.Exists((chapter.ContentPath)))
                    {
                        System.IO.File.Delete((chapter.ContentPath));
                    }
                }

                db.SaveChanges();
            }


            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult ChangeCourseStatus(IEnumerable<int> chid)
        {
            return View();
        }
        public FileResult GetFile(int? id)
        {
            //SqlDataReader rdr; byte[] fileContent = null;
            //string mimeType = ""; string fileName = "";
            //const string connect = @"Server=.\SQLExpress;Database=FileTest;Trusted_Connection=True;";

            //using (var conn = new SqlConnection(connect))
            //{
            //    var qry = "SELECT FileContent, MimeType, FileName FROM FileStore WHERE ID = @ID";
            //    var cmd = new SqlCommand(qry, conn);
            //    cmd.Parameters.AddWithValue("@ID", id);
            //    conn.Open();
            //    rdr = cmd.ExecuteReader();
            //    if (rdr.HasRows)
            //    {
            //rdr.Read();
            //byte[] fileContent = (byte[])db.ref_chapters.Where(a => a.ChapterId == id).Select(a => a.Content).SingleOrDefault();

            //string mimetype = string.Empty;
            //string extntype = string.Empty;
            //switch (db.ref_chapters.Where(a => a.ChapterId == id).Select(a => a.Content_Type).Single())
            //{
            //    case "Word Document":
            //        mimetype = "application/msword";
            //        extntype = ".docx";
            //        break;
            //    case "Pdf File":
            //        mimetype = "application/pdf";
            //        extntype = ".pdf";
            //        break;
            //    case "Presentaion":
            //        mimetype = "application/mspowerpoint";
            //        extntype = ".ppt";
            //        break;
            //    case "Video File":

            //        mimetype = "video/mp4";
            //        extntype = ".mp4";
            //        break;
            //}
            string path = db.ref_chapters.Where(a => a.ChapterId == id).Select(a => a.ContentPath).SingleOrDefault();
            var name = path.Split('/');

            var fileType = path.Split('.');
            string fileType1 = fileType[1].ToString();
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("/" + path));

            return File(fileBytes, fileType1, name[name.Count() - 1]);





            //return File(fileContent, "application/msword", db.ref_chapters.Where(a => a.ChapterId == id).Select(a => a.ChapterName).Single() + ".docx");
            //return File(path, fileType1);
        }

        public ActionResult Question()
        {
            if (Session["UserType"].ToString() != null && Session["UserType"].ToString() == "Manager")
            {
                string UserName = Session["UserName"].ToString();
                var MangerDepList = db.ref_Users.Where(a => a.Email == UserName && a.UserRole == 2).Select(a => a.DepartmentId).ToList();
                // List<ref_modules> CourseList = db.ref_modules.Where(a => MangerDepList.Contains(a.DepartmentId)).ToList();
                ViewBag.DeptLists = new MultiSelectList(db.ref_department.Where(a => MangerDepList.Contains(a.DeptId)).ToList(), "DeptId", "DeptName");
                ViewBag.CourseList = new MultiSelectList(db.ref_modules.Where(a => MangerDepList.Contains(a.DepartmentId)).ToList(), "CourseId", "CourseName");
                ViewBag.QuestionLists = db.ref_questions.Where(a => (db.ref_modules.Where(b => MangerDepList.Contains((int)b.DepartmentId)).Select(b => b.CourseId).ToList()).Contains((int)a.CourseId)).ToList();


                return PartialView("Question");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public ActionResult Question(QuestionText q)
        {
            if (ModelState.IsValid)
            {
                ref_questions obj = new ref_questions();
                obj.Question = q.Question;
                obj.CourseId = q.CourseId;
                obj.Option1 = q.Option1;
                obj.Option2 = q.Option2;
                obj.Option3 = q.Option3;
                obj.Option4 = q.Option4;
                obj.Answer = q.Answer;
                obj.Active = q.Active;
                db.ref_questions.Add(obj);
                db.SaveChanges();

            }

            return RedirectToAction("Index");


        }

        public ActionResult Test()
        {
            var DeptLis = db.ref_department.Select(a => a.DeptId).ToList();
            ViewBag.ModuleList = new SelectList(db.ref_modules.Where(a => DeptLis.Contains(a.DepartmentId)).ToList(), "CourseId", "CourseName");
            ViewBag.QuestionList = db.ref_questions.ToList();
            return PartialView("Test");
        }

        [HttpPost]
        public ActionResult Test(Test model, int Modules, IEnumerable<int> qid)
        {

            if (!string.IsNullOrEmpty(model.Title))
            {
                ref_TestData _unitofWork = new ref_TestData();
                _unitofWork.ExpiryDate = model.ExpiryDate;
                _unitofWork.CreatedDate = DateTime.Now;
                _unitofWork.Title = model.Title;
                _unitofWork.Module_Id = Modules;
                //_unitofWork.Manager_Id = model.ManagerId;
                db.ref_TestData.Add(_unitofWork);
                db.SaveChanges();
                int testid = db.ref_TestData.Max(a => a.TestId);

                //code to addquestions against the testid
                foreach (int id in qid)
                {
                    ref_TestQ obj = new ref_TestQ();
                    obj.TestId = testid;
                    obj.Qid = id;
                    db.ref_TestQ.Add(obj);
                    db.SaveChanges();
                }
                //

            }

            return RedirectToAction("Index");
            //if (!string.IsNullOrEmpty(model.Title))
            //{
            //    ref_TestData _unitofWork = new ref_TestData();
            //    _unitofWork.ExpiryDate = model.ExpiryDate;
            //    _unitofWork.CreatedDate = DateTime.Now;
            //    _unitofWork.Title = model.Title;
            //    _unitofWork.Module_Id = Modules;
            //    //_unitofWork.Manager_Id = model.ManagerId;
            //    db.ref_TestData.Add(_unitofWork);
            //    db.SaveChanges();
            //}

            //return RedirectToAction("Test");
        }

        public ActionResult GetQuestions(int id)
        {
            return PartialView("GetModuleQuestions", db.ref_questions.Where(a => a.CourseId == id).ToList());
        }


        public ActionResult LearnerList()
        {
            string UserName = Session["UserName"].ToString();
            var MangerDepList = db.ref_Users.Where(a => a.Email == UserName && a.UserRole == 2).Select(a => a.DepartmentId).ToList();
            // var assignAllUserCourse1 = db.ref_allusercourse.ToList();
            //var assignAllUserCourse = db.ref_allusercourse.Where(a => a.DepartmentId == MangerDepList.FirstOrDefault()).Select(a => a.CourseID).ToList();
            var courseList = db.ref_modules.Where(a => a.DepartmentId == MangerDepList.FirstOrDefault()).ToList();
            var couseByUser = db.ref_learnercoursemapping.ToList();
            var departLearnerList = db.ref_Users.Where(a => a.DepartmentId == MangerDepList.FirstOrDefault() && a.UserRole == 3 && a.ActiveStatus == true).ToList();
            if (couseByUser.Count > 0)
            {
                var couseNameByUser = (from a in couseByUser
                                       join b in courseList on a.CourseId equals b.CourseId
                                       select new
                                       {
                                           EmailID = a.EmailId,
                                           CourseName = b.CourseName

                                       }).ToList();

                var couseNameByUserGroup = (from i in couseNameByUser
                                            group i.CourseName by i.EmailID into g
                                            select new
                                            {
                                                g.Key,

                                                items = string.Join(",", g.ToArray())
                                            }).ToList();

                var newList = (from a in couseNameByUserGroup
                               join b in departLearnerList on a.Key equals b.Email
                               select new UserWithCourse
                               {

                                   EmailId = b.Email,
                                   Name = b.FirstName + " " + b.LastName,
                                   DepartmentId = b.DepartmentId,
                                   CourseName = a.items,

                               }).ToList();

                ViewBag.LearnerList = newList;

            }
            else
            {
                ViewBag.LearnerList = departLearnerList.Select(b => new UserWithCourse
                {
                    EmailId = b.Email,
                    Name = b.FirstName + " " + b.LastName,
                    DepartmentId = b.DepartmentId,
                    CourseName = "No Course Assigned"
                }).ToList();
            }
            ViewBag.AllCourseLists = new MultiSelectList(courseList, "CourseId", "CourseName", null);
            var v = new MultiSelectList(db.ref_department.Where(a => MangerDepList.Contains(a.DeptId)).ToList(), "DeptId", "DeptName", MangerDepList.FirstOrDefault().ToString().ToList());

            ViewBag.DeptLists = v;



            return PartialView("LearnerList");
        }

        [HttpPost]
        public ActionResult GetUserListByDepartment(int depId)
        {
            var courseList = db.ref_modules.Where(a => a.DepartmentId == depId).ToList();
            var couseByUser = db.ref_learnercoursemapping.ToList();

            var couseNameByUser = (from a in couseByUser
                                   join b in courseList on a.CourseId equals b.CourseId
                                   select new
                                   {
                                       EmailID = a.EmailId,
                                       CourseName = b.CourseName

                                   }).ToList();
            var couseNameByUserGroup = (from i in couseNameByUser
                                        group i.CourseName by i.EmailID into g
                                        select new
                                        {
                                            g.Key,

                                            items = string.Join(",", g.ToArray())
                                        }).ToList();


            var departLearnerList = db.ref_Users.Where(a => a.DepartmentId == depId && a.UserRole == 3 && a.ActiveStatus == true).ToList();

            var newList = (from a in couseNameByUserGroup
                           join b in departLearnerList on a.Key equals b.Email
                           select new UserWithCourse
                           {

                               EmailId = b.Email,
                               Name = b.FirstName + " " + b.LastName,
                               DepartmentId = b.DepartmentId,
                               CourseName = a.items,

                           }).ToList();

            ViewBag.LearnerList = newList;
            ViewBag.AllCourseLists = new MultiSelectList(courseList, "CourseId", "CourseName", null);
            return PartialView("PartialTest");

        }

        [HttpPost]
        public ActionResult LearnerList(IEnumerable<string> user, string command, Learner model)
        {

            foreach (string email in user)
            {

                if (model.SelectedCourseIdlist.Count > 0)
                {
                    model.EmailId = email;
                    var result = Coursebyuser(model, true);

                }
                else
                {

                    return RedirectToAction("LearnerList");
                }
            }


            return RedirectToAction("LearnerList");
        }
        public ActionResult AssignCoursebyUser(string EmailId, string Name, int DepId)
        {
            var assignedCouseList = db.ref_learnercoursemapping.Where(a => a.EmailId == EmailId).Select(x => x.CourseId).ToList();
            var model = new Learner();
            model.EmailId = EmailId;
            model.Name = Name;
            model.DepartmentId = DepId;
            var courseList = db.ref_modules.Where(a => a.DepartmentId == DepId).ToList();
            ViewBag.CourseLists = new MultiSelectList(courseList, "CourseId", "CourseName", assignedCouseList);

            return PartialView("AssignCoursebyUser", model);
        }

        [HttpPost]
        public ActionResult AssignCoursebyUser(Learner model)
        {

            if (model.SelectedCourseIdlist.Count > 0)
            {
                var result = Coursebyuser(model, false);
            }
            else
            {

                return RedirectToAction("LearnerList");
            }
            return RedirectToAction("AssignCoursebyUser", new { EmailId = model.EmailId, Name = model.Name, DepId = 1 });


        }

        public bool Coursebyuser(Learner model, bool alluser)
        {
            var list = db.ref_learnercoursemapping.Where(a => a.EmailId == model.EmailId && a.IsCompleted==false).Select(x => x.CourseId).ToList();
            var aLLUserCourselist = db.ref_learnercoursemapping.Where(a => a.EmailId == model.EmailId).Select(x => x.CourseId).ToList();
            var allList = db.ref_allusercourse.Select(x => x.CourseID).ToList();
            if (list.Count > 0)
            {
                foreach (var id in list)
                {
                    if (model.SelectedCourseIdlist.Contains((int)id))
                    { 
                        continue;
                    }
                    else
                    {
                        ref_learnercoursemapping _unitofWork = db.ref_learnercoursemapping.Where(x => x.EmailId == model.EmailId && x.CourseId == id).SingleOrDefault();
                        if (_unitofWork != null)
                        {
                            db.ref_learnercoursemapping.Remove(_unitofWork);
                        }

                        db.SaveChanges();
                    }
                }

            }
            if (allList.Count > 0)
            {
                if (alluser)
                {
                    foreach (var id in allList)
                    {
                        ref_allusercourse _unitofWork = db.ref_allusercourse.Where(x => x.CourseID == id).SingleOrDefault();
                        if (_unitofWork != null)
                        {
                            db.ref_allusercourse.Remove(_unitofWork);
                        }

                        db.SaveChanges();
                    }
                }
            }


            foreach (var item in model.SelectedCourseIdlist)
            {


                if (aLLUserCourselist.Contains((int)item))
                {
                    continue;
                }
                else
                {

                    ref_learnercoursemapping _unitofWork = new ref_learnercoursemapping();
                    _unitofWork.EmailId = model.EmailId;
                    _unitofWork.CourseId = item;
                    _unitofWork.New = true;
                    _unitofWork.CreatedDate = DateTime.Now;
                    _unitofWork.DeptId = model.DepartmentId;
                    _unitofWork.IsCompleted = false;
                    db.ref_learnercoursemapping.Add(_unitofWork);

                    if (alluser)
                    {
                        ref_allusercourse _c = new ref_allusercourse();
                        _c.CourseID = item;
                        _c.DepartmentId = model.DepartmentId;
                        _c.CreatedDate = DateTime.Now;
                        db.ref_allusercourse.Add(_c);
                    }
                    db.SaveChanges();
                }

            }

            return true;

        }

        //public ActionResult LearnerTestList()
        //{
        //    string UserName = Session["UserName"].ToString();
        //    var MangerDepList = db.ref_Users.Where(a => a.Email == UserName && a.UserRole == 2).Select(a => a.DepartmentId).ToList();
        //    var v = new SelectList(db.ref_department.Where(a => MangerDepList.Contains(a.DeptId)).ToList(), "DeptId", "DeptName");
        //    ViewBag.DepartmentList = v;

        //    //var courseList = db.ref_modules.Where(a => a.DepartmentId == MangerDepList.FirstOrDefault()).ToList();



        //    //var TestData=db.ref_TestData.Where(x=>x.Module_Id==)
        //    // var assignAllUserCourse1 = db.ref_allusercourse.ToList();
        //    //var assignAllUserTest = db.ref_allusercourse.Where(a => a.DepartmentId == MangerDepList.FirstOrDefault()).Select(a => a.CourseID).ToList();


        //    //var couseByUser = db.ref_learnercoursemapping.ToList();
        //    //var departLearnerList = db.ref_Users.Where(a => a.DepartmentId == MangerDepList.FirstOrDefault() && a.UserRole == 3 && a.ActiveStatus == true).ToList();
        //    //if (couseByUser.Count > 0)
        //    //{
        //    //    var couseNameByUser = (from a in couseByUser
        //    //                           join b in courseList on a.CourseId equals b.CourseId
        //    //                           select new
        //    //                           {
        //    //                               EmailID = a.EmailId,
        //    //                               CourseName = b.CourseName

        //    //                           }).ToList();

        //    //    var couseNameByUserGroup = (from i in couseNameByUser
        //    //                                group i.CourseName by i.EmailID into g
        //    //                                select new
        //    //                                {
        //    //                                    g.Key,

        //    //                                    items = string.Join(",", g.ToArray())
        //    //                                }).ToList();

        //    //    var newList = (from a in couseNameByUserGroup
        //    //                   join b in departLearnerList on a.Key equals b.Email
        //    //                   select new UserWithCourse
        //    //                   {

        //    //                       EmailId = b.Email,
        //    //                       Name = b.FirstName + " " + b.LastName,
        //    //                       DepartmentId = b.DepartmentId,
        //    //                       CourseName = a.items,

        //    //                   }).ToList();

        //    //    ViewBag.LearnerList = newList;

        //    //}
        //    //else
        //    //{
        //    //    ViewBag.LearnerList = departLearnerList.Select(b => new UserWithCourse
        //    //    {
        //    //        EmailId = b.Email,
        //    //        Name = b.FirstName + " " + b.LastName,
        //    //        DepartmentId = b.DepartmentId,
        //    //        CourseName = "No Course Assigned"
        //    //    }).ToList();
        //    //}
        //    //ViewBag.AllCourseLists = new MultiSelectList(courseList, "CourseId", "CourseName", assignAllUserCourse);
        //    //var v = new MultiSelectList(db.ref_department.Where(a => MangerDepList.Contains(a.DeptId)).ToList(), "DeptId", "DeptName", MangerDepList.FirstOrDefault().ToString().ToList());

        //    //ViewBag.DeptLists = v;



        //    return View();
        //}

        //[HttpPost]
        //public ActionResult LearnerTestList(IEnumerable<string> user, Test model)
        //{
        //    foreach (string email in user)
        //    {

        //        if (model.CourseId != null)
        //        {
        //            model.EmailId = email;


        //        }
        //    }

        //    return RedirectToAction("LearnerTestList");
        //}

        //gk

        public ActionResult LearnerTestList()
        {
            string UserName = Session["UserName"].ToString();
            var MangerDepList = db.ref_Users.Where(a => a.Email == UserName && a.UserRole == 2).Select(a => a.DepartmentId).ToList();
            var v = new SelectList(db.ref_department.Where(a => MangerDepList.Contains(a.DeptId)).ToList(), "DeptId", "DeptName");
            ViewBag.DepartmentList = v;

            //var courseList = db.ref_modules.Where(a => a.DepartmentId == MangerDepList.FirstOrDefault()).ToList();



            //var TestData=db.ref_TestData.Where(x=>x.Module_Id==)
            // var assignAllUserCourse1 = db.ref_allusercourse.ToList();
            //var assignAllUserTest = db.ref_allusercourse.Where(a => a.DepartmentId == MangerDepList.FirstOrDefault()).Select(a => a.CourseID).ToList();


            //var couseByUser = db.ref_learnercoursemapping.ToList();
            //var departLearnerList = db.ref_Users.Where(a => a.DepartmentId == MangerDepList.FirstOrDefault() && a.UserRole == 3 && a.ActiveStatus == true).ToList();
            //if (couseByUser.Count > 0)
            //{
            //    var couseNameByUser = (from a in couseByUser
            //                           join b in courseList on a.CourseId equals b.CourseId
            //                           select new
            //                           {
            //                               EmailID = a.EmailId,
            //                               CourseName = b.CourseName

            //                           }).ToList();

            //    var couseNameByUserGroup = (from i in couseNameByUser
            //                                group i.CourseName by i.EmailID into g
            //                                select new
            //                                {
            //                                    g.Key,

            //                                    items = string.Join(",", g.ToArray())
            //                                }).ToList();

            //    var newList = (from a in couseNameByUserGroup
            //                   join b in departLearnerList on a.Key equals b.Email
            //                   select new UserWithCourse
            //                   {

            //                       EmailId = b.Email,
            //                       Name = b.FirstName + " " + b.LastName,
            //                       DepartmentId = b.DepartmentId,
            //                       CourseName = a.items,

            //                   }).ToList();

            //    ViewBag.LearnerList = newList;

            //}
            //else
            //{
            //    ViewBag.LearnerList = departLearnerList.Select(b => new UserWithCourse
            //    {
            //        EmailId = b.Email,
            //        Name = b.FirstName + " " + b.LastName,
            //        DepartmentId = b.DepartmentId,
            //        CourseName = "No Course Assigned"
            //    }).ToList();
            //}
            //ViewBag.AllCourseLists = new MultiSelectList(courseList, "CourseId", "CourseName", assignAllUserCourse);
            //var v = new MultiSelectList(db.ref_department.Where(a => MangerDepList.Contains(a.DeptId)).ToList(), "DeptId", "DeptName", MangerDepList.FirstOrDefault().ToString().ToList());

            //ViewBag.DeptLists = v;



            return View();
        }

        [HttpPost]
        public ActionResult LearnerTestList(IEnumerable<string> user, Test model)
        {
            if (ModelState.IsValid)
            {
                foreach (string email in user)
                {
                    ref_learnertestmapping obj = new ref_learnertestmapping();
                    obj.EmailId = email;
                    obj.CreatedDate = DateTime.Now;
                    obj.IsCompleted = false;
                    obj.TestId = model.TestId;
                    db.ref_learnertestmapping.Add(obj);

                    db.SaveChanges();

                }
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult FillCourse(int DepartmentID)
        {
            var courseList = db.ref_modules.Where(c => c.DepartmentId == DepartmentID).ToList();
            List<Courses> _c = new List<Courses>();
            foreach (var item in courseList)
            {
                Courses c = new Courses();
                c.CourseId = item.CourseId;
                c.CourseName = item.CourseName;
                _c.Add(c);

            }

            var v = Json(_c, JsonRequestBehavior.AllowGet);
            return v;
        }

        [HttpPost]
        public ActionResult FillTest(int CourseId)
        {
            var allTest = db.ref_TestData.Where(c => c.Module_Id == CourseId).ToList();
            List<Test> tList = new List<Test>();
            foreach (var item in allTest)
            {
                Test t = new Test();
                t.TestId = item.TestId;
                t.Title = item.Title;
                tList.Add(t);


            }
            return Json(tList, JsonRequestBehavior.AllowGet);
        }


    }

}

