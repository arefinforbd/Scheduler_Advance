using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CASWCFService.Model
{
    [DataContract]
    public class DayHour
    {
        [DataMember]
        public bool IsDay1WorkingDay { get; set; }
        [DataMember]
        public string Day1StartHour { get; set; }
        [DataMember]
        public string Day1EndHour { get; set; }

        [DataMember]
        public bool IsDay2WorkingDay { get; set; }
        [DataMember]
        public string Day2StartHour { get; set; }
        [DataMember]
        public string Day2EndHour { get; set; }

        [DataMember]
        public bool IsDay3WorkingDay { get; set; }
        [DataMember]
        public string Day3StartHour { get; set; }
        [DataMember]
        public string Day3EndHour { get; set; }

        [DataMember]
        public bool IsDay4WorkingDay { get; set; }
        [DataMember]
        public string Day4StartHour { get; set; }
        [DataMember]
        public string Day4EndHour { get; set; }

        [DataMember]
        public bool IsDay5WorkingDay { get; set; }
        [DataMember]
        public string Day5StartHour { get; set; }
        [DataMember]
        public string Day5EndHour { get; set; }

        [DataMember]
        public bool IsDay6WorkingDay { get; set; }
        [DataMember]
        public string Day6StartHour { get; set; }
        [DataMember]
        public string Day6EndHour { get; set; }

        [DataMember]
        public bool IsDay7WorkingDay { get; set; }
        [DataMember]
        public string Day7StartHour { get; set; }
        [DataMember]
        public string Day7EndHour { get; set; }
    }
}