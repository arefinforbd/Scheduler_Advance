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
        public int BusinessStartHour { get; set; }
        public int BusinessEndHour { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
    }
}