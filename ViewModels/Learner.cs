using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace M3LMS.ViewModels
{
    public class Learner
    {


        public int DepartmentId { get; set; }
        public string Name { get; set; }

        [Required]
        public string EmailId { get; set; }
        [Required]
        public int CourseId { get; set; }

        [Required]
        public int TestId { get; set; }
        [Required]
        [ValidJoinDate(ErrorMessage =
            "Join Date can not be greater than current date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        public int NewCourseCount { get; set; }
        public int NewTestCount { get; set; }
        public int TotalCount { get; set; }
        public List<int> SelectedCourseIdlist { get; set; }
        public List<string> CourseName { get; set; }
        public List<int> TestIdList { get; set; }
        public List<string> TestName { get; set; }

        public List<Courses> CourseDetails { get; set; }

        public List<Chapters> ChapterDetails { get; set; }

        public string NewcourseName { get; set; }

        public int CourseCompleted { get; set; }

        public int TotalCourseCount { get; set; }




    }
    public class LearnerMapping
    {
        public int? courseId { get; set; }
        public bool New { get; set; }

        public bool Iscompleted { get; set; }
        //public int courseId { get; set; }
        //public int courseId { get; set; }

    }

    public class UserWithCourseList
    {
        public List<UserWithCourse> userList { get; set; }

    }
    public class UserWithCourse
    {

        public string EmailId { get; set; }
        public string Name { get; set; }
        public int DepartmentId{ get; set; }
        public string CourseName { get; set; }

    }




}