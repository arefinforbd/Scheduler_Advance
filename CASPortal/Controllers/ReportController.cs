using CASPortal.CASWCFService;
using CASPortal.Helper;
using CASPortal.Models;
using CASPortal.Repository;
using CASPortal.WebParser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CASPortal.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/
        public ActionResult Index()
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

        private List<Dictionary<string, object>> GetBarData(List<ChartData> charts)
        {
            DataTable dtTable = null;
            List<BarData> bars = new List<BarData>();
            foreach (ChartData chart in charts)
            {
                bars.Add(new BarData() { DateLabel = chart.DateLabel, barValue = chart.Point, label = chart.Section + " (" + chart.Question + ")" });
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

                JavaScriptSerializer serializer = new JavaScriptSerializer();
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
            var colCount = charts.GroupBy(i => new { Section = i.Section, Question = i.Question })
                 .Select(group => new
                 {
                     Label = group.First().Section
                 }).ToList().Count;

            int weeksCount = charts.Count / colCount;
            List<LineData> lines = new List<LineData>();

            foreach (ChartData chart in charts)
            {
                lines.Add(new LineData() { DateLabel = chart.DateLabel, lineValue = chart.Point, label = chart.Section + " (" + chart.Question + ")", Count = weeksCount });
            }

            return lines;
        }

        public List<PieData> GetPieData(List<ChartData> charts)
        {
            List<PieData> pies = new List<PieData>();


            var aggCharts = charts.GroupBy(x => new { x.Section, x.Question }).
                Select(x => new { Section = x.Max(pd => pd.Section), Question = x.Max(pd => pd.Question), percentage =  x.Sum(pd => pd.Point)});


            foreach (var pie in aggCharts)
            {
                pies.Add(new PieData() { label = pie.Section + " (" + pie.Question + ")", data = pie.percentage });
            }

            //List<PieData> pies = new List<PieData>()
            //{
            //    new PieData(){label = "Series 0", data = 15},
            //    new PieData(){label = "Series 1", data = 5},
            //    new PieData(){label = "Series 2", data = 2},
            //    new PieData(){label = "Series 3", data = 3}
            //};

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

        public ActionResult TrendAnalysisByQuestion()
        {
            BaseHelper helper = new BaseHelper();
            ReportHelper repoHelper = new ReportHelper();
            
            if (!helper.IsValidUser())
                return RedirectToAction("Index", "Login");

            TempData["RootNode"] = "Barcode";

            ViewBag.Sites = repoHelper.LoadSite();
            ViewBag.Contracts = repoHelper.LoadContract();
            ViewBag.TreeNodes = repoHelper.LoadTree();
            ViewBag.Areas = repoHelper.LoadArea();
            
            return View();
        }

        [HttpPost]
        public ActionResult TrendAnalysisByQuestion(int siteNo, int contractNo, string selectedNodes, string area, int frequency, string fromDate, string toDate, int groupBy, string chartType)
        {
            DataTable dtAnswers = new DataTable();
            ReportRepository repository = new ReportRepository();
            DateTime dtFrom = Convert.ToDateTime(fromDate);
            DateTime dtTo = Convert.ToDateTime(toDate);
            ChartType chartTypeObj = new ChartType();
            List<ChartData> charts = new List<ChartData>();

            dtAnswers = AnswerTable(selectedNodes);
            charts = repository.PostTrendAnalysisReportData(siteNo, contractNo, dtAnswers, area, frequency, dtFrom, dtTo, groupBy);

            if (charts != null)
            {
                if (chartType.Equals("BAR") || chartType.Equals("ALL"))
                    chartTypeObj.Bars = GetBarData(charts);

                if (chartType.Equals("LINE") || chartType.Equals("ALL"))
                    chartTypeObj.Lines = GetLineData(charts);

                if (chartType.Equals("PIE") || chartType.Equals("ALL"))
                    chartTypeObj.Pies = GetPieData(charts);

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
            ViewBag.Contracts = repoHelper.LoadContract();
            ViewBag.TreeNodes = repoHelper.LoadTree();
            ViewBag.Areas = repoHelper.LoadArea();

            return View();
        }

        public ActionResult TrendAnalysisByEquip()
        {
            BaseHelper helper = new BaseHelper();
            ReportHelper repoHelper = new ReportHelper();

            if (!helper.IsValidUser())
                return RedirectToAction("Index", "Login");

            TempData["RootNode"] = "Barcode";

            ViewBag.Sites = repoHelper.LoadSite();
            ViewBag.Contracts = repoHelper.LoadContract();
            ViewBag.TreeNodes = repoHelper.LoadTree();
            ViewBag.Areas = repoHelper.LoadArea();

            return View();
        }

        public ActionResult TrendAnalysisGroupLocation()
        {
            BaseHelper helper = new BaseHelper();
            ReportHelper repoHelper = new ReportHelper();

            if (!helper.IsValidUser())
                return RedirectToAction("Index", "Login");

            TempData["RootNode"] = "Barcode";

            ViewBag.Sites = repoHelper.LoadSite();
            ViewBag.Contracts = repoHelper.LoadContract();
            ViewBag.TreeNodes = repoHelper.LoadTree();
            ViewBag.Areas = repoHelper.LoadArea();

            return View();
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
            ViewBag.Contracts = repoHelper.LoadContract();

            return View();
        }

        [HttpPost]
        public ActionResult EquipmentTransaction(FormCollection elemets)
        {
            byte[] fileInfo = null;
            try
            {
                int locationID = 0;
                int advanceID = 0;
                string siteID = elemets["hdnSite"];
                string contractID = elemets["hdnContract"];
                string dateFrom = elemets["dtpFrom"];
                string dateTo = elemets["dtpTo"];
                string location = elemets["rdoLocation"];
                bool printDetails = elemets["hdnPrintDetails"] == "true" ? true : false;
                bool printMaterials = elemets["hdnPrintMaterials"] == "true" ? true : false;
                string advance = elemets["rdoAdvance"];
                string tech = elemets["hdnTech"];
                bool showActiveStations = elemets["hdnShowActiveStations"] == "true" ? true : false;
                bool jobTimes = elemets["hdnJobTimes"] == "true" ? true : false;

                if (location.Equals("Location"))
                    locationID = 1;
                else if (location.Equals("Section"))
                    locationID = 2;
                else if (location.Equals("Area"))
                    locationID = 3;
                else if (location.Equals("DateTime"))
                    locationID = 4;
                else
                    locationID = 5;

                if (advance.Equals("None"))
                    advanceID = 1;
                else if (advance.Equals("Scanned"))
                    advanceID = 2;
                else if (advance.Equals("Un Scanned"))
                    advanceID = 3;
                else if (advance.Equals("New Item"))
                    advanceID = 4;
                else
                    advanceID = 5;

                return File(fileInfo, "application/octet", "file-name");
            }
            catch (Exception ex)
            {
                return File(new byte[2], "application/octet", "file-name");
            }
        }
    }
}