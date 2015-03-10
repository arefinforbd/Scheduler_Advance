using Progress.Open4GL.Proxy;
using SPBoardWCFService.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SPBoardWCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServiceBoardWCFService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServiceBoardWCFService.svc or ServiceBoardWCFService.svc.cs at the Solution Explorer and start debugging.
    public class SPBoardWCFService : ISPBoardWCFService
    {
        public bool Login(string CompanyID, string CompanyPassword, out int Level4ID, out string Message)
        {
            Level4ID = 0;
            Message = "";
            bool success;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword);
                SPBoard sboard = new SPBoard(conn);

                sboard.ws_getlogin(CompanyID, CompanyPassword, out Level4ID, out success, out Message);

                if (success)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<ChartData> GetSalesAnalysis(string CompanyID, string CompanyPassword, int Level4ID, int ReportType)
        {
            string message = "";
            DataSet dsChart;
            DataSet dsLegend;
            ChartData chart = new ChartData();
            List<ChartData> charts = new List<ChartData>();
            StrongTypesNS.ds_tt_sale3DataSet saleDataset;
            StrongTypesNS.ds_legendDataSet legend;

            try
            {
                Level4ID = 1;

                Connection conn = GetConnection(CompanyID, CompanyPassword);
                SPBoard sboard = new SPBoard(conn);

                message = sboard.ws_webgetsumsalesdata(Level4ID, ReportType, System.DateTime.Today.AddDays(-10), System.DateTime.Today, out saleDataset, out legend);
                dsChart = (DataSet)saleDataset;
                dsLegend = (DataSet)legend;

                if (dsChart != null && dsChart.Tables["tt_sale"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsChart.Tables["tt_sale"].Rows)
                    {
                        chart = new ChartData();

                        chart.Sequence = Convert.ToInt32(row["tt_seq"]);
                        chart.Label = row["tt_label"].ToString();
                        chart.DateLabel = row["tt_period"].ToString();
                        chart.Point = Convert.ToDouble(row["tt_amount"]);

                        if (chart.Point <= 0)
                            chart.Point = 1550;

                        charts.Add(chart);
                    }
                }
                else
                    return null;

                return charts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ChartData> GetSalesAnalysisByCategorySum(string CompanyID, string CompanyPassword, int Level4ID, int ReportType)
        {
            string message = "";
            DataSet dsChart;
            DataSet dsLegend;
            ChartData chart = new ChartData();
            List<ChartData> charts = new List<ChartData>();
            StrongTypesNS.ds_tt_sale2DataSet saleDataset;
            StrongTypesNS.ds_legendDataSet legend;

            try
            {
                Level4ID = 1;

                Connection conn = GetConnection(CompanyID, CompanyPassword);
                SPBoard sboard = new SPBoard(conn);

                message = sboard.ws_webgetSaleByCatSum(Level4ID, ReportType, System.DateTime.Today.AddDays(-10), System.DateTime.Today, out saleDataset, out legend);
                dsChart = (DataSet)saleDataset;
                dsLegend = (DataSet)legend;

                if (dsChart != null && dsChart.Tables["tt_sale"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsChart.Tables["tt_sale"].Rows)
                    {
                        chart = new ChartData();

                        chart.Sequence = Convert.ToInt32(row["tt_seq"]);
                        chart.Category = row["tt_category"].ToString();
                        chart.Label = row["tt_label"].ToString();
                        chart.DateLabel = row["tt_period"].ToString();
                        chart.YTDAmount = Convert.ToDouble(row["tt_YTD_amt"]);
                        chart.MTDAmount = Convert.ToDouble(row["tt_MTD_amt"]);

                        charts.Add(chart);
                    }
                }
                else
                    return null;

                return charts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ChartData> GetSalesAnalysisByCategoryDetail(string CompanyID, string CompanyPassword, int Level4ID, string Category, DateTime FromDate, DateTime ToDate)
        {
            string message = "";
            DataSet dsChart;
            DataSet dsLegend;
            ChartData chart = new ChartData();
            List<ChartData> charts = new List<ChartData>();
            StrongTypesNS.ds_tt_saleDataSet saleDataset;
            StrongTypesNS.ds_legendDataSet legend;

            try
            {
                Level4ID = 1;

                Connection conn = GetConnection(CompanyID, CompanyPassword);
                SPBoard sboard = new SPBoard(conn);

                message = sboard.ws_webgetSaleByCatDtl(Level4ID, Category, FromDate, ToDate, out saleDataset, out legend);
                dsChart = (DataSet)saleDataset;
                dsLegend = (DataSet)legend;

                if (dsChart != null && dsChart.Tables["tt_sale"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsChart.Tables["tt_sale"].Rows)
                    {
                        chart = new ChartData();

                        chart.Sequence = Convert.ToInt32(row["tt_seq"]);
                        chart.Category = row["tt_category"].ToString();
                        chart.Label = row["tt_label"].ToString();
                        chart.DateLabel = row["tt_period"].ToString();
                        chart.Point = Convert.ToDouble(row["tt_amount"]);

                        charts.Add(chart);
                    }
                }
                else
                    return null;

                return charts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ChartData> GetDebtorAnalysis(string CompanyID, string CompanyPassword, int Level4ID, string InvoiceType, decimal CustomerFrom, decimal CustomerTo, int SortBy, string Area, bool PrintCredit, string NameFrom, string NameTo, int AgeBal, bool UnIndJobs, DateTime BalanceDate, bool Retention)
        {
            int loopIndex = 0;
            DataSet dsChart;
            DataSet dsLegend;
            ChartData chart = new ChartData();
            List<ChartData> charts = new List<ChartData>();
            StrongTypesNS.ds_totalsDataSet debtorDataset;
            StrongTypesNS.ds_legendDataSet legend;

            try
            {
                Level4ID = 1;

                Connection conn = GetConnection("kevorkt", CompanyPassword);
                SPBoard sboard = new SPBoard(conn);

                //sboard.ws_debtrp(Level4ID, InvoiceType, CustomerFrom, CustomerTo, SortBy, Area, PrintCredit, NameFrom, NameTo, AgeBal, UnIndJobs, BalanceDate, Retention, out debtorDataset, out legend);
                sboard.ws_debtrp(Level4ID, "[ALL]", 1, 100, 2, "[ALL]", false, "", "", 1, false, DateTime.Today, false, out debtorDataset, out legend);
                dsChart = (DataSet)debtorDataset;
                dsLegend = (DataSet)legend;

                if (dsChart != null && dsChart.Tables["tt_totals"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsChart.Tables["tt_totals"].Rows)
                    {
                        foreach (DataColumn col in dsChart.Tables["tt_totals"].Columns)
                        {
                            chart = new ChartData();

                            chart.Label = row[loopIndex].ToString().Trim() ;
                            chart.Point = Convert.ToDouble(row[(loopIndex + 7)]);

                            charts.Add(chart);
                            loopIndex++;

                            if (loopIndex >= 7)
                                break;
                        }
                    }
                }
                else
                    return null;

                return charts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Category> GetCategory(string CompanyID, string CompanyPassword, int Level4ID)
        {
            DataSet dsCategory;
            Category category = new Category();
            List<Category> categories = new List<Category>();
            StrongTypesNS.ds_categoryDataSet categoryDataset;
            StrongTypesNS.ds_areaDataSet areaDataset;
            StrongTypesNS.ds_invTypeDataSet invoiceDataset;

            try
            {
                Level4ID = 1;

                Connection conn = GetConnection(CompanyID, CompanyPassword);
                SPBoard sboard = new SPBoard(conn);

                sboard.ws_combo(Level4ID, out categoryDataset, out areaDataset, out invoiceDataset);
                dsCategory = (DataSet)categoryDataset;

                if (dsCategory != null && dsCategory.Tables["tt_pd_mstr"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsCategory.Tables["tt_pd_mstr"].Rows)
                    {
                        category = new Category();

                        category.Sequence = Convert.ToInt32(row["tt_pd_seq"]);
                        category.CategoryName = row["tt_pd_category"].ToString();

                        categories.Add(category);
                    }
                }
                else
                    return null;

                return categories;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private Connection GetConnection(string CompanyID, string CompanyPassword)
        {
            string appServerURL = ConfigurationManager.AppSettings["AppServerURL"];
            string appServerInfo = CompanyID + (char)1 + CompanyPassword + (char)1 + "" + (char)1;
            Connection conn = new Connection(appServerURL, CompanyID, CompanyPassword, appServerInfo);

            return conn;
        }
    }
}
