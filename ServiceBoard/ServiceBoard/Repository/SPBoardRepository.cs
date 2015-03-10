using ServiceBoard.SPBoardWCFService;
using ServiceBoard.WebParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceBoard.Repository
{
    public class SPBoardRepository
    {
        public List<ChartData> GetSalesAnalysis(int reportType)
        {
            SPBoardParser parser = new SPBoardParser();
            List<ChartData> charts = new List<ChartData>();

            charts = parser.GetSalesAnalysis(reportType);

            return charts;
        }

        public List<ChartData> GetSalesAnalysisByCategorySum(int reportType)
        {
            SPBoardParser parser = new SPBoardParser();
            List<ChartData> charts = new List<ChartData>();

            charts = parser.GetSalesAnalysisByCategorySum(reportType);

            return charts;
        }

        public List<ChartData> GetSalesAnalysisByCategoryDetail(string category, DateTime fromDate, DateTime toDate)
        {
            SPBoardParser parser = new SPBoardParser();
            List<ChartData> charts = new List<ChartData>();

            charts = parser.GetSalesAnalysisByCategoryDetail(category, fromDate, toDate);

            return charts;
        }

        public List<ChartData> GetDebtorAnalysis(string invoiceType, decimal customerFrom, decimal customerTo, int sortBy, string area, bool printCredit, string nameFrom, string nameTo, int ageBal, bool unIndJobs, DateTime balanceDate, bool retention)
        {
            SPBoardParser parser = new SPBoardParser();
            List<ChartData> charts = new List<ChartData>();

            charts = parser.GetDebtorAnalysis(invoiceType, customerFrom, customerTo, sortBy, area, printCredit, nameFrom, nameTo, ageBal, unIndJobs, balanceDate, retention);

            return charts;
        }

        public List<Category> GetCategory()
        {
            SPBoardParser parser = new SPBoardParser();
            List<Category> categories = new List<Category>();

            categories = parser.GetCategory();

            return categories;
        }
    }
}