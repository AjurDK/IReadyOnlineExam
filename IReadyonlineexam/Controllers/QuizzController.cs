using IReadyonlineexam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IReadyonlineexam.Controllers
{
    public class QuizzController : Controller
    {
        ireadytodayEntities2 dbContext = new ireadytodayEntities2();

        // GET: Quizz
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetUser(UserVM user)
        {
            UserVM userConnected = dbContext.StudentProfiles.Where(u => u.Name == user.FullName)
                                         .Select(u => new UserVM
                                         {
                                             UserID = u.Student_ID,
                                             FullName = u.Name,
                                             ProfilImage = u.USN,
                                         }).FirstOrDefault();

            if (userConnected != null)
            {
                Session["UserConnected"] = userConnected;
                return RedirectToAction("SelectQuizz");
            }
            else
            {
                ViewBag.Msg = "Sorry : user is not found !!";
                return View();
            }

        }

        [HttpGet]
        public ActionResult SelectQuizz()
        {

            var Course = dbContext.Course_master.OrderBy(a => a.Course_ID).ToList().Select(b => new SelectListItem
            {
                Value = b.Course_ID.ToString(),
                Text = b.Name
            }).ToList();

            var Subject = dbContext.Subject_Master.OrderBy(a => a.SubjectID).ToList().Select(b => new SelectListItem
            {
                Value = b.SubjectID.ToString(),
                Text = b.Name
            }).ToList();

            var Unit = dbContext.UnitMasters.OrderBy(a => a.Uid).ToList().Select(b => new SelectListItem
            {
                Value = b.Uid.ToString(),
                Text = b.UnitName
            }).ToList();

            ViewBag.Course = Course;
            ViewBag.Subject = Subject;
            ViewBag.Unit = Unit;
            return View();
        }

        [HttpPost]
        public ActionResult SelectQuizz(QuizVM quiz)
        {
            TempData["CourseID"] = quiz.CourseID;
            TempData["SubjectID"] = quiz.SubjectID;
            TempData["UnitID"] = quiz.UnitID;
            return RedirectToAction("StartQuiz");
        }

        [HttpGet]
        public ActionResult StartQuiz()
        {
            Question_details q = new Question_details();
            int CourseID = Int32.Parse(TempData["CourseID"].ToString());
            int SubjectID = Int32.Parse(TempData["SubjectID"].ToString());
            int UnitID = Int32.Parse(TempData["UnitID"].ToString());
            try
            {
                if (TempData["qid"] == null)
                {
                    q = dbContext.Question_details.First(x => x.Course_ID == CourseID && x.SubjectID == SubjectID && x.Uid == UnitID);
                    TempData["qid"] = ++q.Q_no;
                }
                else
                {
                    int qid = Int32.Parse(TempData["qid"].ToString());
                    q = dbContext.Question_details.Where(x => x.Course_ID == CourseID && x.SubjectID == SubjectID && x.Uid == UnitID && x.Q_no == qid).SingleOrDefault();
                    TempData["qid"] = ++q.Q_no;

                }
            }
            catch (Exception)
            {
                return RedirectToAction("SelectQuizz");
            }
            TempData.Keep();
            return View(q);
        }
        [HttpPost]
        public ActionResult StartQuiz(QuizVM model)
        {

            return RedirectToAction("StartQuiz");
        }
    }
}