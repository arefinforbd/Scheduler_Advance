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
        public List<ChartData> GetSalesAnalysis(int reportType, DateTime fromDate, DateTime toDate)
        {
            SPBoardParser parser = new SPBoardParser();
            List<ChartData> charts = new List<ChartData>();

            charts = parser.GetSalesAnalysis(reportType, fromDate, toDate);

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

        public ComboClass GetCombo()
        {
            SPBoardParser parser = new SPBoardParser();
            ComboClass combo = new ComboClass();

            combo = parser.GetCombo();

            return combo;
        }

        public ResourceUtilization GetResourceUtilization(DateTime fromDate, DateTime toDate, bool stacked)
        {
            SPBoardParser parser = new SPBoardParser();
            ResourceUtilization resource = new ResourceUtilization();

            resource = parser.GetResourceUtilization(fromDate, toDate, stacked);

            return resource;
        }

        public ResourceUtilization GetResourceUtilizationOneDayPerTech(DateTime fromDate, bool stacked)
        {
            SPBoardParser parser = new SPBoardParser();
            ResourceUtilization resource = new ResourceUtilization();

            resource = parser.GetResourceUtilizationOneDayPerTech(fromDate, stacked);

            return resource;
        }

        public List<Job> GetBookedJobsInfo(DateTime fromDate, DateTime toDate)
        {
            List<Job> jobs = new List<Job>();
            SPBoardParser parser = new SPBoardParser();

            jobs = parser.GetBookedJobsInfo(fromDate, toDate);

            return jobs;
        }

        public List<Job> GetBookedJobList(DateTime fromDate, DateTime toDate, string area, string suburb, string postCode, string tech)
        {
            List<Job> jobs = new List<Job>();
            SPBoardParser parser = new SPBoardParser();

            jobs = parser.GetBookedJobList(fromDate, toDate, area, suburb, postCode, tech);

            return jobs;
        }
    }
}