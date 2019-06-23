using AngularMVC.BLL.Interface;
using AngularMVC.DAL;
using AngularMVC.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularMVC.BLL
{
    public class QuizManager : IQuizManager
    {
        DbEntities db = new DbEntities();
        public QuizManager()
        {

        }
        public List<Answer> GetAnswer()
        {
            return db.Answers.ToList();
        }

        public List<Question> GetQuestions()
        {
            return db.Questions.ToList();
        }

        public List<Quiz> GetQuizs()
        {
            return db.Quizs.Include("Questions").ToList();
        }

    }
}
