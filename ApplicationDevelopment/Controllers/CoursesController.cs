﻿using ApplicationDevelopment.Helper;
using ApplicationDevelopment.Models;
using ApplicationDevelopment.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApplicationDevelopment.Controllers
{
    [Authorize(Roles = "Staff")]
    public class CoursesController : Controller
    {
        private ApplicationDbContext _db;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        public CoursesController()
        {
            _db = new ApplicationDbContext();
        }
        public CoursesController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Courses
        public ActionResult Index(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return View(_db.Courses.Include(t => t.Category).ToList());
            }
            return View(_db.Courses.Where(t => t.Name.Contains(name)).Include(t => t.Category).ToList());
        }

        
        public ActionResult Create()
        {
            var selectedcategorylist = new CourseCategoryViewModel()
            {
                Categories = _db.Categories.ToList()
            };
            return View(selectedcategorylist);
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseCategoryViewModel model)
        {
            var IfCourseExist = _db.Courses.SingleOrDefault(t => t.Name == model.Course.Name);
            if (IfCourseExist != null)
            {
                var selectedcategorylist = new CourseCategoryViewModel()
                {
                    Categories = _db.Categories.ToList()
                };
                ViewBag.message = "This Course had been created";
                return View(selectedcategorylist);
            }
            else
            {
                _db.Courses.Add(model.Course);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

        }
       
        public ActionResult Edit(int Id)
        {
            var modelInfo = new CourseCategoryViewModel()
            {
                Course = _db.Courses.SingleOrDefault(t => t.Id == Id),
                Categories = _db.Categories.ToList()
            };
            return View(modelInfo);
        }
       
        [HttpPost]
        public ActionResult Edit(CourseCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var modelInfo = new CourseCategoryViewModel()
                {
                    Course = model.Course,
                    Categories = _db.Categories.ToList()
                };
                return View(modelInfo);
            }
            var findCourse = _db.Courses.SingleOrDefault(c => c.Id == model.Course.Id);
            if (findCourse == null)
            {
                return HttpNotFound();
            }
            findCourse.Name = model.Course.Name;
            findCourse.Description = model.Course.Description;
            findCourse.CategoryId = model.Course.CategoryId;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
     
        public ActionResult Delete(int Id)
        {
            var findCourse = _db.Courses.SingleOrDefault(c => c.Id == Id);
            _db.Courses.Remove(findCourse);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}