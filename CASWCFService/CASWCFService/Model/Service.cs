using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CASWCFService.Model
{
    [DataContract]
    public class Service
    {
        [DataMember]
        public int CategoryID { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string ItemID { get; set; }
        [DataMember]
        public string ItemName { get; set; }
        [DataMember]
        public int LineNo { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int Duration { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public float MinStartHour { get; set; }
        [DataMember]
        public float MaxEndHour { get; set; }
        [DataMember]
        public List<TimeSlot> TimeSlots { get; set; }
        [DataMember]
        public List<BusinessHour> BusinessHours { get; set; }
    }
}