using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using M3LMS.Helper;


namespace M3LMS.ViewModels
{
    public class Courses
    {
      
        [Required]
        public string  DepartmentId { get; set; }
        [Required]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        [Required]
        public string  Description { get; set; }
        [Required]
        [ValidJoinDate(ErrorMessage=
            "Join Date can not be greater than current date")] 
        
        public DateTime CreatedDate { get; set; }
        [Required]
        public Boolean Status { get; set; }
    }

    
}