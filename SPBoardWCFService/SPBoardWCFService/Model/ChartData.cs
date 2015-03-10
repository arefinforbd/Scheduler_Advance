using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPBoardWCFService.Model
{
    [DataContract]
    public class ChartData
    {
        [DataMember]
        public int Sequence { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string Label { get; set; }
        [DataMember]
        public string DateLabel { get; set; }
        [DataMember]
        public string SerialNumber { get; set; }
        [DataMember]
        public string Section { get; set; }
        [DataMember]
        public string Question { get; set; }
        [DataMember]
        public string Area { get; set; }
        [DataMember]
        public double Point { get; set; }
        [DataMember]
        public double YTDAmount { get; set; }
        [DataMember]
        public double MTDAmount { get; set; }
        [DataMember]
        public ChartLegend Legend { get; set; }
    }
}