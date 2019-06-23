using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularMVC.Model.Entities
{
   public class QuizLog
    {
        public long ID { get; set; }
        public Guid UserID { get; set; }
        public int QuestionID { get; set; }
        public int AnswerID { get; set; }
        public DateTime PostTime { get; set; }

        public virtual Answer Answers { get; set; }
        public virtual Question Question { get; set; }
        public virtual User User { get; set; }
    }
}
