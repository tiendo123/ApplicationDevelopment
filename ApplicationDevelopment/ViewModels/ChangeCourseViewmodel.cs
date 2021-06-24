using ApplicationDevelopment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationDevelopment.ViewModels
{
    public class ChangeCourseViewmodel
    {
        public Course Course { get; set; }
        public IEnumerable<Course> CourseList { get; set; }
        public string UserId { get; set; }
    }
}