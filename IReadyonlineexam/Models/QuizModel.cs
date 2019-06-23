using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IReadyonlineexam.Models;

namespace IReadyonlineexam.Models
{
    public class QuizModel
    {
        public Quiz quiz {get; set; }
        public List<Config> Config { get; set; }
        public object questions { get; set; }
        
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