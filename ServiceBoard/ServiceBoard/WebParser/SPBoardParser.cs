using ServiceBoard.SPBoardWCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceBoard.WebParser
{
    public class SPBoardParser
    {
        public List<ChartData> GetSalesAnalysis(int reportType)
        {
            List<ChartData> charts = new List<ChartData>();
            SPBoardWCFServiceClient sp = new SPBoardWCFServiceClient();
            string companyID = HttpContext.Current.Session["CompanyID"].ToString();
            string companyPassword = HttpContext.Current.Session["CompanyPassword"].ToString();
            int level4ID = Convert.ToInt32(HttpContext.Current.Session["Level4ID"].ToString());

            if (reportType == 1)
            {
                if (HttpContext.Current.Session["SalesAnalysisYTD"] != null)
                    charts = (List<ChartData>)HttpContext.Current.Session["SalesAnalysisYTD"];
                else
                {
                    charts = sp.GetSalesAnalysis(companyID, companyPassword, level4ID, reportType);
                    HttpContext.Current.Session["SalesAnalysisYTD"] = charts;
                }
            }
            else if (reportType == 2)
            {
                if (HttpContext.Current.Session["SalesAnalysisMTD"] != null)
                    charts = (List<ChartData>)HttpContext.Current.Session["SalesAnalysisMTD"];
                else
                {
                    charts = sp.GetSalesAnalysis(companyID, companyPassword, level4ID, reportType);
                    HttpContext.Current.Session["SalesAnalysisMTD"] = charts;
                }
            }

            return charts;
        }

        public List<ChartData> GetSalesAnalysisByCategorySum(int reportType)
        {
            List<ChartData> charts = new List<ChartData>();
            SPBoardWCFServiceClient sp = new SPBoardWCFServiceClient();
            string companyID = HttpContext.Current.Session["CompanyID"].ToString();
            string companyPassword = HttpContext.Current.Session["CompanyPassword"].ToString();
            int level4ID = Convert.ToInt32(HttpContext.Current.Session["Level4ID"].ToString());

            if (HttpContext.Current.Session["SalesAnalysisByCategorySum"] != null)
                charts = (List<ChartData>)HttpContext.Current.Session["SalesAnalysisByCategorySum"];
            else
            {
                charts = sp.GetSalesAnalysisByCategorySum(companyID, companyPassword, level4ID, reportType);
                HttpContext.Current.Session["SalesAnalysisByCategorySum"] = charts;
            }

            return charts;
        }

        public List<ChartData> GetSalesAnalysisByCategoryDetail(string category, DateTime fromDate, DateTime toDate)
        {
            List<ChartData> charts = new List<ChartData>();
            SPBoardWCFServiceClient sp = new SPBoardWCFServiceClient();
            string companyID = HttpContext.Current.Session["CompanyID"].ToString();
            string companyPassword = HttpContext.Current.Session["CompanyPassword"].ToString();
            int level4ID = Convert.ToInt32(HttpContext.Current.Session["Level4ID"].ToString());

            charts = sp.GetSalesAnalysisByCategoryDetail(companyID, companyPassword, level4ID, category, fromDate, toDate);

            return charts;
        }

        public List<ChartData> GetDebtorAnalysis(string invoiceType, decimal customerFrom, decimal customerTo, int sortBy, string area, bool printCredit, string nameFrom, string nameTo, int ageBal, bool unIndJobs, DateTime balanceDate, bool retention)
        {
            List<ChartData> charts = new List<ChartData>();
            SPBoardWCFServiceClient sp = new SPBoardWCFServiceClient();
            string companyID = HttpContext.Current.Session["CompanyID"].ToString();
            string companyPassword = HttpContext.Current.Session["CompanyPassword"].ToString();
            int level4ID = Convert.ToInt32(HttpContext.Current.Session["Level4ID"].ToString());

            charts = sp.GetDebtorAnalysis(companyID, companyPassword, level4ID, invoiceType, customerFrom, customerTo, sortBy, area, printCredit, nameFrom, nameTo, ageBal, unIndJobs, balanceDate, retention);

            var itemToRemove = charts.Single(r => r.Label.ToLower().Equals("total amount"));
            charts.Remove(itemToRemove);
            itemToRemove = charts.Single(r => r.Label.ToLower().Equals("unallocated total"));
            charts.Remove(itemToRemove);

            return charts;
        }

        public List<Category> GetCategory()
        {
            List<Category> categories = new List<Category>();
            SPBoardWCFServiceClient sp = new SPBoardWCFServiceClient();
            string companyID = HttpContext.Current.Session["CompanyID"].ToString();
            string companyPassword = HttpContext.Current.Session["CompanyPassword"].ToString();
            int level4ID = Convert.ToInt32(HttpContext.Current.Session["Level4ID"].ToString());

            categories = sp.GetCategory(companyID, companyPassword, level4ID);

            return categories;
        }

        public List<ResourceUtilization> GetResourceUtilization(DateTime fromDate, DateTime toDate)
        {
            List<ResourceUtilization> resources = new List<ResourceUtilization>();
            SPBoardWCFServiceClient sp = new SPBoardWCFServiceClient();
            string companyID = HttpContext.Current.Session["CompanyID"].ToString();
            string companyPassword = HttpContext.Current.Session["CompanyPassword"].ToString();
            int level4ID = Convert.ToInt32(HttpContext.Current.Session["Level4ID"].ToString());

            resources = sp.GetResourceUtilization(companyID, companyPassword, level4ID, fromDate, toDate);

            return resources;
        }
    }
}