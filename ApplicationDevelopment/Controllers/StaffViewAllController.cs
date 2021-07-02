using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ApplicationDevelopment.Models;
using ApplicationDevelopment.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;


namespace ApplicationDevelopment.Controllers
{
    [Authorize(Roles = "Staff")]
    public class StaffViewAllController : Controller
    {
        private ApplicationDbContext _db;
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        public StaffViewAllController()
        {
            _db = new ApplicationDbContext();
        }
        public ActionResult SelectCourse()
        {
            return View(_db.Courses.Include(t => t.Category).ToList());
        }
        public ActionResult StaffViewAll(int Id)
        {
            StaffViewAllVm staffViewAllVm = new StaffViewAllVm();
            staffViewAllVm.TraineeList = _db.Enrolls.Where(e => e.CourseId == Id).Include(e => e.Trainee);
            staffViewAllVm.TrainerList = _db.Assigns.Where(a => a.CourseId == Id).Include(a => a.Trainer);

            return View(staffViewAllVm);
        }
        public ActionResult SelectDepartment()
        {
            return View(_db.Departments.ToList());
        }
        public ActionResult StaffViewDepartmentAll(int Id)
        {
            return View(_db.DepartmentAssigns.Where(d=> d.DepartmentId == Id).Include(d => d.Trainer).ToList());
        }
    }
}