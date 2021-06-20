using ApplicationDevelopment.Helper;
using ApplicationDevelopment.Models;
using ApplicationDevelopment.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ApplicationDevelopment.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class UsersController : Controller
    {
        private ApplicationDbContext _db;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        public UsersController()
        {
            _db = new ApplicationDbContext();
        }
        public UsersController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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
        public async Task<ActionResult> Index()
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
            var userList = await UserManager.Users.ToListAsync();
            List<BaseUser> baseUserlist = new List<BaseUser>();
            foreach (var user in userList)
            {
                var roleTemp = await UserManager.GetRolesAsync(user.Id);
                BaseUser baseUser = new BaseUser();
                baseUser.Role = roleTemp.FirstOrDefault();
                baseUser.Email = user.Email;
                baseUser.UserName = user.UserName;
                baseUser.Id = user.Id;
                baseUserlist.Add(baseUser);
            }
            if (User.IsInRole("Admin"))
            {
                var userOfAdmin = baseUserlist.Where(u => u.Role != "Trainee" && u.Id != userIdValue);
                return View(userOfAdmin);
            }
            
            var UserOfStaff = baseUserlist.Where(u => u.Role == "Trainer" || u.Role == "Trainee").Where(u=> u.Id != userIdValue);
            return View(UserOfStaff);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            var user = _db.Users.Find(id);

            if (user == null)
            {
                return View();
            }
      
            var roleTemp = await UserManager.GetRolesAsync(user.Id);
            var role = roleTemp.FirstOrDefault();
            UsersViewModel usersVm = new UsersViewModel();
            if (role == "Trainer")
            {
                var trainer = _db.Trainers.Find(id);
                usersVm.Trainer = trainer;
            }
            else if (role == "Trainee")
            {
                var trainee = _db.Trainees.Find(id);
                usersVm.Trainee = trainee;
            }
            else if (role == "Admin")
            {
                var admin = _db.Admins.Find(id);
                usersVm.Admin = admin;
            }
            else
            {
                var staff = _db.Staffs.Find(id);
                usersVm.Staff = staff;
            }
            return View(usersVm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UsersViewModel user)
        {
            UsersViewModel usersVm = new UsersViewModel();
            if (user.Admin != null)
            {
                var adminUser = _db.Users.Find(user.Admin.Id);

                if (adminUser == null)
                {
                    ViewData["Message"] = "Error: User not already exists";
                    return View(usersVm);
                }

                var admin = _db.Admins.Find(user.Admin.Id);
                admin.FullName = user.Admin.FullName;

                _db.Entry(admin).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else if (user.Staff != null)
            {
                var staffUser = _db.Staffs.Find(user.Staff.Id);

                if (staffUser == null)
                {
                    ViewData["Message"] = "Error: User not already exists";
                    return View(usersVm);
                }

                var staff = _db.Staffs.Find(user.Staff.Id);
                staff.FullName = user.Staff.FullName;

                _db.Entry(staff).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else if (user.Trainee != null)
            {
                var traneeUser = _db.Users.Find(user.Trainee.Id);

                if (traneeUser == null)
                {
                    ViewData["Message"] = "Error: User not already exists";
                    return View(usersVm);
                }
                var traineeProfile = _db.Trainees.Find(user.Trainee.Id);
                traineeProfile.FullName = user.Trainee.FullName;
                traineeProfile.DateOfBirth = user.Trainee.DateOfBirth;
                traineeProfile.Education = user.Trainee.Education;
                traineeProfile.MainProgrammingLanguage = user.Trainee.MainProgrammingLanguage;
                traineeProfile.ToeicScore = user.Trainee.ToeicScore;
                traineeProfile.ExperimentDetail = user.Trainee.ExperimentDetail;
                traineeProfile.Location = user.Trainee.Location;
                traineeProfile.Department = user.Trainee.Department;
                traineeProfile.PhoneNumber = user.Trainee.PhoneNumber;

                _db.Entry(traineeProfile).State = EntityState.Modified;
                _db.SaveChanges();
              
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var tranerUser = _db.Users.Find(user.Trainer.Id);

                if (tranerUser == null)
                {
                    ViewData["Message"] = "Error: User not already exists";
                    return View(usersVm);
                }
                var trainerProfile = _db.Trainers.Find(user.Trainer.Id);

                trainerProfile.FullName = user.Trainer.FullName;
                trainerProfile.WorkingPlace = user.Trainer.WorkingPlace;
                trainerProfile.Type = user.Trainer.Type;
                trainerProfile.PhoneNumber = user.Trainer.PhoneNumber;

                _db.Entry(trainerProfile).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            var applicationUser = _db.BaseUsers.Find(id);
            if (applicationUser == null)
            {
                ViewData["Message"] = "Error: User not exists";
            }
            _db.BaseUsers.Remove(applicationUser);
            _db.SaveChanges();
            ViewData["Message"] = "Success: User Delete Successfully";
            return RedirectToAction("Index");
        }
 
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
           
            return View();
        }

        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}