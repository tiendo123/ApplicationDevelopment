using ApplicationDevelopment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationDevelopment.ViewModels
{
    public class StaffViewAllVm
    {
        public IEnumerable<Assign> TrainerList { get; set; }
        public IEnumerable<Enroll> TraineeList { get; set; }
    }
}