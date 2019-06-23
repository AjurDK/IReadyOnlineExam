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
using Razorpay.Api;

namespace IReadyToday.Controllers
{
    
    public class HomeController : Controller
    {
        public ireadytodayEntities2 dbcontext = new ireadytodayEntities2();

       
        public ActionResult Index()
        {
            var Testimonial = (from a in dbcontext.StudentReviews
                               join b in dbcontext.StudentProfiles on a.StudentUserID equals b.EmailID
                               join c in dbcontext.Course_master on a.course equals c.Course_ID
                               where a.Status != false
                               select new CourseModel
                               {
                                   StudentName = b.Name,
                                   RDescription = a.ReviewDescription,
                                   CourseName = c.Name

                               }).ToList();
            ViewBag.test = Testimonial;

            var coursetab = (from list in dbcontext.Course_master orderby list.CreatedDate descending where list.Course_ID != 0 select list).ToList();
            ViewBag.course = coursetab;

            var Course = (from a in dbcontext.Course_master.Where(x => x.Course_ID != 0) select a).Count();
            ViewBag.Coursecount = Course;

            var Student = (from a in dbcontext.StudentProfiles.Where(x => x.Student_ID != 0) select a).Count();
            ViewBag.Studentcount = Student;

            var Contributor = (from a in dbcontext.ContributorProfiles.Where(x => x.ID != 0) select a).Count();
            ViewBag.Contributorcount = Contributor;

            var CourseRatings = from earning in dbcontext.StudentCourseRatings
                                group earning by earning.CourseID into earns
                                select new CourseRating
                                {
                                    CourseID = (int)earns.Key,
                                    Rating = (int)earns.Average(x => x.Rating),
                                };

            //var CR = (from course in dbcontext.Course_master
            //          join rating in dbcontext.StudentCourseRatings on course.Course_ID equals rating.CourseID
            //          //group rating by rating.CourseID into rate
            //          select new CourseRating
            //          {
            //              //CourseName=course.Name,
            //              CourseID = (int)rate.Key,
            //              Price = course.Price,
            //              Rating=rating.Rating


            //        }).ToList();

            ViewBag.CourseRatings = CourseRatings;

            return View();

            //var coursetab = (from list in dbcontext.Course_master orderby list.CreatedDate descending where list.Course_ID != 0 select list).ToList();
            //return View(coursetab);
            //return RedirectToAction("Index", "Account");

        }

        public ActionResult About()
        {
            

            ViewBag.Message = "Your application description page.";

            return View();
        }

       
        public ActionResult Courses()
        {
            //  ViewBag.Message = "Your contact page.";
            var Courses = (from list in dbcontext.Course_master orderby list.CreatedDate descending where list.Course_ID != 0 select list).ToList();
            var CourseRatings = from earning in dbcontext.StudentCourseRatings
                                group earning by earning.CourseID into earns
                                select new CourseRating
                                {
                                    CourseID = (int)earns.Key,
                                    Rating = (int)earns.Average(x => x.Rating),
                                };

           
            ViewBag.CourseRatings = CourseRatings;
            ViewBag.courses = Courses;
            return View();
            // return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(string firstname, string lastname, string email, string phone, string message)
        {
            SmtpClient smtp1;
            string from = "technical@shlrtechnosoft.in";
            string toEmail = "ashwath@shlrtechnosoft.com";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(from);
                mail.To.Add(toEmail);

                mail.Subject = "IReadyToday Contact us form";

                mail.Body = "Name : " + firstname + Environment.NewLine +
                    "Subject : " + lastname + Environment.NewLine +
                    "Email : " + email + Environment.NewLine +
                    "Contact Number : " + phone + Environment.NewLine +
                    "Message : " + message + Environment.NewLine;


                mail.IsBodyHtml = false;

                smtp1 = new SmtpClient();
                smtp1.Host = "smtp.gmail.com";
                smtp1.Port = 587;

                smtp1.Credentials = new System.Net.NetworkCredential
                ("technical@shlrtechnosoft.in", "Technical@123");

                smtp1.EnableSsl = true;
                smtp1.Send(mail);
            }
            Response.Write("<script>alert('Data Submitted Successfully...Thank you!')</script>");
            return View();
        }

