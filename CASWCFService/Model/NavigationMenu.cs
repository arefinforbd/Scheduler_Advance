using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CASWCFService.Model
{
    [DataContract]
    public class NavigationMenu
    {
        [DataMember]
        public string MenuName { get; set; }
        [DataMember]
        public int MenuOrder { get; set; }
        [DataMember]
        public string MenuTitle { get; set; }
        [DataMember]
        public string MenuCalls { get; set; }
        [DataMember]
        public string MenuCanSee { get; set; }
        [DataMember]
        public bool MenuType { get; set; }
        [DataMember]
        public int MenuImageNo { get; set; }
        [DataMember]
        public int Level4 { get; set; }
        [DataMember]
        public string MenuDescription { get; set; }
    }
}