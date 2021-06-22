using ApplicationDevelopment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationDevelopment.ViewModels
{
    public class CourseCategoryViewModel
    {
        public Course Course { get; set; }
        public Category Category { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}