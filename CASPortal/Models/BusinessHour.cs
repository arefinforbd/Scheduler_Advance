using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASPortal.Models
{
    public class BusinessHour
    {
        public string Date { get; set; }
        public int BusinessStartHour { get; set; }
        public int BusinessEndHour { get; set; }
    }
}