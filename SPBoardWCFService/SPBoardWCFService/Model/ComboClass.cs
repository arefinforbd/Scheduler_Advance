using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPBoardWCFService.Model
{
    public class ComboClass
    {
        [DataMember]
        public List<Category> Categories { get; set; }
        [DataMember]
        public List<Area> Areas { get; set; }
        [DataMember]
        public List<InvoiceType> InvoiceTypes { get; set; }
    }
}