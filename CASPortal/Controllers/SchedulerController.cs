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
        [RedirectingActionAttribute]
        public ActionResult Index()
        {
            ReportHelper repoHelper = new ReportHelper();
            SchedulerParser parser = new SchedulerParser();

            if (Request["customerid"] == null)
            {
                BaseHelper helper = new BaseHelper();
                if (!helper.IsValidUser())
                    return RedirectToAction("Index", "Login");             
            }
            else
                parser.SetLoginCredential(new Guid(Request["customerid"]));

            if (Session["BusinessHours"] == null)
                parser.GetBusinessTime();

            ViewBag.Items = repoHelper.LoadItem();

            if (Request["customerid"] == null)
                ViewBag.Sites = repoHelper.LoadSite();

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

                Service item = new Service();
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