using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IReadyonlineexam.Models.Json
{
    public class JsonResult
    {
        public JsonResult()
        {
            Success = false;
            Message = "";
            NextUrl = "";
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public string NextUrl { get; set; }
        public string Json { get; set; }

        /// <summary>
        /// for extra information about response ex: pagination paging { ispaged:'true' , currentPage:1, totalPage:'12' , pagesize: 15 }
        /// </summary>
        public object Details { get; set; }
    }
}