using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IReadyonlineexam.Models
{
    public class ProfileModel
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string CollegeName { get; set; }
        public string Address { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public string SemName { get; set; }
        public int SemID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Image { get; set; }
    }

    public class Studentsyllabumodel
    {

        public int ssid { get; set; }
        public Nullable<int> SubjectID { get; set; }
        public Nullable<int> Uid { get; set; }
        [Required(ErrorMessage = "Please select Course")]
        public int Course { get; set; }
        [Required(ErrorMessage = "Please select Semester")]
        public int Semester { get; set; }
        public string Name { get; set; }
        public string UnitDescription { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public virtual Subject_Master Subject_Master { get; set; }
        public virtual UnitMaster UnitMaster { get; set; }


    }

    public class RatingModel
    {
        public int? UserID { get; set; }
        public int CourseID { get; set; }
        public int Rating { get; set; }
    }


    public class CourseModel
    {

        public int MemID { get; set; }
        public int? Uid { get; set; }
        public string UnitName { get; set; }
        public string ChapterName { get; set; }
        public string ChapterDesciption { get; set; }
        public int SemID { get; set; }
        public string CollegeName { get; set; }
        public int PhoneNo { get; set; }
        public int SATID { get; set; }
        public string Time { get; set; }
        public int? Numberofattempts { get; set; }
        public string SemName { get; set; }
        public int? SubjectID { get; set; }
        public string SubjectName { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public int? YearID { get; set; }
        public int? YearName { get; set; }
        public int? Membershipcost { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Ssid { get; set; }
        public string Designation { get; set; }
        public string CompanyName { get; set; }
        public string JobDescription { get; set; }
        public DateTime? InterviewDate { get; set; }
        public string InterviewLocation { get; set; }
        public int JobID { get; set; }
        public int ADQID { get; set; }
        public string AQDescription { get; set; }

        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        
        public string Qualification { get; set; }

        public int? Q_Numeber { get; set; }
        public string Q_Name { get; set; }
        public string Q_op1 { get; set; }
        public string Q_op2 { get; set; }
        public string Q_op3 { get; set; }
        public string Q_op4 { get; set; }
        public string Q_Ans { get; set; }
        public int Quizid { get; set; }

        public string ReviewDescription { get; set; }
        public DateTime? ReviewDate { get; set; }

        public Boolean? Status { get; set; }
        public int SrID { get; set; }

        public int? QuizID { get; set; }
        public string QuizName { get; set; }
        public string Description { get; set; }

        public int QuestionID { get; set; }
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }
        public Boolean? IsAnwer { get; set; }

        public int? ScoredMarks { get; set; }
        public int? TotalMarks { get; set; }

        public string StudentName { get; set; }
        public string RDescription { get; set; }
        public string Institution_Name { get; set; }
        public string Website { get; set; }
        public string CurrentPosition { get; set; }
        public string InstitutionName { get; set; }
        public string Skills { get; set; }
        public string Experience { get; set; }




    }
    public class ContAnsModel
    {
        public int QuestionID { get; set; }
        public int? StudentID { get; set; }
        public string CourseName { get; set; }
        public string SemName { get; set; }
        public string SubjectName { get; set; }
        public string UnitName { get; set; }
        public string QuestinoHeading { get; set; }
        public string QuestionDescription { get; set; }
        public String Answer { get; set; }
        public bool IsAnswered { get; set; }
        public DateTime QuestDate { get; set; }

    }

}