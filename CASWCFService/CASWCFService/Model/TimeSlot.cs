using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CASWCFService.Model
{
    [DataContract]
    public class TimeSlot
    {
        [DataMember]
        public int ItemID { get; set; }
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }
        [DataMember]
        public string SpecialInstruction { get; set; }
        [DataMember]
        public bool IsPublicHoliDay { get; set; }
    }
}