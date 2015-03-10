using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CASWCFService.Model
{
    [DataContract]
    public class Welcome
    {
        [DataMember]
        public string MessageOfTheDay { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public byte[] CompanyLogo { get; set; }
        [DataMember]
        public string LogoFileName { get; set; }

        [DataMember]
        public string PrivateFolders { get; set; }
        [DataMember]
        public string PublicFolders { get; set; }
    }
}