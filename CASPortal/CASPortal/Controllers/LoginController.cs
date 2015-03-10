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
                CASWCFServiceClient cas = new CASWCFServiceClient();
                string menu = "";
                string menuString = "";
                NavigationMenu navMenu = new NavigationMenu();
                NavigationMenuHelper navHelper = new NavigationMenuHelper();
                List<NavigationMenu> navMenus = new List<NavigationMenu>();
                NavigationMenuRepository repo = new NavigationMenuRepository();

                bool status = cas.LoginProcess(companyid, companypassword, customerid, customerpassword, out level4id);

                if (status)
                {
                    BaseHelper helper = new BaseHelper();
                    helper.SetSessions(companyid, companypassword, customerid, customerpassword, level4id);

                    if (Session["NavigationMenu"] == null)
                    {
                        Session["NavigationMenu"] = repo.GetNavigationMenu("WebAccess");
                        navMenus = (List<NavigationMenu>)Session["NavigationMenu"];
                    }
                    else
                        navMenus = (List<NavigationMenu>)Session["NavigationMenu"];

                    if (Session["NavigationMenuString"] == null)
                    {
                        menuString = @"<ul class='nav' id='side-menu'>
                        <li class='sidebar-search'>
                            <div class='input-group custom-search-form'>
                                <input type='text' class='form-control' placeholder='Search...'>
                                <span class='input-group-btn'>
                                    <button class='btn btn-default' type='button'>
                                        <i class='fa fa-search'></i>
                                    </button>
                                </span>
                            </div>
                        </li>";

                        menu = menuString + navHelper.GetNavigationMenuString(navMenus, "WebAccess") + "</ul>";
                        Session["NavigationMenuString"] = menu;
                    }
                    else
                        menu = Session["NavigationMenuString"].ToString();

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