using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IReadyToday;
using System.Web.Mvc;
using System.Data.Entity.Migrations;
using IReadyonlineexam.Models;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using ClosedXML.Excel;
using System.Net.Mail;

namespace IReadyonlineexam.Controllers
{

    public class AdminController : Controller
    {
        public ireadytodayEntities2 dbcontext = new ireadytodayEntities2();

       
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            UsersCount();

            var StudentAdditonalQuestion = (from a in dbcontext.Stu_AddQuestionReq.Where(x => x.SubjectID != null) select a).ToList();
            ViewBag.Stu_Questionrequest = StudentAdditonalQuestion;
            return View();
        }

        [HttpPost]
        public ActionResult Index(String ID)
        {
            if (ID == "0")
            {
                List<StudentProfile> sp = dbcontext.StudentProfiles.ToList();
                TempData["List"] = sp;
                return RedirectToAction("ReportStudent");
            }
            else if (ID == "1")
            {


            }
            else if (ID == "2")
            {

            }
            return View();
        }

        public JsonResult JsonList(String ID)
        {
            if (ID == "0")
            {
                dbcontext.Configuration.LazyLoadingEnabled = false;
                List<StudentProfile> sp = dbcontext.StudentProfiles.ToList();
                return Json(sp, JsonRequestBehavior.AllowGet);
            }
            else if (ID == "1")
            {


            }
            else if (ID == "2")
            {

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }



        public ActionResult ReportStudent()
        {
            List<StudentProfile> sp = dbcontext.StudentProfiles.ToList();
            return View(sp);
        }
        public ActionResult ReportContributer()
        {
            List<ContributorProfile> cp = dbcontext.ContributorProfiles.ToList();
            return View(cp);
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
                Name = "please select Semester",
                SemID = 0
            };
            c.Insert(0, sem);
            SelectList selectsem = new SelectList(c, "SemID", "Name ", 0);
            ViewBag.selectedsem = selectsem;
        }

        //Course Name Binding
        private void BindCourse()
        {
            List<Course_master> c = dbcontext.Course_master.Where(x => x.Course_ID != 0).ToList();
            Course_master course = new Course_master
            {
                Name = "Please select Category",
                Course_ID = 0
            };
            c.Insert(0, course);
            SelectList selectcourse = new SelectList(c, "Course_ID", "Name", 0);
            ViewBag.selectedcourse = selectcourse;
        }

        //Unit Name Binding
        private void BindUnit()
        {
            List<UnitMaster> u = dbcontext.UnitMasters.Where(x => x.Uid != 0).ToList();
            UnitMaster unit = new UnitMaster
            {
                UnitName = "Please select Unit",
                Uid = 0
            };
            u.Insert(0, unit);
            SelectList selectunit = new SelectList(u, "Uid", "UnitName", 0);
            ViewBag.selectedunit = selectunit;
        }

        //Subject Name Binding
        private void BindSubjectname()
        {
            List<Subject_Master> s = dbcontext.Subject_Master.Where(x => x.SubjectID != 0).ToList();
            Subject_Master sub = new Subject_Master
            {
                Name = "Please select Course",
                SubjectID = 0
            };
            s.Insert(0, sub);
            SelectList selectsubject = new SelectList(s, "SubjectID", "Name", 0);
            ViewBag.selectedsubject = selectsubject;
        }

        //course wise Semester Binding 
        [HttpPost]
        public ActionResult SemesterMaster(Semester model)
        {
            BindCourse();
            Semester sem = new Semester()
            {
                SemID = model.SemID,
                Name = model.Name,
                CourseID = model.CourseID,
                CreatedDate = DateTime.Now
            };
            dbcontext.Semesters.Add(sem);
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

            
            ViewBag.result = (from a in dbcontext.Semesters
                              join b in dbcontext.Course_master on a.CourseID equals b.Course_ID
                              select new CourseModel
                              {
                                  SemID = a.SemID,
                                  CourseName = b.Name,
                                  SemName = a.Name,
                                  CreatedDate = a.CreatedDate
                              }).ToList();
            return RedirectToAction("SemesterMaster");
        }

        //course wise Semester Binding 
        
        [HttpGet]
        public ActionResult SemesterMaster()
        {
            BindCourse();
            ViewBag.result = (from a in dbcontext.Semesters
                              join b in dbcontext.Course_master on a.CourseID equals b.Course_ID
                              select new CourseModel
                              {
                                  SemID = a.SemID,
                                  CourseName = b.Name,
                                  SemName = a.Name,
                                  CreatedDate = a.CreatedDate
                              }).ToList();
            return View();
        }

        //Edit course wise semester name 
        [HttpPost]
        public ActionResult EditSemestermaster(Semester model)
        {
            Semester semedit = dbcontext.Semesters.Find(model.SemID);
            if (semedit.SemID == model.SemID)
            {
                semedit.SemID = model.SemID;
                semedit.Name = model.Name;
            }
            dbcontext.Semesters.AddOrUpdate(semedit);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {

                TempData["result"] = "Record Edited Successfully!";
                ModelState.Clear();
            }
            else
            {
                TempData["result"] = "Something went wrong!";
            }
            //CourseAdd();
            return RedirectToAction("SemesterMaster");
        }


        //Edit course wise semester name 
       
        [HttpGet]
        public async Task<ActionResult> EditSemestermaster(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester semedit = await dbcontext.Semesters.FindAsync(id);
            if (semedit == null)
            {
                return HttpNotFound();
            }
            return View(semedit);

        }


        //Delete course wise semester name
      
