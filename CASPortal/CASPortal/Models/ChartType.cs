using CASPortal.CASWCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASPortal.Models
{
    public class ChartType
    {
        public List<Dictionary<string, object>> Bars { get; set; }
        public List<LineData> Lines { get; set; }
        public List<PieData> Pies { get; set; }
        public ChartLegend Legend { get; set; }
    }
}