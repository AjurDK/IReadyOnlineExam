using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IReadyonlineexam.Models.Json
{
    public class ScoreModel
    {
        public int? UserID { get; set; }
        public int? CourseID { get; set; }
        public int? SemID { get; set; }
        public int? SubjectID { get; set; }
        public int? UnitID { get; set; }
        public int? ScoredMarks { get; set; }
        public int? TotalMarks { get; set; }
        public int? No_Attempts { get; set; }
        public List<qu> questions { get; set; }


    }

    public class qu
    {
        public int QuizId { get; set; }
        public int QuestionId { get; set; }
        public string Answered { get; set; }

    }
}