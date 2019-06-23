using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IReadyonlineexam.Classes
{
    public class SubjectDetails
    {
    }
    // ******************************************************** User Subject Score Class   ******************************************************

    public class usersubscore
    {
        public int? TotalMarks { get; set; }
        public int? ScoredMarks { get; set; }
        public int? unitcount { get; set; }
        public int UnitID { get; set; }
    }

    // ********************************************* User Performance and  Rank Details Class   ******************************************************
    public class UserRankObject
    {
        public int? Rank { get; set; }
        public int? UserId { get; set; }
        public double? AverageScore { get; set; }
        public int sumScored { get; set; }
        public int sumTotalMarks { get; set; }
        public int? UnitID { get; set; }
    }
    public class UserUnitRankObject
    {
        public int? Rank { get; set; }
        public int? UserId { get; set; }
        public double? AverageScore { get; set; }
        public int sumScored { get; set; }
        public int sumTotalMarks { get; set; }
        public int? UnitID { get; set; }
    }

}