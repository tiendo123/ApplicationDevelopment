using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApplicationDevelopment.Models
{
    public class BaseUser: ApplicationUser
    {
        public string FullName { get; set; }
        [NotMapped]
        public string Role { get; set; }
    }
}