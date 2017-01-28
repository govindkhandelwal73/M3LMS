using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M3LMS.ViewModels
{
    public class Test
    {
        //[Require]
        public string Title { get; set; }

        public int DepartmentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ManagerId { get; set; }
        public int CourseId { get; set; }

        public int TestId { get; set; }
        public string EmailId { get; set; }
    }
}