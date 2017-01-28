using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace M3LMS.ViewModels
{
    public class QuestionText
    {
        public int? QuestionId { get; set; }
        [Required]
        public Nullable<int> CourseId { get; set; }
        [Required]
        public string Question { get; set; }
         [Required]
         
        public string Option1 { get; set; }
        [Required]
        public string Option2 { get; set; }
        [Required]
        public string Option3 { get; set; }
        [Required]
        public string Option4 { get; set; }
        [Required]
        public string Answer { get; set; }
        [Required]
        public Nullable<bool> Active { get; set; }
    }
}