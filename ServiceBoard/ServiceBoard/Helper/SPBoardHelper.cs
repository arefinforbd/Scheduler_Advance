﻿using ServiceBoard.Repository;
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

        public string LoadAreas(bool isForDropDown)
        {
            StringBuilder sb = new StringBuilder("");
            List<Area> areas = new List<Area>();
            SPBoardRepository repo = new SPBoardRepository();

            if (HttpContext.Current.Session["Combo"] == null)
                HttpContext.Current.Session["Combo"] = repo.GetCombo();

            areas = ((ComboClass)HttpContext.Current.Session["Combo"]).Areas;

            foreach (Area area in areas)
            {
                if(isForDropDown)
                    sb.Append("<option value='" + area.AreaCode + "'>" + area.AreaCode + "</option> ");
                else
                    sb.Append(area.AreaCode + ",");
            }

            return sb.Length > 0 ? sb.ToString().Substring(0, sb.Length - 1) : "";
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
            int index = 2;
            StringBuilder sb = new StringBuilder("");
            List<ChartData> charts = new List<ChartData>();
            SPBoardRepository repo = new SPBoardRepository();
            int level4ID = Convert.ToInt32(HttpContext.Current.Session["Level4ID"].ToString());

            if (HttpContext.Current.Session["DateBalance"] == null)
                HttpContext.Current.Session["DateBalance"] = repo.GetDebtorAnalysis("[ALL]", 1, 100, 2, "[ALL]", false, "", "", 1, false, DateTime.Today, false);

            charts = (List<ChartData>)HttpContext.Current.Session["DateBalance"];

            sb.Append("<option value='1'>[ALL]</option>");
            foreach (ChartData chart in charts)
            {
                sb.Append("<option value='" + index + "'>" + chart.Label + "</option>");
                index++;
            }

            return sb.ToString();
        }

        public string LoadTechs(bool isForDropDown)
        {
            StringBuilder sb = new StringBuilder("");
            List<Tech> techs = new List<Tech>();
            SPBoardRepository repo = new SPBoardRepository();

            if (HttpContext.Current.Session["Combo"] == null)
                HttpContext.Current.Session["Combo"] = repo.GetCombo();

            techs = ((ComboClass)HttpContext.Current.Session["Combo"]).Techs;

            foreach (Tech tech in techs)
            {
                if (isForDropDown)
                    sb.Append("<option value='" + tech.TechName + "'>" + tech.TechName + "</option> ");
                else
                    sb.Append(tech.TechName + ",");
            }

            return sb.Length > 0 ? sb.ToString().Substring(0, sb.Length - 1) : "";
        }
    }
}