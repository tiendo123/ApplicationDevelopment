using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationDevelopment.Models
{
    public class Trainer : BaseUser
    {
        public string Type { get; set; }
        public string WorkingPlace { get; set; }

    }
}