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
    [Authorize(Roles = "Trainee")]
    public class TraineeController : Controller
    {
        private ApplicationDbContext _db;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        public TraineeController()
        {
            _db = new ApplicationDbContext();
        }
        public TraineeController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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
            var user = _db.Trainees.Find(userIdValue);
            return View(user);
        }
        [HttpPost]
        public ActionResult Edit(Trainee trainee)
        {
            var tranerUser = _db.Users.Find(trainee.Id);

            if (tranerUser == null)
            {
                ViewData["Message"] = "Error: User not already exists";
                return View(trainee);
            }
            var traineeProfile = _db.Trainees.Find(trainee.Id);
            traineeProfile.FullName = trainee.FullName;
            traineeProfile.DateOfBirth = trainee.DateOfBirth;
            traineeProfile.Education = trainee.Education;
            traineeProfile.MainProgrammingLanguage = trainee.MainProgrammingLanguage;
            traineeProfile.ToeicScore = trainee.ToeicScore;
            traineeProfile.ExperimentDetail = trainee.ExperimentDetail;
            traineeProfile.Location = trainee.Location;
            traineeProfile.Department = trainee.Department;
            traineeProfile.PhoneNumber = trainee.PhoneNumber;

            _db.Entry(traineeProfile).State = EntityState.Modified;
            _db.SaveChanges();
            ViewData["Message"] = "Success update profile";
            return View(trainee);
        }

        public ActionResult CourseEnrolled()
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
            return View(_db.Enrolls.Where(c => c.TraineeId == userIdValue).Include(c => c.Course).ToList());
        }

        public ActionResult CourseAvailable()
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
            List<Course> availablecourse = new List<Course>();
            var allcourse = _db.Courses.ToList();
            var enrollments = _db.Enrolls.Where(c => c.TraineeId == userIdValue).Include(c => c.Course).ToList();
            availablecourse = allcourse.Except(allcourse.Where(i => enrollments.Select(o => o.CourseId).ToList().Contains(i.Id))).ToList();
            return View(availablecourse);
        }
    }
}