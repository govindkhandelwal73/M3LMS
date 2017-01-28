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
    public class LearnerController : Controller
    {
        //
        // GET: /Learner/
        M3LMS.Models.DB_A14BC3_M3LMSEntities db = new M3LMS.Models.DB_A14BC3_M3LMSEntities();
        public ActionResult Index()
        {
            if (Session != null || Session["UserName"] != null || (!string.IsNullOrEmpty(Session["UserName"].ToString())))
            {
                string userName = Session["UserName"].ToString();

                var userDetails = db.ref_Users
                                    .Where(x => x.Email == userName && x.UserRole == 3)
                                    .FirstOrDefault();
                var courseList = db.ref_learnercoursemapping
                                   .Where(x => x.EmailId == userName)
                                   .Select(x => new LearnerMapping { courseId = x.CourseId, New = (bool)x.New,Iscompleted=(bool)x.IsCompleted }).ToList();

                var selectCourseId = courseList.Select(y => y.courseId).ToList();
                var courseDetails = db.ref_modules
                                      .Where(x => selectCourseId.Contains(x.CourseId) && x.Status == true)
                                      .Select(x => new Courses { CourseName = x.CourseName, Description = x.Description, CourseId = x.CourseId }).ToList();
                //var chapterDetails=db.ref_chapters.Where(x=>x.CourseId)

                Learner model = new Learner();
                model.CourseDetails = new List<Courses>();
                model.CourseDetails.AddRange(courseDetails);
                model.Name = userDetails.FirstName + " " + userDetails.LastName;
                model.EmailId = userDetails.Email;
                model.NewCourseCount = courseList.Where(x => x.New == true).ToList().Count;
                model.NewTestCount = db.ref_learnertestmapping.Where(a => a.EmailId == userDetails.Email).Count();
                model.TotalCount = model.NewCourseCount + model.NewTestCount;
                model.CourseCompleted = courseList.Where(x=>x.Iscompleted==true).Count();
                model.TotalCourseCount = courseList.Count();
                Session["LearnerSession"] = model;
                return View(model);
            }
            else
            {

                return RedirectToAction("Index", "Home");
            }


        }

        public ActionResult ChapterDetails(int CourseId, string CourseName)
        {
            var chapterDetails = db.ref_chapters
                                   .Where(x => x.CourseId == CourseId)
                                   .Select(x => new Chapters { ChapterId = x.ChapterId, ChapterName = x.ChapterName, Content_Type = x.Content_Type, Content = x.Content, ContentPath = x.ContentPath })
                                   .ToList();
            // Chapters model = new Chapters();
            Learner model = new Learner();
            model.ChapterDetails = new List<Chapters>();
            var oldModel = (Learner)Session["LearnerSession"];
            model.ChapterDetails.AddRange(chapterDetails);
            model.Name = oldModel.Name;
            model.EmailId = oldModel.EmailId;
            model.NewCourseCount = oldModel.NewCourseCount;
            model.NewTestCount = 0;
            model.NewcourseName = CourseName;
            model.CourseId = CourseId;
            model.TotalCount = oldModel.TotalCount;
            model.CourseCompleted = oldModel.CourseCompleted;
            model.TotalCourseCount = oldModel.TotalCourseCount;
            string userName = Session["UserName"].ToString();

            var lerner = db.ref_learnercoursemapping.Where(x => x.EmailId == userName && x.CourseId == CourseId).SingleOrDefault();
            lerner.New = false;
            db.SaveChanges();

            return View(model);
        }

        public bool CompleteCourse(int CourseId)
        {
            string userName = Session["UserName"].ToString();
           var lerner= db.ref_learnercoursemapping.Where(x => x.EmailId == userName && x.CourseId == CourseId).SingleOrDefault();
           lerner.IsCompleted = true;
           db.SaveChanges();
           return true;

        }

        public ActionResult Tests()
        {
            var username = Session["UserName"].ToString();
            ViewBag.TestList = db.ref_learnertestmapping.Where(a => a.EmailId == username & a.IsCompleted == false).ToList();
            return View();
        }

    }
}
