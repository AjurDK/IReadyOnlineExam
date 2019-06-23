using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IReadyonlineexam.Models;
using IReadyonlineexam.Classes;

namespace IReadyonlineexam.Classes
{
    public class UserDetails
    {
      public  ireadytodayEntities2 dbcontext = new ireadytodayEntities2();

        public string emailID { get; set; }
        public int? Student_ID { get; set; }
        public int? CourseID { get; set; }
        public int? SemID { get; set; }
        public int SubjectID { get; set; }
        public int UnitID { get; set; }
        public int TotalMarks { get; set; }
        public int ScoredMarks { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int No_Attempts { get; set; }
        public int Rank { get; set; }


    }
    public class GetUserByUserName : UserDetails
    {
        // ********************************************** Get User Details By Passing emailID *****************************************
    public GetUserByUserName(string email)
        {
            var tempuser = (from usertab in dbcontext.AspNetUsers
                            join proftab in dbcontext.StudentProfiles on usertab.UserName equals proftab.EmailID

                            where usertab.UserName == email
                            select new UserDetails {
                                Student_ID = proftab.Student_ID,
                                CourseID = proftab.Course,
                                SemID = proftab.Semester,
                               

                            }).FirstOrDefault();
            if (tempuser != null)
            {
                this.Student_ID = tempuser.Student_ID;
                this.CourseID = tempuser.CourseID;
                this.SemID = tempuser.SemID;
                
              
            }
        }
    }

}