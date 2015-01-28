using CASPortal.CASWCFService;
using CASPortal.Helper;
using CASPortal.Models;
using CASPortal.Repository;
using CASPortal.WebParser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CASPortal.Controllers
{
    public class ReportController : BaseController
    {
        //
        // GET: /Report/
        public new ActionResult Index()
        {
            return View();
        }

        public DataTable RowToColumn(List<BarData> lineData, int weeksCount)
        {
            DataRow row;
            int count = lineData.Count / weeksCount;
            DataTable dtTable = new DataTable();

            dtTable.Columns.Add("DateLabel", typeof(string));
            for (int index = 0; index < count; index++)
            {
                dtTable.Columns.Add(lineData[index * weeksCount].label, typeof(string));
            }

            for (int index = 0; index < weeksCount; index++)
            {
                int rowIndex = 1;
                row = dtTable.NewRow();

                row[0] = lineData[index].DateLabel;
                for (int innerIndex = index; innerIndex < lineData.Count; innerIndex += weeksCount)
                {
                    row[rowIndex] = lineData[innerIndex].barValue;
                    rowIndex++;
                }
                dtTable.Rows.Add(row);
            }

            List<DataRow> rowToList = dtTable.AsEnumerable().ToList();

            return dtTable;
        }

        private List<Dictionary<string, object>> GetBarData(List<ChartData> charts, bool IsQuestion)
        {
            DataTable dtTable = null;
            List<BarData> bars = new List<BarData>();
            foreach (ChartData chart in charts)
            {
                if(IsQuestion)
                    bars.Add(new BarData() { DateLabel = chart.DateLabel, barValue = chart.Point, label = chart.Section + chart.Area + chart.Question });
                else
                    bars.Add(new BarData() { DateLabel = chart.DateLabel, barValue = chart.Point, label = chart.SerialNumber });
            }

            var count = bars.GroupBy(i => new { Date = i.label })
                 .Select(group => new
                 {
                     Label = group.First().label
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

        public List<LineData> GetLineData(List<ChartData> charts, bool IsQuestion)
        {
            List<LineData> lines = new List<LineData>();

            if (IsQuestion)
            {
                var colCount = charts.GroupBy(i => new { Section = i.Section, Question = i.Question })
                                 .Select(group => new
                                 {
                                     Label = group.First().Section
                                 }).ToList().Count;

                int weeksCount = charts.Count / colCount;

                foreach (ChartData chart in charts)
                {
                    lines.Add(new LineData() { DateLabel = chart.DateLabel, lineValue = chart.Point, label = chart.Section + chart.Question, Count = weeksCount });
                }
            }
            else
            {
                var colCount = charts.GroupBy(i => new { SerialNumber = i.SerialNumber })
                                 .Select(group => new
                                 {
                                     Label = group.First().SerialNumber
                                 }).ToList().Count;

                int weeksCount = charts.Count / colCount;

                foreach (ChartData chart in charts)
                {
                    lines.Add(new LineData() { DateLabel = chart.DateLabel, lineValue = chart.Point, label = chart.SerialNumber, Count = weeksCount });
                }
            }

            return lines;
        }

        public List<PieData> GetPieData(List<ChartData> charts, bool IsQuestion)
        {
            List<PieData> pies = new List<PieData>();

            if (IsQuestion)
            {
                var aggCharts = charts.GroupBy(x => new { x.Section, x.Question }).
                    Select(x => new { Section = x.Max(pd => pd.Section), Question = x.Max(pd => pd.Question), percentage = x.Sum(pd => pd.Point) });

                foreach (var pie in aggCharts)
                {
                    pies.Add(new PieData() { label = pie.Section + pie.Question, data = pie.percentage });
                }
            }
            else
            {
                var aggCharts = charts.GroupBy(x => new { x.SerialNumber }).
                    Select(x => new { SerialNumber = x.Max(pd => pd.SerialNumber), percentage = x.Sum(pd => pd.Point) });

                foreach (var pie in aggCharts)
                {
                    pies.Add(new PieData() { label = pie.SerialNumber, data = pie.percentage });
                }
            }

            return pies;
        }

        private DataTable AnswerTable(string selectedNodes)
        {
            DataTable dtAnswers = new DataTable();

            selectedNodes = selectedNodes.Replace("[\"", "");
            selectedNodes = selectedNodes.Replace("\"]", "");
            selectedNodes = selectedNodes.Replace("\"", "");

            string[] arr = selectedNodes.Split(',');
            string[] arrDis = arr.Distinct().ToArray();

            dtAnswers.TableName = "TreeAnswers";
            dtAnswers.Columns.Add("RootNode");
            dtAnswers.Columns.Add("SectionID");
            dtAnswers.Columns.Add("SectionDesc");
            dtAnswers.Columns.Add("QuestionID");
            dtAnswers.Columns.Add("QuestionDesc");
            dtAnswers.Columns.Add("AnswerID");
            dtAnswers.Columns.Add("AnswerDesc");
            dtAnswers.Columns.Add("SectionCaption");
            dtAnswers.Columns.Add("QuestionCaption");

            foreach (string node in arrDis)
            {
                if (node.Length > 8 && node.IndexOf("#") > 0)
                {
                    string[] nodeArr = node.Split('#');

                    DataRow row = dtAnswers.NewRow();

                    row["RootNode"] = TempData["RootNode"] == null ? "Barcode" : TempData["RootNode"].ToString();
                    row["SectionID"] = Convert.ToInt32(nodeArr[0]);
                    row["SectionDesc"] = nodeArr[1];
                    row["QuestionID"] = Convert.ToInt32(nodeArr[2]);
                    row["QuestionDesc"] = nodeArr[3];
                    row["AnswerID"] = Convert.ToInt32(nodeArr[4]);
                    row["AnswerDesc"] = nodeArr[5];
                    row["SectionCaption"] = nodeArr[1];
                    row["QuestionCaption"] = nodeArr[3];

                    dtAnswers.Rows.Add(row);
                }
            }

            if (dtAnswers.Rows.Count > 0)
            {
                DataView dv = dtAnswers.DefaultView;
                dv.Sort = "AnswerID";
                dtAnswers = dv.ToTable();

                dv = dtAnswers.DefaultView;
                dv.Sort = "QuestionID";
                dtAnswers = dv.ToTable();

                dv = dtAnswers.DefaultView;
                dv.Sort = "SectionID";
                dtAnswers = dv.ToTable();
            }

            return dtAnswers;
        }

        public ActionResult TrendAnalysis()
        {
            BaseHelper helper = new BaseHelper();
            ReportHelper repoHelper = new ReportHelper();

            if (!helper.IsValidUser())
                return RedirectToAction("Index", "Login");

            TempData["RootNode"] = "Barcode";

            ViewBag.Sites = repoHelper.LoadSite();
            ViewBag.TreeNodes = repoHelper.LoadTree();
            ViewBag.Areas = repoHelper.LoadArea();

            return View();
        }

        [HttpPost]
        public ActionResult TrendAnalysis(int siteNo, int contractNo, string selectedNodes, string area, int frequency, string fromDate, string toDate, int groupBy, bool jobdate, string chartType)
        {
            DataTable dtAnswers = new DataTable();
            ReportRepository repository = new ReportRepository();
            DateTime dtFrom = DateTime.Parse(fromDate, new CultureInfo("en-US"));
            DateTime dtTo = DateTime.Parse(toDate, new CultureInfo("en-US"));
            ChartType chartTypeObj = new ChartType();
            List<ChartData> charts = new List<ChartData>();

            dtAnswers = AnswerTable(selectedNodes);
            charts = repository.GetTrendAnalysis(siteNo, contractNo, dtAnswers, area, frequency, dtFrom, dtTo, groupBy, jobdate);

            if (charts != null)
            {
                if (chartType.Equals("BAR") || chartType.Equals("All Charts"))
                    chartTypeObj.Bars = GetBarData(charts, true);

                if (chartType.Equals("LINE") || chartType.Equals("All Charts"))
                    chartTypeObj.Lines = GetLineData(charts, true);

                if (chartType.Equals("PIE") || chartType.Equals("All Charts"))
                    chartTypeObj.Pies = GetPieData(charts, true);

                return Json(chartTypeObj, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TrendAnalysisByQuestion()
        {
            BaseHelper helper = new BaseHelper();
            ReportHelper repoHelper = new ReportHelper();
            
            if (!helper.IsValidUser())
                return RedirectToAction("Index", "Login");

            TempData["RootNode"] = "Barcode";

            ViewBag.Sites = repoHelper.LoadSite();
            //ViewBag.Contracts = repoHelper.LoadContract("1");
            ViewBag.TreeNodes = repoHelper.LoadTree();
            ViewBag.Areas = repoHelper.LoadArea();
            
            return View();
        }

        [HttpPost]
        public ActionResult TrendAnalysisByQuestion(int siteNo, int contractNo, string selectedNodes, int frequency, string fromDate, string toDate, int groupBy, string chartType)
        {
            DataTable dtAnswers = new DataTable();
            ReportRepository repository = new ReportRepository();
            DateTime dtFrom = DateTime.Parse(fromDate, new CultureInfo("en-US"));
            DateTime dtTo = DateTime.Parse(toDate, new CultureInfo("en-US"));
            ChartType chartTypeObj = new ChartType();
            List<ChartData> charts = new List<ChartData>();

            dtAnswers = AnswerTable(selectedNodes);
            charts = repository.GetTrendAnalysisByQuestion(siteNo, contractNo, dtAnswers, frequency, dtFrom, dtTo, groupBy);

            if (charts != null)
            {
                if (chartType.Equals("BAR") || chartType.Equals("All Charts"))
                    chartTypeObj.Bars = GetBarData(charts, true);

                if (chartType.Equals("LINE") || chartType.Equals("All Charts"))
                    chartTypeObj.Lines = GetLineData(charts, true);

                if (chartType.Equals("PIE") || chartType.Equals("All Charts"))
                    chartTypeObj.Pies = GetPieData(charts, true);

                return Json(chartTypeObj, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TrendAnalysisByJob()
        {
            BaseHelper helper = new BaseHelper();
            ReportHelper repoHelper = new ReportHelper();
            
            if (!helper.IsValidUser())
                return RedirectToAction("Index", "Login");

            TempData["RootNode"] = "Barcode";

            ViewBag.Sites = repoHelper.LoadSite();
            //ViewBag.Contracts = repoHelper.LoadContract("1");
            ViewBag.TreeNodes = repoHelper.LoadTree();
            ViewBag.Areas = repoHelper.LoadArea();

            return View();
        }

        [HttpPost]
        public ActionResult TrendAnalysisByJob(int siteNo, int contractNo, string selectedNodes, string area, string fromDate, string toDate, string chartType)
        {
            DataTable dtAnswers = new DataTable();
            ReportRepository repository = new ReportRepository();
            DateTime dtFrom = DateTime.Parse(fromDate, new CultureInfo("en-US"));
            DateTime dtTo = DateTime.Parse(toDate, new CultureInfo("en-US"));
            ChartType chartTypeObj = new ChartType();
            List<ChartData> charts = new List<ChartData>();

            dtAnswers = AnswerTable(selectedNodes);
            charts = repository.GetTrendAnalysisByJob(siteNo, contractNo, dtAnswers, area, dtFrom, dtTo);

            if (charts != null)
            {
                if (chartType.Equals("BAR") || chartType.Equals("All Charts"))
                    chartTypeObj.Bars = GetBarData(charts, true);

                if (chartType.Equals("LINE") || chartType.Equals("All Charts"))
                    chartTypeObj.Lines = GetLineData(charts, true);

                if (chartType.Equals("PIE") || chartType.Equals("All Charts"))
                    chartTypeObj.Pies = GetPieData(charts, true);

                return Json(chartTypeObj, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TrendAnalysisByEquip()
        {
            BaseHelper helper = new BaseHelper();
            ReportHelper repoHelper = new ReportHelper();

            if (!helper.IsValidUser())
                return RedirectToAction("Index", "Login");

            TempData["RootNode"] = "Barcode";

            ViewBag.Sites = repoHelper.LoadSite();
            //ViewBag.Contracts = repoHelper.LoadContract("1");
            ViewBag.TreeNodes = repoHelper.LoadTree();
            ViewBag.Areas = repoHelper.LoadArea();

            return View();
        }

        [HttpPost]
        public ActionResult TrendAnalysisByEquip(int siteNo, int contractNo, string selectedNodes, int frequency, string fromDate, string toDate, int groupBy, string sortBy, bool exclude, string chartType)
        {
            bool sortedBy = false;
            DataTable dtAnswers = new DataTable();
            ReportRepository repository = new ReportRepository();
            DateTime dtFrom = DateTime.Parse(fromDate, new CultureInfo("en-US"));
            DateTime dtTo = DateTime.Parse(toDate, new CultureInfo("en-US"));
            ChartType chartTypeObj = new ChartType();
            List<ChartData> charts = new List<ChartData>();

            sortedBy = sortBy.Equals("Location") ? false : true;

            dtAnswers = AnswerTable(selectedNodes);
            charts = repository.GetTrendAnalysisByEquipment(siteNo, contractNo, dtAnswers, frequency, dtFrom, dtTo, groupBy, sortedBy, exclude);

            if (charts != null)
            {
                if (chartType.Equals("BAR") || chartType.Equals("All Charts"))
                    chartTypeObj.Bars = GetBarData(charts, false);

                if (chartType.Equals("LINE") || chartType.Equals("All Charts"))
                    chartTypeObj.Lines = GetLineData(charts, false);

                if (chartType.Equals("PIE") || chartType.Equals("All Charts"))
                    chartTypeObj.Pies = GetPieData(charts, false);

                return Json(chartTypeObj, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TrendAnalysisGroupLocation()
        {
            BaseHelper helper = new BaseHelper();
            ReportHelper repoHelper = new ReportHelper();

            if (!helper.IsValidUser())
                return RedirectToAction("Index", "Login");

            TempData["RootNode"] = "Barcode";

            ViewBag.Sites = repoHelper.LoadSite();
            //ViewBag.Contracts = repoHelper.LoadContract("1");
            ViewBag.TreeNodes = repoHelper.LoadTree();
            ViewBag.Areas = repoHelper.LoadArea();

            return View();
        }

        public ActionResult GetContracts(string siteNo)
        {
            try
            {
                ReportHelper repoHelper = new ReportHelper();

                string contracts = repoHelper.LoadContract(siteNo);

                return Json(contracts, JsonRequestBehavior.AllowGet);
            }
            catch (TimeoutException ex)
            {
                return Json("Session timed out", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadChartImage(string base64Data, string chartType)
        {
            byte[] fileInfo = null;

            fileInfo = Convert.FromBase64String(base64Data);

            return File(fileInfo, "application/octet", chartType + "_" + DateTime.Now.Ticks + ".png");
        }

        public ActionResult EquipmentTransaction()
        {
            BaseHelper helper = new BaseHelper();
            ReportHelper repoHelper = new ReportHelper();

            if (!helper.IsValidUser())
                return RedirectToAction("Index", "Login");

            ViewBag.Sites = repoHelper.LoadSite();
            ViewBag.Tech = repoHelper.LoadTech();

            return View();
        }

        [HttpPost]
        public ActionResult EquipmentTransaction(FormCollection elemets)
        {

            //return Json("No Data.", JsonRequestBehavior.AllowGet);

            //byte[] fileInfo = null;
            Equipment equip = new Equipment();
            ReportRepository repository = new ReportRepository();

            try
            {
                int selectionID = 0;
                int sortingID = 0;
                string siteID = elemets["hdnSite"];
                string contractID = elemets["hdnContract"];
                string dateFrom = elemets["dtpFrom"];
                string dateTo = elemets["dtpTo"];
                string selection = elemets["rdoLocation"];
                bool isPrintDetails = elemets["hdnPrintDetails"] == "true" ? true : false;
                bool isPrintMaterials = elemets["hdnPrintMaterials"] == "true" ? true : false;
                string sorting = elemets["rdoAdvance"];
                string tech = elemets["hdnTech"];
                bool isInactive = elemets["hdnShowActiveStations"] == "true" ? true : false;
                bool isShowTime = elemets["hdnJobTimes"] == "true" ? true : false;
                string hdnFormSubmit = elemets["hdnFormSubmit"];

                if (selection.Equals("Location"))
                    selectionID = 1;
                else if (selection.Equals("Section"))
                    selectionID = 2;
                else if (selection.Equals("Area"))
                    selectionID = 3;
                else if (selection.Equals("DateTime"))
                    selectionID = 4;
                else
                    selectionID = 5;

                if (sorting.Equals("None"))
                    sortingID = 1;
                else if (sorting.Equals("Scanned"))
                    sortingID = 2;
                else if (sorting.Equals("Un Scanned"))
                    sortingID = 3;
                else if (sorting.Equals("New Item"))
                    sortingID = 4;
                else
                    sortingID = 5;

                if (hdnFormSubmit == null || hdnFormSubmit.Trim().Length == 0)
                {
                    if (Session["EQUIPTRANSBLOB"] != null)
                        Session.Remove("EQUIPTRANSBLOB");

                    equip = repository.GetEquipmentTransactionBLOB(DateTime.Parse(dateFrom), DateTime.Parse(dateTo), isPrintDetails, isPrintMaterials,
                        sortingID, "[ALL]", selectionID, Convert.ToInt32(contractID), Convert.ToInt32(contractID),
                        isInactive, isShowTime, "[ALL]");
                }

                if (hdnFormSubmit != null && hdnFormSubmit.Trim().Length > 0)
                {
                    if (Session["EQUIPTRANSBLOB"] != null)
                        return File((byte[])Session["EQUIPTRANSBLOB"], "application/octet", (DateTime.Now.Ticks.ToString() + ".pdf"));
                }

                if (equip.OutputMessage.Trim().Length > 0)
                    return Json("No BLOB", JsonRequestBehavior.AllowGet);
                else
                {
                    Session["EQUIPTRANSBLOB"] = equip.FileBLOB;
                    return Json("BLOB", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("No BLOB", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult InstalledEquipment()
        {
            BaseHelper helper = new BaseHelper();
            ReportHelper repoHelper = new ReportHelper();

            if (!helper.IsValidUser())
                return RedirectToAction("Index", "Login");

            ViewBag.Sites = repoHelper.LoadSite();

            return View();
        }

        public ActionResult GetInstalledLocations(int contractNo)
        {
            List<Equipment> equips = new List<Equipment>();
            ReportRepository repo = new ReportRepository();

            equips = repo.GetInstalledEquipment(contractNo, "CO");
            Session["EquipmentList"] = equips;

            return Json(equips, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInstalledLocationDetail(string location)
        {
            List<Equipment> equips = new List<Equipment>();

            equips = (List<Equipment>)Session["EquipmentList"];
            var equip = (from e in equips
                        where e.Location.Equals(location)
                        select e).SingleOrDefault();

            return Json(equip, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EquipmentReport()
        {
            BaseHelper helper = new BaseHelper();
            ReportHelper repoHelper = new ReportHelper();

            if (!helper.IsValidUser())
                return RedirectToAction("Index", "Login");

            ViewBag.Sites = repoHelper.LoadSite();
            //ViewBag.Contracts = repoHelper.LoadContract("1");

            return View();
        }

        [HttpPost]
        public ActionResult EquipmentReport(FormCollection elemets)
        {
            byte[] fileInfo = null;
            Equipment equip = new Equipment();
            ReportRepository repository = new ReportRepository();

            try
            {
                int statusID = 0;
                int sortingID = 0;

                string siteID = elemets["hdnSite"];
                string contractID = elemets["hdnContract"];
                string status = elemets["rdoStatus"];
                string sorting = elemets["rdoSort"];
                string hdnFormSubmit = elemets["hdnFormSubmit"];

                if (status.Equals("All"))
                    statusID = 1;
                else if (status.Equals("Active"))
                    statusID = 2;
                else
                    statusID = 3;

                if (sorting.Equals("Location"))
                    sortingID = 1;
                else if (sorting.Equals("Area"))
                    sortingID = 2;

                if (hdnFormSubmit == null || hdnFormSubmit.Trim().Length == 0)
                {
                    if (Session["EQUIPREPORTBLOB"] != null)
                        Session.Remove("EQUIPREPORTBLOB");

                    equip = repository.GetEquipmentReportBLOB(Convert.ToInt32(contractID), Convert.ToInt32(contractID), sortingID, statusID);
                }

                if (hdnFormSubmit != null && hdnFormSubmit.Trim().Length > 0)
                {
                    if (Session["EQUIPREPORTBLOB"] != null)
                        return File((byte[])Session["EQUIPREPORTBLOB"], "application/octet", (DateTime.Now.Ticks.ToString() + ".pdf"));
                }

                if (equip.OutputMessage.Trim().Length > 0)
                    return Json("No BLOB", JsonRequestBehavior.AllowGet);
                else
                {
                    Session["EQUIPREPORTBLOB"] = equip.FileBLOB;
                    return Json("BLOB", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("No BLOB", JsonRequestBehavior.AllowGet);
            }
        }
    }
}