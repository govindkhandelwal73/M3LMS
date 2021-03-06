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
    
    public partial class ref_modules
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ref_modules()
        {
            this.ref_allusercourse = new HashSet<ref_allusercourse>();
            this.ref_chapters = new HashSet<ref_chapters>();
            this.ref_learnercoursemapping = new HashSet<ref_learnercoursemapping>();
            this.ref_questions = new HashSet<ref_questions>();
            this.ref_TestData = new HashSet<ref_TestData>();
        }
    
        public int CourseId { get; set; }
        public int DepartmentId { get; set; }
        public string CourseName { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ref_allusercourse> ref_allusercourse { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ref_chapters> ref_chapters { get; set; }
        public virtual ref_department ref_department { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ref_learnercoursemapping> ref_learnercoursemapping { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ref_questions> ref_questions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ref_TestData> ref_TestData { get; set; }
    }
}
