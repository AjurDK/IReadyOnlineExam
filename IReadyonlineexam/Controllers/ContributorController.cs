using IReadyonlineexam.Classes;
using IReadyonlineexam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IReadyToday;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.Compression;
using Ionic.Zip;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Web.Script.Serialization;
using System.Web.WebPages;
using System.Data.Entity.Validation;

namespace IReadyonlineexam.Controllers
{
    public class ContributorController : Controller
    {
        public ireadytodayEntities2 dbcontext = new ireadytodayEntities2();
        // GET: Contributor

       
        [Authorize(Roles = "Contributor")]
        public ActionResult Index()
        {
            var QueryNotification = dbcontext.Stu_Additionalquestionrequest.Where(i => i.ADQID != 0).ToList();
            ViewBag.QueryN = QueryNotification;



            var StudyMaterialsNotification = dbcontext.StudyMaterials.Where(i => i.SM_Name != null).Take(3).ToList();
            ViewBag.studymN = StudyMaterialsNotification;
            //StudyMaterials End


            //Additional Question Request Begin
            var AddQueReq = (from Qreq in dbcontext.Stu_Additionalquestionrequest
                             join user in dbcontext.StudentProfiles on Qreq.ProfileId equals user.Student_ID
                             join course in dbcontext.Course_master on Qreq.Course equals course.Course_ID
                             //where user.EmailID == User.Identity.Name
                             select new QuestionRequestModel
                             {
                                 StudentName = user.Name,
                                 Course = course.Name,
                                 Update = (bool)Qreq.Updated == true ? "Updated" : "Pending",
                             }).Take(2).ToList();
            ViewBag.QueReq = AddQueReq;
            //Additional Question Request End




            var jobdetails = (from a in dbcontext.Job_Master.Where(x => x.JobID != 0) select a).ToList();
            ViewBag.jobdetails = jobdetails;
            UsersCount();
            return View();
        }

        //Course Binding
        private void Bindcourse()
        {
            List<Course_master> c = dbcontext.Course_master.Where(x => x.Course_ID != 0).ToList();
            Course_master course = new Course_master
            {
                Name = "----Please select Category----",
                Course_ID = 0
            };
            c.Insert(0, course);
            SelectList selectcourse = new SelectList(c, "Course_ID", "Name", 0);
            ViewBag.selectedcourse = selectcourse;
        }


        public void UsersCount()
        {
            var dt = DateTime.Now.AddDays(-1);
            string st = dt.ToString("yyyy-mm-dd h:mm tt");
            ViewBag.NewRegistered = dbcontext.StudentProfiles.Where(a => a.CreatedDate > dt).Count();
            ViewBag.Queriescount = dbcontext.Stu_Additionalquestionrequest.Count();
            ViewBag.ContributorCount = dbcontext.ContributorProfiles.Count();
            ViewBag.StudentsCount = dbcontext.StudentProfiles.Count();
        }

        //Unit Name Binding
        private void BindUnit()
        {
            List<UnitMaster> u = dbcontext.UnitMasters.Where(x => x.Uid != 0).ToList();
            UnitMaster unit = new UnitMaster
            {
                UnitName = "----Please select Unit-----",
                Uid = 0
            };
            u.Insert(0, unit);
            SelectList selectunit = new SelectList(u, "Uid", "UnitName", 0);
            ViewBag.selectedunit = selectunit;
        }

        //Course Name Binding
        private void BindCourse()
        {
            List<Course_master> c = dbcontext.Course_master.Where(x => x.Course_ID != 0).ToList();
            Course_master course = new Course_master
            {
                Name = "----Please select Category----",
                Course_ID = 0
            };
            c.Insert(0, course);
            SelectList selectcourse = new SelectList(c, "Course_ID", "Name", 0);
            ViewBag.selectedcourse = selectcourse;
        }

        //Subject Name Binding
        private void BindSubjectname()
        {
            List<Subject_Master> s = dbcontext.Subject_Master.Where(x => x.SubjectID != 0).ToList();
            Subject_Master sub = new Subject_Master
            {
                Name = "----Please select Course----",
                SubjectID = 0
            };
            s.Insert(0, sub);
            SelectList selectsubject = new SelectList(s, "SubjectID", "Name", 0);
            ViewBag.selectedsubject = selectsubject;
        }

        // Year Binding 
        private void BindYear()
        {
            var u_list = dbcontext.YearMasters.OrderBy(a => a.YearID).ToList().Select(b => new SelectListItem { Value = b.YearID.ToString(), Text = b.Year.ToString() }).ToList();
            ViewBag.selectedyear = u_list;
        }

        //Semester Name Binding
        private void BindSemester()
        {
            List<Semester_master> c = dbcontext.Semester_master.Where(x => x.SemID != 0).ToList();
            Semester_master sem = new Semester_master
            {
                Name = "----please select Semester----",
                SemID = 0
            };
            c.Insert(0, sem);
            SelectList selectsem = new SelectList(c, "SemID", "Name ", 0);
            ViewBag.selectedsem = selectsem;
        }


        public void BindState()
        {
            var state = dbcontext.Course_master.ToList();
            List<SelectListItem> li = new List<SelectListItem>
            {
                new SelectListItem { Text = "----Select Course----", Value = "0" }
            };

            foreach (var m in state)
            {


                li.Add(new SelectListItem { Text = m.Name, Value = m.Course_ID.ToString() });
                ViewBag.state = li;

            }
        }

