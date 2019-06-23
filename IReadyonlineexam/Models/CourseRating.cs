using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IReadyonlineexam.Models
{
    public class CourseRating
    {
        public int RID { get; set; }
        public int UserID { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public int? Rating { get; set; }
        public int CourceId { get; set; }
        public string CourceName { get; set; }
        public string Description { get; set; }
       
        public int? Price { get; set; }
        public string Imgurl { get; set; }
    }
}