using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPBoardWCFService.Model
{
    public class PostCode
    {
        [DataMember]
        public string PCode { get; set; }
    }
}