using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularMVC.Model.Entities
{
    public class Answer
    {
        public int AnswerID { get; set; }
        public string AnswerText { get; set; }
        public int? QuestionID { get; set; }
        public bool? IsAnwer { get; set; }

        public virtual Question Question { get; set; }
    }
}
