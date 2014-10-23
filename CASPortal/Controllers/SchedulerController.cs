using CASPortal.Models;
using CASPortal.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CASPortal.Controllers
{
    public class SchedulerController : Controller
    {
        //
        // GET: /Scheduler/
        public ActionResult Index()
        {
            List<Item> itemList = new List<Item>();
            SchedulerRepository repository = new SchedulerRepository();

            StringBuilder sb = new StringBuilder("");
            sb.Append("<li style='cursor:pointer'><a>Select Item</a></li>");

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
            Item item = new Item();
            SchedulerRepository repository = new SchedulerRepository();

            item = repository.GetItem();

            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerInformation(FormCollection form)
        {
            string itemID = form["itemID"];
            string scheduleDate = form["scheduleDate"];
            string scheduleStartTime = form["scheduleStartTime"];
            string scheduleEndTime = form["scheduleEndTime"];
            string specialInstruction = form["specialInstruction"];

            TempData["ItemID"] = itemID;
            TempData["ScheduleDate"] = scheduleDate;
            TempData["ScheduleStartTime"] = scheduleStartTime;
            TempData["ScheduleEndTime"] = scheduleEndTime;
            TempData["SpecialInstruction"] = specialInstruction;

            return View();
        }

        [HttpPost]
        public ActionResult SendCustomerInformation(string firstname, string lastname, string houseno, string streetname, string address, string city, string state, string postcode, string phoneno, string mobileno)
        {
            try
            {
                string output = firstname + "/" + TempData["ScheduleDate"] + "/" + TempData["SpecialInstruction"];

                return Json("successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
	}
}