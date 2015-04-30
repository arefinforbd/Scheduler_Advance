using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPBoardWCFService.Model
{
    public class Job
    {
        [DataMember]
        public int JobNumber { get; set; }
        [DataMember]
        public DateTime JobDate { get; set; }
        [DataMember]
        public int LineNumber { get; set; }
        [DataMember]
        public int ScheduleNumber { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string SiteName { get; set; }
        [DataMember]
        public string Tech { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string PostCode { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string BookedBy { get; set; }
        [DataMember]
        public int NumberOfJobs { get; set; }
    }
}