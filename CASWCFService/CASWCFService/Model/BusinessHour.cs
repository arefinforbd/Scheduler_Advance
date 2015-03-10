using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CASWCFService.Model
{
    [DataContract]
    public class BusinessHour
    {
        [DataMember]
        public bool IsWorkingDay { get; set; }
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public int NoOfDay { get; set; }
        [DataMember]
        public float BusinessStartHour { get; set; }
        [DataMember]
        public float BusinessEndHour { get; set; }
    }
}