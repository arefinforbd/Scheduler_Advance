using CASPortal.CASWCFService;
using CASPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASPortal.CASWCFService
{
    public partial class Item
    {
        public List<TimeSlot> TimeSlots { get; set; }
        public List<BusinessHour> BusinessHours { get; set; }
    }
}