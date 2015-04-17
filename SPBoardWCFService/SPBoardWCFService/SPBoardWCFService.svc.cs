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

        public List<ChartData> GetSalesAnalysis(string CompanyID, string CompanyPassword, int Level4ID, int ReportType, DateTime FromDate, DateTime ToDate)
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
                Connection conn = GetConnection(CompanyID, CompanyPassword);
                SPBoard sboard = new SPBoard(conn);

                message = sboard.ws_webgetsumsalesdata(Level4ID, ReportType, FromDate, ToDate, out saleDataset, out legend);
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

                sboard.ws_debtrp(Level4ID, InvoiceType, CustomerFrom, CustomerTo, SortBy, Area, PrintCredit, NameFrom, NameTo, AgeBal, UnIndJobs, BalanceDate, Retention, out debtorDataset, out legend);
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

        public ComboClass GetCombo(string CompanyID, string CompanyPassword, int Level4ID)
        {
            DataSet dsAll;
            Category category = new Category();
            List<Category> categories = new List<Category>();
            Area area = new Area();
            List<Area> areas = new List<Area>();
            InvoiceType invoiceType = new InvoiceType();
            List<InvoiceType> invoiceTypes = new List<InvoiceType>();
            Tech tech = new Tech();
            List<Tech> techs = new List<Tech>();
            StrongTypesNS.ds_allDataSet allDataset;
            ComboClass combo = new ComboClass();

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword);
                SPBoard sboard = new SPBoard(conn);

                sboard.ws_combo(Level4ID, out allDataset);
                dsAll = (DataSet)allDataset;

                if (dsAll.Tables["tt_pd_mstr"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsAll.Tables["tt_pd_mstr"].Rows)
                    {
                        category = new Category();

                        category.Sequence = Convert.ToInt32(row["tt_pd_seq"]);
                        category.CategoryName = row["tt_pd_category"].ToString();

                        categories.Add(category);
                    }
                    combo.Categories = categories;
                }

                if (dsAll.Tables["tt_br_mstr"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsAll.Tables["tt_br_mstr"].Rows)
                    {
                        area = new Area();

                        area.Sequence = Convert.ToInt32(row["tt_br_seq"]);
                        area.AreaCode = row["tt_br_code"].ToString();

                        areas.Add(area);
                    }
                    combo.Areas = areas;
                }

                if (dsAll.Tables["tt_invType"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsAll.Tables["tt_invType"].Rows)
                    {
                        invoiceType = new InvoiceType();

                        invoiceType.Sequence = Convert.ToInt32(row["tt_inv_seq"]);
                        invoiceType.InvoiceTypeCode = row["tt_inv_type"].ToString();

                        invoiceTypes.Add(invoiceType);
                    }
                    combo.InvoiceTypes = invoiceTypes;
                }

                if (dsAll.Tables["tt_tech"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsAll.Tables["tt_tech"].Rows)
                    {
                        tech = new Tech();

                        tech.Sequence = Convert.ToInt32(row["tt_tc_seq"]);
                        tech.TechName = row["tt_tc_tech"].ToString();

                        techs.Add(tech);
                    }
                    combo.Techs = techs;
                }

                return combo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ResourceUtilization GetResourceUtilization(string CompanyID, string CompanyPassword, int Level4ID, DateTime FromDate, DateTime ToDate, bool Stacked = true)
        {
            DataSet dsSummary;
            DataSet dsDetail;
            DataSet dsOneDayPerTech;
            ChartData chart = new ChartData();
            List<ChartData> charts = new List<ChartData>();
            ResourceUtilization resource = new ResourceUtilization();

            ResourceUtilizationSummary summary = new ResourceUtilizationSummary();
            List<ResourceUtilizationSummary> summaries = new List<ResourceUtilizationSummary>();
            ResourceUtilizationDetail detail = new ResourceUtilizationDetail();
            List<ResourceUtilizationDetail> details = new List<ResourceUtilizationDetail>();

            StrongTypesNS.ds_summary2DataSet summaryDataset;
            StrongTypesNS.ds_detailsDataSet detailsDataset;
            StrongTypesNS.ds_lvl2_oneDayPerTechDataSet techDataset;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword);
                SPBoard sboard = new SPBoard(conn);

                sboard.ws_rscUtlz(Level4ID, FromDate, ToDate, out summaryDataset, out detailsDataset, out techDataset);
                dsSummary = (DataSet)summaryDataset;
                dsDetail = (DataSet)detailsDataset;
                dsOneDayPerTech = (DataSet)techDataset;

                if (dsSummary != null && dsSummary.Tables["tt_summary"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsSummary.Tables["tt_summary"].Rows)
                    {
                        summary = new ResourceUtilizationSummary();

                        summary.UsedPercentage = Convert.ToDecimal(row["tt_s_used_percent"]);
                        summary.FreePercentage = Convert.ToDecimal(row["tt_s_free_percent"]);

                        summaries.Add(summary);
                    }
                    resource.ResourceUtilizationSummaries = summaries;
                }

                if (dsDetail != null && dsDetail.Tables["tt_details"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsDetail.Tables["tt_details"].Rows)
                    {
                        if (Stacked)
                        {
                            for (int index = 0; index < 2; index++)
                            {
                                chart = new ChartData();

                                chart.Label = Convert.ToDateTime(row["tt_d_date"]).ToString("dd/MM/yyyy");
                                chart.DateLabel = index == 0 ? "Used Percentage" : "Free Percentage";
                                chart.Point = (index == 0 ? Convert.ToDouble(row["tt_d_used_percent"]) : Convert.ToDouble(row["tt_d_free_percent"])) * 100;
                                chart.Category = "ByDate";

                                charts.Add(chart);
                            }
                        }
                        else
                        {
                            detail = new ResourceUtilizationDetail();

                            detail.DateTimePercentage = Convert.ToDateTime(row["tt_d_date"]);
                            detail.UsedPercentage = Convert.ToDecimal(row["tt_d_used_percent"]);
                            detail.FreePercentage = Convert.ToDecimal(row["tt_d_free_percent"]);

                            details.Add(detail);
                        }
                    }
                    resource.ResourceUtilizationDetails = details.Count > 0 ? details : null;
                    resource.Charts = charts.Count > 0 ? charts : null;
                }

                return resource;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ResourceUtilization GetResourceUtilizationOneDayPerTech(string CompanyID, string CompanyPassword, int Level4ID, DateTime FromDate, DateTime ToDate, bool Stacked = true)
        {
            DataSet dsSummary;
            DataSet dsDetail;
            DataSet dsOneDayPerTech;
            ChartData chart = new ChartData();
            List<ChartData> charts = new List<ChartData>();
            ResourceUtilization resource = new ResourceUtilization();

            ResourceUtilizationOneDayPerTech tech = new ResourceUtilizationOneDayPerTech();
            List<ResourceUtilizationOneDayPerTech> techs = new List<ResourceUtilizationOneDayPerTech>();

            StrongTypesNS.ds_summary2DataSet summaryDataset;
            StrongTypesNS.ds_detailsDataSet detailsDataset;
            StrongTypesNS.ds_lvl2_oneDayPerTechDataSet techDataset;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword);
                SPBoard sboard = new SPBoard(conn);

                sboard.ws_rscUtlz(Level4ID, FromDate, ToDate, out summaryDataset, out detailsDataset, out techDataset);
                dsSummary = (DataSet)summaryDataset;
                dsDetail = (DataSet)detailsDataset;
                dsOneDayPerTech = (DataSet)techDataset;

                if (dsOneDayPerTech != null && dsOneDayPerTech.Tables["tt_lvl2_oneDayPerTech"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsOneDayPerTech.Tables["tt_lvl2_oneDayPerTech"].Rows)
                    {
                        if (Stacked)
                        {
                            for (int index = 0; index < 2; index++)
                            {
                                chart = new ChartData();

                                chart.Label = row["tt_oDpT_tech"].ToString();
                                chart.DateLabel = index == 0 ? "Used Percentage" : "Free Percentage";
                                chart.Point = (index == 0 ? Convert.ToDouble(row["tt_oDpT_used_percent"]) : Convert.ToDouble(row["tt_oDpT_free_percent"])) * 100;
                                chart.Category = "ByTech";

                                charts.Add(chart);
                            }
                        }
                        else
                        {
                            tech = new ResourceUtilizationOneDayPerTech();

                            tech.DateTimePercentage = Convert.ToDateTime(row["tt_oDpT_date"]);
                            tech.TechName = row["tt_oDpT_tech"].ToString();
                            tech.UsedPercentage = Convert.ToDecimal(row["tt_oDpT_used_percent"]);
                            tech.FreePercentage = Convert.ToDecimal(row["tt_oDpT_free_percent"]);

                            techs.Add(tech);
                        }
                    }
                    resource.ResourceUtilizationOneDayPerTechs = techs.Count > 0 ? techs : null;
                    resource.Charts = charts.Count > 0 ? charts : null;
                }

                return resource;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Job> GetBookedJobsInfo(string CompanyID, string CompanyPassword, int Level4ID, DateTime FromDate, DateTime ToDate)
        {
            DataSet dsSummary;
            Job job = new Job();
            List<Job> jobs = new List<Job>();
            StrongTypesNS.ds_summaryDataSet summaryDataset;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword);
                SPBoard sboard = new SPBoard(conn);

                sboard.ws_getJbSum(Level4ID, FromDate, ToDate, out summaryDataset);
                dsSummary = (DataSet)summaryDataset;

                if (dsSummary != null && dsSummary.Tables["tt_jobList_summary"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsSummary.Tables["tt_jobList_summary"].Rows)
                    {
                        job = new Job();

                        job.PostCode = row["tt_jl_postCode"].ToString() + " Australia";
                        job.State = row["tt_jl_state"].ToString();
                        job.JobDate = Convert.ToDateTime(row["tt_jl_date"]);
                        job.JobNumber = Convert.ToInt32(row["tt_jl_jobNum"]);
                        job.BookedBy = row["tt_jl_bookedBy"].ToString();

                        jobs.Add(job);
                    }
                }

                return jobs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Job> GetBookedJobList(string CompanyID, string CompanyPassword, int Level4ID, DateTime FromDate, DateTime ToDate, string Area, string Tech)
        {
            Job job;
            DataSet dsJobs;
            string message = "";
            List<Job> jobs = new List<Job>();
            StrongTypesNS.ds_detailDataSet jobsDataset;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword);
                SPBoard sboard = new SPBoard(conn);

                message = sboard.ws_getJbDtl(Level4ID, FromDate, ToDate, Area, Tech, out jobsDataset);
                dsJobs = (DataSet)jobsDataset;

                if (dsJobs != null && dsJobs.Tables["tt_jobList_detail"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsJobs.Tables["tt_jobList_detail"].Rows)
                    {
                        job = new Job();

                        job.JobNumber = Convert.ToInt32(row["tt_dtl_jobno"]);
                        job.JobDate = Convert.ToDateTime(row["tt_dtl_date"]);
                        job.CustomerName = row["tt_dtl_custname"].ToString();
                        job.SiteName = row["tt_dtl_sitename"].ToString();
                        job.Address = row["tt_dtl_addr"].ToString();
                        job.Tech = row["tt_dtl_tech"].ToString();
                        job.Status = row["tt_dtl_status"].ToString();

                        jobs.Add(job);
                    }
                }
                else
                    return null;

                return jobs;
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
