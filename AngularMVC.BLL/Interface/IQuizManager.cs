using System;
using AngularMVC.Model.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularMVC.BLL.Interface
{
     public interface IQuizManager
    {
        List<Quiz> GetQuizs();
        List<Question> GetQuestions();
        List<Answer> GetAnswer();

    }
}
