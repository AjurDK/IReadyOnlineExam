//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IReadyonlineexam.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Stu_Result
    {
        public int RID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> CourseID { get; set; }
        public Nullable<int> SemID { get; set; }
        public Nullable<int> SubjectID { get; set; }
        public Nullable<int> UnitID { get; set; }
        public Nullable<int> ScoredMarks { get; set; }
        public Nullable<int> TotalMarks { get; set; }
        public Nullable<int> No_Attempts { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<decimal> Ratings { get; set; }
        public Nullable<int> Rating { get; set; }
    }
}