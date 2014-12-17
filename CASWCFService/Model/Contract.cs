using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CASWCFService.Model
{
    [DataContract]
    public class Contract
    {
        [DataMember]
        public int ContractNo { get; set; }
        [DataMember]
        public string ContractName { get; set; }
        [DataMember]
        public string ContractDescription { get; set; }
    }
}