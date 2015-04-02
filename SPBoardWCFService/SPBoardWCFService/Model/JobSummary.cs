using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPBoardWCFService.Model
{
    public class JobSummary
    {
        [DataMember]
        public string PostCode { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public DateTime JobDate { get; set; }
        [DataMember]
        public int JobNumber { get; set; }
        [DataMember]
        public string BookedBy { get; set; }
    }
}