using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPBoardWCFService.Model
{
    public class JobSummaryAll
    {
        [DataMember]
        public List<Job> JobSummaries { get; set; }
        [DataMember]
        public List<AreaAddress> Areas { get; set; }
        [DataMember]
        public List<Suburb> Suburbs { get; set; }
        [DataMember]
        public List<PostCode> PostCodes { get; set; }
        [DataMember]
        public List<Tech> Techs { get; set; }
    }
}