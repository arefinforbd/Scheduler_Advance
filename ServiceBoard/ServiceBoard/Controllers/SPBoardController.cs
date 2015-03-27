using ServiceBoard.Helper;
using ServiceBoard.Models;
using ServiceBoard.Repository;
using ServiceBoard.SPBoardWCFService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceBoard.Controllers
{
    public class SPBoardController : Controller
    {
        //
        // GET: /ServiceBoard/
        public ActionResult Index()
        {
            if (Session["CompanyID"] == null)
                return RedirectToAction("Index", "Login");

            decimal usedPercentage = 0;
            SPBoardRepository repository = new SPBoardRepository();
            ResourceUtilization resource = new ResourceUtilization();

            resource = repository.GetResourceUtilization(DateTime.Today, DateTime.Today, false);
            usedPercentage = resource.ResourceUtilizationSummaries == null ? 0 : resource.ResourceUtilizationSummaries[0].UsedPercentage;

            ViewBag.ResourcePercentage = Math.Round((usedPercentage * 100), 2);

            return View();
        }

        public ActionResult Widgets()
        {
            return View();
        }

        private DataTable RowToColumn(List<Bar> lineData, int weeksCount)
        {
            DataRow row;
            int count = lineData.Count / weeksCount;
            DataTable dtTable = new DataTable();

            dtTable.Columns.Add("BarXLabel", typeof(string));
            for (int index = 0; index < count; index++)
            {
                dtTable.Columns.Add(lineData[index * weeksCount].BarLegend, typeof(string));
            }

            for (int index = 0; index < weeksCount; index++)
            {
                int rowIndex = 1;
                row = dtTable.NewRow();

                row[0] = lineData[index].BarXLabel;
                for (int innerIndex = index; innerIndex < lineData.Count; innerIndex += weeksCount)
                {
                    row[rowIndex] = lineData[innerIndex].BarValue;
                    rowIndex++;
                }
                dtTable.Rows.Add(row);
            }

            List<DataRow> rowToList = dtTable.AsEnumerable().ToList();

            return dtTable;
        }

        private List<Dictionary<string, object>> GetBarData(List<ChartData> charts, bool IsCategory, ServiceBoard.Helper.SPBoardHelper.YearOrMonth yrm = SPBoardHelper.YearOrMonth.None)
        {
            DataTable dtTable = null;
            List<Bar> bars = new List<Bar>();
            SPBoardHelper helper = new SPBoardHelper();

            foreach (ChartData chart in charts)
            {
                if(!IsCategory){
                    if(yrm == SPBoardHelper.YearOrMonth.Year)
                        bars.Add(new Bar() { BarXLabel = chart.Label, BarValue = chart.Point, BarLegend = chart.DateLabel.Substring(0, 4) });
                    else
                        bars.Add(new Bar() { BarXLabel = chart.Label, BarValue = chart.Point, BarLegend = helper.GetYearMonth(chart.DateLabel) });
                }
                else
                {
                    if (yrm == SPBoardHelper.YearOrMonth.Year)
                        bars.Add(new Bar() { BarXLabel = chart.Category, BarValue = chart.YTDAmount, BarLegend = helper.GetYearMonth(chart.DateLabel) });
                    else if (yrm == SPBoardHelper.YearOrMonth.Month)
                        bars.Add(new Bar() { BarXLabel = chart.Category, BarValue = chart.MTDAmount, BarLegend = helper.GetYearMonth(chart.DateLabel) });
                    else
                        bars.Add(new Bar() { BarXLabel = chart.Label, BarValue = chart.Point, BarLegend = chart.DateLabel });
                }
            }

            var count = bars.GroupBy(i => new { Date = i.BarLegend })
                 .Select(group => new
                 {
                     Label = group.First().BarLegend
                 }).ToList().Count;

            int weeksCount = bars.Count / count;

            if (bars.Count >= weeksCount)
            {
                dtTable = RowToColumn(bars, weeksCount);

                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dtTable.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dtTable.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }

                 return rows;
            }
            else
                return null;
        }

        public List<LineData> GetLineData(List<ChartData> charts)
        {
            List<LineData> lines = new List<LineData>();

            var colCount = charts.GroupBy(i => new { Label = i.Label })
                             .Select(group => new
                             {
                                 Label = group.First().Label
                             }).ToList().Count;

            //int weeksCount = charts.Count / colCount;

            foreach (ChartData chart in charts)
                lines.Add(new LineData() { DateLabel = chart.Label, lineValue = chart.Point, label = chart.Label, Count = charts.Count });

            return lines;
        }

        [HttpPost]
        public ActionResult SalesAnalysis(int reportType)
        {
            ChartType chartTypeObj = new ChartType();
            List<ChartData> charts = new List<ChartData>();
            SPBoardRepository repository = new SPBoardRepository();

            charts = repository.GetSalesAnalysis(reportType);

            if (charts != null)
            {
                var chartList = charts.OrderByDescending(c => Convert.ToInt32(c.DateLabel.Substring(0, 4))).ToList();
                chartTypeObj.Bars = GetBarData(chartList, false, SPBoardHelper.YearOrMonth.Year);

                return Json(chartTypeObj, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SalesAnalysisByCategorySum(int reportType, string yearOrMonth)
        {
            ChartType chartTypeObj = new ChartType();
            List<ChartData> charts = new List<ChartData>();
            SPBoardRepository repository = new SPBoardRepository();
            SPBoardHelper.YearOrMonth yrm;

            yrm = yearOrMonth.ToLower() == "y" ? SPBoardHelper.YearOrMonth.Year : SPBoardHelper.YearOrMonth.Month;

            charts = repository.GetSalesAnalysisByCategorySum(reportType);

            if (charts != null)
            {
                var chartList = charts.OrderByDescending(c => Convert.ToInt32(c.DateLabel.Substring(0, 4))).ToList();
                chartTypeObj.Bars = GetBarData(chartList, true, yrm);

                return Json(chartTypeObj, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalesAnalysisByCategoryDetail()
        {
            if (Session["CompanyID"] == null)
                return RedirectToAction("Index", "Login");

            SPBoardHelper helper = new SPBoardHelper();

            ViewBag.Categories = helper.LoadCategory();

            return View();
        }

        [HttpPost]
        public ActionResult SalesAnalysisByCategoryDetail(string category, DateTime fromDate, DateTime toDate)
        {
            ChartType chartTypeObj = new ChartType();
            List<ChartData> charts = new List<ChartData>();
            SPBoardRepository repository = new SPBoardRepository();
            SPBoardHelper.YearOrMonth yrm = SPBoardHelper.YearOrMonth.Year;

            charts = repository.GetSalesAnalysisByCategoryDetail(category, fromDate, toDate);

            if (charts != null)
            {
                var chartList = charts.OrderByDescending(c => Convert.ToInt32(c.DateLabel.Substring(0, 4))).ToList();
                chartTypeObj.Bars = GetBarData(chartList, false, yrm);

                return Json(chartTypeObj, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DebtorAnalysis()
        {
            ChartType chartTypeObj = new ChartType();
            List<ChartData> charts = new List<ChartData>();
            SPBoardRepository repository = new SPBoardRepository();

            charts = repository.GetDebtorAnalysis("[ALL]", 1, 100, 2, "[ALL]", false, "", "", 1, false, DateTime.Today, false);

            if (charts != null)
            {
                //var chartList = charts.OrderByDescending(c => Convert.ToInt32(c.DateLabel.Substring(0, 4))).ToList();
                chartTypeObj.Lines = GetLineData(charts);

                return Json(chartTypeObj, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DebtorAnalysisDetail()
        {
            if (Session["CompanyID"] == null)
                return RedirectToAction("Index", "Login");

            SPBoardHelper helper = new SPBoardHelper();

            ViewBag.InvoiceTypes = helper.LoadInvoiceTypes();
            ViewBag.Areas = helper.LoadAreas();
            ViewBag.DateBalances = helper.LoadDateBalance();

            return View();
        }

        [HttpPost]
        public ActionResult DebtorAnalysisDetail(string invoiceType, decimal customerFrom, decimal customerTo, int sortBy, string area, bool printCredit, string nameFrom, string nameTo, int ageBal, bool unIndJobs, DateTime balanceDate, bool retention)
        {
            //NOT COMPLETED

            ChartType chartTypeObj = new ChartType();
            List<ChartData> charts = new List<ChartData>();
            SPBoardRepository repository = new SPBoardRepository();

            //charts = repository.GetDebtorAnalysis("", 0, 0, 0, "", true, "", "", 0, false, DateTime.Today, true);
            charts = repository.GetDebtorAnalysis(invoiceType, customerFrom, customerTo, sortBy, area, printCredit, nameFrom, nameTo, ageBal, unIndJobs, balanceDate, retention);

            if (charts != null)
            {
                //var chartList = charts.OrderByDescending(c => Convert.ToInt32(c.DateLabel.Substring(0, 4))).ToList();
                chartTypeObj.Lines = GetLineData(charts);
                chartTypeObj.Bars = GetBarData(charts, true);

                return Json(chartTypeObj, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ResourceUtilizationDetail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResourceUtilizationDetail(DateTime fromDate, DateTime toDate)
        {
            ChartType chartTypeObj = new ChartType();
            SPBoardRepository repository = new SPBoardRepository();
            ResourceUtilization resource = new ResourceUtilization();

            resource = repository.GetResourceUtilization(fromDate, toDate, true);
            var dateChartList = resource.Charts.Where(c => c.Category.ToLower().Equals("bydate")).ToList();
            var chartList = dateChartList.OrderByDescending(o => o.DateLabel).ToList();
            chartTypeObj.Bars = GetBarData(chartList, true);

            return Json(chartTypeObj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ResourceUtilizationOneDayPerTech()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResourceUtilizationOneDayPerTech(DateTime fromDate)
        {
            ChartType chartTypeObj = new ChartType();
            SPBoardRepository repository = new SPBoardRepository();
            ResourceUtilization resource = new ResourceUtilization();

            resource = repository.GetResourceUtilizationOneDayPerTech(fromDate, true);
            if (resource != null && resource.Charts != null)
            {
                var techChartList = resource.Charts.Where(c => c.Category.ToLower().Equals("bytech")).ToList().OrderByDescending(o => o.DateLabel).ToList();
                chartTypeObj.Bars = GetBarData(techChartList, true);
            }

            return Json(chartTypeObj, JsonRequestBehavior.AllowGet);
        }
	}
}