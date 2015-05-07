using CASPortal.CASWCFService;
using CASPortal.WebParser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CASPortal.Repository
{
    public class SchedulerRepository
    {
        public SiteNItem GetSiteNItems()
        {
            SiteNItem siteNitem;
            SchedulerParser parser = new SchedulerParser();

            siteNitem = parser.GetSiteNItems();

            return siteNitem;
        }

        private BusinessHour GetBusinessTime(string dateStart)
        {
            try
            {
                List<BusinessHour> businessHours = new List<BusinessHour>();

                DateTime dtStart = DateTime.Parse(dateStart, new CultureInfo("en-US"));
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

        public Service GetTimeSlots(string dateStart)
        {
            Service service = new Service();
            BusinessHour businessHour;
            List<TimeSlot> timeSlots = new List<TimeSlot>();
            List<BusinessHour> businessHours = new List<BusinessHour>();
            List<BusinessHour> fixedBusinessHours = new List<BusinessHour>();
            SchedulerParser parser = new SchedulerParser();

            if (HttpContext.Current.Session["BusinessHours"] != null)
            {
                fixedBusinessHours = (List<BusinessHour>)HttpContext.Current.Session["BusinessHours"];
                service = parser.GetBookedDays(dateStart);

                var listMaxHour = fixedBusinessHours.GroupBy(i => new { BusinessEndHour = i.BusinessEndHour })
                     .Select(group => new
                     {
                         BusinessEndHour = group.First().BusinessEndHour
                     })
                     .OrderByDescending(i => i.BusinessEndHour).ToList();

                service.MaxEndHour = listMaxHour[0].BusinessEndHour;

                var listMinHour = fixedBusinessHours.GroupBy(i => new { BusinessStartHour = i.BusinessStartHour })
                     .Select(group => new
                     {
                         BusinessStartHour = group.First().BusinessStartHour
                     })
                     .OrderBy(i => i.BusinessStartHour).ToList();

                service.MinStartHour = listMinHour[0].BusinessStartHour;

                var list = service.TimeSlots.GroupBy(i => new { Date = i.Date })
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

                service.BusinessHours = businessHours.ToArray();
            }
            return service;
        }

        public bool SendCustomerInformationForSchedule(string firstname, string lastname, string email, string phoneno, string mobileno, string streetNo, string streetName, string streetName2, string suburb, string state, string postCode, int siteNo, int siteCode, string category, string product, int lineNo, string itemID, decimal totalAmount, decimal taxAmount, DateTime scheduledDate, string startTime, string endTime, string duration, string specialInstruction)
        {
            bool response;
            SchedulerParser parser = new SchedulerParser();

            response = parser.SendCustomerInformationForSchedule(firstname, lastname, email, phoneno, mobileno, streetNo, streetName, streetName2, suburb, state, postCode, siteNo, siteCode, category, product, lineNo, itemID, totalAmount, taxAmount, scheduledDate, startTime, endTime, duration, specialInstruction);

            return response;
        }
    }
}