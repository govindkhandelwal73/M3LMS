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
    
    public partial class ref_LearnerMapping
    {
        public int LearnerId { get; set; }
        public string EmailId { get; set; }
        public Nullable<int> CourseId { get; set; }
        public Nullable<int> TestId { get; set; }
        public bool New { get; set; }
        public Nullable<int> Sort_Order { get; set; }
    }
}
