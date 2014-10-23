using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASPortal.Helper
{
    public class BaseHelper
    {
        public static string LogoUrl = System.Configuration.ConfigurationManager.AppSettings["AppLogo"];
        public static string VersionNo = System.Configuration.ConfigurationManager.AppSettings["VersionNo"];
        public static bool AdvertisementStatus = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["AdvertisementStatus"].ToLower());

        public void SetSessions(string companyid, string companypassword, string customerid, string customerpassword, int level4id)
        {
            HttpContext.Current.Session["CompanyID"] = companyid;
            HttpContext.Current.Session["CompanyPassword"] = companypassword;
            HttpContext.Current.Session["CustomerID"] = customerid;
            HttpContext.Current.Session["CustomerPassword"] = customerpassword;
            HttpContext.Current.Session["Level4ID"] = level4id;
        }

        public bool IsValidUser()
        {
            if(HttpContext.Current.Session["CompanyID"] == null || HttpContext.Current.Session["CompanyPassword"] == null
                || HttpContext.Current.Session["CustomerID"] == null || HttpContext.Current.Session["CustomerPassword"] == null)
            {
                return false;
            }

            return true;
        }

        public void RemoveSessions()
        {
            HttpContext.Current.Session.Remove("CompanyID");
            HttpContext.Current.Session.Remove("CompanyPassword");
            HttpContext.Current.Session.Remove("CustomerID");
            HttpContext.Current.Session.Remove("CustomerPassword");
            HttpContext.Current.Session.Remove("Level4ID");
            HttpContext.Current.Session.Remove("CompanyLogo");
            HttpContext.Current.Session.Clear();
        }
    }
}