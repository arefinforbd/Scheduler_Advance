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

        public ActionResult GetTimeSlots(string dateStartedFrom, string itemID)
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
        public ActionResult PostTimeSlot(string siteNo, string itemID, string timeSlots)
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
                var siteInfo = sites.Where(s => s.SiteNo == Convert.ToInt32(siteNo)).SingleOrDefault();
                Session.Add("SiteObject", siteInfo);
            }

            if (Session["Items"] != null)
            {
                var items = (List<Service>)Session["Items"];
                var itemInfo = items.Where(i => i.ItemID == itemID).SingleOrDefault();
                Session.Add("ItemObject", itemInfo);
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

            if (Session["SiteObject"] != null)
            {
                var siteInfo = (Site)Session["SiteObject"];

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
        public ActionResult SendCustomerInformation(string firstname, string lastname, string email, string streetno, string streetname, string streetname2, string suburb, string state, string postcode, string phoneno, string mobileno)
        {
            try
            {
                bool response;
                SchedulerRepository repo = new SchedulerRepository();

                if (Request["customerid"] == null)
                {
                    BaseHelper helper = new BaseHelper();
                    if (!helper.IsValidUser())
                        return RedirectToAction("Index", "Login");
                }

                if (Session["SiteObject"] != null && Session["ItemObject"] != null)
                {
                    Site site = (Site)Session["SiteObject"];
                    Service item = (Service)Session["ItemObject"];
                    List<TimeSlot> timeSlots = (List<TimeSlot>)TempData["TimeSlots"];

                    DateTime scheduledDate;
                    string startTime = "";
                    string endTime = "";
                    string specialInstruction = "";

                    scheduledDate = Convert.ToDateTime(timeSlots[0].Date);
                    startTime = timeSlots[0].StartTime;
                    endTime = timeSlots[0].EndTime;
                    specialInstruction= timeSlots[0].SpecialInstruction;

                    response = repo.SendCustomerInformationForSchedule(firstname, lastname, email, phoneno, mobileno, streetno, streetname, streetname2, suburb, state, postcode, site.SiteNo, site.SiteCode, item.CategoryName, item.ProductName, item.LineNo, item.ItemID, 0, 0, scheduledDate, startTime, endTime, item.Duration.ToString(), specialInstruction);

                    if (response)
                        return Json("successfull", JsonRequestBehavior.AllowGet);
                    else
                        return Json("error", JsonRequestBehavior.AllowGet);
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