using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPBoardWCFService.Model
{
    public class InvoiceType
    {
        [DataMember]
        public int Sequence { get; set; }
        [DataMember]
        public string InvoiceTypeCode { get; set; }
    }
}