using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CASWCFService.Model
{
    [DataContract]
    public class Folder
    {
        [DataMember]
        public int lvl4sequence { get; set; }
        [DataMember]
        public string fileName { get; set; }
        [DataMember]
        public string fileDescription { get; set; }
        [DataMember]
        public string fileUploadedDate { get; set; }
        [DataMember]
        public string fileUploadedBy { get; set; }

        [DataMember]
        public List<string> PrivateFolders { get; set; }
        [DataMember]
        public List<string> PublicFolders { get; set; }
    }
}