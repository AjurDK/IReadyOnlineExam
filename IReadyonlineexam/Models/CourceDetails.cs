using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IReadyonlineexam.Models
{
    public class CourceDetails
    {
        public int CourceId { get; set; }
        public string CourceName { get; set; }
        public string Description { get; set; }
        public int? rating { get; set; }
        public int? Price { get; set; }
        public string imgurl { get; set; }
        
    }
}