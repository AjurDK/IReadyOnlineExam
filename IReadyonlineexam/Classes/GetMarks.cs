using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IReadyonlineexam.Models;


namespace IReadyonlineexam.Classes
{
    public class SujectWiseMarks
    {
        public int UserID { get; set; }
        public int? SubjectID { get; set; }
        public int? UnitID { get; set; }
        public DateTime? Date { get; set; }
        public double? MarksScored { get; set; }

    }
    public class Marksanddate
    {
        public int Day { get; set; }

        public double? Marks { get; set; }
    }


    public class GetMarks : SujectWiseMarks
    {
        ireadytodayEntities2 dbcontext = new ireadytodayEntities2();
        public List<Marksanddate> GetMarksSubjectWise(int? StudentID)
        {


            List<SujectWiseMarks> newlist = new List<SujectWiseMarks>();


            //DateTime date = Convert.ToDateTime("2019-04-05 00:00:00.000");
            DateTime date = DateTime.Now;
            var ScoreList = (from i in dbcontext.Stu_Result
                             where (i.UserID == StudentID && i.CreatedDate.Value.Month == date.Month && i.CreatedDate.Value.Year == date.Year)
                             group i by new { i.SubjectID, i.UnitID, i.CreatedDate }
            into grp
                             select new SujectWiseMarks
                             {
                                 SubjectID = grp.Key.SubjectID,
                                 UnitID = grp.Key.UnitID,
                                 Date = grp.Key.CreatedDate,
                                 MarksScored = grp.Max(a => a.ScoredMarks)
                             }).ToList();

            var marks = (from i in ScoreList orderby i.Date ascending
                         group i by new { i.Date } into list  
                         select new SujectWiseMarks
                         {
                             //SubjectID = list.Key.SubjectID,
                             Date = list.Key.Date,
                             MarksScored = list.Average(a => a.MarksScored)
                         }).ToList();


            var marks1 = marks.Select(a => new Marksanddate
            {
                Day = a.Date.Value.Day,
                Marks = a.MarksScored
            }).ToList();

            return marks1;
        }




    }
}