using ApplicationDevelopment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationDevelopment.ViewModels
{
    public class UsersViewModel
    {
        public Trainee Trainee { get; set; }
        public Trainer Trainer { get; set; }
        public Admin Admin { get; set; }
        public Staff Staff { get; set; }
    }
}