using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThetaProject.Models;

namespace ThetaProject.Controllers
{

    public class StudentController : Controller
    {
        private ProjectDBContext ORM = null;

        public StudentController(ProjectDBContext ORM)
        {
            this.ORM = ORM;
        }
        [HttpGet]
        public IActionResult AddStudent()
        {

            return View();
        }
        [HttpPost]
        public IActionResult AddStudent(Student S)
        {
            ORM.Student.Add(S);
            ORM.SaveChanges();
            ViewBag.Message = "New student has added to list";
            return View();
        }
        public IActionResult AllStudents()
        {
            IList<Student> SS = ORM.Student.ToList<Student>();
            return View(SS);
        }
    }

}
