using IReadyonlineexam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IReadyonlineexam.Classes
{
    public class Stud
    {
        public ireadytodayEntities2 dbcontext = new ireadytodayEntities2();

    }
    public class GetActivityofStud : Stud
    {
        public bool Active;
        public GetActivityofStud(int ProfileID)
        {
            Active = dbcontext.StudentProfiles.Where(Student => Student.Student_ID == ProfileID).Any();
        }
    }

    public class GetStudID : Stud
    {
        public int ProfileID;
        public GetStudID(string Email)
        {
            ProfileID = dbcontext.StudentProfiles.Where(Student => Student.EmailID == Email).Select(u => u.Student_ID).FirstOrDefault();
        }
    }

    public class CheckStud : Stud
    {
        public bool Exist;
        public CheckStud(string Email)
        {
            Exist = dbcontext.StudentProfiles.Where(Student => Student.EmailID == Email).Any();
        }
    }
}