using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
    }
}