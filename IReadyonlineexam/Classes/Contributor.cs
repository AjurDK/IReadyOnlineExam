using IReadyonlineexam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IReadyonlineexam.Classes
{
    public class Contributor
    {
        public ireadytodayEntities2 dbcontext = new ireadytodayEntities2();

    }
    public class GetActivity : Contributor
    {
        public bool Active;
        public GetActivity(int ProfileID)
        {
             Active = dbcontext.ContributorProfiles.Where(contributor => contributor.ID == ProfileID && contributor.Status == true).Any();           
        }
    }

    public class GetProfileID : Contributor
    {
        public int ProfileID;
        public GetProfileID(string Email)
        {
            ProfileID = dbcontext.ContributorProfiles.Where(contributor => contributor.Email == Email).Select(u => u.ID).FirstOrDefault();
        }
    }

    public class CheckProfile : Contributor
    {
        public bool Exist;
        public CheckProfile(string Email)
        {
            Exist = dbcontext.ContributorProfiles.Where(contributor => contributor.Email == Email).Any();
        }
    }

    public class MyQuizDetail : Contributor
    {
        public List<Quiz> loadQuiz()
        {
            List<Quiz> _quiz = new List<Quiz>();
            var list = dbcontext.Quizs.Where(q => q.CourseID != 0 && q.SubjectID != 0 && q.UnitID != 0).ToList();
            if (list != null)
            {
                return list;
            }
            return list;
        }

        public List<Question> loadQuestion(int _QuizID)
        {
            List<Question> _quiz = new List<Question>();
            var list = dbcontext.Questions.Where(q => q.QuizID==_QuizID).ToList();
            if (list != null)
            {
                return list;
            }
            return list;
        }

        public List<Answer> loadAnswer(int _QuestionID)
        {
            List<Answer> _quiz = new List<Answer>();
            var list = dbcontext.Answers.Where(q => q.QuestionID == _QuestionID).ToList();
            if (list != null)
            {
                return list;
            }
            return list;
        }

    }

}