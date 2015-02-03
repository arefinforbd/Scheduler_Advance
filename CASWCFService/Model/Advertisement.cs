using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CASWCFService.Model
{
    [DataContract]
    public class Advertisement
    {
        [DataMember]
        public string Header { get; set; }
        [DataMember]
        public string ImageURL { get; set; }
        [DataMember]
        public string TextContents { get; set; }
    }
}