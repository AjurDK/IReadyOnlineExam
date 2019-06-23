using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IReadyonlineexam.Classes
{
    public class ChartView
    {
        public int Student_ID { get; set; }
        public int UserCount { get; set; }
        public DateTime CreatedDate { get; set; }

    }
    //*********************************************************** User Count *************************************************
    public class UserData
    {
        public int UserCount { get; set; }
        public string Month { get; set; }


    }
    //*********************************************************** Rank Details *************************************************

    public class GetRankDetail
    {
        public int Student_ID { get; set; }
        public int CourseID { get; set; }
        public int SemID { get; set; }
        public int SubjectID { get; set; }
        public int UnitID { get; set; }
        public int TotalMarks { get; set; }
        public int Rank { get; set; }       
     
    }

}