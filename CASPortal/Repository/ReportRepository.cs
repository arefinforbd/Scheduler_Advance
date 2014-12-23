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
        public List<Contract> GetContracts(string siteNo)
        {
            ReportParser parser = new ReportParser();
            List<Contract> contracts = new List<Contract>();

            contracts = parser.GetContracts(siteNo);

            return contracts;
        }

        public TreeNode GetTrendAnalysisTreeNodes()
        {
            TreeNode treeNode;
            ReportParser parser = new ReportParser();

            treeNode = parser.GetTrendAnalysisTreeNodes();

            return treeNode;
        }

        public List<ChartData> GetTrendAnalysisByJob(int siteNo, int contractNo, DataTable answers, string area, DateTime dtFrom, DateTime dtTo)
        {
            ReportParser parser = new ReportParser();
            List<ChartData> charts = new List<ChartData>();

            charts = parser.GetTrendAnalysisByJob(siteNo, contractNo, answers, area, dtFrom, dtTo);

            return charts;
        }

        public List<ChartData> GetTrendAnalysisByQuestion(int siteNo, int contractNo, DataTable answers, string area, int frequency, DateTime dtFrom, DateTime dtTo, int groupBy)
        {
            ReportParser parser = new ReportParser();
            List<ChartData> charts = new List<ChartData>();

            charts = parser.GetTrendAnalysisByQuestion(siteNo, contractNo, answers, area, frequency, dtFrom, dtTo, groupBy);

            return charts;
        }
    }
}