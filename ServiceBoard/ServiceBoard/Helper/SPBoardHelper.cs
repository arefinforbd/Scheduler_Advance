using ServiceBoard.Repository;
using ServiceBoard.SPBoardWCFService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace ServiceBoard.Helper
{
    public class SPBoardHelper
    {
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

        public string GetMonthName(string yearMonth)
        {
            int year = 0;
            string monthName = "";
            int monthNo = 0;

            year = Convert.ToInt32(yearMonth.Substring(0, 4));
            monthNo = Convert.ToInt32(yearMonth.Substring(4, 2));

            monthName = new DateTime(year, monthNo, 1).ToString("MMM", CultureInfo.InvariantCulture);

            return monthName;
        }

        public string GetYearMonth(string yearMonth)
        {
            int year = 0;
            year = Convert.ToInt32(yearMonth.Substring(0, 4));

            if (year == DateTime.Now.Year)
                return "TY$";
            else if (year == (DateTime.Now.Year - 1))
                return "LY$";

            return "";
        }

        public enum YearOrMonth
        {
            Year,
            Month
        };

        public string LoadCategory()
        {
            List<Category> categories = new List<Category>();
            StringBuilder sb = new StringBuilder("");
            SPBoardRepository repo = new SPBoardRepository();

            if (HttpContext.Current.Session["Categories"] == null)
                HttpContext.Current.Session["Categories"] = repo.GetCategory();

            categories = (List<Category>)HttpContext.Current.Session["Categories"];
            sb.Append("<option selected='selected'>Select Category</option>");

            if (categories == null)
                return sb.ToString();

            foreach (var item in categories)
                sb.Append("<option>" + item.CategoryName + "</option>");

            return sb.ToString();
        }
    }
}