        [HttpGet]
        public async Task<ActionResult> DeleteSemmaster(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester semedit = await dbcontext.Semesters.FindAsync(id);
            if (semedit == null)
            {
                return HttpNotFound();
            }
            return View(semedit);

        }

        //Delete course wise semester name
        [HttpPost]
        public ActionResult DeleteSemmaster(Semester model)
        {
            Semester semedit = dbcontext.Semesters.Find(model.SemID);
            if (semedit.SemID == model.SemID)
            {
                semedit.SemID = model.SemID;
                semedit.Name = model.Name;
            }
            dbcontext.Semesters.Remove(semedit);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {

                TempData["result"] = "Record Deleted Successfully!";
                ModelState.Clear();
            }
            else
            {
                TempData["result"] = "Something went wrong!";
            }
            //CourseAdd();
            return RedirectToAction("SemesterMaster");
        }




        //Semester Name Storing
        [HttpPost]
        public ActionResult SemesterAdd(Semester_master model)

        {
            Semester_master sem = new Semester_master()
            {
                Name = model.Name,
                CraetedDate = DateTime.Now
            };
            dbcontext.Semester_master.Add(sem);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {

                TempData["result"] = "Record Inserted Successfully!";
                ModelState.Clear();
            }
            else
            {
                TempData["result"] = "Something went wrong!";
            }
            //CourseAdd();
            return RedirectToAction("SemesterAdd");

        }


        //Semester Name Displaying 
        
        [HttpGet]
        public ActionResult SemesterAdd()
        {
            ViewBag.list1 = (from list in dbcontext.Semester_master where list.SemID != 0 select list).ToList();
            return View();
        }

        //Edit Semester Name
        [HttpPost]
        public ActionResult EditSemester(Semester_master model)
        {
            Semester_master semedit = dbcontext.Semester_master.Find(model.SemID);
            if (semedit.SemID == model.SemID)
            {
                semedit.Name = model.Name;
            }
            dbcontext.Semester_master.AddOrUpdate(semedit);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {

                TempData["result"] = "Record Edited Successfully!";
                ModelState.Clear();
            }
            else
            {
                TempData["result"] = "Something went wrong!";
            }
            //CourseAdd();
            return RedirectToAction("SemesterAdd");
        }


        //Edit Semester Name
       
        [HttpGet]
        public async Task<ActionResult> EditSemester(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester_master semedit = await dbcontext.Semester_master.FindAsync(id);
            if (semedit == null)
            {
                return HttpNotFound();
            }
            return View(semedit);

        }


        //Delete Semester Name
       
