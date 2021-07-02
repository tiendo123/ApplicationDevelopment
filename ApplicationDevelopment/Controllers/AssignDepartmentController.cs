using ApplicationDevelopment.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApplicationDevelopment.Controllers
{
    [Authorize(Roles = "Staff")]
    public class AssignDepartmentController : Controller
    {
        private ApplicationDbContext _db;
        private static int departmentId;
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        public AssignDepartmentController()
        {
            _db = new ApplicationDbContext();
        }
        public ActionResult SelectDepartment()
        {
            return View(_db.Departments.ToList());
        }

        public ActionResult SelectTrainer(int? id)
        {
            if (id != null)
            {
                departmentId = id.Value;
            }
            return View(_db.Users.OfType<Trainer>().ToList());
        }
        public ActionResult Assignment(string Id)
        {
            if (Id == null)
            {
                ViewBag.message = "Error when Assignment";
                return RedirectToAction("SelectDepartment");
            }
            DepartmentAssign department = new DepartmentAssign()
            {
                TrainerId = Id,
                DepartmentId = departmentId
            };
            var assignExist = _db.DepartmentAssigns.Where(c => c.TrainerId == Id);
            if (assignExist.Any())
            {
                ViewBag.message = "Error when enroll";
                return RedirectToAction("SelectTrainer");
            }
            _db.DepartmentAssigns.Add(department);
            _db.SaveChanges();
            ViewBag.message = "Assignment Successfully";
            return RedirectToAction("SelectDepartment");
        }
        public ActionResult DeleteAssignment(string id)
        {
            if (id == null)
            {
                ViewBag.message = "Error when Delete";
                return RedirectToAction("SelectCourse");
            }
            var assignment = _db.DepartmentAssigns.Where(e => e.DepartmentId == departmentId && e.TrainerId == id).FirstOrDefault();
            _db.DepartmentAssigns.Remove(assignment);
            _db.SaveChanges();
            ViewBag.message = "Delete Successfully";
            return RedirectToAction("SelectDepartment");
        }
    }
}