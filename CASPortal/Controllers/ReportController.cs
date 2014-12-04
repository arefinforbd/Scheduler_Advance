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
                    row[rowIndex] = lineData[innerIndex].lineValue;
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
                bars.Add(new BarData() { DateLabel = chart.DateLabel, lineValue = chart.Point, label = chart.Section + "__" + chart.Question + "_" });
            }


            //List<BarData> bars2 = new List<BarData>()
            //{
            //    new BarData(){DateLabel = "01/01/2014", lineValue = 0, label="EXTERN__MOUSE_"},
            //    new BarData(){DateLabel = "08/01/2014", lineValue = 1, label="EXTERN__MOUSE_"},
            //    new BarData(){DateLabel = "15/01/2014", lineValue = 0, label="EXTERN__MOUSE_"},
            //    new BarData(){DateLabel = "22/01/2014", lineValue = 1, label="EXTERN__MOUSE_"},
            //    new BarData(){DateLabel = "29/01/2014", lineValue = 0, label="EXTERN__MOUSE_"},
            //    new BarData(){DateLabel = "05/02/2014", lineValue = 1, label="EXTERN__MOUSE_"},
            //    new BarData(){DateLabel = "12/02/2014", lineValue = 0, label="EXTERN__MOUSE_"},
            //    new BarData(){DateLabel = "19/02/2014", lineValue = 1, label="EXTERN__MOUSE_"},
            //    new BarData(){DateLabel = "26/02/2014", lineValue = 0, label="EXTERN__MOUSE_"},
            //    new BarData(){DateLabel = "05/03/2014", lineValue = 1, label="EXTERN__MOUSE_"},
            //    new BarData(){DateLabel = "12/03/2014", lineValue = 0, label="EXTERN__MOUSE_"},
            //    new BarData(){DateLabel = "19/03/2014", lineValue = 1, label="EXTERN__MOUSE_"},

            //    new BarData(){DateLabel = "01/01/2014", lineValue = 5, label="EXTERN__RAT_"},
            //    new BarData(){DateLabel = "08/01/2014", lineValue = 1, label="EXTERN__RAT_"},
            //    new BarData(){DateLabel = "15/01/2014", lineValue = 5, label="EXTERN__RAT_"},
            //    new BarData(){DateLabel = "22/01/2014", lineValue = 1, label="EXTERN__RAT_"},
            //    new BarData(){DateLabel = "29/01/2014", lineValue = 5, label="EXTERN__RAT_"},
            //    new BarData(){DateLabel = "05/02/2014", lineValue = 1, label="EXTERN__RAT_"},
            //    new BarData(){DateLabel = "12/02/2014", lineValue = 5, label="EXTERN__RAT_"},
            //    new BarData(){DateLabel = "19/02/2014", lineValue = 1, label="EXTERN__RAT_"},
            //    new BarData(){DateLabel = "26/02/2014", lineValue = 5, label="EXTERN__RAT_"},
            //    new BarData(){DateLabel = "05/03/2014", lineValue = 1, label="EXTERN__RAT_"},
            //    new BarData(){DateLabel = "12/03/2014", lineValue = 5, label="EXTERN__RAT_"},
            //    new BarData(){DateLabel = "19/03/2014", lineValue = 1, label="EXTERN__RAT_"},

            //    new BarData(){DateLabel = "01/01/2014", lineValue = 1, label="INTERN__EFK_"},
            //    new BarData(){DateLabel = "08/01/2014", lineValue = 2, label="INTERN__EFK_"},
            //    new BarData(){DateLabel = "15/01/2014", lineValue = 1, label="INTERN__EFK_"},
            //    new BarData(){DateLabel = "22/01/2014", lineValue = 2, label="INTERN__EFK_"},
            //    new BarData(){DateLabel = "29/01/2014", lineValue = 1, label="INTERN__EFK_"},
            //    new BarData(){DateLabel = "05/02/2014", lineValue = 2, label="INTERN__EFK_"},
            //    new BarData(){DateLabel = "12/02/2014", lineValue = 1, label="INTERN__EFK_"},
            //    new BarData(){DateLabel = "19/02/2014", lineValue = 2, label="INTERN__EFK_"},
            //    new BarData(){DateLabel = "26/02/2014", lineValue = 1, label="INTERN__EFK_"},
            //    new BarData(){DateLabel = "05/03/2014", lineValue = 2, label="INTERN__EFK_"},
            //    new BarData(){DateLabel = "12/03/2014", lineValue = 1, label="INTERN__EFK_"},
            //    new BarData(){DateLabel = "19/03/2014", lineValue = 2, label="INTERN__EFK_"},

            //    new BarData(){DateLabel = "01/01/2014", lineValue = 1, label="INTERN__MOUSE_"},
            //    new BarData(){DateLabel = "08/01/2014", lineValue = 2, label="INTERN__MOUSE_"},
            //    new BarData(){DateLabel = "15/01/2014", lineValue = 1, label="INTERN__MOUSE_"},
            //    new BarData(){DateLabel = "22/01/2014", lineValue = 2, label="INTERN__MOUSE_"},
            //    new BarData(){DateLabel = "29/01/2014", lineValue = 1, label="INTERN__MOUSE_"},
            //    new BarData(){DateLabel = "05/02/2014", lineValue = 2, label="INTERN__MOUSE_"},
            //    new BarData(){DateLabel = "12/02/2014", lineValue = 1, label="INTERN__MOUSE_"},
            //    new BarData(){DateLabel = "19/02/2014", lineValue = 2, label="INTERN__MOUSE_"},
            //    new BarData(){DateLabel = "26/02/2014", lineValue = 1, label="INTERN__MOUSE_"},
            //    new BarData(){DateLabel = "05/03/2014", lineValue = 2, label="INTERN__MOUSE_"},
            //    new BarData(){DateLabel = "12/03/2014", lineValue = 1, label="INTERN__MOUSE_"},
            //    new BarData(){DateLabel = "19/03/2014", lineValue = 2, label="INTERN__MOUSE_"}
            //};

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

                //return Json(serializer.Serialize(rows), JsonRequestBehavior.AllowGet);
                return rows;
            }
            else
                return null;
        }

        public List<LineData> GetLineData(List<ChartData> charts)
        {
            /*
             * Get number of columns and rows.
             * Substract 1 from number of columns.
             * Divide number of rows by number of columns.
             * 
             */

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

            //List<LineData> charts = new List<LineData>()
            //{
            //    new LineData(){DateLabel = "01/012014", lineValue = 0, label="EXTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "08/01/2014", lineValue = 1, label="EXTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "15/01/2014", lineValue = 0, label="EXTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "22/01/2014", lineValue = 1, label="EXTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "29/01/2014", lineValue = 0, label="EXTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "05/02/2014", lineValue = 1, label="EXTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "12/02/2014", lineValue = 0, label="EXTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "19/02/2014", lineValue = 1, label="EXTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "26/02/2014", lineValue = 0, label="EXTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "05/03/2014", lineValue = 1, label="EXTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "12/03/2014", lineValue = 0, label="EXTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "19/03/2014", lineValue = 1, label="EXTERN (MOUSE)", Count = weeksCount},

            //    new LineData(){DateLabel = "01/012014", lineValue = 0.5, label="EXTERN (RAT)", Count = weeksCount},
            //    new LineData(){DateLabel = "08/01/2014", lineValue = 1.5, label="EXTERN (RAT)", Count = weeksCount},
            //    new LineData(){DateLabel = "15/01/2014", lineValue = 0.5, label="EXTERN (RAT)", Count = weeksCount},
            //    new LineData(){DateLabel = "22/01/2014", lineValue = 1.5, label="EXTERN (RAT)", Count = weeksCount},
            //    new LineData(){DateLabel = "29/01/2014", lineValue = 0.5, label="EXTERN (RAT)", Count = weeksCount},
            //    new LineData(){DateLabel = "05/02/2014", lineValue = 1.5, label="EXTERN (RAT)", Count = weeksCount},
            //    new LineData(){DateLabel = "12/02/2014", lineValue = 0.5, label="EXTERN (RAT)", Count = weeksCount},
            //    new LineData(){DateLabel = "19/02/2014", lineValue = 1.5, label="EXTERN (RAT)", Count = weeksCount},
            //    new LineData(){DateLabel = "26/02/2014", lineValue = 0.5, label="EXTERN (RAT)", Count = weeksCount},
            //    new LineData(){DateLabel = "05/03/2014", lineValue = 1.5, label="EXTERN (RAT)", Count = weeksCount},
            //    new LineData(){DateLabel = "12/03/2014", lineValue = 0.5, label="EXTERN (RAT)", Count = weeksCount},
            //    new LineData(){DateLabel = "19/03/2014", lineValue = 1.5, label="EXTERN (RAT)", Count = weeksCount},

            //    new LineData(){DateLabel = "01/012014", lineValue = 1, label="INTERN (EFK)", Count = weeksCount},
            //    new LineData(){DateLabel = "08/01/2014", lineValue = 2, label="INTERN (EFK)", Count = weeksCount},
            //    new LineData(){DateLabel = "15/01/2014", lineValue = 1, label="INTERN (EFK)", Count = weeksCount},
            //    new LineData(){DateLabel = "22/01/2014", lineValue = 2, label="INTERN (EFK)", Count = weeksCount},
            //    new LineData(){DateLabel = "29/01/2014", lineValue = 1, label="INTERN (EFK)", Count = weeksCount},
            //    new LineData(){DateLabel = "05/02/2014", lineValue = 2, label="INTERN (EFK)", Count = weeksCount},
            //    new LineData(){DateLabel = "12/02/2014", lineValue = 1, label="INTERN (EFK)", Count = weeksCount},
            //    new LineData(){DateLabel = "19/02/2014", lineValue = 2, label="INTERN (EFK)", Count = weeksCount},
            //    new LineData(){DateLabel = "26/02/2014", lineValue = 1, label="INTERN (EFK)", Count = weeksCount},
            //    new LineData(){DateLabel = "05/03/2014", lineValue = 2, label="INTERN (EFK)", Count = weeksCount},
            //    new LineData(){DateLabel = "12/03/2014", lineValue = 1, label="INTERN (EFK)", Count = weeksCount},
            //    new LineData(){DateLabel = "19/03/2014", lineValue = 2, label="INTERN (EFK)", Count = weeksCount},

            //    new LineData(){DateLabel = "01/01/2014", lineValue = 1.5, label="INTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "08/01/2014", lineValue = 2.5, label="INTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "15/01/2014", lineValue = 1.5, label="INTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "22/01/2014", lineValue = 2.5, label="INTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "29/01/2014", lineValue = 1.5, label="INTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "05/02/2014", lineValue = 2.5, label="INTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "12/02/2014", lineValue = 1.5, label="INTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "19/02/2014", lineValue = 2.5, label="INTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "26/02/2014", lineValue = 1.5, label="INTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "05/03/2014", lineValue = 2.5, label="INTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "12/03/2014", lineValue = 1.5, label="INTERN (MOUSE)", Count = weeksCount},
            //    new LineData(){DateLabel = "19/03/2014", lineValue = 2.5, label="INTERN (MOUSE)", Count = weeksCount}
            //};

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

        public ActionResult TrendAnalysis()
        {
            BaseHelper helper = new BaseHelper();
            if (!helper.IsValidUser())
                return RedirectToAction("Index", "Login");

            ReportRepository repository = new ReportRepository();
            StringBuilder sb = new StringBuilder("");
            StringBuilder sbArea = new StringBuilder("");

            SiteNItem siteNitem;
            SchedulerRepository schRepository = new SchedulerRepository();

            sb.Append("<li style='cursor:pointer'><a>Select Contract</a></li>");

            siteNitem = schRepository.GetSiteNItems();

            foreach (var item in siteNitem.listOfItems)
            {
                sb.Append("<li id=" + item.ItemID + " duration='" + item.Duration + "' desc='" + item.Description + "' style='cursor:pointer'><a>" + item.ItemName + "</a></li>");
            }
            ViewBag.Contracts = sb;
            sb = new StringBuilder("");

            StringBuilder siteFullName = new StringBuilder("");
            sb.Append("<li style='cursor:pointer'><a>Select Site</a></li>");

            foreach (var site in siteNitem.sites)
            {
                siteFullName = new StringBuilder(site.StreetNo.Trim().Length > 0 ? site.StreetNo + ", " : "");
                siteFullName.Append(site.Address1.Trim().Length > 0 ? site.Address1 + " " : "");
                siteFullName.Append(site.Address2.Trim().Length > 0 ? site.Address2 + " " : "");
                siteFullName.Append(site.Address3.Trim().Length > 0 ? site.Address3 + ", " : "");
                siteFullName.Append(site.Suburb.Trim().Length > 0 ? site.Suburb + ", " : "");
                siteFullName.Append(site.State.Trim().Length > 0 ? site.State + "-" : "");
                siteFullName.Append(site.PostCode.Trim().Length > 0 ? site.PostCode : "");

                sb.Append("<li id=" + site.SiteCode + " style='cursor:pointer'><a>" + siteFullName.ToString() + "</a></li>");
            }
            ViewBag.Sites = sb;
            sb = new StringBuilder("");
            TreeNode trNode = repository.GetTrendAnalysisTreeNodes();

            if (trNode != null && trNode.listLeve1.Count() > 0)
            {
                TempData["RootNode"] = trNode.listLeve1[0].RootCaption;
                sb.Append("<ul>");
                sb.Append("<li>");
                sb.Append(trNode.listLeve1[0].RootCaption);
                sb.Append("<ul>");
                foreach (TreeNodeLevel1 t1 in trNode.listLeve1)
                {
                    sb.Append("<li>");
                    sb.Append(t1.SectionCaption);
                    sb.Append("<ul>");

                    foreach (TreeNodeLevel2 t2 in trNode.listLeve2.Where(t => t.SectionID == t1.SectionID))
                    {
                        sb.Append("<li>");
                        sb.Append(t2.QuestionCaption);
                        sb.Append("<ul>");

                        foreach (TreeNodeLevel3 t3 in trNode.listLeve3.Where(t => t.SectionID == t1.SectionID && t.QuestionID == t2.QuestionID))
                        {
                            string nodeID = t1.SectionID + "#" + t1.SectionCaption + "#" + t2.QuestionID + "#" + t2.QuestionCaption + "#" + t3.AnswerID + "#" + t3.AnswerCaption;
                            sb.Append("<li id='" + nodeID + "'>");
                            sb.Append(t3.AnswerCaption);
                            //sb.Append("<ul>");

                            //foreach (TreeNodeLevel4 t4 in trNode.listLeve4.Where(t => t.SectionID == t1.SectionID && t.QuestionID == t2.QuestionID && t.AnswerID == t3.AnswerID))
                            //{
                            //    sb.Append("<li>");
                            //    sb.Append(t4.AdditionalAnswerCaption);
                            //    sb.Append("</li>");
                            //}
                            //sb.Append("</ul>");
                            sb.Append("</li>");
                        }
                        sb.Append("</ul>");
                        sb.Append("</li>");
                    }
                    sb.Append("</ul>");
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
                sb.Append("</li>");
                sb.Append("</ul>");

                sbArea.Append("<li style='cursor:pointer'><a>[ALL]</a></li>");
                foreach (string area in trNode.listAreaName)
                {
                    sbArea.Append("<li style='cursor:pointer'><a>" + area + "</a></li>");
                }
            }

            ViewBag.TreeNodes = sb.ToString();
            ViewBag.Areas = sbArea.ToString();

            return View();
        }

        [HttpPost]
        public ActionResult TrendAnalysis(int siteNo, int contractNo, string selectedNodes, string area, int frequency, string fromDate, string toDate, int groupBy, string chartType)
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

            ReportRepository repository = new ReportRepository();
            DateTime dtFrom = Convert.ToDateTime(fromDate);
            DateTime dtTo = Convert.ToDateTime(toDate);
            ChartType chartTypeObj = new ChartType();
            List<ChartData> charts = new List<ChartData>();
            List<Dictionary<string, object>> bars = new List<Dictionary<string, object>>();

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

        public ActionResult EquipmentTransaction()
        {
            BaseHelper helper = new BaseHelper();
            if (!helper.IsValidUser())
                return RedirectToAction("Index", "Login");

            SiteNItem siteNitem;
            SchedulerRepository repository = new SchedulerRepository();

            StringBuilder sb = new StringBuilder("");
            sb.Append("<li style='cursor:pointer'><a>Select Contract</a></li>");

            siteNitem = repository.GetSiteNItems();

            foreach (var item in siteNitem.listOfItems)
            {
                sb.Append("<li id=" + item.ItemID + " duration='" + item.Duration + "' desc='" + item.Description + "' style='cursor:pointer'><a>" + item.ItemName + "</a></li>");
            }
            ViewBag.Contracts = sb;

            StringBuilder siteFullName = new StringBuilder("");
            sb = new StringBuilder("");
            sb.Append("<li style='cursor:pointer'><a>Select Site</a></li>");

            foreach (var site in siteNitem.sites)
            {
                siteFullName = new StringBuilder(site.StreetNo.Trim().Length > 0 ? site.StreetNo + ", " : "");
                siteFullName.Append(site.Address1.Trim().Length > 0 ? site.Address1 + " " : "");
                siteFullName.Append(site.Address2.Trim().Length > 0 ? site.Address2 + " " : "");
                siteFullName.Append(site.Address3.Trim().Length > 0 ? site.Address3 + ", " : "");
                siteFullName.Append(site.Suburb.Trim().Length > 0 ? site.Suburb + ", " : "");
                siteFullName.Append(site.State.Trim().Length > 0 ? site.State + "-" : "");
                siteFullName.Append(site.PostCode.Trim().Length > 0 ? site.PostCode : "");

                sb.Append("<li id=" + site.SiteCode + " style='cursor:pointer'><a>" + siteFullName.ToString() + "</a></li>");
            }
            ViewBag.Sites = sb;

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