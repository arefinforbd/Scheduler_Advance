using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASPortal.Models
{
    public class BusinessHour
    {
        public bool IsWorkingDay { get; set; }
        public string Date { get; set; }
        public int NoOfDay { get; set; }
        public float BusinessStartHour { get; set; }
        public float BusinessEndHour { get; set; }
    }
}