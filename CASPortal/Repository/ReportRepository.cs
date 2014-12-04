using CASPortal.CASWCFService;
using CASPortal.WebParser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CASPortal.Repository
{
    public class ReportRepository
    {
        public TreeNode GetTrendAnalysisTreeNodes()
        {
            TreeNode treeNode;
            ReportParser parser = new ReportParser();

            treeNode = parser.GetTrendAnalysisTreeNodes();

            return treeNode;
        }

        public List<ChartData> PostTrendAnalysisReportData(int siteNo, int contractNo, DataTable answers, string area, int frequency, DateTime dtFrom, DateTime dtTo, int groupBy)
        {
            ReportParser parser = new ReportParser();
            List<ChartData> charts = new List<ChartData>();

            charts = parser.PostTrendAnalysisReportData(siteNo, contractNo, answers, area, frequency, dtFrom, dtTo, groupBy);

            return charts;
        }
    }
}