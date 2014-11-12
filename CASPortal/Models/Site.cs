using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASPortal.Models
{
    public class Site
    {
        public string CompanyName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Suburb { get; set; }
        public string PostCode { get; set; }
        public string State { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public int SiteNo { get; set; }
        public string StreetNo { get; set; }
        public int SiteCode { get; set; }
        public int Level4 { get; set; }
    }
}