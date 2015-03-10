using CASPortal.CASWCFService;
using CASPortal.Helper;
using CASPortal.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CASPortal.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetAdvertisement()
        {
            BaseRepository baseRepo = new BaseRepository();
            List<Advertisement> ads = new List<Advertisement>();

            if (Session["Advertisements"] == null){
                ads = baseRepo.GetAdvertisement();
                Session.Add("Advertisements", ads);
            }
            else
                ads = (List<Advertisement>)Session["Advertisements"];

            return Json(ads, JsonRequestBehavior.AllowGet);
        }
	}
}