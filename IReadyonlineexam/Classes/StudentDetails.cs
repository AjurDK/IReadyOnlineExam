using IReadyonlineexam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IReadyonlineexam.Classes
{
    public class Details
    {
        public int Stuid { get; set; }

        public int? CourseId { get; set; }
        public int? Sem { get; set; }

    }
    public class StudentDetails : Details
    {
        ireadytodayEntities2 dbcontext = new ireadytodayEntities2();

        public StudentDetails(string email)
        {
            var Details = dbcontext.StudentProfiles.Where(a => a.EmailID == email).Select(a => new Details
            {
                Stuid = a.Student_ID,
                CourseId = a.Course,
                Sem = a.Semester,
            }).FirstOrDefault();

            this.Stuid = Details.Stuid;
            this.CourseId = Details.CourseId;
            this.Sem = Details.Sem;
        }
    }
}