using System;
using AngularMVC.Model.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularMVC.Model.Entities
{
    public class Question
    {
        public Question()
        {
            this.Answers = new HashSet<Answer>();
        }

        public int QuestionID { get; set; }
        public string QuestionText { get; set; }
        public Nullable<int> QuizID { get; set; }

        // me
        public int Delay { get; set; }
        public QuestionType Type { get; set; }


        public virtual ICollection<Answer> Answers { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}
