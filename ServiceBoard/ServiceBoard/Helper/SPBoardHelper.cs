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
            Month,
            None
        };

        public string LoadCategory()
        {
            List<Category> categories = new List<Category>();
            StringBuilder sb = new StringBuilder("");
            SPBoardRepository repo = new SPBoardRepository();

            if (HttpContext.Current.Session["Categories"] == null)
                HttpContext.Current.Session["Categories"] = repo.GetCombo().Categories;

            categories = (List<Category>)HttpContext.Current.Session["Categories"];
            sb.Append("<option selected='selected'>Select Category</option>");

            if (categories == null)
                return sb.ToString();

            foreach (var item in categories)
                sb.Append("<option>" + item.CategoryName + "</option>");

            return sb.ToString();
        }

        public string LoadAreas()
        {
            StringBuilder sb = new StringBuilder("");
            List<Area> areas = new List<Area>();
            SPBoardRepository repo = new SPBoardRepository();

            if (HttpContext.Current.Session["Combo"] == null)
                HttpContext.Current.Session["Combo"] = repo.GetCombo();

            areas = ((ComboClass)HttpContext.Current.Session["Combo"]).Areas;

            foreach (Area area in areas)
                sb.Append("<option value='" + area.AreaCode + "'>" + area.AreaCode + "</option>");

            return sb.ToString();
        }

        public string LoadInvoiceTypes()
        {
            StringBuilder sb = new StringBuilder("");
            SPBoardRepository repo = new SPBoardRepository();
            List<InvoiceType> invoiceTypes = new List<InvoiceType>();

            if (HttpContext.Current.Session["Combo"] == null)
                HttpContext.Current.Session["Combo"] = repo.GetCombo();

            invoiceTypes = ((ComboClass)HttpContext.Current.Session["Combo"]).InvoiceTypes;
            var invoice = invoiceTypes.Where(i => i.InvoiceTypeCode.ToLower().Contains("all")).SingleOrDefault();
            invoiceTypes.Remove(invoice);

            sb.Append("<select multiple='multiple' id='ddlinvoiceType'>");
            foreach (InvoiceType invoiceType in invoiceTypes)
                sb.Append("<option value='" + invoiceType.InvoiceTypeCode + "'>" + invoiceType.InvoiceTypeCode + "</option>");

            sb.Append("</select>");

            return sb.ToString();
        }

        public string LoadDateBalance()
        {
            StringBuilder sb = new StringBuilder("");
            List<ChartData> charts = new List<ChartData>();
            SPBoardRepository repo = new SPBoardRepository();

            if (HttpContext.Current.Session["DateBalance"] == null)
                HttpContext.Current.Session["DateBalance"] = repo.GetDebtorAnalysis("", 0, 0, 0, "", true, "", "", 0, false, DateTime.Today, true);

            charts = (List<ChartData>)HttpContext.Current.Session["DateBalance"];

            sb.Append("<option value='[All]'>[All]</option>");
            foreach (ChartData chart in charts)
                sb.Append("<option value='" + chart.Label + "'>" + chart.Label + "</option>");

            return sb.ToString();
        }
    }
}