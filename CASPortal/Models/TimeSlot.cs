using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASPortal.Models
{
    public class TimeSlot
    {
        public int ItemID { get; set; }
        public string Date { get; set; }
        public float BusinessStartHour { get; set; }
        public float BusinessEndHour { get; set; }
        public float StartTime { get; set; }
        public float EndTime { get; set; }
    }
}