using ServiceBoard.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceBoard.Controllers
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
        public ActionResult Index(string companyId, string companyPassword)
        {
            int level4ID;
            string message;
            LoginRepository repo = new LoginRepository();

            bool status = repo.Login(companyId, companyPassword, out level4ID, out message);

            if (status)
            {
                Session.Add("CompanyID", companyId);
                Session.Add("CompanyPassword", companyPassword);
                Session.Add("Level4ID", level4ID);

                return RedirectToAction("Index", "SPBoard");
            }
            else
                ModelState.AddModelError("", message);

            return View();
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Login");
        }
	}
}