using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        public StudentController(ProjectDBContext ORM , IHostingEnvironment ENV)
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
            ORM.Student.Add(S);
            ORM.SaveChanges();

            MailMessage Obj = new MailMessage();
            Obj.From = new MailAddress("meharsalman073@gamil.com");
            Obj.To.Add(new MailAddress(S.Email));
            Obj.Subject = "Welcome to theta Solution:";
            Obj.Body = "Dear" + S.Name + "<br ><br >" +
            "Thanks for registration with Theta Solution";
            Obj.IsBodyHtml = true;
            if (!string.IsNullOrEmpty(S.Cv))
            {
                Obj.Attachments.Add(new Attachment(ENV.WebRootPath + S.Cv));
            }

            SmtpClient SMTP = new SmtpClient();
            SMTP.Host = "meharsalman073@gamil.com";
            SMTP.Port = 587;
            SMTP.EnableSsl = true;
            SMTP.Credentials = new System.Net.NetworkCredential("meharsalman073@gmail.com", "salman123");

            try
            {
                SMTP.Send(Obj);
            }
            catch(Exception ex)
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

            IList<Student> SS = ORM.Student.Where(m => m.Name.Contains(SearchByName) || m.Dept.Contains(SearchByDept) || m.Address.Contains(SearchByAddress)).ToList<Student>();
            return View(SS);
        }
        public IActionResult DetailStudent(int Id)
        {
            Student S = ORM.Student.Where(m => m.Id == Id).FirstOrDefault<Student>();

            return View(S);
        }
        public IActionResult DeleteStudent(Student S)
        {
            ORM.Student.Remove(S);
            ORM.SaveChanges();
            
            return RedirectToAction("AllStudents");
        }
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
       
        
        
    }

}
