using CASPortal.CASService;
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

                /*string companyID = HttpContext.Current.Session["CompanyID"].ToString();
                string companyPassword = HttpContext.Current.Session["CompanyPassword"].ToString();
                string customerPassword = HttpContext.Current.Session["CustomerPassword"].ToString();*/

                ds = cas.GetBusinessTime("kevorkt", "", "1.000", 1);
                //ds = cas.GetBusinessTime(companyID, companyPassword, customerPassword, 1);

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

            /*string companyID = HttpContext.Current.Session["CompanyID"].ToString();
            string companyPassword = HttpContext.Current.Session["CompanyPassword"].ToString();
            string customerPassword = HttpContext.Current.Session["CustomerPassword"].ToString();*/

            dateStart = dateStart.Substring(0, dateStart.IndexOf("GMT") - 1);
            ds = cas.GetScheduledTime("kevorkt", "", "1.000", Convert.ToDateTime(dateStart));
            //ds = cas.GetScheduledTime(companyID, companyPassword, customerPassword, Convert.ToDateTime(dateStart));

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
                            Date = scheduledDate.ToString("dd/MMM/yyyy"),
                            StartTime = startTime,
                            EndTime = endTime
                        }
                    );
                }
            }

            item.TimeSlots = timeSlots;

            /*item.ItemID = 2;
            item.ItemName = "Item 2";
            item.SpecialInstruction = "Description Description Description 2002";
            item.Duration = 30;
            item.Price = 150;

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
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = Convert.ToDateTime("01/Nov/2014").ToString("dd/MMM/yyyy"),
                    StartTime = "10",
                    EndTime = "12"
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = Convert.ToDateTime("30/Oct/2014").ToString("dd/MMM/yyyy"),
                    StartTime = "12.5",
                    EndTime = "13.5"
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = Convert.ToDateTime("30/Oct/2014").ToString("dd/MMM/yyyy"),
                    StartTime = "10",
                    EndTime = "11.5"
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = Convert.ToDateTime("02/Nov/2014").ToString("dd/MMM/yyyy"),
                    StartTime = "16.5",
                    EndTime = "17.5"
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = Convert.ToDateTime("05/Nov/2014").ToString("dd/MMM/yyyy"),
                    StartTime = "15",
                    EndTime = "16"
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = Convert.ToDateTime("04/Nov/2014").ToString("dd/MMM/yyyy"),
                    StartTime = "0",
                    EndTime = "0",
                    IsPublicHoliDay = true
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = Convert.ToDateTime("31/Oct/2014").ToString("dd/MMM/yyyy"),
                    StartTime = "0",
                    EndTime = "0"
                }
            };*/

            return item;
        }
    }
}