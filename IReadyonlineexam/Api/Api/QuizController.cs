
using IReadyonlineexam.Classes;
using IReadyonlineexam.Models;
using IReadyonlineexam.Models.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace IReadyonlineexam.Api
{
    public class QuizController : ApiController
    {
        // QuizManager quiz = new QuizManager();
        public ireadytodayEntities2 dbcontext = new ireadytodayEntities2();

        public object GetQuiz()
        {
            ireadytodayEntities2 dbcontext = new ireadytodayEntities2();
            var q = dbcontext.Quizs.Select(a => new { Id = a.QuizID, name = a.QuizName }).ToList();
            return q;
        }

        // GET: api/Quiz
        public QuizModel Get(int Id)
        {
            ireadytodayEntities2 dbcontext = new ireadytodayEntities2();
            
            List<Config> tempcon = new List<Config>
            {
                new Config() { ShuffleQuestions = true, Automove = true, Allowback = false, ShowPager = false }
            };
            List<QuestionType> tempqtype = new List<QuestionType>
            {
                new QuestionType() { Id = 1, IsActive = true, Name = "MCQ" }
            };


            //System.Data.Entity.DbSet<Question> s = dbcontext.Questions;
            //IQueryable<Question> questions = dbcontext.Questions.Where(q => q.QuizID == 1)
            //    .Select(a => new Question
            //    {
            //        QuizID = a.QuizID,
            //        QuestionID = a.QuestionID,
            //        QuestionText = a.QuestionText,
            //        Answers = a.Answers.Where(q => q.QuestionID == a.QuestionID).Select(c => new Answer
            //        {
            //            AnswerID = c.AnswerID,
            //            AnswerText = c.AnswerText,
            //            IsAnwer = c.IsAnwer,
            //            QuestionID = a.QuestionID,
            //        }).ToList()
            //    }).AsQueryable();

            

            var q1 = dbcontext.Quizs.Where(a => a.QuizID == Id).FirstOrDefault();
            var qq = q1.Questions.Select(a => new
            {
                Id = a.QuestionID,
                Name = a.QuestionText,
                QuestionTypeId = 1,
                Options = a.Answers.Select(b => new
                {
                    Id = b.AnswerID,
                    QuestionId = a.QuestionID,
                    Name = b.AnswerText,
                    IsAnswer = b.IsAnwer ?? false
                })
            }).Take(10).ToList();
            dbcontext.Configuration.LazyLoadingEnabled = false;
            QuizModel qz = new QuizModel()
            {
                quiz = q1,
                Config = tempcon,
                questions = qq.OrderBy(x=> Guid.NewGuid()).Take(10).ToList()
            };

            //var js = JsonConvert.SerializeObject(qz);


            return qz;

            // return quiz.GetQuizs();
        }


        public int GetSession()
        {
            var user = User.Identity.Name;           
            int quizid = Int32.Parse(HttpContext.Current.Session["QuizId"].ToString());
            return quizid;
        }



        public List<Quiz> Gets(int subject,int unit)
        {
            ireadytodayEntities2 dbcontext = new ireadytodayEntities2();
            dbcontext.Configuration.LazyLoadingEnabled = false;
            return dbcontext.Quizs.Where(x=>x.SubjectID==subject && x.UnitID==unit).ToList();
        }

        

        public List<Course_master>GetCourse()
        {
            ireadytodayEntities2 dbcontext = new ireadytodayEntities2();                       
            var usercourse = (from a in dbcontext.StudentProfiles where a.EmailID == User.Identity.Name select a.Course).FirstOrDefault();
            dbcontext.Configuration.LazyLoadingEnabled = false;
            return dbcontext.Course_master.Where(x => x.Course_ID == usercourse).ToList();
        }

        public List<Semester> GetSemester()
        {
            ireadytodayEntities2 dbcontext = new ireadytodayEntities2();            
            var usercourse = (from a in dbcontext.StudentProfiles where a.EmailID == User.Identity.Name select a.Course).FirstOrDefault();
            dbcontext.Configuration.LazyLoadingEnabled = false;
            return dbcontext.Semesters.Where(a => a.CourseID == usercourse).ToList();
        }

        public List<Subject_Master> GetSubject()

        {
            ireadytodayEntities2 dbcontext = new ireadytodayEntities2();
            dbcontext.Configuration.LazyLoadingEnabled = false;
            var usercourse = (from a in dbcontext.StudentProfiles where a.EmailID == User.Identity.Name select a.Course).FirstOrDefault(); 
                            var sub = dbcontext.Subject_Master.Where(a => a.Course == usercourse).Select(a=>a).ToList();

            //return dbcontext.Subject_Master.ToList();
            return sub;
        }

        public List<UnitMaster> GetUnit()
        {
            ireadytodayEntities2 dbcontext = new ireadytodayEntities2();
            dbcontext.Configuration.LazyLoadingEnabled = false;
            return dbcontext.UnitMasters.ToList();

        }

        public JsonResult SaveScore(ScoreModel model)
        {
            JsonResult jsonResult = new JsonResult()
            {
                Success = false
            };
            try
            {
                // 1.2 get student_id
                int totalmarks = 100;
                var username = User.Identity.Name;
                StudentProfile student = dbcontext.StudentProfiles.Where(x => x.EmailID.ToLower() == username).SingleOrDefault();
                var StudentCourse = (from a in dbcontext.StudentProfiles.Where(x => x.EmailID == username) select a.Course ).SingleOrDefault();
                //var SemesterID = (from b in dbcontext.Subject_Master.Where(x => x.SubjectID != 0) select b.Semester).SingleOrDefault(); 
              
                    Stu_Result stu_Result = new Stu_Result()
                    {
                        SemID = 124,
                        SubjectID = model.SubjectID,
                        ScoredMarks = model.ScoredMarks,
                        TotalMarks = totalmarks,
                        UnitID = model.UnitID,
                        CourseID = StudentCourse,
                        UserID = student.Student_ID,
                        No_Attempts=1,
                        CreatedDate = DateTime.Now
                    };

                    dbcontext.Stu_Result.Add(stu_Result);

                    if (dbcontext.SaveChanges() > 0)

                        jsonResult.Success = true;
              }
            catch (Exception ex)
            {
               // SystemLogManager.Add("Api|Notify|Get", ex.Message, ex.InnerException.Message ?? "-");
            }

            return jsonResult;
        }

        public JsonResult SaveRating(RatingModel Model)
        {
            JsonResult jsonResult = new JsonResult()
            {
                Success = false
            };

            try
            {
                var username = User.Identity.Name;
                StudentProfile student = dbcontext.StudentProfiles.Where(x => x.EmailID.ToLower() == username).SingleOrDefault();
                var StudentCourse = (from a in dbcontext.StudentProfiles.Where(x => x.EmailID == username) select a.Course).SingleOrDefault();

                StudentCourseRating sc = new StudentCourseRating()
                {
                    StudentID = student.Student_ID,
                    CourseID = StudentCourse,
                    Rating = Model.Rating,
                    CreatedDate = DateTime.Now
                };

                dbcontext.StudentCourseRatings.Add(sc);

                if (dbcontext.SaveChanges() > 0)

                    jsonResult.Success = true;

            }
            catch (Exception ex)
            {
                // SystemLogManager.Add("Api|Notify|Get", ex.Message, ex.InnerException.Message ?? "-");
            }

            return jsonResult;
        }        



    }
}
