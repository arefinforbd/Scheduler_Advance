using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CASWCFService.Model
{
    [DataContract]
    public class Site
    {
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public string Address1 { get; set; }
        [DataMember]
        public string Address2 { get; set; }
        [DataMember]
        public string Address3 { get; set; }
        [DataMember]
        public string Suburb { get; set; }
        [DataMember]
        public string PhoneNo { get; set; }
        [DataMember]
        public string MobileNo { get; set; }
        [DataMember]
        public string PostCode { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public int SiteNo { get; set; }
        [DataMember]
        public string StreetNo { get; set; }
        [DataMember]
        public int SiteCode { get; set; }
        [DataMember]
        public int Level4 { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Email { get; set; }
        
    }
}