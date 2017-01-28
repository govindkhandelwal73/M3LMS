using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace M3LMS.ViewModels
{
    public class User
    {
        public int? Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        
        
        public HttpPostedFileBase ImagePath { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        public int? RoleId { get; set; }

        public bool ActiveStatus { get; set; }


    }
}