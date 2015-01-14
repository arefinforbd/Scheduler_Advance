using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CASWCFService.Model
{
    [DataContract]
    public class ChartData
    {
        [DataMember]
        public string DateLabel { get; set; }
        [DataMember]
        public string SerialNumber { get; set; }
        [DataMember]
        public string Section { get; set; }
        [DataMember]
        public string Question { get; set; }
        [DataMember]
        public double Point { get; set; }
    }
}