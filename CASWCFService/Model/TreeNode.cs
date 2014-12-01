using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CASWCFService.Model
{
    [DataContract]
    public class TreeNode
    {
        [DataMember]
        public List<TreeNodeLevel1> listLeve1 { get; set; }

        [DataMember]
        public List<TreeNodeLevel2> listLeve2 { get; set; }

        [DataMember]
        public List<TreeNodeLevel3> listLeve3 { get; set; }

        [DataMember]
        public List<TreeNodeLevel4> listLeve4 { get; set; }

        [DataMember]
        public List<string> listAreaName { get; set; }
    }
}