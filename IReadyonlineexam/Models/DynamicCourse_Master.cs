using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IReadyonlineexam.Models
{
    public class DynamicCourse_Master
    {
        public int Course_ID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Title { get; set; }
        [DisplayName("UploadFile")]
        public int Price { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public int Rating { get; set; }
        public string ImageUrl { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
    public partial class DynamicSubject
    {
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Name { get; set; }
        [DisplayName("UploadFile")]
        public string ImageUrl { get; set; }
        public int Course { get; set; }
        public int Semester { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public HttpPostedFileBase ImageData { get; set; }



    }

}