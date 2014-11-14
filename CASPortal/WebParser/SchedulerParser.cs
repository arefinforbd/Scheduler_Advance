using CASPortal.CASWCFService;
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

        private DataTable GetBusinessHourTable(DayHour[] dayHourArr)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("IsDay1WorkingDay", typeof(bool));
            dt.Columns.Add("Day1StartHour", typeof(string));
            dt.Columns.Add("Day1EndHour", typeof(string));

            dt.Columns.Add("IsDay2WorkingDay", typeof(bool));
            dt.Columns.Add("Day2StartHour", typeof(string));
            dt.Columns.Add("Day2EndHour", typeof(string));

            dt.Columns.Add("IsDay3WorkingDay", typeof(bool));
            dt.Columns.Add("Day3StartHour", typeof(string));
            dt.Columns.Add("Day3EndHour", typeof(string));

            dt.Columns.Add("IsDay4WorkingDay", typeof(bool));
            dt.Columns.Add("Day4StartHour", typeof(string));
            dt.Columns.Add("Day4EndHour", typeof(string));

            dt.Columns.Add("IsDay5WorkingDay", typeof(bool));
            dt.Columns.Add("Day5StartHour", typeof(string));
            dt.Columns.Add("Day5EndHour", typeof(string));

            dt.Columns.Add("IsDay6WorkingDay", typeof(bool));
            dt.Columns.Add("Day6StartHour", typeof(string));
            dt.Columns.Add("Day6EndHour", typeof(string));

            dt.Columns.Add("IsDay7WorkingDay", typeof(bool));
            dt.Columns.Add("Day7StartHour", typeof(string));
            dt.Columns.Add("Day7EndHour", typeof(string));

            dt.Rows.Add(dayHourArr[0].IsDay1WorkingDay, dayHourArr[0].Day1StartHour, dayHourArr[0].Day1EndHour, 
                dayHourArr[0].IsDay2WorkingDay, dayHourArr[0].Day2StartHour, dayHourArr[0].Day2EndHour, 
                dayHourArr[0].IsDay3WorkingDay, dayHourArr[0].Day3StartHour, dayHourArr[0].Day3EndHour, 
                dayHourArr[0].IsDay4WorkingDay, dayHourArr[0].Day4StartHour, dayHourArr[0].Day4EndHour, 
                dayHourArr[0].IsDay5WorkingDay, dayHourArr[0].Day5StartHour, dayHourArr[0].Day5EndHour, 
                dayHourArr[0].IsDay6WorkingDay, dayHourArr[0].Day6StartHour, dayHourArr[0].Day6EndHour, 
                dayHourArr[0].IsDay7WorkingDay, dayHourArr[0].Day7StartHour, dayHourArr[0].Day7EndHour);

            return dt;
        }

        public void GetBusinessTime()
        {
            try
            {
                int colIndex = 0;
                BusinessHour businessHour;
                DayHour[] dayHourArr = null;
                List<BusinessHour> businessHours = new List<BusinessHour>();
                CASWCFServiceClient cas = new CASWCFServiceClient();

                string companyID = HttpContext.Current.Session["CompanyID"].ToString();
                string companyPassword = HttpContext.Current.Session["CompanyPassword"].ToString();
                string customerPassword = HttpContext.Current.Session["CustomerPassword"].ToString();

                //ds = cas.GetBusinessTime("kevorkt", "", "1.000", 1);
                dayHourArr = cas.GetBusinessTime(companyID, companyPassword, customerPassword, 1);

                if (dayHourArr != null && dayHourArr.Count() > 0)
                {
                    DataTable dtayHour = GetBusinessHourTable(dayHourArr);
                    for (int index = 0; index <= 6; index++)
                    {
                        colIndex = (index * 3);
                        businessHour = new BusinessHour();

                        string startHour = Convert.ToString(dtayHour.Rows[0][colIndex + 1]);
                        startHour = startHour.IndexOf("30") > 0 ? startHour.Replace("30", "50") : startHour;

                        string endHour = Convert.ToString(dtayHour.Rows[0][colIndex + 2]);
                        endHour = endHour.IndexOf("30") > 0 ? endHour.Replace("30", "50") : endHour;

                        float startHr = Convert.ToSingle(startHour);
                        startHr = startHr / 100;

                        float endHr = Convert.ToSingle(endHour);
                        endHr = endHr / 100;

                        businessHour.IsWorkingDay = Convert.ToBoolean(dtayHour.Rows[0][colIndex]);
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
            TimeSlot[] timeSlotArr = null;
            List<TimeSlot> timeSlots = new List<TimeSlot>();
            CASWCFServiceClient cas = new CASWCFServiceClient();

            string companyID = HttpContext.Current.Session["CompanyID"].ToString();
            string companyPassword = HttpContext.Current.Session["CompanyPassword"].ToString();
            string customerPassword = HttpContext.Current.Session["CustomerPassword"].ToString();

            dateStart = dateStart.Substring(0, dateStart.IndexOf("GMT") - 1);
            //ds = cas.GetScheduledTime("kevorkt", "", "1.000", Convert.ToDateTime(dateStart));
            timeSlotArr = cas.GetScheduledTime(companyID, companyPassword, customerPassword, Convert.ToDateTime(dateStart));

            if (timeSlotArr != null && timeSlotArr.Count() > 0)
            {
                foreach (TimeSlot timeSlotItem in timeSlotArr)
                {
                    scheduledDate = Convert.ToDateTime(timeSlotItem.Date);
                    startTime = timeSlotItem.StartTime;
                    endTime = timeSlotItem.EndTime;

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

        public SiteNItem GetSiteNItems()
        {
            SiteNItem siteNitem;
            List<Item> items = new List<Item>();
            DataSet ds = new DataSet();
            CASWCFServiceClient cas = new CASWCFServiceClient();

            string companyID = HttpContext.Current.Session["CompanyID"].ToString();
            string companyPassword = HttpContext.Current.Session["CompanyPassword"].ToString();
            string customerPassword = HttpContext.Current.Session["CustomerPassword"].ToString();
            decimal customerID = Convert.ToDecimal(HttpContext.Current.Session["CustomerID"]);
            int level4ID = Convert.ToInt32(HttpContext.Current.Session["Level4ID"].ToString());

            //itemArr = cas.GetCategoryProductService(companyID, companyPassword, customerPassword, level4ID);
            siteNitem = cas.GetCategoryProductService(companyID, companyPassword, customerID, customerPassword, level4ID);

            if (siteNitem != null && siteNitem.sites.Count() > 0 && siteNitem.items.Count() > 0)
            {
                return siteNitem;
            }

            return null;
        }

        public List<Site> GetSites()
        {
            Site site;
            Site[] siteArr = null;
            List<Site> sites = new List<Site>();
            DataSet ds = new DataSet();
            CASWCFServiceClient cas = new CASWCFServiceClient();

            string companyID = HttpContext.Current.Session["CompanyID"].ToString();
            string companyPassword = HttpContext.Current.Session["CompanyPassword"].ToString();
            string customerID = HttpContext.Current.Session["CustomerID"].ToString();
            string customerPassword = HttpContext.Current.Session["CustomerPassword"].ToString();
            int level4ID = Convert.ToInt32(HttpContext.Current.Session["Level4ID"].ToString());

            siteArr = cas.GetCustomerSite(companyID, companyPassword, customerPassword, customerID, level4ID);

            if (siteArr != null && siteArr.Count() > 0)
            {
                foreach (Site siteItem in siteArr)
                {
                    site = new Site();

                    site.CompanyName = siteItem.CompanyName;
                    site.LastName = siteItem.LastName;
                    site.Address1 = siteItem.Address1;
                    site.Address2 = siteItem.Address2;
                    site.Address3 = siteItem.Address3;
                    site.Suburb = siteItem.Suburb;
                    site.PostCode = siteItem.PostCode;
                    site.State = siteItem.State;
                    site.PhoneNo = siteItem.PhoneNo;
                    site.MobileNo = siteItem.MobileNo;
                    site.Email = siteItem.Email;
                    site.SiteNo = siteItem.SiteNo;
                    site.StreetNo = siteItem.StreetNo;
                    site.SiteCode = siteItem.SiteCode;
                    site.Level4 = siteItem.Level4;

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