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

        public string PostTrendAnalysisReportData(DataTable answers)
        {
            try
            {
                CASWCFServiceClient cas = new CASWCFServiceClient();

                string companyID = HttpContext.Current.Session["CompanyID"].ToString();
                string companyPassword = HttpContext.Current.Session["CompanyPassword"].ToString();
                string customerPassword = HttpContext.Current.Session["CustomerPassword"].ToString();
                decimal customerID = Convert.ToDecimal(HttpContext.Current.Session["CustomerID"]);
                int level4ID = Convert.ToInt32(HttpContext.Current.Session["Level4ID"].ToString());

                string responseMessage = cas.PostTrendAnalysisReportData(companyID, companyPassword, customerID, customerPassword, level4ID, 1029, 0, DateTime.Now, DateTime.Now, answers);

                return responseMessage;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}