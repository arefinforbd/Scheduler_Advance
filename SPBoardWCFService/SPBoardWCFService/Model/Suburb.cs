using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPBoardWCFService.Model
{
    public class Suburb
    {
        [DataMember]
        public string SuburbName { get; set; }
    }
}