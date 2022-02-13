using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using MVCDTS.Models;
using MVCDTS.DAL;
using Microsoft.AspNetCore.Http;

namespace MVCDTS.Controllers
{
    public class StudentController : Controller
    {
        StudentDAL studentDAL = new StudentDAL();
        public ActionResult ShowDeatils()
        {
            using(DataSet ds = studentDAL.GetStudents())
            {
                ViewBag.student = ds.Tables[0];
            }
            
            return View();
        }

        [HttpGet]
        public ActionResult AddRecord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddRecord(StudentModel studentModel)
        {
            if (ModelState.IsValid)
            {
                studentDAL.InsertStudents(studentModel);
                ViewBag.Res="true";
                return RedirectToAction("ShowDeatils");
            }
            return View(studentModel);
        }

        public ActionResult UpdateRecord(int Id)
        {
            using (DataSet ds = studentDAL.ShowRecordById(Id))
            {
                ViewBag.empRecord = ds.Tables[0];
            }
            return View();
        }

        [HttpPost]
        public ActionResult UpdateRecord(StudentModel studentModel)
        {

            if (ModelState.IsValid)
            {
                studentDAL.UpdateStudent(studentModel);
                return RedirectToAction("ShowDeatils");
            }
            return View();
        }

        public ActionResult DeleteRecord(int Id)
        {
            studentDAL.DeleteStudent(Id);
            return RedirectToAction("ShowDeatils");
        }
    }
}
