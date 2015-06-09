using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicePROWeb.Models
{
    public class TaskViewModel : ISchedulerEvent
    {
        public int TaskID { get; set; }
        public string Title { get; set; }
        public int TechID { get; set; }
        public string TechName { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }

        private DateTime start;
        public DateTime Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value.ToUniversalTime();
            }
        }

        private DateTime end;
        public DateTime End
        {
            get
            {
                return end;
            }
            set
            {
                end = value.ToUniversalTime();
            }
        }

        public string RecurrenceRule { get; set; }
        public int? RecurrenceID { get; set; }
        public string RecurrenceException { get; set; }
        public bool IsAllDay { get; set; }
        public int? OwnerID { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public int RoomID { get; set; }
        public int MeetingID { get; set; }
        public int SkillID { get; set; }
        public string SkillName { get; set; }
        public IEnumerable<int> Techs { get; set; }
    }
}