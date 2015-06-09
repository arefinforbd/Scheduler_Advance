using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePROWeb.Models
{
    public class Tech
    {
        public int TechID { get; set; }
        public int SkillID { get; set; }
        public string TechName { get; set; }
        public string Color { get; set; }
    }
}