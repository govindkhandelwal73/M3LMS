//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace M3LMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ref_Users
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImagePath { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public string Password { get; set; }
        public Nullable<bool> ActiveStatus { get; set; }
        public int UserRole { get; set; }
    
        public virtual ref_department ref_department { get; set; }
        public virtual ref_roles ref_roles { get; set; }
    }
}