        public ActionResult Aboutus()
        {
            var Testimonial = (from a in dbcontext.StudentReviews
                               join b in dbcontext.StudentProfiles on a.StudentUserID equals b.EmailID
                               join c in dbcontext.Course_master on a.course equals c.Course_ID
                               where a.Status != false
                               select new CourseModel
                               {
                                   StudentName = b.Name,
                                   RDescription = a.ReviewDescription,
                                   CourseName = c.Name

                               }).ToList();
            ViewBag.test = Testimonial;

            var coursetab = (from list in dbcontext.Course_master orderby list.CreatedDate descending where list.Course_ID != 0 select list).ToList();
            ViewBag.course = coursetab;

            var Course = (from a in dbcontext.Course_master.Where(x => x.Course_ID != 0) select a).Count();
            ViewBag.Coursecount = Course;

            var Student = (from a in dbcontext.StudentProfiles.Where(x => x.Student_ID != 0) select a).Count();
            ViewBag.Studentcount = Student;

            var Contributor = (from a in dbcontext.ContributorProfiles.Where(x => x.ID != 0) select a).Count();
            ViewBag.Contributorcount = Contributor;

            

            return View();
        }
        public ActionResult Instructorlist()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult NotFound()
        {
            ViewBag.Text1 = Session["Text1"] as string;
            ViewBag.Text2 = Session["Text2"] as string;
            return View();
        }


        //[HttpPost]
        public ActionResult AddCart(string[] values)
        {
            var result = new List<string>();

            string[] c = values;
            string p = c[0];
            var fooArray = p.Split('&');
            //string dd=// now you have an array of 3 strings
            foreach (var pc in fooArray)
            {
                if (pc == "")
                {
                }
                else
                {
                    result.Add(pc);
                }

            }


            //foreach (var s in values)
            //{

            //    result.Add(v);
            //}

            //var proid = Session["cartproduct"] as string;

            ViewBag.pid = result;

            return View();
            //return Json(values, JsonRequestBehavior.AllowGet);
        }

