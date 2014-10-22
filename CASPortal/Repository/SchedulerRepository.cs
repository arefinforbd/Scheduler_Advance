using CASPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASPortal.Repository
{
    public class SchedulerRepository
    {
        public Item GetItem()
        {
            Item item = new Item();
            BusinessHour businessHour;
            List<TimeSlot> timeSlots = new List<TimeSlot>();
            List<BusinessHour> businessHours = new List<BusinessHour>();

            item.ItemID = 2;
            item.ItemName = "Item 2";
            item.SpecialInstruction = "Description Description Description 2002";
            item.Duration = 30;
            item.Price = 150;

            item.TimeSlots = new List<TimeSlot>(){

                new TimeSlot
                {
                    ItemID = 2,
                    Date = DateTime.Now.AddDays(2).ToString("dd/MMM/yyyy"),
                    BusinessStartHour = 7,
                    BusinessEndHour = 18,
                    StartTime = 16,
                    EndTime = 17
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = DateTime.Now.AddDays(2).ToString("dd/MMM/yyyy"),
                    BusinessStartHour = 7,
                    BusinessEndHour = 18,
                    StartTime = 9,
                    EndTime = 10
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = DateTime.Now.AddDays(3).ToString("dd/MMM/yyyy"),
                    BusinessStartHour = 8,
                    BusinessEndHour = 19,
                    StartTime = 14,
                    EndTime = 16
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = DateTime.Now.AddDays(3).ToString("dd/MMM/yyyy"),
                    BusinessStartHour = 8,
                    BusinessEndHour = 19,
                    StartTime = 10,
                    EndTime = 12
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = DateTime.Now.AddDays(4).ToString("dd/MMM/yyyy"),
                    BusinessStartHour = 9,
                    BusinessEndHour = 15
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = DateTime.Now.AddDays(4).ToString("dd/MMM/yyyy"),
                    BusinessStartHour = 9,
                    BusinessEndHour = 15
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = DateTime.Now.AddDays(1).ToString("dd/MMM/yyyy"),
                    BusinessStartHour = 9.5F,
                    BusinessEndHour = 15.5F,
                    StartTime = 13.5F,
                    EndTime = 14.5F
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = DateTime.Now.AddDays(1).ToString("dd/MMM/yyyy"),
                    BusinessStartHour = 9.5F,
                    BusinessEndHour = 15.5F,
                    StartTime = 10,
                    EndTime = 11.5F
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = new DateTime(2014, 11, 5).ToString("dd/MMM/yyyy"),
                    BusinessStartHour = 9.5F,
                    BusinessEndHour = 15.5F,
                    StartTime = 11.5F,
                    EndTime = 12F
                },
                new TimeSlot
                {
                    ItemID = 2,
                    Date = new DateTime(2014, 11, 5).ToString("dd/MMM/yyyy"),
                    BusinessStartHour = 9.5F,
                    BusinessEndHour = 15.5F,
                    StartTime = 13F,
                    EndTime = 14.5F
                }
            };

            var listMaxHour = item.TimeSlots.GroupBy(i => new { BusinessEndHour = i.BusinessEndHour })
                 .Select(group => new
                 {
                     BusinessEndHour = group.First().BusinessEndHour
                 })
                 .OrderByDescending(i => i.BusinessEndHour).ToList();

            item.MaxEndHour = listMaxHour[0].BusinessEndHour;

            var listMinHour = item.TimeSlots.GroupBy(i => new { BusinessStartHour = i.BusinessStartHour })
                 .Select(group => new
                 {
                     BusinessStartHour = group.First().BusinessStartHour
                 })
                 .OrderBy(i => i.BusinessStartHour).ToList();

            item.MinStartHour = listMinHour[0].BusinessStartHour;

            var list = item.TimeSlots.GroupBy(i => new { Date = i.Date, BusinessStartHour = i.BusinessStartHour, BusinessEndHour = i.BusinessEndHour })
                 .Select(group => new
                 {
                     Date = group.First().Date,
                     BusinessStartHour = group.First().BusinessStartHour,
                     BusinessEndHour = group.First().BusinessEndHour
                 })
                 .OrderBy(i => DateTime.Parse(i.Date));

            foreach (var bhour in list)
            {
                businessHour = new BusinessHour();

                businessHour.Date = bhour.Date;
                businessHour.BusinessStartHour = bhour.BusinessStartHour;
                businessHour.BusinessEndHour = bhour.BusinessEndHour;

                businessHours.Add(businessHour);
            }

            item.BusinessHours = businessHours;

            return item;
        }
    }
}