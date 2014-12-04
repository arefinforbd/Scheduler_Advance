using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace CASPortal.Helper
{
    public class BaseHelper
    {
        public static string AppName = System.Configuration.ConfigurationManager.AppSettings["AppName"];
        public static string LogoUrl = System.Configuration.ConfigurationManager.AppSettings["AppLogo"];
        public static string VersionNo = System.Configuration.ConfigurationManager.AppSettings["VersionNo"];
        public static bool AdvertisementStatus = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["AdvertisementStatus"].ToLower());
        public static string OnlyCAS = System.Configuration.ConfigurationManager.AppSettings["OnlyCAS"].ToLower();

        public static string GetSiteUrl()
        {
            string strApp = HttpContext.Current.Request.ApplicationPath;
            string strPath = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            string strLastChar = strApp.Substring(strApp.Length - 1);

            if (strLastChar == "/")
            {
                strApp = strApp.Substring(0, strApp.Length - 1);
            }

            strPath += strApp;
            return strPath;
        }

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
            HttpContext.Current.Session.RemoveAll();
        }
    }
}