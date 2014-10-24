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

                ds = cas.GetBusinessTime("kevorkt", "", "1.000", 1);

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

        public Item GetBookedDays()
        {
            Item item = new Item();

            item.ItemID = 2;
            item.ItemName = "Item 2";
            item.SpecialInstruction = "Description Description Description 2002";
            item.Duration = 30;
            item.Price = 150;

            item.TimeSlots = new List<TimeSlot>(){
                new TimeSlot
                {
                    ItemID = 2,
                    Date = DateTime.Now.AddDays(7).ToString("dd/MMM/yyyy"),
                    StartTime = 16,
                    EndTime = 17
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = DateTime.Now.AddDays(7).ToString("dd/MMM/yyyy"),
                    StartTime = 9,
                    EndTime = 10
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = DateTime.Now.AddDays(0).ToString("dd/MMM/yyyy"),
                    StartTime = 9,
                    EndTime = 10
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = DateTime.Now.AddDays(3).ToString("dd/MMM/yyyy"),
                    StartTime = 14,
                    EndTime = 16
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = DateTime.Now.AddDays(3).ToString("dd/MMM/yyyy"),
                    StartTime = 10,
                    EndTime = 12
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = DateTime.Now.AddDays(4).ToString("dd/MMM/yyyy"),
                    StartTime = 12.5F,
                    EndTime = 13.5F
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = DateTime.Now.AddDays(1).ToString("dd/MMM/yyyy"),
                    StartTime = 10,
                    EndTime = 11.5F
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = DateTime.Now.AddDays(6).ToString("dd/MMM/yyyy"),
                    StartTime = 16.5F,
                    EndTime = 17.5F
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = DateTime.Now.AddDays(6).ToString("dd/MMM/yyyy"),
                    StartTime = 15,
                    EndTime = 16
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = DateTime.Now.AddDays(5).ToString("dd/MMM/yyyy"),
                    StartTime = 0,
                    EndTime = 0,
                    IsPublicHoliDay = true
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = DateTime.Now.AddDays(2).ToString("dd/MMM/yyyy"),
                    StartTime = 0,
                    EndTime = 0
                }
            };

            return item;
        }
    }
}