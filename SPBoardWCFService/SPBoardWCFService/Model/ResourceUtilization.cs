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
        public List<ResourceUtilizationSummary> ResourceUtilizationSummaries { get; set; }
        [DataMember]
        public List<ResourceUtilizationDetail> ResourceUtilizationDetails { get; set; }
        [DataMember]
        public List<ResourceUtilizationOneDayPerTech> ResourceUtilizationOneDayPerTechs { get; set; }
        [DataMember]
        public List<ChartData> Charts { get; set; }

    }

    public class ResourceUtilizationSummary : ResourceUtilization
    {

    }

    public class ResourceUtilizationDetail : ResourceUtilization
    {
        [DataMember]
        public DateTime DateTimePercentage { get; set; }
    }

    public class ResourceUtilizationOneDayPerTech : ResourceUtilization
    {
        [DataMember]
        public DateTime DateTimePercentage { get; set; }
        [DataMember]
        public string TechName { get; set; }
    }
}