        public JsonResult GetCity(int id)
        {
            var ddlCity = dbcontext.Semesters.Where(x => x.CourseID == id).ToList();
            List<SelectListItem> licities = new List<SelectListItem>
            {
                new SelectListItem { Text = "--Select Semester--", Value = "0" }
            };
            if (ddlCity != null)
            {
                foreach (var x in ddlCity)
                {
                    licities.Add(new SelectListItem { Text = x.Name, Value = x.SemID.ToString() });
                }
            }
            return Json(new SelectList(licities, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        public JsonResult GetSubject(int id)
        {
            var ddlSubject = dbcontext.Subject_Master.Where(x => x.Semester == id).ToList();
            List<SelectListItem> lisubjects = new List<SelectListItem>
            {
                new SelectListItem { Text = "--Select Subject--", Value = "0" }
            };
            if (ddlSubject != null)
            {
                foreach (var x in ddlSubject)
                {
                    lisubjects.Add(new SelectListItem { Text = x.Name, Value = x.SubjectID.ToString() });
                }
            }
            return Json(new SelectList(lisubjects, "Value", "Text", JsonRequestBehavior.AllowGet));
        }


        public ActionResult CreateProfile()
        {
            bool ProfileExists = new CheckProfile(User.Identity.Name).Exist;
            if (!ProfileExists)
            {
                string tempUsername = User.Identity.Name.ToString();
                var pdata = (from p in dbcontext.AspNetUsers
                             where p.UserName == tempUsername
                             select new { UserName = tempUsername }).FirstOrDefault();

                ContributorProfile ptemp = new ContributorProfile
                {
                    Email = pdata.UserName
                };
                return View(ptemp);
            }
            else
            {
                var ProfileData = dbcontext.ContributorProfiles.Where(contributor => contributor.Email == User.Identity.Name).First();
                return View(ProfileData);
            }

        }

        [HttpPost]
        public ActionResult CreateProfile(ContributorProfile _cprofile, HttpPostedFileBase FileUpload)
        {
            string filesnames = "default.jpg";
            if (FileUpload != null)
            {
                string savedFileName = DateTime.Now.ToString("MMddHHmmssfff") + Path.GetFileName(FileUpload.FileName);
                FileUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/Profile"), savedFileName)); // Save the file
                filesnames = savedFileName;
            }

            bool ProfileExists = new CheckProfile(User.Identity.Name).Exist;
            if (!ProfileExists)
            {

                BindCourse();
                ContributorProfile insert = new ContributorProfile()
                {
                    Image = filesnames,
                    Name = _cprofile.Name,

                    DOB = _cprofile.DOB,
                    Email = _cprofile.Email,
                    CourseID = _cprofile.CourseID,
                    Current_Position = _cprofile.Current_Position,
                    Experience = _cprofile.Experience,
                    Institution_Name = _cprofile.Institution_Name,
                    PhoneNo = _cprofile.PhoneNo,
                    Skills = _cprofile.Skills,
                    Website = _cprofile.Website,
                    Address = _cprofile.Address,
                    Status = true,
                    CreatedDate = DateTime.Now
                };
                Session["UserID"] = insert.ID;
                dbcontext.ContributorProfiles.Add(insert);
            }
            else
            {
                BindCourse();
                var ProfileID = new GetProfileID(User.Identity.Name).ProfileID;
                ContributorProfile update = dbcontext.ContributorProfiles.Find(ProfileID);
                if (update.ID == ProfileID)
                {
                    update.Image = filesnames;
                    update.Name = _cprofile.Name;

                    update.DOB = _cprofile.DOB;
                    update.Email = _cprofile.Email;
                    update.CourseID = _cprofile.CourseID;
                    update.Current_Position = _cprofile.Current_Position;
                    update.Experience = _cprofile.Experience;
                    update.Institution_Name = _cprofile.Institution_Name;
                    update.PhoneNo = _cprofile.PhoneNo;
                    update.Skills = _cprofile.Skills;
                    update.Website = _cprofile.Website;
                    update.Address = _cprofile.Address;
                    update.ModifiedDate = DateTime.Now;
                }
                Session["UserID"] = update.ID;
                dbcontext.ContributorProfiles.AddOrUpdate(update);
            }

            int alert = dbcontext.SaveChanges();
            if (alert > 0)
            {
                TempData["AlertBox"] = "<script>alert('Sucessfully Created Profile');</script>";
            }
            return RedirectToAction("Redirect", "Contributor");
        }

        [HttpGet]
        public ActionResult ProfileView()
        {
           var details = (from a in dbcontext.ContributorProfiles
                           join b in dbcontext.Course_master on a.CourseID equals b.Course_ID
                           where a.Email == User.Identity.Name
                           select new ContributorProfileModel
                           {
                               Name = a.Name,
                               Email = a.Email,
                               DOB = a.DOB,
                               PhoneNo = a.PhoneNo,
                               Image = a.Image,
                               Gender = a.Gender,
                               Address = a.Address,
                               Website = a.Website,
                               Current_Position = a.Current_Position,
                               Institution_Name = a.Institution_Name,
                               Skills = a.Skills,
                               CourseName = b.Name,
                               Experience = a.Experience,
                               CreatedDate = a.CreatedDate

                           }).FirstOrDefault();
            return View(details);



        }

        public ActionResult Redirect()
        {
            int ProfileID = Convert.ToInt32(Session["UserID"]);
            bool activity = new GetActivity(ProfileID).Active;
            if (!activity)
            {
                Session["Text1"] = "You're Currently Inactive";
                Session["Text2"] = "Please Contact Admin";
                return RedirectToAction("NotFound", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Contributor");
            }
        }



        [HttpPost]
        public ActionResult StuSyllabus(studentsyllabu model)
        {
            BindState();
            BindUnit();
            BindSubjectname();
            studentsyllabu syl = new studentsyllabu()
            {
                ssid = model.ssid,
                SubjectID = model.SubjectID,
                Uid = model.Uid,
                Name = model.Name,
                UnitDescription = model.UnitDescription,
                CreatedDate = DateTime.Now
            };
            dbcontext.studentsyllabus.Add(syl);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                TempData["result"] = "Record Inserted Successfully";
                ModelState.Clear();
            }
            else
            {
                TempData["result"] = "Something went wrong!";
            }
            ViewBag.syllabus = (from list in dbcontext.studentsyllabus
                                join a in dbcontext.Subject_Master on list.SubjectID equals a.SubjectID
                                join b in dbcontext.UnitMasters on list.Uid equals b.Uid
                                select new CourseModel
                                {
                                    Ssid = list.ssid,
                                    SubjectName = list.Name,
                                    UnitName = list.Name,
                                    ChapterName = list.Name,
                                    ChapterDesciption = list.UnitDescription,
                                    CreatedDate = list.CreatedDate

                                }).ToList();
            return RedirectToAction("StuSyllabus");
        }



        [HttpGet]
        public ActionResult StuSyllabus()
        {
            BindState();
            BindUnit();
            BindSubjectname();
            ViewBag.syllabus = (from list in dbcontext.studentsyllabus
                                join subject in dbcontext.Subject_Master on list.SubjectID equals subject.SubjectID
                                join unit in dbcontext.UnitMasters on list.Uid equals unit.Uid
                                select new CourseModel
                                {
                                    Ssid = list.ssid,
                                    SubjectName = subject.Name,
                                    UnitName = unit.UnitName,
                                    ChapterName = list.Name,
                                    ChapterDesciption = list.UnitDescription,
                                    CreatedDate = list.CreatedDate

                                }).OrderByDescending(x => x.CreatedDate).ToList();
            return View();
        }


        [HttpPost]
        public ActionResult EditStuSyllabus(studentsyllabu model)
        {
            studentsyllabu stusyllabusedit = dbcontext.studentsyllabus.Find(model.ssid);
            if (stusyllabusedit.ssid == model.ssid)
            {
                stusyllabusedit.ssid = model.ssid;
                stusyllabusedit.Name = model.Name;
                stusyllabusedit.UnitDescription = model.UnitDescription;

            };
            dbcontext.studentsyllabus.AddOrUpdate(stusyllabusedit);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {

                TempData["result"] = "Record edited Successfully!";
                ModelState.Clear();
            }
            else
            {
                TempData["result"] = "Please Try Again!";
            }

            return RedirectToAction("StuSyllabus");
        }


        [HttpGet]
        [Authorize(Roles = "Contributor")]
        public async Task<ActionResult> EditStuSyllabus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            studentsyllabu stusyllabusedit = await dbcontext.studentsyllabus.FindAsync(id);
            if (stusyllabusedit == null)
            {
                return HttpNotFound();
            }
            return View(stusyllabusedit);

        }


        [HttpGet]
        public async Task<ActionResult> DeleteStuSyllabus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            studentsyllabu studentsyllabusdelete = await dbcontext.studentsyllabus.FindAsync(id);
            if (studentsyllabusdelete == null)
            {
                return HttpNotFound();
            }
            return View(studentsyllabusdelete);

        }

        [HttpPost]
        public ActionResult DeleteStuSyllabus(studentsyllabu model)
        {
            studentsyllabu studentsyllabudelete = dbcontext.studentsyllabus.Find(model.ssid);
            if (studentsyllabudelete.ssid == model.ssid)
            {
                studentsyllabudelete.ssid = model.ssid;
                studentsyllabudelete.Name = model.Name;
                studentsyllabudelete.UnitDescription = model.UnitDescription;
            }
            dbcontext.studentsyllabus.Remove(studentsyllabudelete);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {

                TempData["result"] = "Deleted Successfully!";

            }
            else
            {
                TempData["result"] = "Please Try Again!";
            }

            return RedirectToAction("StuSyllabus");
        }


        [HttpGet]
        public ActionResult Jobmaster()
        {
            BindYear();
            BindCourse();
            ViewBag.list = (from job in dbcontext.Job_Master
                            join yr in dbcontext.YearMasters on job.year equals yr.YearID
                            join course in dbcontext.Course_master on job.Course equals course.Course_ID

                            select new CourseModel
                            {
                                JobID = job.JobID,
                                YearName = yr.Year,
                                CourseName = course.Name,

                                Designation = job.Designation,
                                CompanyName = job.Company_Name,
                                JobDescription = job.Description,
                                InterviewDate = job.ScheduledDate,
                                InterviewLocation = job.Location,
                                CreatedDate = job.CraetedDate
                            }).OrderByDescending(x => x.CreatedDate).ToList();
            return View();
        }



        [HttpPost]
        public ActionResult Jobmaster(Job_Master model, string interviewdate)
        {
            var IntDate = DateTime.Parse(interviewdate);
            BindYear();
            BindCourse();
            Job_Master jm = new Job_Master()
            {
                year = model.year,
                Course = model.Course,

                Designation = model.Designation,
                Company_Name = model.Company_Name,
                Description = model.Description,
                ScheduledDate = IntDate,
                Location = model.Location,
                CraetedDate = DateTime.Now
            };
            dbcontext.Job_Master.Add(jm);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                TempData["result"] = "Record Inserted Successfully";
                ModelState.Clear();
            }
            else
            {
                TempData["result"] = "Something went wrong!";
            }
            ViewBag.list = (from job in dbcontext.Job_Master
                            join yr in dbcontext.YearMasters on job.year equals yr.YearID
                            join course in dbcontext.Course_master on job.Course equals course.Course_ID

                            select new CourseModel
                            {
                                JobID = job.JobID,
                                YearName = yr.Year,
                                CourseName = course.Name,

                                Designation = job.Designation,
                                CompanyName = job.Company_Name,
                                JobDescription = job.Description,
                                InterviewDate = job.ScheduledDate,
                                InterviewLocation = job.Location,
                                CreatedDate = job.CraetedDate
                            }).OrderByDescending(x => x.CreatedDate).ToList();

            return RedirectToAction("Jobmaster");
        }




