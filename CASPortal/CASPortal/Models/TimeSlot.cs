﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASPortal.Models
{
    public class TimeSlot
    {
        public int ItemID { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string SpecialInstruction { get; set; }
        public bool IsPublicHoliDay { get; set; }
    }
}