using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IReadyonlineexam.Models;
using IReadyonlineexam.Classes;
using System.Web.Mvc;
using AngularMVC.Model.Entities;

namespace IReadyonlineexam.Classes
{
    public class GetCalculatePerform
    {


      public  ireadytodayEntities2 dbcontext = new ireadytodayEntities2();

        public UserRankObject tempobj { get; set; }
        public UserUnitRankObject temp { get; set; }

        //********************************************************Performance & Rank Calculation********************************************


        public GetCalculatePerform(int? Student_ID, int? CourseID, int? SemID)
        {
            try
            {
                var scoreCard = dbcontext.Stu_Result
                                .Where(u => u.CourseID == CourseID && u.SemID == SemID && u.UserID != null)
                                .ToList()
                                .GroupBy(u => u.UserID)
                                .OrderByDescending(grp => grp.Average(u => u.ScoredMarks))
                                .Select((grp, i) => new UserRankObject
                                {
                                    UserId = grp.Key,
                                    Rank = i + 1,
                                    AverageScore = grp.Average(u => u.ScoredMarks)
                                }).ToList().Single(u => u.UserId == Student_ID);
                this.tempobj = scoreCard;
            }
            catch (InvalidOperationException)
            {
                UserRankObject scorecard = new UserRankObject
                {
                    UserId = Student_ID,
                    Rank = 0,
                    AverageScore = 0
                };
                this.tempobj = scorecard;

            }



        }
        public GetCalculatePerform(int? StudentID, int? CourseID, int? SemID, int? SubjectID)
        {
            var unitscore = dbcontext.Stu_Result.Where(u => u.CourseID == CourseID && u.SemID == SemID && u.SubjectID == SubjectID)
                .ToList()
                .GroupBy(u => u.UserID)
                .OrderByDescending(g => g.Average(u => u.ScoredMarks))
                .Select((g, i) => new UserUnitRankObject
                {
                    UserId = g.Key,
                    Rank = i + 1,
                    AverageScore = g.Average(u => u.ScoredMarks)
                }).ToList().Single(u => u.UserId == StudentID);


            this.temp = unitscore;
        }

    }
}
    

