using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CASWCFService.Model
{
    [DataContract]
    public class SiteNItem
    {
        [DataMember]
        public List<Site> sites { get; set; }
        [DataMember]
        public List<Item> items { get; set; }
    }
}