using IReadyonlineexam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using IReadyToday;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.Compression;
using Ionic.Zip;

namespace IReadyonlineexam.Controllers
{

    public class CommitteMemberController : Controller
    {
        public ireadytodayEntities2 dbcontext = new ireadytodayEntities2();


        // GET: CommitteMember
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Details()
        {
            bindState();
            return View();
        }

        public void bindState()
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

        public JsonResult getCity(int id)
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

        //Semester Binding
        private void BindSemester()
        {
            List<Semester_master> c = dbcontext.Semester_master.Where(x => x.SemID != 0).ToList();
            Semester_master sem = new Semester_master
            {
                Name = "----Please select Semester----",
                SemID = 0
            };
            c.Insert(0, sem);
            SelectList selectsem = new SelectList(c, "SemID", "Name ", 0);
            ViewBag.selectedsem = selectsem;
        }

        //Unit Binding
        private void BindUnit()
        {
            List<UnitMaster> u = dbcontext.UnitMasters.Where(x => x.Uid != 0).ToList();
            UnitMaster unit = new UnitMaster
            {
                UnitName = "----Please select Unit",
                Uid = 0
            };
            u.Insert(0, unit);
            SelectList selectunit = new SelectList(u, "Uid", "UnitName", 0);
            ViewBag.selectedunit = selectunit;
        }

        //Subjects Name Binding
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
        //Quiz ID Binding
        private void BindQuizID()
        {
            List<Quiz> q = dbcontext.Quizs.Where(x => x.QuizID != 0).ToList();
            Quiz qu = new Quiz
            {
                QuizName = "Please select Quiz Name",
                QuizID = 0
            };
            q.Insert(0, qu);
            SelectList selectquizname = new SelectList(q, "QuizID", "QuizName", 0);
            ViewBag.selectedquizname = selectquizname;
        }

        //Quiz Questions Binding
        private void BindMcqQuestions()
        {
            List<Question> q = dbcontext.Questions.Where(x => x.QuizID != 0).ToList();
            Question qu = new Question
            {
                QuestionText = "please select Quiz Question",
                QuestionID = 0
            };
            q.Insert(0, qu);
            SelectList selectquestion = new SelectList(q, "QuestionID", "QuestionText", 0);
            ViewBag.selectedquestion = selectquestion;
        }

        //Year Binding
        private void BindYear()
        {
            var u_list = dbcontext.YearMasters.OrderBy(a => a.YearID).ToList().Select(b => new SelectListItem { Value = b.YearID.ToString(), Text = b.Year.ToString() }).ToList();
            ViewBag.selectedyear = u_list;

        }

        [HttpGet]
        public ActionResult CommittememProfile_Creation()
        {
            Bindcourse();
            return View();
        }

        //Committee Member profile Creation
        [HttpPost]
        public ActionResult CommittememProfile_Creation(CommitteeMembers_Profile model)
        {

            string tempUsername = User.Identity.Name.ToString();
            Bindcourse();
            CommitteeMembers_Profile Cmprofile = new CommitteeMembers_Profile()
            {
                Name = model.Name,
                Gender = model.Gender,
                MobileNumber = model.MobileNumber,
                EmailID = tempUsername,
                CourseID = model.CourseID,
                Address = model.Address,
                Qualification = model.Qualification,
                CreatedDate = DateTime.Now
            };
            dbcontext.CommitteeMembers_Profile.Add(Cmprofile);
            int save = dbcontext.SaveChanges();
            if (save > 0)
            {
                ViewBag.result = "Committe Member Profile Created Successfully";
                ModelState.Clear();
            }
            else
            {
                ViewBag.result = "Something went wrong!";
            }
            return RedirectToAction("Login", "Account");

        }

        

        

        //Student Additional Question Displaying
        [HttpGet]
        public ActionResult StudentAdditionQuestionRequest()
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

        //Committe Profile Displaying
        [HttpGet]
        public ActionResult CommittememProfile()
        {
            string tempUsername = User.Identity.Name.ToString();

            ViewBag.username = (from a in dbcontext.CommitteeMembers_Profile.Where(x => x.EmailID == tempUsername) select a.Name).FirstOrDefault();
            ViewBag.myprofile = (from a in dbcontext.CommitteeMembers_Profile
                                 join b in dbcontext.Course_master on a.CourseID equals b.Course_ID
                                 where a.EmailID == tempUsername
                                 select new CourseModel
                                 {
                                     Name = a.Name,
                                     Gender = a.Gender,
                                     MobileNumber = a.MobileNumber,
                                     Email = a.EmailID,
                                     CourseName = b.Name,
                                     Address = a.Address,
                                     Qualification = a.Qualification,
                                     CreatedDate = a.CreatedDate

                                 }).ToList();
            return View();
        }

        //User Name Retrieving
        public ActionResult UserName()
        {
            string tempUsername = User.Identity.Name.ToString();

            ViewBag.username = (from a in dbcontext.CommitteeMembers_Profile.Where(x => x.EmailID == tempUsername) select a.Name).FirstOrDefault();
            return View();
        }

        
        
        // Upload Files
        [HttpPost]
        public ActionResult UploadScanfiles(string str, string FileTypeName, StudyMaterial model)
        {
            string userID = User.Identity.Name;
            var userid = (from a in dbcontext.CommitteeMembers_Profile where a.EmailID == userID select a.CmtID).FirstOrDefault();
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
            var userid = (from a in dbcontext.CommitteeMembers_Profile where a.EmailID == userID select a.CmtID).FirstOrDefault();
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

            return RedirectToAction("FileUpload", "CommitteMember", new { msg });
        }


        public JsonResult Getfiles(int? sem, int? cource)
        {
            string userID = User.Identity.Name;
            var userid = (from a in dbcontext.CommitteeMembers_Profile where a.EmailID == userID select a.CmtID).FirstOrDefault();
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
            var userid = (from a in dbcontext.CommitteeMembers_Profile where a.EmailID == userID select a.CmtID).FirstOrDefault();

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