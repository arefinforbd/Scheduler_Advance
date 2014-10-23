using CASPortal.CASService;
using CASPortal.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CASPortal.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string companyid, string companypassword, string customerid, string customerpassword)
        {
            try
            {
                int level4id;
                CASWebService cas = new CASWebService();
                bool status = cas.LoginProcess(companyid, companypassword, customerid, customerpassword, out level4id);

                if (status)
                {
                    BaseHelper helper = new BaseHelper();
                    helper.SetSessions(companyid, companypassword, customerid, customerpassword, level4id);

                    return RedirectToAction("WelcomeMessage", "CustomerInformation");
                }
                else
                    ModelState.AddModelError("", "Username or Password don't match.");

                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }


        public ActionResult Logout()
        {
            BaseHelper helper = new BaseHelper();
            helper.RemoveSessions();
            
            return RedirectToAction("Index", "Login");
        }
	}
}