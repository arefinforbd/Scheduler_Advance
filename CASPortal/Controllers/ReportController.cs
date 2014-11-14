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
}