        public JsonResult cartitems(string[] values)
        {
            string[] c = values;
            string p = c[0];

            var pp = p;

            return Json(pp, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Carttable(string values)
        {
            var result = new List<string>();

            if (values != null)
            {
                //string[] c = values;
                string p = values;
                var fooArray = p.Split('A');
                //string dd=// now you have an array of 3 strings
                foreach (var pc in fooArray)
                {
                    if (pc == "")
                    {
                    }
                    else
                    {

                        result.Add(pc);
                    }

                }


                //for(int v=0; v<result.Count;v++)
                //{
                //    bool dup = false;
                //    for (int i=0;i<v;i++)
                //    {

                //        if(result[i]==result[v])
                //        {
                //            dup = true;
                //            duplicate.Add(result[i]);
                //        }

                //    }
                //    if(!dup)
                //    {
                //        Uniqeid.Add(result[v]);
                //    }
                //}


                ViewBag.pid = result;
            }
            ViewBag.pid = result;
            //return View(ViewBag.pid);
            return View();
        }



        public ActionResult Checkout()
        {




            return View();
        }


        public ActionResult CourseDetails(int? id)
        {
            if (id != null)
            {
                //var cource_details = dbcontext.Course_master.Where(c => c.Course_ID == id
            
                var cource_details = (from cu in dbcontext.Course_master
                                      where cu.Course_ID == id

                                      select new CourceDetails
                                      {
                                          CourceId = cu.Course_ID,
                                          CourceName = cu.Name,
                                          Description = cu.Description,
                                          Price = cu.Price,
                                          imgurl = cu.Imageurl,
                                          rating = cu.Rating
                                      }

                                    ).ToList();
                ViewBag.Cource_details = cource_details;
             

            }
            else
            {
                Response.Write("<script>alert('Course Not Found....!')</Script>");
            }
            return View();
        }


        //save coures in db
        public ActionResult SaveCoures(string values)
        {
            var result = new List<string>();
            var Pricelist = new List<int?>();

            if(values=="")
            {

            }

            if (values!="" && values != null)
            {
                //string[] c = values;
                string p = values;
                var fooArray = p.Split('A');
                //string dd=// now you have an array of 3 strings
                foreach (var pc in fooArray)
                {
                    if (pc == "")
                    {
                    }
                    else
                    {

                        result.Add(pc);
                    }

                }
            
            foreach (var i in result)
            {
                if (i != "undefined")
                {
                    int id = Int32.Parse(i);
                    int? price = dbcontext.Course_master.Where(a => a.Course_ID == id).Select(a => a.Price).FirstOrDefault();
                    Pricelist.Add(price);
                }
                    
            }

            int? finalPrice = Pricelist.Sum();

            Session["Price"] = finalPrice;

            //if(User.Identity.IsAuthenticated)
            //{
            //    // redirect him to payment page
            //}
            //else
            //{
            //    //login page
            //}

           // return View();
           // return RedirectToAction("Payment", new { id = 99 }values);
                return RedirectToAction("Payment", "Home", new { @values = values });
            }
            else
            {
               // return View();
                return RedirectToAction("Carttable");

            }
        }

     //  [Authorize(Roles = "Student")]
        [HttpGet]
        public ActionResult Payment(string values)
        {
            string OrderID;
           try
            {
                int price = Int32.Parse(Session["Price"].ToString());

                var courses = new List<string>();
                var result = new List<string>();
                var Pricelist = new List<int?>();

                if (values != "" && values != null)
                {
                    //string[] c = values;
                    string p = values;
                    var fooArray = p.Split('A');
                    //string dd=// now you have an array of 3 strings
                    foreach (var pc in fooArray)
                    {
                        if (pc == "")
                        {
                        }
                        else
                        {

                            result.Add(pc);
                        }

                    }

                    var crs_list = new List<Course_master>();
                    foreach (var i in result)
                    {
                        if (i != "undefined")
                        {
                            int id = Int32.Parse(i);
                            var course_price = dbcontext.Course_master.Where(a => a.Course_ID == id).Select(a => new {a.Price, a.Name}).FirstOrDefault();
                            string course = dbcontext.Course_master.Where(a => a.Course_ID == id).Select(a => a.Name).FirstOrDefault();
                              Pricelist.Add(course_price.Price);
                           
                            courses.Add(course_price.Name);
                        }

                    }
                }

                   ViewBag.Courses = courses;
                   ViewBag.Pricelist = Pricelist;

                    //To create order for new payment

                    RazorpayClient Clients = new RazorpayClient("rzp_test_OIkqkKP1mCz6bE", "F3Jl0GAAisSB1Y54WsxyhnHo");
                Dictionary<string, object> input = new Dictionary<string, object>();
                input.Add("amount", Convert.ToInt32(price + "00")); // this amount should be same as transaction amount ** Note: Amount calculated is in paisa
                input.Add("currency", "INR");
                input.Add("receipt", "Iready" + DateTime.Now);
                input.Add("payment_capture", 1);


                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                //order creation

                Razorpay.Api.Order order = Clients.Order.Create(input);
                OrderID = order["id"].ToString();

                Session["OrderID"] = OrderID;
                Session["Price"] = price;

              

                string getName = dbcontext.StudentProfiles.Where(a => a.EmailID == User.Identity.Name).Select(a => a.Name).FirstOrDefault();

                Session["User"] = getName;


                return View();
                
            }
            catch (NullReferenceException)
            {
                TempData["Error"] = "Your session has expired, Please try again";
                return RedirectToAction("ErrorPage");
            }
        }

        [HttpGet]
        public ActionResult ErrorPage()
        {
            TempData.Keep();
            return View();
        }

        [Authorize(Roles = "Student")]
        public ActionResult PaymentSuccess(string payid)
        {
            ViewBag.PaymentID = payid;

            //Course Subscription - Write Save Function

            return RedirectToAction("Studentregistration", "student");
        }








    }
}