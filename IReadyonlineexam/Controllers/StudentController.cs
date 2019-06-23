using IReadyonlineexam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IReadyToday;
using System.Web.Mvc;
using System.Net;
using SelectPdf;
using IReadyonlineexam.Classes;
using System.Net.Mail;
using System.Data.Entity;
using System.IO;
using System.Data.Entity.Migrations;

namespace IReadyonlineexam.Controllers
{
    public class StudentController : Controller
    {
        //database connection
        public ireadytodayEntities2 dbcontext = new ireadytodayEntities2();

        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public ActionResult Index()
        {
            //JobsNotification Begin
            var JobsNotification = dbcontext.Job_Master.Where(i => i.JobID != 0).ToList();
            ViewBag.JobsN = JobsNotification;
            //JobsNotification End


            //StudyMaterials Begin
            var StudyMaterialsNotification = dbcontext.StudyMaterials.Where(i => i.SM_Name != null).Take(3).ToList();
            ViewBag.studymN = StudyMaterialsNotification;
            //StudyMaterials End

            var StudentName = (from a in dbcontext.StudentProfiles.Where(x => x.EmailID == User.Identity.Name) select a.Name).FirstOrDefault();
            ViewBag.name= StudentName;

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

            var Student = new GetUserByUserName(User.Identity.Name);
            var CourseID = Student.CourseID;
            var SemID = Student.SemID;
            var Student_ID = Student.Student_ID;
            var UnitID = Student.UnitID;
            var SubjectID = Student.SubjectID;

            List<Subject_Master> s = dbcontext.Subject_Master.Where(x => x.Course == CourseID && x.Semester == SemID).ToList();
            Subject_Master sub = new Subject_Master
            {
                Name = "----please select Course----",
                SubjectID = 0
            };
            s.Insert(0, sub);
            SelectList selectsubject = new SelectList(s, "SubjectID", "Name", 0);
            ViewBag.selectedsubject = selectsubject;

            List<UnitMaster> u = dbcontext.UnitMasters.ToList();
            UnitMaster unit = new UnitMaster
            {
                UnitName = "----Please select unit----",
                Uid = 0
            };
            u.Insert(0, unit);
            SelectList selectunit = new SelectList(u, "Uid", "UnitName", 0);
            ViewBag.selectedunit = selectunit;
            return View();



        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public ActionResult JobNotification()
        {
            var JobDetails = (from job in dbcontext.Job_Master
                              join year in dbcontext.YearMasters on job.year equals year.YearID
                              join course in dbcontext.Course_master on job.Course equals course.Course_ID
                              select new JobModel
                              {
                                  JobID = job.JobID,
                                  CompanyName = job.Company_Name,
                                  YearName = (int)year.Year,
                                  CourseName = course.Name,
                                  Designation = job.Designation,
                                  Description = job.Description,
                                  Location = job.Location,
                                  ScheduledDate = job.ScheduledDate,
                                  CreatedDate = job.CraetedDate,
                              }).OrderByDescending(s => s.ScheduledDate).ToList();
            return View(JobDetails);
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public ActionResult StudyMaterialNotification()
        {
            var StudyMaterial = (from studym in dbcontext.StudyMaterials
                                 join course in dbcontext.Course_master on studym.CourseID equals course.Course_ID
                                 join sem in dbcontext.Semesters on studym.SemID equals sem.SemID
                                 join profile in dbcontext.StudentProfiles on course.Course_ID equals profile.Course
                                 select new StudyMaterialModel
                                 {
                                     ID = studym.SMID,
                                     Name = studym.SM_Name,
                                     Description = studym.SM_Description,
                                     CourseName = course.Name,
                                     SemesterName = sem.Name,
                                     Attachement = studym.Attachments,
                                     UploadedBy = studym.UploadBy,
                                     CreatedDate = studym.CreatedDate
                                 }).OrderByDescending(d => d.CreatedDate).ToList();
            return View(StudyMaterial);
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public ActionResult QuizInstruction()
        {
            return View();
        }
        //Student DashBoard
        [Authorize(Roles = "Student")]
        [HttpGet]
        public ActionResult StudentDashBoard()
        {
            //JobsNotification Begin
            var JobsNotification = dbcontext.Job_Master.Where(i => i.JobID != 0).ToList();
            ViewBag.JobsN = JobsNotification;
            //JobsNotification End


            //StudyMaterials Begin
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





            //var jobdetails = (from a in dbcontext.Job_Master.Where(x => x.JobID != 0) select a).ToList();
            //ViewBag.jobdetails = jobdetails;

            //var Student = new GetUserByUserName(User.Identity.Name);
            //var CourseID = Student.CourseID;
            //var SemID = Student.SemID;
            //var Student_ID = Student.Student_ID;
            //var UnitID = Student.UnitID;
            //var SubjectID = Student.SubjectID;

            //List<Subject_Master> s = dbcontext.Subject_Master.Where(x => x.Course == CourseID && x.Semester == SemID).ToList();
            //Subject_Master sub = new Subject_Master
            //{
            //    Name = "Please select Course",
            //    SubjectID = 0
            //};
            //s.Insert(0, sub);
            //SelectList selectsubject = new SelectList(s, "SubjectID", "Name", 0);
            //ViewBag.selectedsubject = selectsubject;

            //List<UnitMaster> u = dbcontext.UnitMasters.ToList();
            //UnitMaster unit = new UnitMaster
            //{
            //    UnitName = "Please select unit",
            //    Uid = 0
            //};
            //u.Insert(0, unit);
            //SelectList selectunit = new SelectList(u, "Uid", "UnitName", 0);
            //ViewBag.selectedunit = selectunit;

            return View();
        }


        //Binding Semester Name
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

        //Binding Course Name
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

        //Binding Subject Name
        private void BindSubject()
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

        //Binding subjects by user course wise
        private void BindSubjectByUser()
        {
            var getCourseID = (from profile in dbcontext.StudentProfiles where profile.EmailID == User.Identity.Name select profile.Course).FirstOrDefault();
            List<Subject_Master> s = dbcontext.Subject_Master.Where(x => x.Course == getCourseID).ToList();
            Subject_Master sub = new Subject_Master
            {
                Name = "----Please select Course----",
                SubjectID = 0
            };
            s.Insert(0, sub);
            SelectList selectsubject = new SelectList(s, "SubjectID", "Name", 0);
            ViewBag.selectedsubject = selectsubject;
        }

        //Binding Unit Name
        private void BindUnitName()
        {
            List<UnitMaster> u = dbcontext.UnitMasters.Where(x => x.Uid != 0).ToList();
            UnitMaster unit = new UnitMaster
            {
                UnitName = "----Please select Unit Name----",
                Uid = 0
            };
            u.Insert(0, unit);
            SelectList selectunit = new SelectList(u, "Uid", "UnitName", 0);
            ViewBag.selectedunit = selectunit;
        }

        //Binding Year Name
        private void BindYear()
        {
            var u_list = dbcontext.YearMasters.OrderBy(a => a.YearID).ToList().Select(b => new SelectListItem { Value = b.YearID.ToString(), Text = b.Year.ToString() }).ToList();
            ViewBag.selectedyear = u_list;
        }



        public void BindState()
        {
            var state = dbcontext.Course_master.ToList();
            List<SelectListItem> li = new List<SelectListItem>
            {
                new SelectListItem { Text = "--=-Select Course----", Value = "0" }
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
                new SelectListItem { Text = "----Select Semester----", Value = "0" }
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

        // Student profile view
        public ActionResult ProfileStudent()
        {
            var details = (from a in dbcontext.StudentProfiles
                           join b in dbcontext.Course_master on a.Course equals b.Course_ID
                           //join c in dbcontext.Semester_master on a.Semester equals c.SemID 
                           where a.EmailID == User.Identity.Name
                           select new ProfileModel
                           {
                               Name = a.Name,
                               Gender = a.Gender,
                               DOB = a.DOB,
                               MobileNumber = a.MobileNumber,
                               Email = a.EmailID,                         
                               CollegeName = a.CollegeName,
                               CourseName = b.Name,
                               Address = a.Address,
                               CreatedDate = a.CreatedDate,
                               Image = a.Image


                           }).FirstOrDefault();
            return View(details);

        }
        //student registration

        [HttpGet]
        public ActionResult Studentregistration()
        {
            BindState();
            //BindCourse();
            BindSemester();
            return View();
        }

        //Student registration 
        [HttpPost]
       
        public ActionResult Studentregistration(StudentProfile model, string interviewdate, HttpPostedFileBase ImageFile)
        {
            var IntDate = DateTime.Parse(interviewdate);
            var getUserID = User.Identity.Name.ToString();
            string filesnames = "";

            BindSemester();
            //BindCourse();
            BindState();
            if (ImageFile != null)
            {
                string savedFileName = DateTime.Now.ToString("MMddHHmmssfff") + Path.GetFileName(ImageFile.FileName);
                ImageFile.SaveAs(Path.Combine(Server.MapPath("~/Content/Profile/"), savedFileName)); // Save the file
                filesnames = savedFileName;
            }
            StudentProfile stupro = new StudentProfile()
            {
                Name = model.Name,
                Gender = model.Gender,
                DOB = IntDate,
                MobileNumber = model.MobileNumber,
                EmailID = getUserID,
                CollegeName = model.CollegeName,
                Course = model.Course,
                Semester = model.Semester,
                Address = model.Address,
                CreatedDate = DateTime.Now,
                Image = filesnames,
            };
            dbcontext.StudentProfiles.Add(stupro);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                ViewBag.result = "Student Registration Completed Successfully!";
                ModelState.Clear();
            }
            else
            {
                ViewBag.result = "Something went wrong";
            }

            return RedirectToAction("Index", "Home");

        }

        //Edit Stud Profile

        [HttpGet]
        public ActionResult EditStudentProfile()
        {
            bool ProfileExists = new CheckStud(User.Identity.Name).Exist;
            if (!ProfileExists)
            {
                string tempUsername = User.Identity.Name.ToString();
                var pdata = (from p in dbcontext.AspNetUsers
                             where p.UserName == tempUsername
                             select new { UserName = tempUsername }).FirstOrDefault();

                StudentProfile ptemp = new StudentProfile
                {
                    EmailID = pdata.UserName
                };
                return View(ptemp);
            }
            else
            {
                var ProfileData = dbcontext.StudentProfiles.Where(stud => stud.EmailID == User.Identity.Name).First();
                return View(ProfileData);
            }

        }



        [HttpPost]
        public ActionResult EditStudentProfile(StudentProfile model, HttpPostedFileBase FileUpload)
        {
            string filesnames = "default.jpg";
            if (FileUpload != null)
            {
                string savedFileName = DateTime.Now.ToString("MMddHHmmssfff") + Path.GetFileName(FileUpload.FileName);
                FileUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/Profile"), savedFileName)); // Save the file
                filesnames = savedFileName;
            }

            bool ProfileExists = new CheckStud(User.Identity.Name).Exist;
            if (!ProfileExists)
            {

                BindCourse();
                StudentProfile insert = new StudentProfile()
                {
                    Name = model.Name,
                    Gender = model.Gender,
                    MobileNumber = model.MobileNumber,
                    EmailID = model.EmailID,
                    CollegeName = model.CollegeName,
                    //Course = model.Course,
                    //Semester = model.Semester,
                    Address = model.Address,
                    CreatedDate = DateTime.Now,
                    Image = filesnames,
                    // Status = true,
                    ModifiedDate = DateTime.Now
                };
                Session["UserID"] = insert.Student_ID;
                dbcontext.StudentProfiles.Add(insert);
            }
            else
            {
                BindCourse();
                var ProfileID = new GetStudID(User.Identity.Name).ProfileID;
                StudentProfile update = dbcontext.StudentProfiles.Find(ProfileID);
                if (update.Student_ID == ProfileID)
                {
                    update.Name = model.Name;
                   // update.Gender = model.Gender;
                    update.MobileNumber = model.MobileNumber;
                    update.EmailID = model.EmailID;
                    update.CollegeName = model.CollegeName;
                    //update.Course = model.Course;
                    //update.Semester = model.Semester;
                    update.Address = model.Address;
                    update.CreatedDate = DateTime.Now;
                    if (FileUpload != null)
                    {
                        update.Image = filesnames;
                    }
                    // Status = true,
                    update.ModifiedDate = DateTime.Now;





                }
                Session["UserID"] = update.Student_ID;
                dbcontext.StudentProfiles.AddOrUpdate(update);
            }

            int alert = dbcontext.SaveChanges();
            if (alert > 0)
            {
                TempData["AlertBox"] = "<script>alert('Sucessfully Created Profile');</script>";
            }
            return RedirectToAction("ProfileStudent", "Student");
        }


        [HttpPost]
        public ActionResult Studentsyllabusdisplay(int? SubjectID)
        {
            BindSubjectByUser();
            var getUserID = User.Identity.Name.ToString();
            var StudentID = (from a in dbcontext.StudentProfiles.Where(x => x.EmailID == getUserID) select a.Student_ID).FirstOrDefault();
            if (SubjectID != null)
            {
                var list = (from a in dbcontext.studentsyllabus
                            join b in dbcontext.Subject_Master on a.SubjectID equals b.SubjectID
                            join c in dbcontext.UnitMasters on a.Uid equals c.Uid
                            where b.SubjectID == SubjectID
                            select new CourseModel
                            {
                                Ssid = a.ssid,
                                SubjectName = b.Name,
                                UnitName = c.UnitName,
                                ChapterName = a.Name,
                                ChapterDesciption = a.UnitDescription,
                                CreatedDate = a.CreatedDate
                            }).ToList();
                ViewBag.ssd = list;
            }
            BindSubjectByUser();
            return View();
        }


       
        [HttpGet]
        public ActionResult Studentsyllabusdisplay()
        {

            BindSubjectByUser();
            string tempUsername = User.Identity.Name.ToString();
            ViewBag.ssd = (from a in dbcontext.StudentProfiles
                           join b in dbcontext.Course_master on a.Course equals b.Course_ID
                           join c in dbcontext.Subject_Master on a.Course equals c.Course
                           join d in dbcontext.studentsyllabus on c.SubjectID equals d.SubjectID
                           join e in dbcontext.UnitMasters on d.Uid equals e.Uid
                           where a.EmailID == tempUsername
                           select new CourseModel
                           {
                               Ssid = d.ssid,
                               SubjectName = c.Name,
                               UnitName = e.UnitName,
                               ChapterName = d.Name,
                               ChapterDesciption = d.UnitDescription,
                               CreatedDate = d.CreatedDate
                           }).ToList();
            return View();
        }



        //Student Jobdetails display
        [HttpGet]
        public ActionResult Jobdetails()
        {

            var jobdetails = (from a in dbcontext.Job_Master.Where(x => x.JobID != 0) select a).ToList();
            ViewBag.jobdetails = jobdetails;
            return View(jobdetails);
        }

       
        [HttpGet]
        public ActionResult StudentProfile()
        {
            string tempUsername = User.Identity.Name.ToString();
            ViewBag.studentprofile = (from a in dbcontext.StudentProfiles
                                      join b in dbcontext.Course_master on a.Course equals b.Course_ID
                                      where a.EmailID == tempUsername
                                      select new CourseModel
                                      {
                                          Name = a.Name,
                                          Gender = a.Gender,
                                          DOB = a.DOB,
                                          MobileNumber = a.MobileNumber,
                                          Email = a.EmailID,
                                          CollegeName = a.CollegeName,
                                          CourseName = b.Name,
                                          SemName = a.Name,
                                          Address = a.Address,
                                          CreatedDate = a.CreatedDate
                                      }).ToList();

            return View();
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public ActionResult StuProfile()
        {
            string tempUsername = User.Identity.Name.ToString();
            ViewBag.studentprofile = (from a in dbcontext.StudentProfiles
                                      join b in dbcontext.Course_master on a.Course equals b.Course_ID
                                      //join c in dbcontext.Semesters on a.Semester equals c.CourseID
                                      where a.EmailID == tempUsername
                                      select new ProfileModel
                                      {
                                          Name = a.Name,
                                          Gender = a.Gender,
                                          DOB = a.DOB,
                                          MobileNumber = a.MobileNumber,
                                          Email = a.EmailID,
                                          CollegeName = a.CollegeName,
                                          CourseName = b.Name,
                                          SemName = a.Semester.ToString(),
                                          Address = a.Address,
                                          CreatedDate = a.CreatedDate
                                      }).ToList();
            return View(ViewBag.studentprofile);
        }


        [Authorize(Roles = "Student")]
        [HttpGet]
        public ActionResult StudentQuizResult()
        {
            var StudnetID = (from a in dbcontext.StudentProfiles where a.EmailID == User.Identity.Name select a.Student_ID).FirstOrDefault();
            ViewBag.StudentQuizResult = (from a in dbcontext.Stu_Result
                                         join b in dbcontext.Subject_Master on a.SubjectID equals b.SubjectID
                                         join c in dbcontext.UnitMasters on a.UnitID equals c.Uid
                                         where a.UserID == StudnetID
                                         select new CourseModel
                                         {
                                             SubjectName = b.Name,
                                             UnitName = c.UnitName,
                                             ScoredMarks = a.ScoredMarks,
                                             TotalMarks = a.TotalMarks,
                                             CreatedDate = a.CreatedDate

                                         }).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult StudentFeedback(StudentReview model)
        {
            string tempUsername = User.Identity.Name.ToString();
            var usercourse = (from a in dbcontext.StudentProfiles where a.EmailID == tempUsername select a.Course).FirstOrDefault();
            StudentReview review = new StudentReview()
            {
                StudentUserID = tempUsername,
                course = usercourse,
                ReviewDescription = model.ReviewDescription,
                Status = false,
                CreatedDate = DateTime.Now
            };
            dbcontext.StudentReviews.Add(review);
            int c = dbcontext.SaveChanges();
            if (c > 0)
            {
                ViewBag.result = "Review Inserted Successfully!";
                ModelState.Clear();
            }
            else
            {
                ViewBag.result = "Something went wrong";
            }

            var StudentFeedback = (from a in dbcontext.StudentReviews where a.StudentUserID == tempUsername select a).ToList();
            ViewBag.StuFeedback = StudentFeedback;
            return View();
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public ActionResult StudentFeedback()
        {
            string userid = User.Identity.Name;
            var StudentFeedback = (from a in dbcontext.StudentReviews where a.StudentUserID == userid select a).ToList();
            ViewBag.StuFeedback = StudentFeedback;
            return View();
        }


        //genrate pdf function
        //[Authorize(Roles = "Student")]
        //[HttpGet]
        //public ActionResult GetPdftest()
        //{
        //    var converter = new HtmlToPdf();

        //    converter.Options.PdfPageSize = PdfPageSize.A4;
        //    converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
        //    converter.Options.WebPageWidth = 1700;
        //    converter.Options.WebPageHeight = 1200;
        //    var doc = converter.ConvertUrl("http://ireadytoday.shlrtechnosoft.net/Student/GetPdftest");
        //    doc.Save(System.Web.HttpContext.Current.Response, true, "test.pdf");
        //    doc.Close();

        //    return null;
        //}


        //public ActionResult GoldCirtificate()
        //{




        //    ViewBag.studentName = "Aswath";

        //    ViewBag.Course = "Computer Science and Engineering";
        //    ViewBag.Fromyear = "2019";
        //    ViewBag.Toyear = "2020";

        //    return View();
        //}

        //Get Rank Details Class
        public class GetRankDetail
        {
            public int Student_ID { get; set; }
            public int CourseID { get; set; }
            public int SemID { get; set; }
            public int SubjectID { get; set; }
            public int UnitID { get; set; }
            public int TotalMarks { get; set; }
            public int ScoredMarks { get; set; }
            public int Rank { get; set; }

        }




        //Recent Written test score 
        public JsonResult Test()

        {
            ireadytodayEntities2 dbcontext = new ireadytodayEntities2();

            var Student = new GetUserByUserName(User.Identity.Name);

            var details = (from i in dbcontext.Stu_Result
                           join j in dbcontext.UnitMasters on i.UnitID equals j.Uid
                           where (i.UserID == Student.Student_ID && i.CourseID == Student.CourseID && i.SemID == Student.SemID)
                           orderby i.CreatedDate descending
                           select new
                           {
                               i.ScoredMarks,
                               i.UnitID,
                               j.UnitName
                           }).FirstOrDefault();

            return Json(new { UserDetails = details }, JsonRequestBehavior.AllowGet);
        }


        //Subject Performance

        public JsonResult SubjectPerformance()
        {

            ireadytodayEntities2 dbcontext = new ireadytodayEntities2();
            var Student = new GetUserByUserName(User.Identity.Name);
            var CourseID = Student.CourseID;
            var SemID = Student.SemID;
            var Student_ID = Student.Student_ID;
            // var Uid = Student.UnitID;
            var subjectid = Student.SubjectID;
            var score = new GetCalculatePerform(Student_ID, CourseID, SemID).tempobj;

            GetMarks obj = new GetMarks();

            int? userid = new GetUserByUserName(User.Identity.Name).Student_ID;
            var test = obj.GetMarksSubjectWise(userid);





            return Json(new { scoreCard = score, marks = test }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetUnitPerform(int? UnitID, int SubjectID)
        {
            ireadytodayEntities2 dbcontext = new ireadytodayEntities2();
            var Student = new GetUserByUserName(User.Identity.Name);
            var Courseid = Student.CourseID;
            var Semid = Student.SemID;
            var Student_ID = Student.Student_ID;
            var Uid = Student.UnitID;
            //var subjectid = Student.SubjectID;
            var Unitid = Student.UnitID;
            var unitscore = new GetCalculatePerform(Student_ID, Courseid, Semid).temp;
            return Json(new { scoreCard = unitscore }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult StartQuiz()
        {
            return Redirect("/ngquiz/index.html");

        }



        //public JsonResult Downloadfile()
        //{
        //    string userID = User.Identity.Name;
        //    var std = new GetUserByUserName(User.Identity.Name);
        //    var userid = (from a in dbcontext.CommitteeMembers_Profile where a.EmailID == userID select a.CmtID).FirstOrDefault();
        //    var v = (from i in dbcontext.StudyMaterials
        //             where i.SemID == std.SemID && i.CourseID == std.CourseID

        //             select new
        //             {
        //                 userid = i.CmtID,
        //                 Attachment = i.Attachments
        //             }).ToList();
        //    if (v.Count > 0)
        //    {
        //        return Json(v, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {

        //    }

        //    return Json("", JsonRequestBehavior.AllowGet);
        //}

        public ActionResult StudentMarks()
        {
            string userID = User.Identity.Name;

            var StudentName = (from a in dbcontext.StudentProfiles.Where(x => x.EmailID == userID) select a.Name).FirstOrDefault();
            var Studentcoursename = (from a in dbcontext.StudentProfiles
                                     join b in dbcontext.Course_master on a.Course equals b.Course_ID
                                     select new CourseModel
                                     {
                                         CourseName = b.Name
                                     }).FirstOrDefault();

            var ScoredMarks = (from a in dbcontext.Stu_Result
                               join b in dbcontext.StudentProfiles on a.UserID equals b.Student_ID
                               where b.EmailID == userID
                               select a.ScoredMarks).Sum();
            var Totalmarks = (from a in dbcontext.Stu_Result
                              join b in dbcontext.StudentProfiles on a.UserID equals b.Student_ID
                              where b.EmailID == userID
                              select a.TotalMarks).Sum();
            return View();
        }

        ////////////////////////////////////////Student Question Request //////////////////////////////

        [Authorize(Roles = "Student")]
        [HttpGet]
        public ActionResult NewQuestion_Request()
        {
            var user = new StudentDetails(User.Identity.Name);

            ViewBag.Subjects = dbcontext.Subject_Master.Where(a => a.Course == user.CourseId).Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.SubjectID.ToString()
            }).ToList();

            ViewBag.Units = dbcontext.UnitMasters.Select(a => new SelectListItem
            {
                Text = a.UnitName,
                Value = a.Uid.ToString()
            }).ToList();


            return View();
        }

        [HttpPost]
        public ActionResult NewQuestion_Request(string Sub, string Unit, string Q_heading, string Q_desc)
        {

            var user = new StudentDetails(User.Identity.Name);

            Stu_AddQuestionReq sa = new Stu_AddQuestionReq()
            {
                StudentID = user.Stuid,
                CourseID = user.CourseId,
                Sem = user.Sem,
                SubjectID = Int32.Parse(Sub),
                Unit = Int32.Parse(Unit),
                Qestion_Heading = Q_heading,
                Question_Description = Q_desc,
                IsAnswered = false,
                Question_Date = DateTime.Now
            };
            dbcontext.Stu_AddQuestionReq.Add(sa);
            int status = dbcontext.SaveChanges();
            if (status > 0)
            {
                TempData["Message"] = "Your Question has been Posted Successfully";
            }
            else
            {
                TempData["Message"] = "There was an error While Posting Your Question, Please try after sometime";
            }
            return RedirectToAction("Success");
        }


        
        [HttpGet]
        public ActionResult Success()
        {
            TempData.Keep();
            return View();
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public ActionResult DisplayQuestionS()
        {
            var user = new StudentDetails(User.Identity.Name);

            var questions = dbcontext.Stu_AddQuestionReq.Where(a => a.StudentID == user.Stuid).Select(a => a).ToList();

            return View(questions);
        }





        public ActionResult GetPdftest(string uid)
        {

            var userIDs = dbcontext.StudentProfiles.Where(ss => ss.EmailID == User.Identity.Name).FirstOrDefault();
            int id = userIDs.Student_ID;
            string userID = User.Identity.Name;

            var ScoredMarks = (from a in dbcontext.Stu_Result
                               join b in dbcontext.StudentProfiles on a.UserID equals b.Student_ID
                               where b.EmailID == userID
                               select a.ScoredMarks).Sum();


            var Totalmarks = (from a in dbcontext.Stu_Result
                              join b in dbcontext.StudentProfiles on a.UserID equals b.Student_ID
                              where b.EmailID == userID
                              select a.TotalMarks).Sum();

            int? maxmarks = 0;
            maxmarks = ScoredMarks * 100 / Totalmarks;



            var converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
            converter.Options.WebPageWidth = 1700;
            converter.Options.WebPageHeight = 1200;
            //above 80
            if (maxmarks >= 80)
            {
                // var doc = converter.ConvertUrl("https://ireadytoday.shlrtechnosoft.net/Student/GoldCirtificate");
                var doc = converter.ConvertUrl("http://ireadytoday.shlrtechnosoft.net/Student/Downloadgold/" + id);
                doc.Save(System.Web.HttpContext.Current.Response, true, "test.pdf");
                doc.Close();
            }
            //60-79
            else if (maxmarks >= 60 && maxmarks < 80)
            {
                //var doc = converter.ConvertUrl("https://ireadytoday.shlrtechnosoft.net/Student/SilverCirtificate");
                var doc = converter.ConvertUrl("http://ireadytoday.shlrtechnosoft.net/Student/Downloadsilver/" + id);
                doc.Save(System.Web.HttpContext.Current.Response, true, "test.pdf");
                doc.Close();
            }
            //
            else if (maxmarks >= 50 && maxmarks < 60)
            {
                //var doc = converter.ConvertUrl("https://ireadytoday.shlrtechnosoft.net/Student/bronzeCirtificate");
                var doc = converter.ConvertUrl("http://ireadytoday.shlrtechnosoft.net/Student/downloadpdf/" + id);
                doc.Save(System.Web.HttpContext.Current.Response, true, "test.pdf");
                doc.Close();
            }
            else
            {
                return RedirectToAction("Index", "Student");
            }
                return null;
        }

        public ActionResult downloadpdf(int? id)
        {
            if (id != null)
            {
                return RedirectToAction("bronzeCirtificate", new { uid = id });
            }
            return View();
        }

        public ActionResult Downloadgold(int? id)
        {
            if (id != null)
            {
                return RedirectToAction("GoldCirtificate", new { uid = id });
            }
            return View();
        }

        public ActionResult Downloadsilver(int? id)
        {
            if (id != null)
            {
                return RedirectToAction("SilverCirtificate", new { uid = id });
            }
            return View();
        }

        public ActionResult GoldCirtificate(int? uid)
        {

            var stu_profile = (from stu_prof in dbcontext.StudentProfiles join cou_mast in dbcontext.Course_master on stu_prof.Course equals cou_mast.Course_ID where stu_prof.Student_ID == uid select new { Student_Name = stu_prof.Name, Cource_Name = cou_mast.Name }).FirstOrDefault();

            if (stu_profile != null)
            {
                ViewBag.studentName = stu_profile.Student_Name;
                ViewBag.Course = stu_profile.Cource_Name;
                ViewBag.Fromyear = DateTime.Now.Year;
                ViewBag.Toyear = DateTime.Now.Year + 1;
                return View();

            }



            return View();
        }

        public ActionResult SilverCirtificate(int? uid)
        {
            var stu_profile = (from stu_prof in dbcontext.StudentProfiles join cou_mast in dbcontext.Course_master on stu_prof.Course equals cou_mast.Course_ID where stu_prof.Student_ID == uid select new { Student_Name = stu_prof.Name, Cource_Name = cou_mast.Name }).FirstOrDefault();

            if (stu_profile != null)
            {
                ViewBag.studentName = stu_profile.Student_Name;
                ViewBag.Course = stu_profile.Cource_Name;
                ViewBag.Fromyear = DateTime.Now.Year;
                ViewBag.Toyear = DateTime.Now.Year + 1;
                return View();

            }



            return View();
        }

        public ActionResult bronzeCirtificate(int? uid)
        {
            string userID = User.Identity.Name;


            var stu_profile = (from stu_prof in dbcontext.StudentProfiles join cou_mast in dbcontext.Course_master on stu_prof.Course equals cou_mast.Course_ID where stu_prof.Student_ID == uid select new { Student_Name = stu_prof.Name, Cource_Name = cou_mast.Name }).FirstOrDefault();

            if (stu_profile != null)
            {
                ViewBag.studentName = stu_profile.Student_Name;
                ViewBag.Course = stu_profile.Cource_Name;
                ViewBag.Fromyear = DateTime.Now.Year;
                ViewBag.Toyear = DateTime.Now.Year + 1;
                return View();

            }
            return View();
        }



        public ActionResult List(string movieGenre, string searchString)
        {
            //bind the genre list in the dropdown list
            var GenreLst = new List<string>();

            var GenreQry = from d in dbcontext.Movies
                           orderby d.Genre
                           select d.Genre;

            GenreLst.AddRange(GenreQry.Distinct());
            ViewBag.movieGenre = new SelectList(GenreLst);


            //string searchString = id;
            var movies = from m in dbcontext.Movies
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
            }
            //return PartialView("_list", movies.ToList());
            return PartialView("~/Views/Student/List.cshtml", movies.ToList());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult PostRating(int rating, int mid)
        {
            //save data into the database

            StarRating rt = new StarRating();
            string ip = Request.ServerVariables["REMOTE_ADDR"];
            rt.Rate = rating;
            rt.IpAddress = ip;
            rt.MovieId = mid;

            //save into the database 
            dbcontext.StarRatings.Add(rt);
            dbcontext.SaveChanges();



            return Json("You rated this " + rating.ToString() + " star(s)");
        }


        public ActionResult CourseRating()
        {
            //int avg;
            //var courseID = (from a in dbcontext.Course_master.Where(x => x.Course_ID != 0) select a).ToList();

            //int Coursewisetotalratingcount = (from a in dbcontext.Stu_Result.Where(x => x.CourseID != 0) select a).Count();
            //var Sumofcoursewiserating = (from a in dbcontext.Stu_Result.Where(x => x.Rating != 0) select a).Sum(x => x.Rating);
            // avg = Coursewisetotalratingcount / Sumofcoursewiserating;

            //var RatingAvg=(from a in dbcontext.Stu_Result
            //               where a.CourseID != 0
            //               group a.CourseID)


            //var Ratings = dbcontext.Stu_Result.Where(x=>x.CourseID != 0 ).GroupBy(i => i.CourseID)
            //  .Select(g => new 
            //  {
            //      CourseID = g.Key,
            //      Average = g.Average(i => i.Rating)
            //  });

            var CourseRatings = from earning in dbcontext.StudentCourseRatings
                                group earning by earning.CourseID into earns
                                select new RatingModel
                                {
                                    CourseID = (int)earns.Key,
                                    Rating = (int)earns.Average(x => x.Rating),
                                };
            ViewBag.CourseRatings = CourseRatings;

            return View(CourseRatings);
        }

        public JsonResult GetScore()
        {
            GetMarks obj = new GetMarks();

            int? userid = new GetUserByUserName(User.Identity.Name).Student_ID;
            var test = obj.GetMarksSubjectWise(userid);
            return Json(new { marksdata = test }, JsonRequestBehavior.AllowGet);

        }


    }

}
