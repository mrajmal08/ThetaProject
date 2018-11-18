using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThetaProject.Models;


namespace ThetaProject.Controllers
{

    public class StudentController : Controller
    {
        private ProjectDBContext ORM = null;
        private IHostingEnvironment ENV = null;

        public StudentController(ProjectDBContext ORM , IHostingEnvironment ENV )
        {
            this.ORM = ORM;
            this.ENV = ENV;

        }
        [HttpGet]
        public IActionResult AddStudent()
        {

            return View();
        }
        [HttpPost]
        public IActionResult AddStudent(Student S , IFormFile Cv)
        {
            
            String CVPath = ENV.WebRootPath + "/WebData/CVs/";
            String CVName = Guid.NewGuid().ToString();
            String CVExtension = Path.GetExtension(Cv.FileName);

            FileStream FS = new FileStream(CVPath + CVName + CVExtension, FileMode.Create);
            Cv.CopyTo(FS);
            FS.Close();
            S.Cv = "/WebData/CVs/" + CVName + CVExtension;

                S.CreatedBy = HttpContext.Session.GetString("LIUID");
                ORM.Student.Add(S);
                ORM.SaveChanges();

            //String ApiUrl = "http://bulksms.com.pk/api/sms.php?username=923338311685&password=2915&sender=BrandName&mobile=" + S.Contact + "@message=WelCome to Our team";
            //var APIClient = new HttpClient();
            //var RM = APIClient.GetAsync(ApiUrl);
            //var FR = RM.Result.Content.ReadAsStringAsync();
            String ApiUrl = "http://bulksms.com.pk/api/sms.php?username=923044300620&password=1836&sender=BrandName&mobile=" + S.Contact + "@Message = Welcome toour team";
            var APIClient = new HttpClient();
            var RM = APIClient.GetAsync(ApiUrl);
            var FR = RM.Result.Content.ReadAsStringAsync();

            //Email...
            MailMessage Obj = new MailMessage();
            Obj.From = new MailAddress("meharsalman073@gmail.com");
            Obj.To.Add(new MailAddress(S.Email));
            Obj.Subject = "Welcome to theta Solution:";
            Obj.Body = "Dear"+""+"Mr "+" " + S.Name + "<br ><br >" +
            "Thanku for registration with Theta Solution"+"<br><br>"+
            "Reguards Mr Ajmal...";
            Obj.IsBodyHtml = true;
            if (!string.IsNullOrEmpty(S.Cv))
            {
                Obj.Attachments.Add(new Attachment(ENV.WebRootPath + S.Cv));
            }

            //
            

            //

            SmtpClient SMTP = new SmtpClient();
            SMTP.Host = "smtp.gmail.com";
            SMTP.Port = 587;
            SMTP.EnableSsl = true;
            SMTP.Credentials = new System.Net.NetworkCredential("meharsalman073@gmail.com", "salman123");

            try
            {
                SMTP.Send(Obj);
            }
            catch(Exception )
            {
                ViewBag.Message = "Mail has sent successfully";
            }


            ViewBag.Message = "New student has added to list";
            return View();
        }
        [HttpGet]
        public IActionResult AllStudents()
        {
            IList<Student> SS = ORM.Student.ToList<Student>();
            return View(SS);
        }
        [HttpPost]
        public IActionResult AllStudents(String SearchByName , String SearchByDept , String SearchByAddress)
        {
            if (HttpContext.Session.GetString("LIUID") == null)
            {
                return RedirectToAction("Login");
            }

            IList<Student> SS = ORM.Student.Where(m => m.Name.Contains(SearchByName) || m.Dept.Contains(SearchByDept) || m.Address.Contains(SearchByAddress)).ToList<Student>();

        
            return View(SS);
        }
        public IActionResult DetailStudent(int Id)
        {
            Student S = ORM.Student.Where(m => m.Id == Id).FirstOrDefault<Student>();

            return View(S);
        }


        //Delete Operation
        public IActionResult DeleteStudent(Student S)
        {
            ORM.Student.Remove(S);
            ORM.SaveChanges();
            return RedirectToAction("AllStudents");
        }

        public IActionResult DelStudent(Student S)
        {
            ORM.Student.Remove(S);
            ORM.SaveChanges();
            return RedirectToAction("AllStudents");
        }


        public String deletestudentajax(Student S)
        {
            String result = "";
            try
            {
                ORM.Student.Remove(S);
                ORM.SaveChanges();
                result = "Yes";

            }
            catch(Exception )
            {
                result = "No";
            }

           
            
            return result;
        }

        //Edit Student
        [HttpGet]
        public IActionResult EditStudent(int Id)
        {
            Student S = ORM.Student.Where(m => m.Id == Id).FirstOrDefault<Student>();

            return View(S);
        }
        [HttpPost]
        public IActionResult EditStudent(Student S)
        {
            ORM.Student.Update(S);
            ORM.SaveChanges();

            return RedirectToAction("AllStudents");
        }
        


        public FileResult DownloadCV(String Id)
        {
            if (String.IsNullOrEmpty(Id))
            {
                ViewBag.Message = "There is nothing in this link";
                return null; 
            }
            var M = new MimeSharp.Mime();
            String Extension = Path.GetExtension(Id);
            String Name = DateTime.Now.ToString("ddmmyyyy") + Extension;


            return File(Id , M.Lookup(Id), Name);
        }

       

        public String ShowAdd()
        {
            String Add = "";


            Add = "<img class='img img-responsive' src = 'http://lorempixel.com/400/200/sports/1/'/>";

            return Add;
        }
        public String AllStudentsList()
        {
            String result = "";
            IList<Student> Model = ORM.Student.ToList<Student>();
            result += "<h1 class='alert alert-success' > Total Students: "+Model.Count+"</h1>";

            foreach(Student S in Model)
            {
               result += "<a href='/Student/DetailStudent?id=" +S.Id+ "'><p><span class='glyphicon glyphicon-user'></span> " +S.Name+ "</p></a> <a href='/Student/DelStudent?id'"+S.Id+" Delete </a> ";

            }


            return result;

        }
        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginUser U)
        {
            LoginUser LU = ORM.LoginUser.Where(m => m.Email == U.Email && m.Password == U.Password).FirstOrDefault<LoginUser>();
            if (LU == null)
            {
                ViewBag.Message = "Invalid User Name or Password";
                return View();
            }
            HttpContext.Session.SetString("LIUID", LU.Id.ToString());
            return RedirectToAction("AllStudents");

        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(LoginUser U)
        {
            var userWithSameEmail = ORM.LoginUser.Where(m => m.Email == U.Email).SingleOrDefault(); //checking if the emailid already exits for any user
            if (ModelState.IsValid)
            {
                if (userWithSameEmail == null)
                {
                    ORM.LoginUser.Add(U);
                    ORM.SaveChanges();
                    ViewBag.Message = "Registration Done";
                    return RedirectToAction("AllStudents");
                }
                else
                {
                    ViewBag.Message = "User with this Email Already Exist";
                    return View("SignUp");
                }
            }

            else
            {
                return View("AllStudents");
            }
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();

            return RedirectToAction ("Login");
        }



    }

}
