using CASPortal.CASService;
using CASPortal.Helper;
using CASPortal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CASPortal.WebParser
{
    public class SchedulerParser
    {
        public void GetBusinessTime()
        {
            DataSet ds = new DataSet();

            try
            {
                int colIndex = 0;
                BusinessHour businessHour;
                List<BusinessHour> businessHours = new List<BusinessHour>();
                CASWebService cas = new CASWebService();

                string companyID = HttpContext.Current.Session["CompanyID"].ToString();
                string companyPassword = HttpContext.Current.Session["CompanyPassword"].ToString();
                string customerPassword = HttpContext.Current.Session["CustomerPassword"].ToString();

                //ds = cas.GetBusinessTime("kevorkt", "", "1.000", 1);
                ds = cas.GetBusinessTime(companyID, companyPassword, customerPassword, 1);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int index = 0; index <= 6; index++)
                    {
                        colIndex = (index * 3);
                        businessHour = new BusinessHour();

                        string startHour = Convert.ToString(ds.Tables[0].Rows[0][colIndex + 2]);
                        startHour = startHour.IndexOf("30") > 0 ? startHour.Replace("30", "50") : startHour;

                        string endHour = Convert.ToString(ds.Tables[0].Rows[0][colIndex + 3]);
                        endHour = endHour.IndexOf("30") > 0 ? endHour.Replace("30", "50") : endHour;

                        float startHr = Convert.ToSingle(startHour);
                        startHr = startHr / 100;

                        float endHr = Convert.ToSingle(endHour);
                        endHr = endHr / 100;

                        businessHour.IsWorkingDay = Convert.ToBoolean(ds.Tables[0].Rows[0][colIndex + 1]);
                        businessHour.BusinessStartHour = startHr;
                        businessHour.BusinessEndHour = endHr;
                        businessHour.NoOfDay = index;

                        businessHours.Add(businessHour);
                    }
                    HttpContext.Current.Session.Add("BusinessHours", businessHours);
                }
                else
                {
                    HttpContext.Current.Session.Add("BusinessHours", null);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public Item GetBookedDays(string dateStart)
        {
            DateTime scheduledDate;
            string startTime = string.Empty;
            string endTime = string.Empty;
            Item item = new Item();
            List<TimeSlot> timeSlots = new List<TimeSlot>();
            DataSet ds = new DataSet();
            CASWebService cas = new CASWebService();

            string companyID = HttpContext.Current.Session["CompanyID"].ToString();
            string companyPassword = HttpContext.Current.Session["CompanyPassword"].ToString();
            string customerPassword = HttpContext.Current.Session["CustomerPassword"].ToString();

            dateStart = dateStart.Substring(0, dateStart.IndexOf("GMT") - 1);
            //ds = cas.GetScheduledTime("kevorkt", "", "1.000", Convert.ToDateTime(dateStart));
            ds = cas.GetScheduledTime(companyID, companyPassword, customerPassword, Convert.ToDateTime(dateStart));

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    scheduledDate = Convert.ToDateTime(row["tt_ts_date"].ToString());
                    startTime = row["tt_ts_start_time"].ToString();
                    endTime = row["tt_ts_end_time"].ToString();

                    startTime = (Convert.ToSingle(startTime) / 100).ToString();
                    endTime = (Convert.ToSingle(endTime) / 100).ToString();

                    startTime = startTime.Replace(".3", ".5");
                    endTime = endTime.Replace(".3", ".5");

                    timeSlots.Add(
                        new TimeSlot()
                        {
                            Date = scheduledDate.ToString("MMM dd, yyyy"),
                            StartTime = startTime,
                            EndTime = endTime
                        }
                    );
                }
            }

            item.TimeSlots = timeSlots;

            /*
            item.TimeSlots = new List<TimeSlot>(){
                new TimeSlot
                {
                    ItemID = 2,
                    Date = Convert.ToDateTime("03/Nov/2014").ToString("dd/MMM/yyyy"),
                    StartTime = "9",
                    EndTime = "10"
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = Convert.ToDateTime("03/Nov/2014").ToString("dd/MMM/yyyy"),
                    StartTime = "14",
                    EndTime = "16"
                }
            };*/

            return item;
        }

        public List<Item> GetItems()
        {
            Item item;
            List<Item> items = new List<Item>();
            DataSet ds = new DataSet();
            CASWebService cas = new CASWebService();

            string companyID = HttpContext.Current.Session["CompanyID"].ToString();
            string companyPassword = HttpContext.Current.Session["CompanyPassword"].ToString();
            string customerPassword = HttpContext.Current.Session["CustomerPassword"].ToString();
            int level4ID = Convert.ToInt32(HttpContext.Current.Session["Level4ID"].ToString());

            //ds = cas.GetCategoryProductService("kevorkt", "", "1.000", 1);
            ds = cas.GetCategoryProductService(companyID, companyPassword, customerPassword, level4ID);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    item = new Item();

                    item.CategoryName = row["pd_category"].ToString();
                    item.ProductName = row["pd_code"].ToString();
                    item.ItemName = row["pdl_prcode"].ToString();
                    item.ItemID = Convert.ToInt32(row["pdl_lineno"].ToString());
                    item.Description = row["pdl_desc"].ToString();
                    item.Price = Convert.ToDouble(row["pdl_price"].ToString());
                    item.Duration = Convert.ToInt32(row["pdl_duration"].ToString());

                    items.Add(item);
                }
            }

            return items;
        }

        public List<Site> GetSites()
        {
            Site site;
            List<Site> sites = new List<Site>();
            DataSet ds = new DataSet();
            CASWebService cas = new CASWebService();

            string companyID = HttpContext.Current.Session["CompanyID"].ToString();
            string companyPassword = HttpContext.Current.Session["CompanyPassword"].ToString();
            string customerID = HttpContext.Current.Session["CustomerID"].ToString();
            string customerPassword = HttpContext.Current.Session["CustomerPassword"].ToString();
            int level4ID = Convert.ToInt32(HttpContext.Current.Session["Level4ID"].ToString());

            ds = cas.GetCustomerSite(companyID, companyPassword, customerPassword, customerID, level4ID);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    site = new Site();

                    site.CompanyName = row["cs_company"].ToString();
                    site.LastName = row["cs_lastname"].ToString();
                    site.Address1 = row["cs_addr1"].ToString();
                    site.Address2 = row["cs_addr2"].ToString();
                    site.Address3 = row["cs_addr3"].ToString();
                    site.Suburb = row["cs_suburb"].ToString();
                    site.PostCode = row["cs_pcode"].ToString();
                    site.State = row["cs_state"].ToString();
                    site.PhoneNo = row["cs_phone"].ToString();
                    site.MobileNo = row["cs_mobile"].ToString();
                    site.Email = row["cs_email"].ToString();
                    site.SiteNo = Convert.ToInt32(row["cs_siteno"].ToString());
                    site.StreetNo = row["cs_streetno"].ToString();
                    site.SiteCode = Convert.ToInt32(row["cs_sitecode"].ToString());
                    site.Level4 = Convert.ToInt32(row["cs_lvl4_sequence"].ToString());

                    sites.Add(site);
                }
            }

            HttpContext.Current.Session.Add("Sites", sites);
            return sites;
        }

        public void SetLoginCredential(Guid customerid)
        {
            BaseHelper helper = new BaseHelper();
            helper.SetSessions("kevorkt", "", "1.000", "1.000", 1);
        }
    }
}