using CASPortal.CASWCFService;
using CASPortal.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CASPortal.WebParser
{
    public class ReportParser
    {
        public TreeNode GetTrendAnalysisTreeNodes()
        {
            TreeNode treeNode;
            DataSet ds = new DataSet();
            CASWCFServiceClient cas = new CASWCFServiceClient();

            string companyID = HttpContext.Current.Session["CompanyID"].ToString();
            string companyPassword = HttpContext.Current.Session["CompanyPassword"].ToString();
            string customerPassword = HttpContext.Current.Session["CustomerPassword"].ToString();
            decimal customerID = Convert.ToDecimal(HttpContext.Current.Session["CustomerID"]);
            int level4ID = Convert.ToInt32(HttpContext.Current.Session["Level4ID"].ToString());

            treeNode = cas.GetTrendAnalysisTreeNodes(companyID, companyPassword, customerID, customerPassword, level4ID);

            if (treeNode != null)
            {
                return treeNode;
            }

            return null;
        }

        public List<ChartData> PostTrendAnalysisReportData(int siteNo, int contractNo, DataTable answers, string area, int frequency, DateTime dtFrom, DateTime dtTo, int groupBy)
        {
            try
            {
                ChartData[] chartArr = null;
                List<ChartData> charts = new List<ChartData>();
                CASWCFServiceClient cas = new CASWCFServiceClient();

                string companyID = HttpContext.Current.Session["CompanyID"].ToString();
                string companyPassword = HttpContext.Current.Session["CompanyPassword"].ToString();
                string customerPassword = HttpContext.Current.Session["CustomerPassword"].ToString();
                decimal customerID = Convert.ToDecimal(HttpContext.Current.Session["CustomerID"]);
                int level4ID = Convert.ToInt32(HttpContext.Current.Session["Level4ID"].ToString());

                chartArr = cas.PostTrendAnalysisReportData(companyID, companyPassword, customerID, customerPassword, level4ID, siteNo, 1029, answers, area, frequency, dtFrom, dtTo, groupBy);

                if (chartArr != null)
                {
                    foreach (ChartData chart in chartArr)
                        charts.Add(new ChartData() { DateLabel = chart.DateLabel, Section = chart.Section, Question = chart.Question, Point = chart.Point });

                    return charts;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}