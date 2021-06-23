using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ApplicationDevelopment.Helper;
using ApplicationDevelopment.Models;
using ApplicationDevelopment.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace ApplicationDevelopment.Controllers
{
    [Authorize(Roles = "Trainer")]
    public class TrainersController : Controller
    {
        private ApplicationDbContext _db;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        public TrainersController()
        {
            _db = new ApplicationDbContext();
        }
        public TrainersController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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

        public ActionResult Edit()
        {
            string userIdValue = String.Empty;
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    userIdValue = userIdClaim.Value;
                }
            }
            var user = _db.Trainers.Find(userIdValue);
            return View(user);
        }
        [HttpPost]
        public ActionResult Edit(Trainer trainer)
        {
            var tranerUser = _db.Users.Find(trainer.Id);

            if (tranerUser == null)
            {
                ViewData["Message"] = "Error: User not already exists";
                return View(trainer);
            }
            var trainerProfile = _db.Trainers.Find(trainer.Id);

            trainerProfile.FullName = trainer.FullName;
            trainerProfile.WorkingPlace = trainer.WorkingPlace;
            trainerProfile.Type = trainer.Type;
            trainerProfile.PhoneNumber = trainer.PhoneNumber;

            _db.Entry(trainerProfile).State = EntityState.Modified;
            _db.SaveChanges();
            ViewData["Message"] = "Success update profile";
            return View(trainer);
        }

        public ActionResult CourseAssigned()
        {
            string userIdValue = String.Empty;
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    userIdValue = userIdClaim.Value;
                }
            }
            return View(_db.Assigns.Where(c => c.TrainerId == userIdValue).Include(c => c.Course).ToList());
        }

    }
}