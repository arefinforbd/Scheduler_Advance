using CASPortal.Models;
using CASPortal.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
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
            string ss = "";
            decimal dur = 30 / 60M;
            
            for (decimal i = 7; i <= 18; i += dur)
            {
                ss += i.ToString() + " - ";
            }

            return View();
        }

        public ActionResult GetTimeSlots()
        {
            Item item = new Item();
            SchedulerRepository repository = new SchedulerRepository();

            item = repository.GetItem();

            return Json(item, JsonRequestBehavior.AllowGet);
        }
	}
}