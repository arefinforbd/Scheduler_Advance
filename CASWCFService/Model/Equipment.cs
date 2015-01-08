using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CASWCFService.Model
{
    [DataContract]
    public class Equipment
    {
        [DataMember]
        public string EquipmentType { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public string Serial { get; set; }
        [DataMember]
        public string JC { get; set; }
        [DataMember]
        public int JobNo { get; set; }
        [DataMember]
        public int SequenceNo { get; set; }
        [DataMember]
        public DateTime DateInstalled { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public string Area { get; set; }
        [DataMember]
        public bool Status { get; set; }
        [DataMember]
        public string ReportName { get; set; }
        [DataMember]
        public decimal SectionID { get; set; }
        [DataMember]
        public decimal QuestionID { get; set; }
        [DataMember]
        public int Frequency { get; set; }
        [DataMember]
        public DateTime LastJob { get; set; }
        [DataMember]
        public DateTime NextJob { get; set; }
        [DataMember]
        public string Manufacturer { get; set; }
        [DataMember]
        public int Level4 { get; set; }
        [DataMember]
        public DateTime ManufactureDate { get; set; }
    }
}