        [HttpGet]
        public async Task<ActionResult> DeleteSem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester_master semedit = await dbcontext.Semester_master.FindAsync(id);
            if (semedit == null)
            {
                return HttpNotFound();
            }
            return View(semedit);

        }

        //Delete Semester Name
        [HttpPost]
        public ActionResult DeleteSem(Semester_master model)
        {
            Semester_master semedit = dbcontext.Semester_master.Find(model.SemID);
            if (semedit.SemID == model.SemID)
            {
                semedit.Name = model.Name;
            }
            dbcontext.Semester_master.Remove(semedit);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {

                TempData["result"] = "Record Deleted Successfully!";
                ModelState.Clear();
            }
            else
            {
                TempData["result"] = "Something went wrong!";
            }
            //CourseAdd();
            return RedirectToAction("SemesterAdd");
        }


       
        [HttpGet]
        public ActionResult CourseAdd()
        {
            ViewBag.list = (from list in dbcontext.Course_master where list.Course_ID != 0 select list).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult CourseAdd(Course_master model)
        {
            Course_master course = new Course_master()
            {
                Name = model.Name,
                CreatedDate = DateTime.Now
            };
            dbcontext.Course_master.Add(course);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                ViewBag.result = "Course Name Inserted Successfully!";
                ModelState.Clear();
            }
            else
            {
                ViewBag.result = "Please try again later";
            }
            CourseAdd();
            return View();
        }


        
        [HttpGet]
        public ActionResult DynCrc()
        {

            ViewBag.list = (from list in dbcontext.Course_master where list.Course_ID != 0 select list).OrderByDescending(List => List.CreatedDate).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult DynCrc(DynamicCourse_Master imageModel, HttpPostedFileBase ImageFile)
        {
            string filesnames = "";
            if (ImageFile != null)
            {
                string savedFileName = DateTime.Now.ToString("MMddHHmmssfff") + Path.GetFileName(ImageFile.FileName);
                ImageFile.SaveAs(Path.Combine(Server.MapPath("~/Images/CourseImages/"), savedFileName)); // Save the file
                filesnames = savedFileName;
            }
            Course_master dcm = new Course_master()
            {
                Name = imageModel.Title,
                Price = imageModel.Price,
                Description = imageModel.Description,
                CreatedDate = DateTime.Now,
                Imageurl = filesnames,
            };
            dbcontext.Course_master.Add(dcm);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {

                TempData["result"] = "Course Name Inserted Successfully!";
                ModelState.Clear();
            }
            else
            {
                TempData["result"] = "Please Try Again!";
            }
            //CourseAdd();
            return RedirectToAction("DynCrc");
        }



        [HttpPost, ValidateInput(false)]
        public ActionResult Editcourse(Course_master model, HttpPostedFileBase FileUpload)
        {
            string filesnames = dbcontext.Course_master.Where(i => i.Course_ID == model.Course_ID).Select(i => i.Imageurl).FirstOrDefault();
            filesnames = filesnames == null ? "default.jpg" : filesnames;

            if (FileUpload != null)
            {
                if (!System.IO.Directory.Exists(Server.MapPath("\\Images\\CourseImages" + 1)))
                {
                    string savedFileName = DateTime.Now.ToString("MMddHHmmssfff") + Path.GetFileName(FileUpload.FileName);
                    FileUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/CourseImages"), savedFileName)); // Save the file
                    filesnames = savedFileName;
                }
            }

            Course_master course = dbcontext.Course_master.Find(model.Course_ID);
            if (course.Course_ID == model.Course_ID)
            {
                course.Name = model.Name;
                course.Imageurl = filesnames;
                course.Price = model.Price;
                course.Description = model.Description;
            }
            dbcontext.Course_master.AddOrUpdate(course);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                TempData["result"] = "Record Edited Successfully!";

            }
            else
            {
                TempData["result"] = "Something went wrong";
            }

            return RedirectToAction("DynCrc", "Admin");

        }

        
        [HttpGet]
        public async Task<ActionResult> Editcourse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course_master course = await dbcontext.Course_master.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);

        }

        [HttpPost]
        public ActionResult Deletecourse(Course_master model)
        {

            Course_master course = dbcontext.Course_master.Find(model.Course_ID);
            if (course.Course_ID == model.Course_ID)
            {
                var filesnames = dbcontext.Course_master.Where(i => i.Course_ID == model.Course_ID).Select(i => i.Imageurl).FirstOrDefault();
                var filePath = Server.MapPath("~/Images/CourseImages/" + filesnames);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                dbcontext.Course_master.Remove(course);
            }

            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                TempData["result"] = "Record deleted Successfully";
            }
            else
            {
                TempData["result"] = "Something went wrong";
            }
            //return View(course);
            return RedirectToAction("DynCrc", "Admin");
        }



       
        [HttpGet]
        public async Task<ActionResult> Deletecourse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course_master course = await dbcontext.Course_master.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);

        }

        [HttpPost]
        public ActionResult YearAdd(YearMaster model)
        {
            YearMaster insert = new YearMaster()
            {
                Year = model.Year,
                CreatedDate = DateTime.Now
            };
            dbcontext.YearMasters.Add(insert);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {

                ViewBag.result = "Record Inserted Successfully!";
                ModelState.Clear();
            }
            else
            {
                ViewBag.result = "Something went wrong!";
            }

            ViewBag.list = (from list in dbcontext.YearMasters where list.YearID != 0 select list).ToList();
            return View();
        }

       
        [HttpGet]
        public ActionResult YearAdd()
        {
            ViewBag.list = (from list in dbcontext.YearMasters where list.YearID != 0 select list).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult EditYear(YearMaster model)
        {
            YearMaster year = dbcontext.YearMasters.Find(model.YearID);
            if (year.YearID == model.YearID)
            {
                year.Year = model.Year;
            }
            dbcontext.YearMasters.AddOrUpdate(year);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                ViewBag.result = "Record Edited Successfully!";
            }
            else
            {
                ViewBag.result = "Something went wrong!";
            }
            return View();
        }

        
        [HttpGet]
        public async Task<ActionResult> EditYear(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YearMaster year = await dbcontext.YearMasters.FindAsync(id);
            if (year == null)
            {
                return HttpNotFound();
            }
            return View(year);

        }


        [HttpPost]
        public ActionResult DeleteYear(YearMaster model)
        {
            YearMaster yeardelete = dbcontext.YearMasters.Find(model.YearID);
            if (yeardelete.YearID == model.YearID)
            {
                yeardelete.Year = model.Year;
            }
            dbcontext.YearMasters.Remove(yeardelete);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                ViewBag.result = "Record Deleted Successfully!";
            }
            else
            {
                ViewBag.result = "Something went wrong!";
            }
            return View();
        }

     
        [HttpGet]
        public async Task<ActionResult> DeleteYear(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YearMaster yeardelete = await dbcontext.YearMasters.FindAsync(id);
            if (yeardelete == null)
            {
                return HttpNotFound();
            }
            return View(yeardelete);

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
                            }).ToList();
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
                Semester = model.Semester,
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
                            }).OrderByDescending(x=>x.CreatedDate).ToList();

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

        
        [HttpPost]
        public ActionResult Smstemplatemaster(Smstemplate_master model)
        {

            Smstemplate_master sms = new Smstemplate_master()
            {
                smstemp_title = model.smstemp_title,
                smstemp_description = model.smstemp_description,
                CreatedDate = DateTime.Now

            };
            dbcontext.Smstemplate_master.Add(sms);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {

                TempData["result"] = "Record Inserted Successfully!";
                ModelState.Clear();
            }
            else
            {
                TempData["result"] = "Please Try Again!";
            }
            //CourseAdd();
           

            //ViewBag.sms = (from list in dbcontext.Smstemplate_master where list.SmstemplateID != 0 select list).ToList();
            return RedirectToAction("Smstemplatemaster");
        }

       
        [HttpGet]
        public ActionResult Smstemplatemaster()
        {
            ViewBag.sms = (from list in dbcontext.Smstemplate_master where list.SmstemplateID != 0 select list).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult EditSmstemplatemaster(Smstemplate_master model)

        {
            Smstemplate_master smstemplateedit = dbcontext.Smstemplate_master.Find(model.SmstemplateID);
            if (smstemplateedit.SmstemplateID == model.SmstemplateID)
            {
                smstemplateedit.smstemp_title = model.smstemp_title;
                smstemplateedit.smstemp_description = model.smstemp_description;
            }
            dbcontext.Smstemplate_master.AddOrUpdate(smstemplateedit);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {

                TempData["result"] = "Record Edited Successfully!";
                ModelState.Clear();
            }
            else
            {
                TempData["result"] = "Please Try Again!";
            }
           
            return RedirectToAction("Smstemplatemaster");
        }

        
        [HttpGet]
        public async Task<ActionResult> EditSmstemplatemaster(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Smstemplate_master smstemplateedit = await dbcontext.Smstemplate_master.FindAsync(id);
            if (smstemplateedit == null)
            {
                return HttpNotFound();
            }
            return View(smstemplateedit);

        }

        [HttpPost]
        public ActionResult DeleteSmstemplatemaster(Smstemplate_master model)
        {
            Smstemplate_master smstempDelete = dbcontext.Smstemplate_master.Find(model.SmstemplateID);
            if (smstempDelete.SmstemplateID == model.SmstemplateID)
            {
                smstempDelete.smstemp_title = model.smstemp_title;
                smstempDelete.smstemp_description = model.smstemp_description;
            }
            dbcontext.Smstemplate_master.Remove(smstempDelete);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {

                TempData["result"] = "Record Deleted Successfully!";
                ModelState.Clear();
            }
            else
            {
                TempData["result"] = "Please Try Again!";
            }
           
            return RedirectToAction("Smstemplatemaster");
        }

        [HttpGet]
        public async Task<ActionResult> DeleteSmstemplatemaster(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Smstemplate_master smstempDelete = await dbcontext.Smstemplate_master.FindAsync(id);
            if (smstempDelete == null)
            {
                return HttpNotFound();
            }
            return View(smstempDelete);

        }



        [HttpPost]
        public ActionResult MailTemplateMaster(MailTemplateMaster model)
        {
            MailTemplateMaster mail = new MailTemplateMaster()
            {
                MailID = model.MailID,
                MailTitle = model.MailTitle,
                MailDescription = model.MailDescription,
                CreatedDate = DateTime.Now
            };
            dbcontext.MailTemplateMasters.Add(mail);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                TempData["result"] = "Record Inserted Successfully";
            }
            else
            {
                TempData["result"] = "Something Went wrong";
            }
            
            ViewBag.Mail = (from a in dbcontext.MailTemplateMasters where a.MailID != 0 select a).ToList();
            return RedirectToAction("MailTemplateMaster");
        }

       
        [HttpGet]
        public ActionResult MailTemplateMaster()
        {

            ViewBag.Mail = (from a in dbcontext.MailTemplateMasters where a.MailID != 0 select a).ToList();
            return View();
        }


        [HttpPost]
        public ActionResult EditMailTemplateMaster(MailTemplateMaster model)
        {
            MailTemplateMaster mailedit = dbcontext.MailTemplateMasters.Find(model.MailID);
            if (mailedit.MailID == model.MailID)
            {
                mailedit.MailTitle = model.MailTitle;
                mailedit.MailDescription = model.MailDescription;
            }
            dbcontext.MailTemplateMasters.AddOrUpdate(mailedit);
            int save = dbcontext.SaveChanges();

            if (save > 0)
            {
                TempData["result"] = "Record Edited Successfully";
            }
            else
            {
                TempData["result"] = "Something Went wrong";
            }
            return RedirectToAction("MailTemplateMaster");
        }



    
        [HttpGet]
        public async Task<ActionResult> EditMailTemplateMaster(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MailTemplateMaster mailedit = await dbcontext.MailTemplateMasters.FindAsync(id);
            if (mailedit == null)
            {
                return HttpNotFound();
            }
            return View(mailedit);

        }

       
        [HttpGet]
        public async Task<ActionResult> DeleteMailTemplateMaster(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MailTemplateMaster maildelete = await dbcontext.MailTemplateMasters.FindAsync(id);
            if (maildelete == null)
            {
                return HttpNotFound();
            }
            return View(maildelete);

        }

        [HttpPost]
        public ActionResult DeleteMailTemplateMaster(MailTemplateMaster model)
        {
            MailTemplateMaster Maildelete = dbcontext.MailTemplateMasters.Find(model.MailID);
            if (Maildelete.MailID == model.MailID)
            {

                Maildelete.MailTitle = model.MailTitle;
                Maildelete.MailDescription = model.MailDescription;
            }
            dbcontext.MailTemplateMasters.Remove(Maildelete);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                TempData["result"] = "Record Deleted Successfully";
            }
            else
            {
                TempData["result"] = "Something Went wrong";
            }
            return RedirectToAction("MailTemplateMaster");
        }




        [HttpPost]
        public ActionResult Unitmaster(UnitMaster model)
        {
            UnitMaster unit = new UnitMaster()
            {
                UnitName = model.UnitName,
                CreatedDate = DateTime.Now
            };
            dbcontext.UnitMasters.Add(unit);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                TempData["Message"] = "Record Edited Successfully";
            }
            else
            {
                TempData["Message"] = "Something Went wrong";
            }
            
            ViewBag.unit = (from list in dbcontext.UnitMasters where list.Uid != 0 select list).ToList();
            return RedirectToAction("Unitmaster");
        }

       
        [HttpGet]
        public ActionResult Unitmaster()
        {

            ViewBag.unit = (from list in dbcontext.UnitMasters where list.Uid != 0 select list).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult EditUnit(UnitMaster model)
        {
            UnitMaster unitedit = dbcontext.UnitMasters.Find(model.Uid);
            if (unitedit.Uid == model.Uid)
            {
                unitedit.UnitName = model.UnitName;
            }
            dbcontext.UnitMasters.AddOrUpdate(unitedit);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                TempData["Message"] = "Record Edited Successfully";
            }
            else
            {
                TempData["Message"] = "Something Went wrong";
            }
            return RedirectToAction("Unitmaster");
        }


      
        [HttpGet]
        public async Task<ActionResult> EditUnit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitMaster unitedit = await dbcontext.UnitMasters.FindAsync(id);
            if (unitedit == null)
            {
                return HttpNotFound();
            }
            return View(unitedit);

        }

        
        [HttpGet]
        public async Task<ActionResult> Deleteunit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitMaster unitdelete = await dbcontext.UnitMasters.FindAsync(id);
            if (unitdelete == null)
            {
                return HttpNotFound();
            }
            return View(unitdelete);

        }


        [HttpPost]
        public ActionResult Deleteunit(UnitMaster model)
        {
            UnitMaster unitdelete = dbcontext.UnitMasters.Find(model.Uid);
            if (unitdelete.Uid == model.Uid)
            {
                unitdelete.UnitName = model.UnitName;
            }
            dbcontext.UnitMasters.Remove(unitdelete);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                ViewBag.result = "Record deleted Successfully";
            }
            else
            {
                ViewBag.result = "Something went wrong";
            }
            return View();
        }



        [HttpPost]
        public ActionResult SubjectAddmaster(Subject_Master model)
        {
            BindYear();
            BindState();
            Subject_Master subadd = new Subject_Master()
            {

                Course = model.Course,
                Semester = model.Semester,
                Name = model.Name,
                CreatedDate = DateTime.Now
            };
            dbcontext.Subject_Master.Add(subadd);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                TempData["result"] = "Record Inserted Successfully";
            }
            else
            {
                TempData["result"] = "Something Went wrong";
            }
           
            ViewBag.result = (from sub in dbcontext.Subject_Master
                              join b in dbcontext.Course_master on sub.Course equals b.Course_ID
                              join c in dbcontext.Semesters on sub.Semester equals c.SemID
                              select new CourseModel
                              {
                                  SubjectID = sub.SubjectID,
                                  CourseName = b.Name,
                                  SemName = c.Name,
                                  SubjectName = sub.Name,
                                  CreatedDate = sub.CreatedDate
                              }).ToList();
            return RedirectToAction("SubjectAddmaster");

        }

     
        [HttpGet]
        public ActionResult SubjectAddmaster()
        {
            BindYear();
            BindState();
            ViewBag.result = (from sub in dbcontext.Subject_Master
                              join b in dbcontext.Course_master on sub.Course equals b.Course_ID
                              join c in dbcontext.Semesters on sub.Semester equals c.SemID
                              select new CourseModel
                              {
                                  SubjectID = sub.SubjectID,
                                  CourseName = b.Name,
                                  SemName = c.Name,
                                  SubjectName = sub.Name,
                                  CreatedDate = sub.CreatedDate
                              }).ToList();
            
            return View();
        }


        [HttpPost]
        public ActionResult EditSubject(Subject_Master model)
        {
            Subject_Master Subjectedit = dbcontext.Subject_Master.Find(model.SubjectID);
            if (Subjectedit.SubjectID == model.SubjectID)
            {
                Subjectedit.SubjectID = model.SubjectID;
                Subjectedit.Name = model.Name;
            }
            dbcontext.Subject_Master.AddOrUpdate(Subjectedit);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                TempData["result"] = "Record Edited Successfully";
            }
            else
            {
                TempData["result"] = "Something Went wrong";
            }
            return RedirectToAction("SubjectAddmaster");
        }


        
        [HttpGet]
        public async Task<ActionResult> EditSubject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject_Master Subjectedit = await dbcontext.Subject_Master.FindAsync(id);
            if (Subjectedit == null)
            {
                return HttpNotFound();
            }
            return View(Subjectedit);

        }

        
        [HttpGet]
        public async Task<ActionResult> DeletSubject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject_Master Subjectdelete = await dbcontext.Subject_Master.FindAsync(id);
            if (Subjectdelete == null)
            {
                return HttpNotFound();
            }
            return View(Subjectdelete);

        }


        [HttpPost]
        public ActionResult DeletSubject(Subject_Master model)
        {
            Subject_Master Subjectdelete = dbcontext.Subject_Master.Find(model.SubjectID);
            if (Subjectdelete.SubjectID == model.SubjectID)
            {
                Subjectdelete.SubjectID = model.SubjectID;
                Subjectdelete.Name = model.Name;
            }
            dbcontext.Subject_Master.Remove(Subjectdelete);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                TempData["result"] = "Record Deleted Successfully";
            }
            else
            {
                TempData["result"] = "Something Went wrong";
            }
            return RedirectToAction("SubjectAddmaster");
        }

        public void BindState()
        {
            var state = dbcontext.Course_master.ToList();
            List<SelectListItem> li = new List<SelectListItem>
            {
                new SelectListItem { Text = "--Select Course--", Value = "0" }
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


        [HttpPost]
        public ActionResult StuSyllabus(studentsyllabu model)
        {
            BindUnit();
            BindSubjectname();
            studentsyllabu syl = new studentsyllabu()
            {
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
                TempData["Message"] = "Record Inserted Successfully";
            }
            else
            {
                TempData["Message"] = "Something Went wrong";
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

                                }).ToList();
            return View();
        }


        [HttpPost]
        public ActionResult EditStuSyllabus(studentsyllabu model)
        {
            studentsyllabu stusyllabusedit = dbcontext.studentsyllabus.Find(model.ssid);
            if (stusyllabusedit.ssid == model.ssid)
            {
                stusyllabusedit.Name = model.Name;
                stusyllabusedit.UnitDescription = model.UnitDescription;

            };
            dbcontext.studentsyllabus.AddOrUpdate(stusyllabusedit);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                TempData["Message"] = "Record Edited Successfully";
            }
            else
            {
                TempData["Message"] = "Something Went wrong";
            }
            return RedirectToAction("StuSyllabus");
        }

       
        [HttpGet]
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
                studentsyllabudelete.Name = model.Name;
                studentsyllabudelete.UnitDescription = model.UnitDescription;
            }
            dbcontext.studentsyllabus.Remove(studentsyllabudelete);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                TempData["Message"] = "Record Deleted Successfully";
            }
            else
            {
                TempData["Message"] = "Something Went wrong";
            }
            return RedirectToAction("StuSyllabus");
        }

       
        [HttpGet]
        public ActionResult RegisteredStudents()
        {
            ViewBag.RegStudents = (from a in dbcontext.StudentProfiles
                                   join b in dbcontext.Course_master on a.Course equals b.Course_ID
                                   join c in dbcontext.Semester_master on a.Semester equals c.SemID

                                   select new CourseModel
                                   {
                                       Name = a.Name,
                                       Gender = a.Gender,
                                       DOB = a.DOB,
                                       MobileNumber = a.MobileNumber,
                                       Email = a.EmailID,
                                       CourseName = b.Name,
                                       SemName = c.Name,
                                       Address = a.Address,
                                       CreatedDate = a.CreatedDate
                                   }).ToList();



            return View();
        }



        
        [HttpGet]
        public ActionResult Studentadditionquestion()
        {
            ViewBag.saqr = (from a in dbcontext.Stu_Additionalquestionrequest
                            join b in dbcontext.YearMasters on a.Year equals b.YearID
                            join c in dbcontext.Course_master on a.Course equals c.Course_ID
                            join d in dbcontext.Semester_master on a.Semester equals d.SemID
                            join e in dbcontext.Subject_Master on a.Subject equals e.SubjectID
                            select new CourseModel
                            {
                                YearName = b.Year,
                                CourseName = c.Name,
                                SemName = d.Name,
                                SubjectName = e.Name,
                                AQDescription = a.Description,
                                CreatedDate = a.CreatedDate
                            }).ToList();

            return View();
        }

       
        [HttpGet]
        public ActionResult StudentReviewforApprove(StudentReview model)
        {
            ViewBag.reviewaord = (from a in dbcontext.StudentReviews
                                  join b in dbcontext.StudentProfiles on a.StudentUserID equals b.EmailID
                                  join c in dbcontext.Course_master on a.course equals c.Course_ID
                                  select new CourseModel
                                  {
                                      SrID = a.SrID,
                                      Name = b.Name,
                                      CourseName = c.Name,
                                      ReviewDescription = a.ReviewDescription,
                                      Status = a.Status,
                                      ReviewDate = a.CreatedDate
                                  }).ToList();
            return View();
        }

        public ActionResult ReviewActive(int? ID)
        {
            if (ModelState.IsValid)
            {
                StudentReview update = dbcontext.StudentReviews.Find(ID);
                if (update.SrID == ID && update.Status != true)
                {
                    update.Status = true;
                }
                else
                {
                    update.Status = false;
                }
                dbcontext.StudentReviews.AddOrUpdate(update);
                int c = dbcontext.SaveChanges();
            }
            TempData["msg"] = "<script>alert('Updated Successfully');</script>";
            return RedirectToAction("StudentReviewforApprove", "Admin");
        }


        
        [HttpGet]
        public async Task<ActionResult> EditReview(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentReview ReviewEdit = await dbcontext.StudentReviews.FindAsync(id);
            if (ReviewEdit == null)
            {
                return HttpNotFound();
            }
            return View(ReviewEdit);

        }

        [HttpPost]
        public ActionResult EditReview(StudentReview model)
        {
            StudentReview ReviewEdit = dbcontext.StudentReviews.Find(model.SrID);
            if (ReviewEdit.SrID == model.SrID)
            {
                ReviewEdit.SrID = model.SrID;
                ReviewEdit.Status = model.Status;
                ReviewEdit.ModifiedDate = DateTime.Now;
            };
            dbcontext.StudentReviews.AddOrUpdate(ReviewEdit);
            int save = dbcontext.SaveChanges();
            if (save > 0)
                {
                    TempData["Message"] = "Record Deleted Successfully";
                }
                else
                {
                    TempData["Message"] = "Something Went wrong";
                }
            return RedirectToAction("StudentReviewforApprove");
        }

        
        [HttpGet]
        public async Task<ActionResult> DeleteReview(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentReview DeleteReview = await dbcontext.StudentReviews.FindAsync(id);
            if (DeleteReview == null)
            {
                return HttpNotFound();
            }
            return View(DeleteReview);

        }

        [HttpPost]
        public ActionResult DeleteReview(StudentReview model)
        {
            StudentReview DeleteReview = dbcontext.StudentReviews.Find(model.SrID);
            if (DeleteReview.SrID == model.SrID)
            {
                DeleteReview.SrID = model.SrID;
                DeleteReview.Status = model.Status;
            }
            dbcontext.StudentReviews.Remove(DeleteReview);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                TempData["Message"] = "Record Deleted Successfully";
            }
            else
            {
                TempData["Message"] = "Something Went wrong";
            }
            return RedirectToAction("StudentReviewforApprove");
        }


        
        [HttpGet]
        public ActionResult SMStoStudent()

        {
            BindTextYear();
            BindState();
            return View();
        }

        [HttpPost]
        public ActionResult SMStoStudent(string Year, int Course, int Semester)
        {

            ViewBag.getstuddata = (from t in dbcontext.StudentProfiles.ToList() where t.CreatedDate.Value.Year.ToString() == Year && t.Course == Course && t.Semester == Semester orderby t.Student_ID select t).ToArray();

            BindTextYear();
            BindState();
            GetsmsTitle();
            GetsmsTemplates();
            return View("SMStoStudent");
        }

        public void BindTextYear()
        {
            var u_list = dbcontext.YearMasters.OrderBy(a => a.YearID).ToList().Select(b => new SelectListItem { Value = b.Year.ToString(), Text = b.Year.ToString() }).ToList();
            ViewBag.selectYear = u_list;

        }

        protected void GetsmsTitle()
        {
            var getsmstitle = dbcontext.Smstemplate_master.OrderBy(s => s.SmstemplateID).Select(m => new SelectListItem { Text = m.smstemp_title.ToString(), Value = m.SmstemplateID.ToString() }).ToList();
            ViewBag.selectsmsTitle = getsmstitle;
        }


        public void GetsmsTemplates()
        {
            var getsmstemp = dbcontext.Smstemplate_master.OrderBy(s => s.SmstemplateID).Select(m => new SelectListItem { Value = m.SmstemplateID.ToString(), Text = m.smstemp_description.ToString() }).ToList();
            ViewBag.seletedVal = getsmstemp;
        }


        [HttpPost]
        public JsonResult SendSms(string[] values, string message)
        {

            foreach (string Num in values)
            {
                string MobNum = Num.Trim();
                string SenderID = "SHLRTE";
                string HASH = "ZV8dVW4zhec-eg0uH4206fetokQt4cQ6WxGgxsZKuj";

                string Message = "Dear User. Your OTP is " + 111 + "";// append string message here

                string strUrl = "http://smslocal.smshouse.in/api2/send/?apikey=" + HASH + "&numbers=91" + MobNum + "&sender=" + SenderID + "&message=" + Message.Trim() + "&msgtype=0";

                WebRequest request = HttpWebRequest.Create(strUrl);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream s = (Stream)response.GetResponseStream();
                StreamReader readStream = new StreamReader(s);
                string dataString = readStream.ReadToEnd();
            }
            return Json(new { lcount = 1, responstext = "Message Sent Successfully...Thank you!" }, JsonRequestBehavior.AllowGet);
        }

      
        [HttpGet]
        public ActionResult MailtoStudent()

        {
            BindTextYear();
            //BindCourse();
            //BindSemester();
            BindState();
            return View();
        }
        [HttpPost]
        public ActionResult MailtoStudent(string Year, int Course, int Semester)
        {

            ViewBag.getstuddata = (from t in dbcontext.StudentProfiles.ToList() where t.CreatedDate.Value.Year.ToString() == Year && t.Course == Course && t.Semester == Semester orderby t.Student_ID select t).ToArray();

            BindTextYear();
            BindState();
            GetMailTitle();
            GetMailTemplates();

            return View("MailtoStudent");
        }

        public void MailBindTextYear()
        {
            var u_list = dbcontext.YearMasters.OrderBy(a => a.YearID).ToList().Select(b => new SelectListItem { Value = b.Year.ToString(), Text = b.Year.ToString() }).ToList();
            ViewBag.selectYear = u_list;

        }

        protected void GetMailTitle()
        {
            var getmailtitle = dbcontext.MailTemplateMasters.OrderBy(s => s.MailID).Select(m => new SelectListItem { Text = m.MailTitle.ToString(), Value = m.MailID.ToString() }).ToList();
            ViewBag.selectsmsTitle = getmailtitle;
        }


        public void GetMailTemplates()
        {
            var getmailtemp = dbcontext.MailTemplateMasters.OrderBy(s => s.MailID).Select(m => new SelectListItem { Value = m.MailID.ToString(), Text = m.MailDescription.ToString() }).ToList();
            ViewBag.seletedVal = getmailtemp;

        }

        [HttpPost]
        public JsonResult SendMail(string[] values, string message, string MailSubject)
        {

            foreach (string Mail in values)
            {
                string MailID = Mail.Trim();
                SmtpClient smtp1;
                string from = "technical@shlrtechnosoft.in";
                string toEmail = MailID;

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(from);
                    mail.To.Add(toEmail);
                    mail.Subject = MailSubject;
                    mail.Body = message;
                    mail.IsBodyHtml = false;
                    smtp1 = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,

                        Credentials = new System.Net.NetworkCredential
                    ("technical@shlrtechnosoft.in", "Technical@123"),

                        EnableSsl = true
                    };
                    smtp1.Send(mail);
                }
            }
            return Json(new { lcount = 1, responstext = "Mail Sent Successfully...Thank you!" }, JsonRequestBehavior.AllowGet);
        }

        public class UserData
        {
            public int UserCount { get; set; }
            public string Month { get; set; }
        }


        public JsonResult viewCount()
        {
            var list = dbcontext.StudentProfiles.GroupBy(u => u.CreatedDate.Value.Month)
                .Select(group => new UserData
                {
                    Month = group.Key.ToString(),
                    UserCount = group.Count()
                }).ToList();

            var newlist = new List<UserData>();
            newlist.Add(new UserData() { Month = "1", UserCount = 0 });
            newlist.Add(new UserData() { Month = "2", UserCount = 0 });
            newlist.Add(new UserData() { Month = "3", UserCount = 0 });
            newlist.Add(new UserData() { Month = "4", UserCount = 0 });
            newlist.Add(new UserData() { Month = "5", UserCount = 0 });
            newlist.Add(new UserData() { Month = "6", UserCount = 0 });
            newlist.Add(new UserData() { Month = "7", UserCount = 0 });
            newlist.Add(new UserData() { Month = "8", UserCount = 0 });
            newlist.Add(new UserData() { Month = "9", UserCount = 0 });
            newlist.Add(new UserData() { Month = "10", UserCount = 0 });
            newlist.Add(new UserData() { Month = "11", UserCount = 0 });
            newlist.Add(new UserData() { Month = "12", UserCount = 0 });


            foreach (var item in newlist)
            {
                foreach (var i in list)
                {
                    if (i.Month == item.Month)
                    {
                        item.UserCount = i.UserCount;
                    }
                }
            }


            return Json(new { UserData = newlist }, JsonRequestBehavior.AllowGet);
        }

        public void UsersCount()
        {

            var dt = DateTime.Now.AddDays(-1);

            string st = dt.ToString("yyyy-MM-dd h:mm tt");

            ViewBag.StudentCount = dbcontext.StudentProfiles.Count();
            ViewBag.ContributorCount = dbcontext.ContributorProfiles.Count();
            ViewBag.NewRegistration = dbcontext.AspNetUsers.Where(a => a.CreatedDate > dt).Count();
            ViewBag.Visits = dbcontext.AspNetUsers.Count();
            ViewBag.ActiveUsers = dbcontext.AspNetUsers.Where(a => a.Isactive == true).Count();




        }

        public FileResult StudentExlDownload()
        {
            ViewBag.Studentlist = from cmlist in dbcontext.StudentProfiles.Take(10) select cmlist;
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[10]
            {
                                            new DataColumn("Name"),
                                            new DataColumn("Gender"),
                                            new DataColumn("DOB"),
                                            new DataColumn("MobileNumber"),
                                            new DataColumn("EmailID"),
                                            new DataColumn("Course"),
                                            new DataColumn("Semester"),
                                            new DataColumn("Address"),
                                            new DataColumn("CollegeNAme"),
                                            new DataColumn("CreatedDate")

            });


            ViewBag.RegStudents = (from a in dbcontext.StudentProfiles
                                   join b in dbcontext.Course_master on a.Course equals b.Course_ID
                                   join c in dbcontext.Semesters on a.Semester equals c.SemID


                                   select new CourseModel
                                   {
                                       Name = a.Name,
                                       Gender = a.Gender,
                                       DOB = a.DOB,
                                       MobileNumber = a.MobileNumber,
                                       Email = a.EmailID,
                                       CourseName = b.Name,
                                       SemName = c.Name,
                                       Address = a.Address,
                                       CollegeName = a.CollegeName,
                                       CreatedDate = a.CreatedDate
                                   }).ToList();

            foreach (var cmlist in ViewBag.RegStudents)
            {
                dt.Rows.Add(cmlist.Name, cmlist.Gender, cmlist.DOB, cmlist.MobileNumber, cmlist.Email, cmlist.CourseName, cmlist.SemName, cmlist.Address, cmlist.CollegeName, cmlist.CreatedDate);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Studentslist.xlsx");



                }
            }
        }


        //[HttpGet]
        //public ActionResult Export1()
        //{
        //    ViewBag.committememberlist = from cmlist in dbcontext.CommitteeMembers_Profile.Take(10)
        //                                 select cmlist;
        //    return View(ViewBag.committememberlist);
        //}
        //[HttpGet]
        //public ActionResult RegisteredCommitteMember()
        //{
        //    ViewBag.RegCm = (from a in dbcontext.CommitteeMembers_Profile
        //                     join b in dbcontext.Course_master on a.CourseID equals b.Course_ID
        //                     select new CourseModel
        //                     {
        //                         Name = a.Name,
        //                         Gender = a.Gender,
        //                         MobileNumber = a.MobileNumber,
        //                         Email = a.EmailID,
        //                         CourseName = b.Name,
        //                         Address = a.Address,
        //                         Qualification = a.Qualification,
        //                         CreatedDate = a.CreatedDate

        //                     }).ToList();


        //    return View();
        //}




        //***************************************************** ContributorList Export to Excel Sheet    *********************************************************************************************
        public FileResult ContributorExlDownload()
        {
            ViewBag.contributorlist = from cmlist in dbcontext.ContributorProfiles.Take(12) select cmlist;
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[12] {
                                            new DataColumn("Name"),
                                            new DataColumn("EmailID"),
                                            new DataColumn("MobileNumber"),
                                            new DataColumn("Gender"),
                                            new DataColumn("Address"),
                                            new DataColumn("Website"),
                                            new DataColumn("CurrentPosition"),
                                            new DataColumn("InstitutionName"),
                                            new DataColumn("Skills"),
                                            new DataColumn("Course"),
                                            new DataColumn("Experience"),
                                            new DataColumn("CreatedDate"),
                                                 });


            ViewBag.RegCm = (from a in dbcontext.ContributorProfiles
                             join b in dbcontext.Course_master on a.CourseID equals b.Course_ID
                             select new CourseModel
                             {
                                 Name = a.Name,
                                 Email = a.Email,
                                 MobileNumber = a.PhoneNo,
                                 Gender = a.Gender,
                                 Address = a.Address,
                                 Website = a.Website,
                                 CurrentPosition = a.Current_Position,
                                 InstitutionName = a.Institution_Name,
                                 Skills = a.Skills,
                                 CourseName = b.Name,
                                 Experience = a.Experience,
                                 CreatedDate = a.CreatedDate
                             }).ToList();


            foreach (var cmlist in ViewBag.RegCm)
            {
                dt.Rows.Add(cmlist.Name, cmlist.Email, cmlist.MobileNumber, cmlist.Gender, cmlist.Address, cmlist.Website, cmlist.CurrentPosition, cmlist.InstitutionName, cmlist.Skills, cmlist.CourseName, cmlist.Experience, cmlist.CreatedDate);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ContributorList.xlsx");
                }
            }
        }


    }



}
