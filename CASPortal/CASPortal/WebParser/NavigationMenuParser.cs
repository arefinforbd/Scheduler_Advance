using CASPortal.CASWCFService;
using CASPortal.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CASPortal.WebParser
{
    public class NavigationMenuParser
    {
        public List<NavigationMenu> GetNavigationMenu(string rootMenu)
        {
            NavigationMenu[] navMenuArr = null;
            List<NavigationMenu> navMenus = new List<NavigationMenu>();
            DataSet ds = new DataSet();
            CASWCFServiceClient cas = new CASWCFServiceClient();

            if (HttpContext.Current.Session["CompanyID"] == null)
                throw new TimeoutException("Session timed out");

            string companyID = HttpContext.Current.Session["CompanyID"].ToString();
            string companyPassword = HttpContext.Current.Session["CompanyPassword"].ToString();
            string customerPassword = HttpContext.Current.Session["CustomerPassword"].ToString();
            decimal customerID = Convert.ToDecimal(HttpContext.Current.Session["CustomerID"]);
            int level4ID = Convert.ToInt32(HttpContext.Current.Session["Level4ID"].ToString());

            navMenuArr = cas.GetNavigationMenu(companyID, companyPassword, customerID, customerPassword, level4ID, rootMenu);

            if (navMenuArr != null)
            {
                foreach (NavigationMenu navMenu in navMenuArr)
                    navMenus.Add(new NavigationMenu()
                    {
                        MenuName = navMenu.MenuName,
                        MenuOrder = navMenu.MenuOrder,
                        MenuTitle = navMenu.MenuTitle,
                        MenuCalls = navMenu.MenuCalls,
                        MenuCanSee = navMenu.MenuCanSee,
                        MenuType = navMenu.MenuType,
                        MenuImageNo = navMenu.MenuImageNo,
                        Level4 = navMenu.Level4,
                        MenuDescription = navMenu.MenuDescription
                    });

                return navMenus;
            }
            else
                throw new Exception("This account is not authorised.");
        }
    }
}