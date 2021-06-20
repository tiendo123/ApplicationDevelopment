using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationDevelopment.Models
{
    public class BaseUser: ApplicationUser
    {
        public string FullName { get; set; }
    }
}