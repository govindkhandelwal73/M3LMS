using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace M3LMS.ViewModels
{
    public class Chapters
    {
        public int? ChapterId { get; set; }
        [Required]
        public Nullable<int> CourseId { get; set; }
        [Required]
        public string ChapterName { get; set; }
        [Required]
        public string  Content_Type { get; set; }
        [Required]
        public HttpPostedFileBase ChapContent { get; set; }
        [Required]
        public Nullable<int> Sort_Order { get; set; }
        public byte[] Content { get; set; }
        public string ContentPath { get; set; }
    }
}