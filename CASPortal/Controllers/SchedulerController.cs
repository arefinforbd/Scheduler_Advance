using CASPortal.Helper;
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

            StringBuilder sb = new StringBuilder("");
            sb.Append("<li style='cursor:pointer'><a>Select Item</a></li>");

            if (Session["BusinessHours"] == null)
            {
                parser.GetBusinessTime();
            }
            itemList = repository.GetAllItem();

            foreach (var item in itemList)
            {
                sb.Append("<li id=" + item.ItemID + " style='cursor:pointer'><a>" + item.ItemName + " (" + item.SpecialInstruction + ")" + "</a></li>");
            }

            ViewBag.Items = sb;

            return View();
        }

        public ActionResult GetTimeSlots(string dateStartedFrom, int itemID)
        {
            try
            {
                Item item = new Item();
                SchedulerRepository repository = new SchedulerRepository();

                TempData.Clear();
                item = repository.GetItem(dateStartedFrom);

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
            TempData["ItemID"] = itemID;
            var selectedItemSlots = Serializer.Deserialize<List<TimeSlot>>(timeSlots);
            TempData["TimeSlots"] = selectedItemSlots;
            
            return Json("successfull", JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerInformation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendCustomerInformation(string firstname, string lastname, string houseno, string streetname, string address, string city, string state, string postcode, string phoneno, string mobileno)
        {
            try
            {
                List<TimeSlot> timeSlots;
                string output = firstname;

                if(TempData["TimeSlots"] != null)
                    timeSlots = (List<TimeSlot>)TempData["TimeSlots"];

                return Json("successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
	}
}