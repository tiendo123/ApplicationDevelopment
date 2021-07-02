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
    public class DepartmentsController : Controller
    {
        private ApplicationDbContext _db;
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public DepartmentsController()
        {
            _db = new ApplicationDbContext();
        }

        public ActionResult Index(string departName)
        {
            if (String.IsNullOrWhiteSpace(departName))
            {
                return View(_db.Departments.ToList());
            }
            return View(_db.Departments.Where(t => t.DepartmentName.Contains(departName)).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Department c)
        {
            var IfDepartmentExist = _db.Departments.SingleOrDefault(t => t.DepartmentName == c.DepartmentName);
            if (IfDepartmentExist != null)
            {
                ViewBag.message = "This Department had been created!";
                return View();
            }
            else
            {
                _db.Departments.Add(c);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(int Id)
        {
            var department = _db.Departments.SingleOrDefault(t => t.Id == Id);
            return View(department);
        }

        [HttpPost]
        public ActionResult Edit(Department c)
        {
            var department = _db.Departments.SingleOrDefault(t => t.Id == c.Id);
            department.DepartmentName = c.DepartmentName;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int Id)
        {
            var department = _db.Departments.SingleOrDefault(t => t.Id == Id);
            _db.Departments.Remove(department);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}