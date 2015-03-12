using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPBoardWCFService.Model
{
    [DataContract]
    public class ResourceUtilization
    {
        [DataMember]
        public decimal UsedPercentage { get; set; }
        [DataMember]
        public decimal FreePercentage { get; set; }
        [DataMember]
        public DateTime DateTimePercentage { get; set; }

    }
}