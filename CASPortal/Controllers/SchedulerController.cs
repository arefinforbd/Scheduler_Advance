﻿using CASPortal.Helper;
using CASPortal.Models;
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
            List<Item> itemList = new List<Item>();
            SchedulerRepository repository = new SchedulerRepository();
            SchedulerParser parser = new SchedulerParser();

            if (Request["customerid"] == null)
            {
                BaseHelper helper = new BaseHelper();
                if (!helper.IsValidUser())
                    return RedirectToAction("Index", "Login");             
            }
            else
            {
                parser.SetLoginCredential(new Guid(Request["customerid"]));
            }

            StringBuilder sb = new StringBuilder("");
            sb.Append("<li style='cursor:pointer'><a>Select Item</a></li>");

            if (Session["BusinessHours"] == null)
            {
                parser.GetBusinessTime();
            }
            itemList = repository.GetAllItem();

            foreach (var item in itemList)
            {
                sb.Append("<li id=" + item.ItemID + " duration='" + item.Duration + "' desc='" + item.Description + "' style='cursor:pointer'><a>" + item.ItemName + "</a></li>");
            }

            ViewBag.Items = sb;

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
        public ActionResult PostTimeSlot(string itemID, string timeSlots)
        {
            if (Request["customerid"] == null)
            {
                BaseHelper helper = new BaseHelper();
                if (!helper.IsValidUser())
                    return RedirectToAction("Index", "Login");
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
	}
}