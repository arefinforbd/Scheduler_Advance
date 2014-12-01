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

        public string PostTrendAnalysisReportData(DataTable answers)
        {
            ReportParser parser = new ReportParser();

            string responseMessage = parser.PostTrendAnalysisReportData(answers);

            return responseMessage;
        }
    }
}