using CASPortal.CASService;
using CASPortal.Models;
using CASPortal.WebParser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CASPortal.Repository
{
    public class SchedulerRepository
    {
        public List<Item> GetAllItem()
        {
            Item item = new Item();
            List<Item> itemList;

            itemList = new List<Item>(){
                new Item
                {
                    ItemID = 10,
                    ItemName = "Pest Control 10",
                    SpecialInstruction = "SP Description 100"
                },
                new Item
                {
                    ItemID = 2,
                    ItemName = "Pest Control 2",
                    SpecialInstruction = "SP Description 20"
                },
                new Item
                {
                    ItemID = 5,
                    ItemName = "Pest Control 5",
                    SpecialInstruction = "SP Description 50"
                }
            };

            return itemList;
        }

        private BusinessHour GetBusinessTime(string dateStart)
        {
            try
            {
                List<BusinessHour> businessHours = new List<BusinessHour>();
                CASWebService cas = new CASWebService();

                DateTime dtStart = Convert.ToDateTime(dateStart);
                int dayOfWeek = (int)dtStart.DayOfWeek;

                if (HttpContext.Current.Session["BusinessHours"] != null)
                {
                    businessHours = (List<BusinessHour>)HttpContext.Current.Session["BusinessHours"];

                    var result = (from d in businessHours
                                  where d.NoOfDay == dayOfWeek
                                  select d).SingleOrDefault();

                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Item GetItem(string dateStart)
        {
            Item item = new Item();
            BusinessHour businessHour;
            List<TimeSlot> timeSlots = new List<TimeSlot>();
            List<BusinessHour> businessHours = new List<BusinessHour>();
            List<BusinessHour> fixedBusinessHours = new List<BusinessHour>();
            SchedulerParser parser = new SchedulerParser();

            if (HttpContext.Current.Session["BusinessHours"] != null)
            {
                fixedBusinessHours = (List<BusinessHour>)HttpContext.Current.Session["BusinessHours"];
            }

            item = parser.GetBookedDays();

            var listMaxHour = fixedBusinessHours.GroupBy(i => new { BusinessEndHour = i.BusinessEndHour })
                 .Select(group => new
                 {
                     BusinessEndHour = group.First().BusinessEndHour
                 })
                 .OrderByDescending(i => i.BusinessEndHour).ToList();

            item.MaxEndHour = listMaxHour[0].BusinessEndHour;

            var listMinHour = fixedBusinessHours.GroupBy(i => new { BusinessStartHour = i.BusinessStartHour })
                 .Select(group => new
                 {
                     BusinessStartHour = group.First().BusinessStartHour
                 })
                 .OrderBy(i => i.BusinessStartHour).ToList();

            item.MinStartHour = listMinHour[0].BusinessStartHour;

            var list = item.TimeSlots.GroupBy(i => new { Date = i.Date })
                 .Select(group => new
                 {
                     Date = group.First().Date
                 })
                 .OrderBy(i => DateTime.Parse(i.Date));

            foreach (var bhour in list)
            {
                businessHour = new BusinessHour();

                businessHour.Date = bhour.Date;
                businessHour.BusinessStartHour = GetBusinessTime(bhour.Date).BusinessStartHour;
                businessHour.BusinessEndHour = GetBusinessTime(bhour.Date).BusinessEndHour;
                businessHour.IsWorkingDay = GetBusinessTime(bhour.Date).IsWorkingDay;

                businessHours.Add(businessHour);
            }

            item.BusinessHours = businessHours;

            return item;
        }
    }
}