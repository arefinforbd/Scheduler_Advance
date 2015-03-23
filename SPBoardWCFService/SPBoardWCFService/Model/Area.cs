using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPBoardWCFService.Model
{
    public class Area
    {
        [DataMember]
        public int Sequence { get; set; }
        [DataMember]
        public string AreaCode { get; set; }
    }
}