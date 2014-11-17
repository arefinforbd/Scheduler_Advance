using CASPortal.CASWCFService;
using CASPortal.Repository;
using CASPortal.WebParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult GetBarData()
        {
            List<BarData> charts = new List<BarData>()
            {
                new BarData(){y = "2010", a = 85, b = 100},
                new BarData(){y = "2011", a = 90, b = 80},
                new BarData(){y = "2012", a = 50, b = 70},
                new BarData(){y = "2013", a = 60, b = 95},
                new BarData(){y = "2014", a = 95, b = 30}
            };

            return Json(charts, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetLineData()
        {
            List<LineData> charts = new List<LineData>()
            {
                new LineData(){x = "01/Jan/2014", y = 0},
                new LineData(){x = "08/Jan/2014", y = 0},
                new LineData(){x = "15/Jan/2014", y = 0.5},
                new LineData(){x = "22/Jan/2014", y = 0},
                new LineData(){x = "29/Jan/2014", y = .5},
                new LineData(){x = "05/Feb/2014", y = 1.5},
                new LineData(){x = "12/Feb/2014", y = 1.5},
                new LineData(){x = "19/Feb/2014", y = 1.0},
                new LineData(){x = "26/Feb/2014", y = 1.0},
                new LineData(){x = "05/Mar/2014", y = 2.0},
                new LineData(){x = "12/Mar/2014", y = 2.5},
                new LineData(){x = "19/Mar/2014", y = 1.0}
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

    class BarData
    {
        public string y { get; set; }
        public int a { get; set; }
        public int b { get; set; }
    }

    class LineData
    {
        public string x { get; set; }
        public double y { get; set; }
    }

    class PieData
    {
        public string label { get; set; }
        public int data { get; set; }
    }
}