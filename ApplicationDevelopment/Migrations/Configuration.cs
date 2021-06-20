namespace ApplicationDevelopment.Migrations
{
    using ApplicationDevelopment.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Runtime.CompilerServices;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDevelopment.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
       
        protected override void Seed(ApplicationDevelopment.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            string[] roles = new string[] { "Admin", "Staff","Trainer","Trainee" };
            foreach (string role in roles)
            {
                if (!context.Roles.Any(r => r.Name == role))
                {
                    context.Roles.Add(new IdentityRole(role));
                }
            }

            //create user UserName:Owner Role:Admin
            if (!context.Users.Any(u => u.UserName == "Admin@gmail.com"))
            {
                var userManager = new UserManager<Admin>(new UserStore<Admin>(context));
                var user = new Admin
                {
                    FullName = "Tien Do",
                    Email = "Admin@gmail.com",
                    UserName = "Admin@gmail.com",
                    PhoneNumber = "+111111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    PasswordHash = userManager.PasswordHasher.HashPassword("Admin123@"),
                    LockoutEnabled = true,
                };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Admin");
            }

            if (!context.Users.Any(u => u.UserName == "Staff@gmail.com"))
            {
                var userManager = new UserManager<Staff>(new UserStore<Staff>(context));
                var user = new Staff
                {
                    FullName = "Tien Do",
                    Email = "Staff@gmail.com",
                    UserName = "Staff@gmail.com",
                    PhoneNumber = "+111111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    PasswordHash = userManager.PasswordHasher.HashPassword("Staff123@"),
                    LockoutEnabled = true,
                };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Staff");
            }
            if (!context.Users.Any(u => u.UserName == "Trainee@gmail.com"))
            {
                var userManager = new UserManager<Trainee>(new UserStore<Trainee>(context));
                var user = new Trainee
                {
                    FullName = "Tien Do",
                    Email = "Trainee@gmail.com",
                    UserName = "Trainee@gmail.com",
                    PhoneNumber = "+111111111111",
                    DateOfBirth = DateTime.Now,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    PasswordHash = userManager.PasswordHasher.HashPassword("Trainee123@"),
                    LockoutEnabled = true,
                };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Trainee");
            }
            if (!context.Users.Any(u => u.UserName == "Trainer@gmail.co"))
            {
                var userManager = new UserManager<Trainer>(new UserStore<Trainer>(context));
                var user = new Trainer
                {
                    FullName = "Tien Do",
                    Email = "Trainer@gmail.com",
                    UserName = "Trainer@gmail.co",
                    PhoneNumber = "+111111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    PasswordHash = userManager.PasswordHasher.HashPassword("Trainer123@"),
                    LockoutEnabled = true,
                };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Trainer");
            }

            context.SaveChanges();
        }
    }
}
