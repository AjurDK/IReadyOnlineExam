using AngularMVC.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularMVC.BLL
{
  
        public class QuizModel
        {
            public List<Quiz> Quiz { get; set; }
            public List<Config> Config { get; set; }
            public IQueryable<Question> Question { get; set; }

        }

        public class Config
        {
            public bool ShuffleQuestions { get; set; }
            public bool ShowPager { get; set; }
            public bool Allowback { get; set; }
            public bool Automove { get; set; }
        }

        public class QuestionType
        {
            public int Id { get; set; }
            public string Name { get; set; }// Type of questions like multiple choice etc
            public bool IsActive { get; set; }
        }



    }

