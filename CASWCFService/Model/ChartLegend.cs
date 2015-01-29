using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CASWCFService.Model
{
    [DataContract]
    public class ChartLegend
    {
        [DataMember]
        public string Line1 { get; set; }
        [DataMember]
        public string Line2 { get; set; }
        [DataMember]
        public string Line3 { get; set; }
        [DataMember]
        public string Line4 { get; set; }
        [DataMember]
        public string Footer { get; set; }
    }
}