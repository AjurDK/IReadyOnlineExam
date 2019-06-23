using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IReadyonlineexam.Models
{
    public class ContributorModel
    {

    }
    public class ContributorProfileModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string PhoneNo { get; set; }
        [DataType(DataType.MultilineText)]
        public string Skills { get; set; }
        public string Website { get; set; }
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Image { get; set; }
        public string Gender { get; set; }

        public Nullable<int> CourseID { get; set; }
        public string CourseName { get; set; }

        public string Institution_Name { get; set; }
        public string Experience { get; set; }
        public string Current_Position { get; set; }
    }
    public class MyQuizModel
    {
        [Required(ErrorMessage = "CourseID is required.")]
        public int CourseID { get; set; }
        [Required(ErrorMessage = "CourseName is required.")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "Semester is required.")]
        public int SemesterID { get; set; }
        public string SemesterName { get; set; }

        public int SubjectID { get; set; }
        public string SubjectName { get; set; }

        public int UnitID { get; set; }
        public string UnitName { get; set; }

        public int QuizID { get; set; }
        //[Required(ErrorMessage = "QuizName is required.")]
        public string QuizName { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }
    }

    public class MyAnswerModel
    {
        public int QuizID { get; set; }
        public string QuizName { get; set; }

        public int QuestionID { get; set; }
        public string QuestionName { get; set; }


        public int AnswerID { get; set; }
        public string AnswerName { get; set; }
        public bool isAnswer { get; set; }
    }

    public class MyQuestionModel
    {
        public int QuizID { get; set; }
        public string QuizName { get; set; }

        public int QuestionID { get; set; }
        public string QuestionName { get; set; }

        public List AnswerEntries { get; set; }
    }

    public class QuestionRequestModel
    {
        public string StudentName { get; set; }
        public string Course { get; set; }
        public string Update { get; set; }
    }

    public class JobModel
    {
        public int JobID { get; set; }

        public Nullable<int> YearID { get; set; }
        public int YearName { get; set; }

        public Nullable<int> CourseID { get; set; }
        public string CourseName { get; set; }

        public Nullable<int> SemesterID { get; set; }
        public string SemesterName { get; set; }

        public string Designation { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }

        public Nullable<System.DateTime> ScheduledDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public int Days
        {
            get
            {
                if (!CreatedDate.HasValue)
                    return 1;

                return (DateTime.Now - CreatedDate.Value).Days;
            }
        }

    }

    public class StudyMaterialModel
    {
        public int ID { get; set; }

        public Nullable<int> CourseID { get; set; }
        public string CourseName { get; set; }

        public Nullable<int> SemesterID { get; set; }
        public string SemesterName { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Attachement { get; set; }

        public string UploadedBy { get; set; }
        public string ContributorID { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int Days
        {
            get
            {
                if (!CreatedDate.HasValue)
                    return 1;

                return (DateTime.Now - CreatedDate.Value).Days;
            }
        }
    }
}