        [HttpPost]
        public ActionResult EditJobUploaded(Job_Master model)

        {
            Job_Master JobEdit = dbcontext.Job_Master.Find(model.JobID);
            if (JobEdit.JobID == model.JobID)
            {
                JobEdit.JobID = model.JobID;
                JobEdit.Designation = model.Designation;
                JobEdit.Company_Name = model.Company_Name;
                JobEdit.Description = model.Description;
                JobEdit.Location = model.Location;

            }
            dbcontext.Job_Master.AddOrUpdate(JobEdit);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                TempData["result"] = "Record Edited Successfully";
            }
            else
            {
                TempData["result"] = "Something went wrong!";
            }
            return RedirectToAction("Jobmaster");
        }

        [HttpPost]
        public ActionResult DeleteJobUploaded(Job_Master model)

        {
            Job_Master JobDelete = dbcontext.Job_Master.Find(model.JobID);
            if (JobDelete.JobID == model.JobID)
            {
                JobDelete.JobID = model.JobID;
                JobDelete.Designation = model.Designation;
                JobDelete.Company_Name = model.Company_Name;
                JobDelete.Description = model.Description;
                JobDelete.Location = model.Location;

            }
            dbcontext.Job_Master.Remove(JobDelete);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                TempData["result"] = "Record Deleted Successfully";
            }
            else
            {
                TempData["result"] = "Something went wrong!";
            }
            return RedirectToAction("Jobmaster");
        }

     
        [HttpGet]
        public async Task<ActionResult> EditJobUploaded(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job_Master JobEdit = await dbcontext.Job_Master.FindAsync(id);
            if (JobEdit == null)
            {
                return HttpNotFound();
            }
            return View(JobEdit);

        }

        [Authorize(Roles = "Contributor")]
        [HttpGet]
        public async Task<ActionResult> DeleteJobUploaded(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job_Master JobDelete = await dbcontext.Job_Master.FindAsync(id);
            if (JobDelete == null)
            {
                return HttpNotFound();
            }
            return View(JobDelete);

        }

        /// <summary>
        /// Contributor Quiz Section Begin
        /// </summary>
        /// <returns></returns>


        /// <summary>
        /// Quiz Add, List,Complete Details, Delete------------------------Begin
        /// </summary>

        public void UnitList()
        {
            List<UnitMaster> unit = dbcontext.UnitMasters.Where(x => x.Uid != 0).ToList();
            UnitMaster u = new UnitMaster();
            u.UnitName = "--Select Unit--";
            u.Uid = 0;
            unit.Insert(0, u);
            SelectList unitdata = new SelectList(unit, "Uid", "UnitName", 0);
            ViewBag.UnitDDL = unitdata;
        }

        public void BindQuiz()
        {
            List<Quiz> quiz = dbcontext.Quizs.Where(id => id.QuizID != 0).ToList();
            Quiz q = new Quiz
            {
                QuizID = 0,
                QuizName = "--Select Quiz--"
            };
            quiz.Insert(0, q);
            SelectList quizdata = new SelectList(quiz, "QuizID", "QuizName", 0);
            ViewBag.QuizList = quizdata;
        }

        public void QuizBindCourse()
        {
            var course = dbcontext.Course_master.ToList();
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "--Select Course--", Value = "0" });

            foreach (var m in course)
            {
                li.Add(new SelectListItem { Text = m.Name, Value = m.Course_ID.ToString() });
                ViewBag.CourseDDL = li;
            }
        }

        public JsonResult GetSemester(int CourseID)
        {
            var ddlSem = dbcontext.Semesters.Where(x => x.CourseID == CourseID).ToList();
            List<SelectListItem> lisems = new List<SelectListItem>();

            lisems.Add(new SelectListItem { Text = "--Select Subject--", Value = "0" });
            if (ddlSem != null)
            {
                foreach (var x in ddlSem)
                {
                    lisems.Add(new SelectListItem { Text = x.Name, Value = x.SemID.ToString() });
                }
            }
            return Json(new SelectList(lisems, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        public JsonResult QuizGetSubject(int SemesterID)
        {
            var ddlSub = dbcontext.Subject_Master.Where(x => x.Semester == SemesterID).ToList();
            List<SelectListItem> lisubjects = new List<SelectListItem>();

            lisubjects.Add(new SelectListItem { Text = "--Select Subject--", Value = "0" });
            if (ddlSub != null)
            {
                foreach (var x in ddlSub)
                {
                    lisubjects.Add(new SelectListItem { Text = x.Name, Value = x.SubjectID.ToString() });
                }
            }
            return Json(new SelectList(lisubjects, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        
        [HttpGet]
        public ActionResult QuizCreate()
        {
            QuizBindCourse();
            UnitList();
            return View();
        }

        [HttpPost]
        public ActionResult QuizCreate(Quiz _quiz)
        {
            QuizBindCourse();
            UnitList();

            var CategoryCheck = dbcontext.Quizs.
                Where(q => q.CourseID == _quiz.CourseID && q.SemesterID == _quiz.SemesterID &&
                q.SubjectID == _quiz.SubjectID && q.UnitID == _quiz.UnitID).Any();
            if (CategoryCheck == true)
            {
                TempData["msg"] = "<script>alert('Category Already Exist! Add Questions ');</script>";
            }
            else
            {
                Quiz insert = new Quiz()
                {
                    CourseID = _quiz.CourseID,
                    SemesterID = _quiz.SemesterID,
                    SubjectID = _quiz.SubjectID,
                    UnitID = _quiz.UnitID,
                    QuizName = _quiz.QuizName,
                    CreatedDate = DateTime.Now
                };
                try
                {
                    dbcontext.Quizs.Add(insert);
                    int c = dbcontext.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }
            return View();
        }

        [ChildActionOnly]
        public ActionResult QuizLists()
        {
            var QuizList = (from quiz in dbcontext.Quizs
                            join course in dbcontext.Course_master on quiz.CourseID equals course.Course_ID
                            join subject in dbcontext.Subject_Master on quiz.SubjectID equals subject.SubjectID
                            join unit in dbcontext.UnitMasters on quiz.UnitID equals unit.Uid
                            select new MyQuizModel
                            {
                                QuizID = quiz.QuizID,
                                QuizName = quiz.QuizName,

                                CourseName = course.Name,
                                SubjectName = subject.Name,
                                UnitName = unit.UnitName,
                                CreatedDate = quiz.CreatedDate
                            }).OrderByDescending(d => d.CreatedDate).ToList();
            ViewBag.QuizList = QuizList;
            return PartialView(QuizList);
        }

        public ActionResult QuizDetails(int _QuizID)
        {
            var Quiz = dbcontext.Quizs.Single(p => p.QuizID == _QuizID);
            ViewBag.QuizName = Quiz.QuizName;
            ViewBag.QuizID = Quiz.QuizID;

            var Questions = dbcontext.Questions.Where(qu => qu.QuizID == _QuizID).OrderByDescending(d => d.CreatedDate).ToList();
            var _questionIDs = (from que in dbcontext.Questions where que.QuizID == _QuizID select que.QuestionID).ToList();

            var answers = dbcontext.Answers.Where(i => _questionIDs.Contains((int)i.QuestionID)).ToList();
            ViewBag.Answers = answers;

            return View(Questions);
        }

   
        [HttpGet]
        public ActionResult QuizDelete(int _QuizID)
        {
            var Quiz = dbcontext.Quizs.Single(p => p.QuizID == _QuizID);
            ViewBag.QuizName = Quiz.QuizName;
            ViewBag.QuizID = Quiz.QuizID;

            var Questions = dbcontext.Questions.Where(qu => qu.QuizID == _QuizID).OrderByDescending(d => d.CreatedDate).ToList();
            var _questionIDs = (from que in dbcontext.Questions where que.QuizID == _QuizID select que.QuestionID).ToList();

            var answers = dbcontext.Answers.Where(i => _questionIDs.Contains((int)i.QuestionID)).ToList();
            ViewBag.Answers = answers;

            return View(Questions);
        }

        [HttpPost]
        public ActionResult QuizDelete(Quiz model, int _QuizID)
        {
            Quiz _quiz = dbcontext.Quizs.Find(_QuizID);
            var _question = dbcontext.Questions.Where(q => q.QuizID == _QuizID)./*Select(id => id.QuestionID).*/ToList();
            if (_question != null)
            {
                var _questionIDs = (from que in dbcontext.Questions where que.QuizID == _QuizID select que.QuestionID).ToList();
                var _IsAnswers = dbcontext.Answers.Where(i => _questionIDs.Contains((int)i.QuestionID)).ToList();

                _IsAnswers.ForEach(x =>
                {
                    int? QID = x.QuestionID;
                    var _removeAnswer = dbcontext.Answers.Where(q => q.QuestionID == QID);
                    if (_removeAnswer != null)
                    {
                        dbcontext.Answers.RemoveRange(_removeAnswer);
                    }
                });
                dbcontext.Questions.RemoveRange(_question);
                dbcontext.Quizs.Remove(_quiz);
                dbcontext.SaveChanges();
                TempData["msg"] = "<script>alert('Sucessfully Deleted !');</script>";
            }
            else
            {
                dbcontext.Quizs.Remove(_quiz);
                dbcontext.SaveChanges();
                TempData["msg"] = "<script>alert('Sucessfully Deleted !');</script>";
            }
            //return View();
            return RedirectToAction("QuizCreate", "Contributor");
        }

        /// <summary>
        /// Quiz Add, List,Complete Details, Delete------------------------End
        /// </summary>



        /// <summary>
        /// Question Add, List,Complete Details, Delete------------------------Begin
        /// </summary>

        
        [HttpGet]
        public ActionResult QuestionInsert(int _quizID)
        {
            Session["_QuizID"] = _quizID;
            ViewBag._QuizID = _quizID;
            BindQuiz();
            return View();
        }

        [HttpPost]
        public ActionResult QuestionInsert(String QuestionText, List<MyAnswerModel> _answers)
        {
            int _quizID = Convert.ToInt32(Session["_QuizID"]);
            ViewBag._QuizID = _quizID;

            Question _insertQuestion = new Question()
            {
                QuizID = _quizID,
                QuestionText = QuestionText,
                CreatedDate = DateTime.Now
            };
            dbcontext.Questions.Add(_insertQuestion);
            dbcontext.SaveChanges();

            foreach (var i in _answers)
            {
                Answer _insertAnswer = new Answer()
                {
                    QuestionID = _insertQuestion.QuestionID,
                    AnswerText = i.AnswerName,
                    IsAnwer = i.isAnswer,
                    CreatedDate = DateTime.Now
                };
                dbcontext.Answers.Add(_insertAnswer);
            }
            int c = dbcontext.SaveChanges();
            if (c > 0)
            {
                ModelState.Clear();
            }
            TempData["msg"] = "<script>alert('Question Added Successfully');</script>";
            return View();
        }

        public ActionResult QuestionDelete(int _QuizID, int _QuestionID)
        {
            Question _question = dbcontext.Questions.Where(q => q.QuizID == _QuizID && q.QuestionID == _QuestionID).FirstOrDefault();

            var _questionIDs = (from que in dbcontext.Answers where que.QuestionID == _QuestionID select que.QuestionID).ToList();
            if (_questionIDs != null)
            {
                var _answers = dbcontext.Answers.Where(i => _questionIDs.Contains((int)i.QuestionID)).ToList();
                _answers.ForEach(x =>
                {
                    int? QID = x.QuestionID;
                    var _removeAnswer = dbcontext.Answers.Where(q => q.QuestionID == QID);
                    if (_removeAnswer != null)
                    {
                        dbcontext.Answers.RemoveRange(_removeAnswer);
                    }
                });
                dbcontext.Questions.Remove(_question);
                dbcontext.SaveChanges();
            }
            else
            {
                dbcontext.Questions.Remove(_question);
                dbcontext.SaveChanges();
            }
            return RedirectToAction("QuizDetails", new { _QuizID = _QuizID });
        }

        /// <summary>
        /// Question Add, List,Complete Details, Delete------------------------End
        /// </summary>

        //Excel Upload
        
        [HttpGet]
        public ActionResult QuestionUpload(int _quizID)
        {
            Session["_QuizID"] = _quizID;
            Session["_CategoryID"] = _quizID;
            return View();
        }

        [HttpPost]
       
        public ActionResult QuestionUpload(HttpPostedFileBase postedFile)
        {
            int _quizID = Convert.ToInt32(Session["_QuizID"]);
            Session["_CategoryID"] = _quizID;

            string filePath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/Questions/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                string conString = string.Empty;
                switch (extension)
                {
                    case ".xls": //Excel 97-03.
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 and above.
                        conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                }

                DataTable dt = new DataTable();
                conString = string.Format(conString, filePath);

                using (OleDbConnection connExcel = new OleDbConnection(conString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();
                        }
                    }
                }
                Question _insertQuestion = null;
                int skip4 = 1;
                //Insert records to database table.
                foreach (DataRow row in dt.Rows)
                {
                    if (row["AnswerText"].ToString().IsEmpty())
                    {
                        _insertQuestion = new Question()
                        {
                            QuizID = _quizID,
                            QuestionText = row["QuestionText"].ToString(),
                            CreatedDate = DateTime.Now
                        };
                        dbcontext.Questions.Add(_insertQuestion);
                        dbcontext.SaveChanges();
                    }
                    foreach (DataRow ans in dt.Rows) //.Cast<DataRow>().Skip(skip4)
                    {
                        if (!row["QuestionText"].ToString().IsEmpty())
                        {
                            if (!ans["QuestionText"].ToString().IsEmpty() || (dt.Rows.IndexOf(row) == skip4))
                            {
                                //
                            }
                            else if (ans["QuestionText"].ToString().IsEmpty())
                            {
                                if (row["ID"].ToString() == ans["ID"].ToString())
                                {
                                    Answer _insertAnswer = new Answer()
                                    {
                                        QuestionID = _insertQuestion.QuestionID,
                                        AnswerText = ans["AnswerText"].ToString(),
                                        IsAnwer = Convert.ToBoolean(ans["isAnswer"].ToString()),
                                        CreatedDate = DateTime.Now
                                    };
                                    dbcontext.Answers.Add(_insertAnswer);
                                    dbcontext.SaveChanges();
                                }
                            }
                        }

                    }
                }
                skip4 += 5;
            }
            return RedirectToAction("QuestionUpload", new { _quizID = _quizID });
        }

        [ChildActionOnly]
        public ActionResult CategoryDetails()
        {
            int _QuizID = Convert.ToInt32(Session["_CategoryID"]) != 0 ? Convert.ToInt32(Session["_CategoryID"]) : Convert.ToInt32(Session["_QuizID"]);

            var Quiz = dbcontext.Quizs.Single(p => p.QuizID == _QuizID);
            ViewBag.QuizName = Quiz.QuizName;
            ViewBag.QuizID = Quiz.QuizID;

            var Questions = dbcontext.Questions.Where(qu => qu.QuizID == _QuizID).OrderByDescending(d => d.CreatedDate).ToList();
            var _questionIDs = (from que in dbcontext.Questions where que.QuizID == _QuizID select que.QuestionID).ToList();

            var answers = dbcontext.Answers.Where(i => _questionIDs.Contains((int)i.QuestionID)).ToList();
            ViewBag.Answers = answers;
            return PartialView(Questions);
        }



        /// <summary>
        /// Contributor Quiz Section End
        /// </summary>
        /// <returns></returns>

        /////////////////////////////////////////////////////Contributor Answer Question////////////


       
        [HttpGet]
        public ActionResult AnswerQuestion()
        {
            var list = (from stuadd in dbcontext.Stu_AddQuestionReq
                        join coursemaster in dbcontext.Course_master on stuadd.CourseID equals coursemaster.Course_ID
                        join submaster in dbcontext.Subject_Master on stuadd.SubjectID equals submaster.SubjectID
                        join semester in dbcontext.Semesters on stuadd.Sem equals semester.SemID
                        join unit in dbcontext.UnitMasters on stuadd.Unit equals unit.Uid
                        where (stuadd.IsAnswered == false)
                        select new ContAnsModel
                        {
                            QuestionID = stuadd.QuestionID,
                            StudentID = stuadd.StudentID,
                            CourseName = coursemaster.Name,
                            SemName = semester.Name,
                            SubjectName = submaster.Name,
                            UnitName = unit.UnitName,
                            QuestinoHeading = stuadd.Qestion_Heading,
                            QuestionDescription = stuadd.Question_Description,
                            Answer = stuadd.Answer,
                        }).ToList();

            return View(list);
        }

       
        [HttpGet]
        public ActionResult Answer(int id)
        {
            var question = dbcontext.Stu_AddQuestionReq.Where(a => a.QuestionID == id).Select(a => a).FirstOrDefault();
            return View(question);
        }

        [HttpPost]
        public ActionResult Answer(Stu_AddQuestionReq model)
        {
            Stu_AddQuestionReq st = dbcontext.Stu_AddQuestionReq.Where(a => a.QuestionID == model.QuestionID).Select(a => a).FirstOrDefault();

            st.Answer = model.Answer;
            st.Anwered_Date = DateTime.Now;
            st.IsAnswered = true;
            st.ContributerID = (from i in dbcontext.ContributorProfiles where (i.Email == User.Identity.Name) select i.ID).FirstOrDefault();
            int c = dbcontext.SaveChanges();
            if (c > 0)
            {
                TempData["Message"] = "Your Answer has been Posted Successfully";
            }
            else
            {
                TempData["Message"] = "There was an error While Posting Your Answer, Please try after sometime";
            }
            return RedirectToAction("Success");
        }

       
        [HttpGet]
        [Authorize(Roles = "Contributor")]
        public ActionResult Success()
        {
            TempData.Keep();
            return View();
        }

        //User Name Retrieving
        public ActionResult UserName()
        {
            string tempUsername = User.Identity.Name.ToString();

            ViewBag.username = (from a in dbcontext.ContributorProfiles.Where(x => x.Email == tempUsername) select a.Name).FirstOrDefault();
            return View();
        }



        // Upload Files
        [HttpPost]
        public ActionResult UploadScanfiles(string str, string FileTypeName, StudyMaterial model)
        {
            string userID = User.Identity.Name;
            var userid = (from a in dbcontext.ContributorProfiles where a.Email == userID select a.ID).FirstOrDefault();
            string msg = "";
            int Status = 0;
            string fileName = "";

            string filesnames = ""; var Stringimages = new List<string>();
            string savedFileName = "";

            DateTime d1 = DateTime.Now;

            var storedData = dbcontext.StudyMaterials.Where(up => up.CmtID == userid).ToList();
            if (storedData.Count > 0)
            {
                if (!System.IO.Directory.Exists(Server.MapPath("\\Content\\Doc\\" + userid)))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("\\Content\\Doc\\" + userid));
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i]; //Uploaded file      //Use the following properties to get file's name, size and MIMEType
                        int fileSize = file.ContentLength;
                        fileName = file.FileName;
                        string mimeType = file.ContentType;
                        System.IO.Stream fileContent = file.InputStream;
                        savedFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetFileName(fileName);
                        file.SaveAs(Path.Combine(Server.MapPath("~/Content/Doc/" + userid), savedFileName)); // Save the file
                                                                                                             //  errormsg.Text += String.Format("{0}<br />", file);

                        Stringimages.Add(savedFileName);
                    }
                    filesnames = string.Join("|||", Stringimages.ToArray());
                }
                else
                {

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i]; //Uploaded file      //Use the following properties to get file's name, size and MIMEType
                        int fileSize = file.ContentLength;
                        fileName = file.FileName;
                        string mimeType = file.ContentType;
                        System.IO.Stream fileContent = file.InputStream;
                        savedFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetFileName(fileName);
                        file.SaveAs(Path.Combine(Server.MapPath("~/Content/Doc/" + userid), savedFileName)); // Save the file
                                                                                                             //  errormsg.Text += String.Format("{0}<br />", file);

                        Stringimages.Add(savedFileName);


                    }
                    filesnames = string.Join("|||", Stringimages.ToArray());

                }


                //data opration perperom here
                int len = storedData.Count;

                var smdat = dbcontext.StudyMaterials.FirstOrDefault(s => s.CmtID == userid);
                {

                    smdat.CourseID = model.CourseID;
                    smdat.SemID = model.SemID;
                    smdat.SM_Description = model.SM_Description;
                    smdat.SM_Name = model.SM_Name;
                    smdat.Attachments = savedFileName;
                    smdat.ModifiedDate = DateTime.Now;
                }

                var In = dbcontext.SaveChanges(); ;

                if (In > 0)
                {
                    msg = "file uploaded Successfully";
                }
                else
                {
                    msg = "Error Occured";
                }
            }
            else
            {

                if (!System.IO.Directory.Exists(Server.MapPath("\\Content\\Doc\\" + userid)))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("\\Content\\Doc\\" + userid));
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i]; //Uploaded file      //Use the following properties to get file's name, size and MIMEType
                        int fileSize = file.ContentLength;
                        fileName = file.FileName;
                        string mimeType = file.ContentType;
                        System.IO.Stream fileContent = file.InputStream;
                        savedFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetFileName(fileName);
                        file.SaveAs(Path.Combine(Server.MapPath("~/Content/Doc/" + userid), savedFileName)); // Save the file
                                                                                                             //  errormsg.Text += String.Format("{0}<br />", file);

                        Stringimages.Add(savedFileName);
                    }
                    filesnames = string.Join("|||", Stringimages.ToArray());
                }
                else
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i]; //Uploaded file      //Use the following properties to get file's name, size and MIMEType
                        int fileSize = file.ContentLength;
                        fileName = file.FileName;
                        string mimeType = file.ContentType;
                        System.IO.Stream fileContent = file.InputStream;
                        savedFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetFileName(fileName);
                        file.SaveAs(Path.Combine(Server.MapPath("~/Content/Doc/" + userid), savedFileName)); // Save the file
                                                                                                             //  errormsg.Text += String.Format("{0}<br />", file);

                        Stringimages.Add(savedFileName);
                    }
                    filesnames = string.Join("|||", Stringimages.ToArray());

                }
                int len = storedData.Count;
                StudyMaterial studyupload = new StudyMaterial();
                {
                    studyupload.CmtID = userid;
                    studyupload.CourseID = model.CourseID;
                    studyupload.SemID = model.SemID;
                    studyupload.SM_Description = model.SM_Description;
                    studyupload.SM_Name = model.SM_Name;
                    studyupload.Attachments = savedFileName;
                    studyupload.CreatedDate = DateTime.Now;
                }

                dbcontext.StudyMaterials.Add(studyupload);
                Status = dbcontext.SaveChanges();
                if (Status > 0)
                {
                    msg = "file uploaded Successfully";
                }
                else
                {
                    msg = "Erro Occuerred";
                }
            }
            return Json(new { responstext = msg }, JsonRequestBehavior.AllowGet);
        }



        //Download File
        public void DownLoadFiles(int? cmid)
        {
            cmid = 1016;
            var dir = new System.IO.DirectoryInfo(Server.MapPath("~/Content/Doc/" + cmid));
            System.IO.FileInfo[] fileNames = dir.GetFiles("*.*"); List<string> items = new List<string>();
            items.Add("");
            foreach (var file in fileNames)
            {
                items.Add(file.Name);
            }

            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;

                zip.AddDirectoryByName("Files");

                foreach (var file in fileNames)
                {
                    string filePath = Server.MapPath("~/Content/Doc/" + cmid + "/" + file.Name);
                    zip.AddFile(filePath, "Files");
                }

                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();

            }


        }

        [HttpGet]
        public ActionResult FileUpload(string msg)
        {

            if (msg == "1")
            {
                //Response.Write("<script>alert('Enter the New Cours...!')");
                ViewBag.mes = msg;
            }
            else if (msg == "0")
            {
                //Response.Write("<script>alert('Enter the old Cours...!')");
                ViewBag.mes = msg; ;
            }
            else
            {
                ViewBag.mes = null;
            }


            //if(!string.IsNullOrEmpty(model.SemID.ToString())&& !string.IsNullOrEmpty(model.CourseID.ToString()))
            //{

            //    var stmat = dbcontext.StudyMaterials.FirstOrDefault(c => c.CourseID == model.CourseID && c.SemID == model.SemID);

            //    if (stmat == null)
            //    {
            //        //Response.Write("<script>alert('Enter the New Cours...!')");
            //        Bindcourse();
            //        BindSemester();
            //        ViewBag.uploadedfilesdisplay = (from a in dbcontext.StudyMaterials.Where(x => x.SMID != 0) select a).ToList();
            //        return View();
            //    }
            //    else
            //    {
            //        //Response.Write("<script>alert('Enter the old Cours...!')");
            //        Bindcourse();
            //        BindSemester();
            //        ViewBag.uploadedfilesdisplay = (from a in dbcontext.StudyMaterials.Where(x => x.SMID != 0) select a).ToList();
            //        return View();
            //    }



            //}
            //else
            //{

            //}


            Bindcourse();
            BindSemester();
            ViewBag.uploadedfilesdisplay = (from a in dbcontext.StudyMaterials.Where(x => x.SMID != 0) select a).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult FileUpload(StudyMaterial model)
        {

            string msg = "";

            int Status = 0;
            string fileName = "";

            string filesnames = ""; var Stringimages = new List<string>();
            string savedFileName = "";

            DateTime d1 = DateTime.Now;
            string userID = User.Identity.Name;
            var userid = (from a in dbcontext.ContributorProfiles where a.Email == userID select a.ID).FirstOrDefault();
            if (model.SemID != 0 && model.CourseID != 0)
            {

                var sdmin = dbcontext.StudyMaterials.Where(s => s.SemID == model.SemID && s.CourseID == model.CourseID && s.CmtID == userid && s.CmtID == userid).FirstOrDefault();


                if (sdmin == null)
                {
                    //Insert New Files in table

                    if (!System.IO.Directory.Exists(Server.MapPath("\\Content\\Doc\\" + userid)))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("\\Content\\Doc\\" + userid));

                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            HttpPostedFileBase file = Request.Files[i]; //Uploaded file      //Use the following properties to get file's name, size and MIMEType
                            int fileSize = file.ContentLength;
                            fileName = file.FileName;
                            string mimeType = file.ContentType;
                            System.IO.Stream fileContent = file.InputStream;
                            savedFileName = DateTime.Now.Date.ToString("dd/MM/yyyy") + Path.GetFileName(fileName);
                            file.SaveAs(Path.Combine(Server.MapPath("~/Content/Doc/" + userid), savedFileName)); // Save the file
                                                                                                                 //  errormsg.Text += String.Format("{0}<br />", file);

                            Stringimages.Add(savedFileName);
                        }
                        filesnames = string.Join("|||", Stringimages.ToArray());
                    }
                    else
                    {
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            HttpPostedFileBase file = Request.Files[i]; //Uploaded file      //Use the following properties to get file's name, size and MIMEType
                            int fileSize = file.ContentLength;
                            fileName = file.FileName;
                            string mimeType = file.ContentType;
                            System.IO.Stream fileContent = file.InputStream;
                            savedFileName = DateTime.Now.Date.ToString("dd/MM/yyyy") + Path.GetFileName(fileName);
                            file.SaveAs(Path.Combine(Server.MapPath("~/Content/Doc/" + userid), savedFileName)); // Save the file
                                                                                                                 //  errormsg.Text += String.Format("{0}<br />", file);

                            Stringimages.Add(savedFileName);
                        }
                        filesnames = string.Join("|||", Stringimages.ToArray());

                    }

                    //int len = storedData.Count;
                    StudyMaterial studyupload = new StudyMaterial();
                    {
                        studyupload.CmtID = userid;
                        studyupload.CourseID = model.CourseID;
                        studyupload.SemID = model.SemID;
                        studyupload.SM_Description = model.SM_Description;
                        studyupload.SM_Name = model.SM_Name;
                        studyupload.Attachments = savedFileName;
                        studyupload.UploadBy = userID;
                        studyupload.CreatedDate = DateTime.Now;
                    }

                    dbcontext.StudyMaterials.Add(studyupload);
                    Status = dbcontext.SaveChanges();
                    if (Status > 0)
                    {
                        msg = "1";
                    }
                    else
                    {
                        msg = "0";
                    }

                }
                else
                {
                    //Update Exisitng tabale data

                    if (!System.IO.Directory.Exists(Server.MapPath("\\Content\\Doc\\" + userid)))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("\\Content\\Doc\\" + userid));
                        for (int i = 0; i < Request.Files.Count; i++)
                        {

                            HttpPostedFileBase file = Request.Files[i]; //Uploaded file      //Use the following properties to get file's name, size and MIMEType
                            int fileSize = file.ContentLength;
                            fileName = file.FileName;
                            string mimeType = file.ContentType;
                            System.IO.Stream fileContent = file.InputStream;
                            savedFileName = DateTime.Now.Date.ToString("dd/MM/yyyy") + Path.GetFileName(fileName);
                            file.SaveAs(Path.Combine(Server.MapPath("~/Content/Doc/" + userid), savedFileName)); // Save the file
                                                                                                                 //  errormsg.Text += String.Format("{0}<br />", file);

                            Stringimages.Add(savedFileName);
                        }
                        filesnames = string.Join("|||", Stringimages.ToArray());
                    }
                    else
                    {
                        // sdmin

                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            HttpPostedFileBase file = Request.Files[i]; //Uploaded file      //Use the following properties to get file's name, size and MIMEType
                            int fileSize = file.ContentLength;
                            fileName = file.FileName;
                            string mimeType = file.ContentType;
                            System.IO.Stream fileContent = file.InputStream;
                            savedFileName = DateTime.Now.Date.ToString("dd/MM/yyyy") + Path.GetFileName(fileName);
                            file.SaveAs(Path.Combine(Server.MapPath("~/Content/Doc/" + userid), savedFileName)); // Save the file
                                                                                                                 //  errormsg.Text += String.Format("{0}<br />", file);

                            Stringimages.Add(savedFileName);


                        }
                        filesnames = string.Join("|||", Stringimages.ToArray());

                    }


                    //data opration perperom here
                    //int len = storedData.Count;

                    var smdat = dbcontext.StudyMaterials.FirstOrDefault(s => s.CmtID == userid && s.SemID == model.SemID && s.CourseID == model.CourseID && s.CmtID == userid);
                    {

                        smdat.CourseID = model.CourseID;
                        smdat.SemID = model.SemID;
                        smdat.SM_Description = model.SM_Description;
                        smdat.SM_Name = model.SM_Name;
                        smdat.Attachments = savedFileName;
                        smdat.UploadBy = userID;
                        smdat.ModifiedDate = DateTime.Now;
                    }

                    var In = dbcontext.SaveChanges(); ;

                    if (In > 0)
                    {
                        msg = "1";
                    }
                    else
                    {
                        msg = "0";
                    }

                    // msg = "1";

                }
            }
            else
            {
                msg = "0";
            }

            Bindcourse();
            BindSemester();
            ViewBag.uploadedfilesdisplay = (from a in dbcontext.StudyMaterials.Where(x => x.SMID != 0) select a).ToList();

            return RedirectToAction("FileUpload", "Contributor", new { msg });
        }


        public JsonResult Getfiles(int? sem, int? cource)
        {
            string userID = User.Identity.Name;
            var userid = (from a in dbcontext.ContributorProfiles where a.Email == userID select a.ID).FirstOrDefault();
            var v = (from i in dbcontext.StudyMaterials
                     where i.CmtID == userid && i.SemID == sem && i.CourseID == cource

                     select new
                     {
                         userid = i.CmtID,
                         Attachment = i.Attachments
                     }).ToList();

            if (v.Count > 0)
            {
                return Json(v, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }


        public JsonResult Removefile(int? semid, int? CourseID)
        {


            string userID = User.Identity.Name;
            var userid = (from a in dbcontext.ContributorProfiles where a.Email == userID select a.ID).FirstOrDefault();

            var v = (from stm in dbcontext.StudyMaterials where stm.CmtID == userid && stm.SemID == semid && stm.CourseID == CourseID select stm).FirstOrDefault();

            if (v != null)
            {
                v.Attachments = "";

                dbcontext.SaveChanges();
                Getfiles(semid, CourseID);

            }
            else
            {

            }


            return Json(v, JsonRequestBehavior.AllowGet);
        }
    }
}