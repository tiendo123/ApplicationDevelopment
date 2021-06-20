using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationDevelopment.Models
{
    public class Trainee : BaseUser
    {
        public string MainProgrammingLanguage { get; set; }
        public int ToeicScore { get; set; }
        public string ExperimentDetail { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}