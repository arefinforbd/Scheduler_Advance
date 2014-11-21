using CASPortal.CASWCFService;
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

        public ActionResult GetBarData()
        {
            DataTable dtTable = null;
            List<BarData> charts = new List<BarData>()
            {
                new BarData(){DateLabel = "01/01/2014", lineValue = 0, label="EXTERN__MOUSE_"},
                new BarData(){DateLabel = "08/01/2014", lineValue = 1, label="EXTERN__MOUSE_"},
                new BarData(){DateLabel = "15/01/2014", lineValue = 0, label="EXTERN__MOUSE_"},
                new BarData(){DateLabel = "22/01/2014", lineValue = 1, label="EXTERN__MOUSE_"},
                new BarData(){DateLabel = "29/01/2014", lineValue = 0, label="EXTERN__MOUSE_"},
                new BarData(){DateLabel = "05/02/2014", lineValue = 1, label="EXTERN__MOUSE_"},
                new BarData(){DateLabel = "12/02/2014", lineValue = 0, label="EXTERN__MOUSE_"},
                new BarData(){DateLabel = "19/02/2014", lineValue = 1, label="EXTERN__MOUSE_"},
                new BarData(){DateLabel = "26/02/2014", lineValue = 0, label="EXTERN__MOUSE_"},
                new BarData(){DateLabel = "05/03/2014", lineValue = 1, label="EXTERN__MOUSE_"},
                new BarData(){DateLabel = "12/03/2014", lineValue = 0, label="EXTERN__MOUSE_"},
                new BarData(){DateLabel = "19/03/2014", lineValue = 1, label="EXTERN__MOUSE_"},

                new BarData(){DateLabel = "01/01/2014", lineValue = 5, label="EXTERN__RAT_"},
                new BarData(){DateLabel = "08/01/2014", lineValue = 1, label="EXTERN__RAT_"},
                new BarData(){DateLabel = "15/01/2014", lineValue = 5, label="EXTERN__RAT_"},
                new BarData(){DateLabel = "22/01/2014", lineValue = 1, label="EXTERN__RAT_"},
                new BarData(){DateLabel = "29/01/2014", lineValue = 5, label="EXTERN__RAT_"},
                new BarData(){DateLabel = "05/02/2014", lineValue = 1, label="EXTERN__RAT_"},
                new BarData(){DateLabel = "12/02/2014", lineValue = 5, label="EXTERN__RAT_"},
                new BarData(){DateLabel = "19/02/2014", lineValue = 1, label="EXTERN__RAT_"},
                new BarData(){DateLabel = "26/02/2014", lineValue = 5, label="EXTERN__RAT_"},
                new BarData(){DateLabel = "05/03/2014", lineValue = 1, label="EXTERN__RAT_"},
                new BarData(){DateLabel = "12/03/2014", lineValue = 5, label="EXTERN__RAT_"},
                new BarData(){DateLabel = "19/03/2014", lineValue = 1, label="EXTERN__RAT_"},

                new BarData(){DateLabel = "01/01/2014", lineValue = 1, label="INTERN__EFK_"},
                new BarData(){DateLabel = "08/01/2014", lineValue = 2, label="INTERN__EFK_"},
                new BarData(){DateLabel = "15/01/2014", lineValue = 1, label="INTERN__EFK_"},
                new BarData(){DateLabel = "22/01/2014", lineValue = 2, label="INTERN__EFK_"},
                new BarData(){DateLabel = "29/01/2014", lineValue = 1, label="INTERN__EFK_"},
                new BarData(){DateLabel = "05/02/2014", lineValue = 2, label="INTERN__EFK_"},
                new BarData(){DateLabel = "12/02/2014", lineValue = 1, label="INTERN__EFK_"},
                new BarData(){DateLabel = "19/02/2014", lineValue = 2, label="INTERN__EFK_"},
                new BarData(){DateLabel = "26/02/2014", lineValue = 1, label="INTERN__EFK_"},
                new BarData(){DateLabel = "05/03/2014", lineValue = 2, label="INTERN__EFK_"},
                new BarData(){DateLabel = "12/03/2014", lineValue = 1, label="INTERN__EFK_"},
                new BarData(){DateLabel = "19/03/2014", lineValue = 2, label="INTERN__EFK_"},

                new BarData(){DateLabel = "01/01/2014", lineValue = 1, label="INTERN__MOUSE_"},
                new BarData(){DateLabel = "08/01/2014", lineValue = 2, label="INTERN__MOUSE_"},
                new BarData(){DateLabel = "15/01/2014", lineValue = 1, label="INTERN__MOUSE_"},
                new BarData(){DateLabel = "22/01/2014", lineValue = 2, label="INTERN__MOUSE_"},
                new BarData(){DateLabel = "29/01/2014", lineValue = 1, label="INTERN__MOUSE_"},
                new BarData(){DateLabel = "05/02/2014", lineValue = 2, label="INTERN__MOUSE_"},
                new BarData(){DateLabel = "12/02/2014", lineValue = 1, label="INTERN__MOUSE_"},
                new BarData(){DateLabel = "19/02/2014", lineValue = 2, label="INTERN__MOUSE_"},
                new BarData(){DateLabel = "26/02/2014", lineValue = 1, label="INTERN__MOUSE_"},
                new BarData(){DateLabel = "05/03/2014", lineValue = 2, label="INTERN__MOUSE_"},
                new BarData(){DateLabel = "12/03/2014", lineValue = 1, label="INTERN__MOUSE_"},
                new BarData(){DateLabel = "19/03/2014", lineValue = 2, label="INTERN__MOUSE_"}
            };

            var count = charts.GroupBy(i => new { Date = i.label })
                 .Select(group => new
                 {
                     Label = group.First().label
                 }).ToList().Count;

            int weeksCount = charts.Count / count;

            if (charts.Count >= weeksCount)
            {
                dtTable = RowToColumn(charts, weeksCount);

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

                return Json(serializer.Serialize(rows), JsonRequestBehavior.AllowGet);
            }
            else
                return null;
        }

        [HttpGet]
        public ActionResult GetLineData()
        {
            /*
             * Get number of columns and rows.
             * Substract 1 from number of columns.
             * Divid number of rows by number of columns.
             * 
             */
            int weeksCount = 12;

            List<LineData> charts = new List<LineData>()
            {
                new LineData(){DateLabel = "01/012014", lineValue = 0, label="EXTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "08/01/2014", lineValue = 1, label="EXTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "15/01/2014", lineValue = 0, label="EXTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "22/01/2014", lineValue = 1, label="EXTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "29/01/2014", lineValue = 0, label="EXTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "05/02/2014", lineValue = 1, label="EXTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "12/02/2014", lineValue = 0, label="EXTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "19/02/2014", lineValue = 1, label="EXTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "26/02/2014", lineValue = 0, label="EXTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "05/03/2014", lineValue = 1, label="EXTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "12/03/2014", lineValue = 0, label="EXTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "19/03/2014", lineValue = 1, label="EXTERN (MOUSE)", Count = weeksCount},

                new LineData(){DateLabel = "01/012014", lineValue = 0.5, label="EXTERN (RAT)", Count = weeksCount},
                new LineData(){DateLabel = "08/01/2014", lineValue = 1.5, label="EXTERN (RAT)", Count = weeksCount},
                new LineData(){DateLabel = "15/01/2014", lineValue = 0.5, label="EXTERN (RAT)", Count = weeksCount},
                new LineData(){DateLabel = "22/01/2014", lineValue = 1.5, label="EXTERN (RAT)", Count = weeksCount},
                new LineData(){DateLabel = "29/01/2014", lineValue = 0.5, label="EXTERN (RAT)", Count = weeksCount},
                new LineData(){DateLabel = "05/02/2014", lineValue = 1.5, label="EXTERN (RAT)", Count = weeksCount},
                new LineData(){DateLabel = "12/02/2014", lineValue = 0.5, label="EXTERN (RAT)", Count = weeksCount},
                new LineData(){DateLabel = "19/02/2014", lineValue = 1.5, label="EXTERN (RAT)", Count = weeksCount},
                new LineData(){DateLabel = "26/02/2014", lineValue = 0.5, label="EXTERN (RAT)", Count = weeksCount},
                new LineData(){DateLabel = "05/03/2014", lineValue = 1.5, label="EXTERN (RAT)", Count = weeksCount},
                new LineData(){DateLabel = "12/03/2014", lineValue = 0.5, label="EXTERN (RAT)", Count = weeksCount},
                new LineData(){DateLabel = "19/03/2014", lineValue = 1.5, label="EXTERN (RAT)", Count = weeksCount},

                new LineData(){DateLabel = "01/012014", lineValue = 1, label="INTERN (EFK)", Count = weeksCount},
                new LineData(){DateLabel = "08/01/2014", lineValue = 2, label="INTERN (EFK)", Count = weeksCount},
                new LineData(){DateLabel = "15/01/2014", lineValue = 1, label="INTERN (EFK)", Count = weeksCount},
                new LineData(){DateLabel = "22/01/2014", lineValue = 2, label="INTERN (EFK)", Count = weeksCount},
                new LineData(){DateLabel = "29/01/2014", lineValue = 1, label="INTERN (EFK)", Count = weeksCount},
                new LineData(){DateLabel = "05/02/2014", lineValue = 2, label="INTERN (EFK)", Count = weeksCount},
                new LineData(){DateLabel = "12/02/2014", lineValue = 1, label="INTERN (EFK)", Count = weeksCount},
                new LineData(){DateLabel = "19/02/2014", lineValue = 2, label="INTERN (EFK)", Count = weeksCount},
                new LineData(){DateLabel = "26/02/2014", lineValue = 1, label="INTERN (EFK)", Count = weeksCount},
                new LineData(){DateLabel = "05/03/2014", lineValue = 2, label="INTERN (EFK)", Count = weeksCount},
                new LineData(){DateLabel = "12/03/2014", lineValue = 1, label="INTERN (EFK)", Count = weeksCount},
                new LineData(){DateLabel = "19/03/2014", lineValue = 2, label="INTERN (EFK)", Count = weeksCount},

                new LineData(){DateLabel = "01/01/2014", lineValue = 1.5, label="INTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "08/01/2014", lineValue = 2.5, label="INTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "15/01/2014", lineValue = 1.5, label="INTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "22/01/2014", lineValue = 2.5, label="INTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "29/01/2014", lineValue = 1.5, label="INTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "05/02/2014", lineValue = 2.5, label="INTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "12/02/2014", lineValue = 1.5, label="INTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "19/02/2014", lineValue = 2.5, label="INTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "26/02/2014", lineValue = 1.5, label="INTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "05/03/2014", lineValue = 2.5, label="INTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "12/03/2014", lineValue = 1.5, label="INTERN (MOUSE)", Count = weeksCount},
                new LineData(){DateLabel = "19/03/2014", lineValue = 2.5, label="INTERN (MOUSE)", Count = weeksCount}
            };

            return Json(charts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPieData()
        {
            List<PieData> charts = new List<PieData>()
            {
                new PieData(){label = "Series 0", data = 15},
                new PieData(){label = "Series 1", data = 5},
                new PieData(){label = "Series 2", data = 2},
                new PieData(){label = "Series 3", data = 3}
            };

            return Json(charts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TrendAnalysis()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul>");
            sb.Append("<li>Root node 1");
            sb.Append("<ul>");
            sb.Append("<li id='child_node_1'>Child node 1</li>");
            sb.Append("<li>Child node 2</li>");
            sb.Append("</ul>");
            sb.Append("</li>");
            sb.Append("<li>Root node 2</li>");
            sb.Append("</ul>");

            ViewBag.TreeNodes = sb.ToString();

            return View();
        }

        public ActionResult EquipmentTransaction()
        {
            SiteNItem siteNitem;
            SchedulerRepository repository = new SchedulerRepository();

            StringBuilder sb = new StringBuilder("");
            sb.Append("<li style='cursor:pointer'><a>Select Contract</a></li>");

            siteNitem = repository.GetSiteNItems();

            foreach (var item in siteNitem.items)
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

    public class BarData
    {
        public string DateLabel { get; set; }
        public double lineValue { get; set; }
        public string label { get; set; }
    }

    public class LineData
    {
        public string DateLabel { get; set; }
        public double lineValue { get; set; }
        public string label { get; set; }
        public int Count { get; set; }
    }

    class PieData
    {
        public string label { get; set; }
        public int data { get; set; }
    }
}