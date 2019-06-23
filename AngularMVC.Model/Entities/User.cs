using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularMVC.Model.Entities
{
   public class User
    {
        public static object Identity { get; set; }
        public Guid UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
