using CASPortal.CASWCFService;
using CASPortal.Helper;
using CASPortal.Repository;
using CASPortal.WebParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CASPortal.Controllers
{
    public class SchedulerController : Controller
    {
        public JavaScriptSerializer Serializer
        {
            get { return new JavaScriptSerializer(); }
        }

        //
        // GET: /Scheduler/
        public ActionResult Index()
        {
            SiteNItem siteNitem;
            List<Item> itemList = new List<Item>();
            List<Site> siteList = new List<Site>();
            SchedulerRepository repository = new SchedulerRepository();
            SchedulerParser parser = new SchedulerParser();

            if (Request["customerid"] == null)
            {
                BaseHelper helper = new BaseHelper();
                if (!helper.IsValidUser())
                    return RedirectToAction("Index", "Login");             
            }
            else
                parser.SetLoginCredential(new Guid(Request["customerid"]));

            StringBuilder sb = new StringBuilder("");
            sb.Append("<li style='cursor:pointer'><a>Select Item</a></li>");

            if (Session["BusinessHours"] == null)
                parser.GetBusinessTime();

            siteNitem = repository.GetSiteNItems();

            foreach (var item in siteNitem.items)
            {
                sb.Append("<li id=" + item.ItemID + " duration='" + item.Duration + "' desc='" + item.Description + "' style='cursor:pointer'><a>" + item.ItemName + "</a></li>");
            }
            ViewBag.Items = sb;

            if (Request["customerid"] == null)
            {
                StringBuilder siteFullName = new StringBuilder("");
                sb = new StringBuilder("");
                sb.Append("<li style='cursor:pointer'><a>Select Site</a></li>");

                foreach (var site in siteNitem.sites)
                {
                    //siteFullName = new StringBuilder(site.StreetNo + ", " + site.Address1 + " " + site.Address2 + " " + site.Address3 + ", " + site.Suburb + "-" + site.PostCode + ", " + site.State);
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
            }

            return View();
        }

        public ActionResult GetTimeSlots(string dateStartedFrom, int itemID)
        {
            try
            {
                if (Request["customerid"] == null)
                {
                    BaseHelper helper = new BaseHelper();
                    if (!helper.IsValidUser())
                        return RedirectToAction("Index", "Login");
                }

                Item item = new Item();
                SchedulerRepository repository = new SchedulerRepository();

                TempData.Clear();
                item = repository.GetTimeSlots(dateStartedFrom);

                return Json(item, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public ActionResult PostTimeSlot(string siteID, string itemID, string timeSlots)
        {
            if (Request["customerid"] == null)
            {
                BaseHelper helper = new BaseHelper();
                if (!helper.IsValidUser())
                    return RedirectToAction("Index", "Login");
            }

            if (Session["Sites"] != null)
            {
                var sites = (List<Site>)Session["Sites"];
                var siteInfo = sites.Where(s => s.SiteCode == Convert.ToInt32(siteID)).SingleOrDefault();
                TempData["SiteInfo"] = siteInfo;
            }
            var selectedItemSlots = Serializer.Deserialize<List<TimeSlot>>(timeSlots);
            var timeSlotList = selectedItemSlots.OrderBy(d => DateTime.Parse(d.Date)).ThenBy(t => Convert.ToInt32(t.StartTime.Replace(":", ""))).ToList();
            TempData["TimeSlots"] = timeSlotList;
            
            return Json("successfull", JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerInformation()
        {
            if (Request["customerid"] == null)
            {
                BaseHelper helper = new BaseHelper();
                if (!helper.IsValidUser())
                    return RedirectToAction("Index", "Login");
            }

            if (TempData["SiteInfo"] != null)
            {
                var siteInfo = (Site)TempData["SiteInfo"];

                ViewData["LastName"] = siteInfo.LastName;
                ViewData["CompanyName"] = siteInfo.CompanyName;
                ViewData["Email"] = siteInfo.Email;

                ViewData["StreetNo"] = siteInfo.StreetNo;
                ViewData["Address1"] = siteInfo.Address1;
                ViewData["Address2"] = siteInfo.Address2;
                ViewData["Address3"] = siteInfo.Address3;
                ViewData["Suburb"] = siteInfo.Suburb;
                ViewData["State"] = siteInfo.State;
                ViewData["PostCode"] = siteInfo.PostCode;

                ViewData["PhoneNo"] = siteInfo.PhoneNo;
                ViewData["MobileNo"] = siteInfo.MobileNo;
            }

            return View();
        }

        [HttpPost]
        public ActionResult SendCustomerInformation(string firstname, string lastname, string email, string houseno, string streetname, string address, string city, string state, string postcode, string phoneno, string mobileno)
        {
            try
            {
                if (Request["customerid"] == null)
                {
                    BaseHelper helper = new BaseHelper();
                    if (!helper.IsValidUser())
                        return RedirectToAction("Index", "Login");
                }

                string output = firstname;

                if (TempData["TimeSlots"] != null)
                {
                    var timeSlots = (List<TimeSlot>)TempData["TimeSlots"];
                    return Json("successfull", JsonRequestBehavior.AllowGet);
                }

                return Json("error", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ViewSample()
        {
            return View();
        }
	}
}