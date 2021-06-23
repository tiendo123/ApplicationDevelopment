using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ApplicationDevelopment.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace ApplicationDevelopment.Controllers
{
    [Authorize(Roles = "Staff")]
    public class EnrollmentsController : Controller
    {
        private ApplicationDbContext _db;
        private static int courseId;
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        public EnrollmentsController()
        {
            _db = new ApplicationDbContext();
        }
        // GET: Enrollments
        public ActionResult SelectCourse()
        {
            return View(_db.Courses.Include(t => t.Category).ToList());
        }

        public ActionResult SelectTrainee(int? id)
        {
            if (id != null)
            {
                courseId = id.Value;
            }
            return View(_db.Users.OfType<Trainee>().ToList());
        }

        public ActionResult Enrollment(string Id)
        {
            if (Id == null)
            {
                ViewBag.message = "Error when enroll";
                return RedirectToAction("SelectCourse");
            }
            Enroll enrollment = new Enroll()
            {
                TraineeId = Id,
                CourseId = courseId
            };
            _db.Enrolls.Add(enrollment);
            _db.SaveChanges();
            ViewBag.message = "Enroll Successfully";
            return RedirectToAction("SelectCourse");
        }

        public ActionResult DeleteEnroll(string id)
        {
            if (id == null)
            {
                ViewBag.message = "Error when Delete";
                return RedirectToAction("SelectCourse");
            }
            var enrollment = _db.Enrolls.Where(e => e.CourseId == courseId && e.TraineeId == id).FirstOrDefault();
            _db.Enrolls.Remove(enrollment);
            _db.SaveChanges();
            ViewBag.message = "Delete Successfully";
            return RedirectToAction("SelectCourse");
        }
    }
}