using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASPortal.Models
{
    public class Item
    {
        public int CategoryID { get; set; }
        public int ProductID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string SpecialInstruction { get; set; }
        public int Duration { get; set; }
        public double Price { get; set; }
        public float MinStartHour { get; set; }
        public float MaxEndHour { get; set; }

        public List<TimeSlot> TimeSlots { get; set; }
        public List<BusinessHour> BusinessHours { get; set; }
    }
}