using ApplicationDevelopment.Helper;
using ApplicationDevelopment.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApplicationDevelopment.Controllers
{
    [Authorize(Roles = "Staff")]
    public class CategoriesController : Controller
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
        public CategoriesController()
        {
            _db = new ApplicationDbContext();
        }
        public CategoriesController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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
        
        public ActionResult Index(string cateName)
        {
            if (String.IsNullOrWhiteSpace(cateName))
            {
                return View(_db.Categories.ToList());
            }
            return View(_db.Categories.Where(t => t.Name.Contains(cateName)).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }
      
        [HttpPost]
        public ActionResult Create(Category c)
        {
            var IfCategoryExist = _db.Categories.SingleOrDefault(t => t.Name == c.Name);
            if (IfCategoryExist != null)
            {
                ViewBag.message = "This Category had been created!";
                return View();
            }
            else
            {
                _db.Categories.Add(c);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        
        public ActionResult Edit(int Id)
        {
            var cate = _db.Categories.SingleOrDefault(t => t.Id == Id);
            return View(cate);
        }
        
        [HttpPost]
        public ActionResult Edit(Category c)
        {
            var catedb = _db.Categories.SingleOrDefault(t => t.Id == c.Id);
            catedb.Name = c.Name;
            catedb.Description = c.Description;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
       
        public ActionResult Delete(int Id)
        {
            var Catedb = _db.Categories.SingleOrDefault(t => t.Id == Id);
            _db.Categories.Remove(Catedb);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}