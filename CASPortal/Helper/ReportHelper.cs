using CASPortal.CASWCFService;
using CASPortal.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CASPortal.Helper
{
    public class ReportHelper
    {
        public string LoadSite()
        {
            SiteNItem siteNitem;
            StringBuilder sb = new StringBuilder("");
            StringBuilder siteFullName = new StringBuilder("");
            SchedulerRepository schRepository = new SchedulerRepository();

            if (HttpContext.Current.Session["SiteNItem"] == null)
                HttpContext.Current.Session["SiteNItem"] = schRepository.GetSiteNItems();

            siteNitem = (SiteNItem)HttpContext.Current.Session["SiteNItem"];
            sb.Append("<li style='cursor:pointer'><a>Select Site</a></li>");

            if(siteNitem == null)
                return sb.ToString();

            foreach (var site in siteNitem.sites)
            {
                siteFullName = new StringBuilder(site.StreetNo.Trim().Length > 0 ? site.StreetNo + ", " : "");
                siteFullName.Append(site.Address1.Trim().Length > 0 ? site.Address1 + " " : "");
                siteFullName.Append(site.Address2.Trim().Length > 0 ? site.Address2 + " " : "");
                siteFullName.Append(site.Address3.Trim().Length > 0 ? site.Address3 + ", " : "");
                siteFullName.Append(site.Suburb.Trim().Length > 0 ? site.Suburb + ", " : "");
                siteFullName.Append(site.State.Trim().Length > 0 ? site.State + "-" : "");
                siteFullName.Append(site.PostCode.Trim().Length > 0 ? site.PostCode : "");

                sb.Append("<li id=" + site.SiteNo + " style='cursor:pointer'><a>" + siteFullName.ToString() + "</a></li>");
            }

            return sb.ToString();
        }

        public string LoadContract(string siteNo)
        {
            List<Contract> contracts = new List<Contract>();
            StringBuilder sb = new StringBuilder("");
            ReportRepository repository = new ReportRepository();

            if (HttpContext.Current.Session["Contracts"] == null)
                HttpContext.Current.Session["Contracts"] = repository.GetContracts(siteNo);

            contracts = (List<Contract>)HttpContext.Current.Session["Contracts"];
            sb.Append("<li style='cursor:pointer'><a>Select Contract</a></li>");

            if (contracts == null)
                return sb.ToString();

            foreach (var contract in contracts)
                sb.Append("<li id=" + contract.ContractNo + " style='cursor:pointer'><a>" + contract.ContractName + " (" + contract.ContractDescription + ")</a></li>");

            return sb.ToString();
        }

        public string LoadItem()
        {
            SiteNItem siteNitem;
            StringBuilder sb = new StringBuilder("");
            SchedulerRepository schRepository = new SchedulerRepository();

            if (HttpContext.Current.Session["SiteNItem"] == null)
                HttpContext.Current.Session["SiteNItem"] = schRepository.GetSiteNItems();

            siteNitem = (SiteNItem)HttpContext.Current.Session["SiteNItem"];
            sb.Append("<li style='cursor:pointer'><a>Select Item</a></li>");

            if (siteNitem == null)
                return sb.ToString();

            foreach (var item in siteNitem.listOfItems)
                sb.Append("<li id=" + item.ItemID + " duration='" + item.Duration + "' desc='" + item.Description + "' style='cursor:pointer'><a>" + item.ItemName + "</a></li>");

            return sb.ToString();
        }

        public string LoadTree()
        {
            ReportRepository repository = new ReportRepository();
            StringBuilder sb = new StringBuilder("");
            TreeNode trNode = repository.GetTrendAnalysisTreeNodes();

            if (trNode != null && trNode.listLeve1.Count() > 0)
            {
                sb.Append("<ul>");
                sb.Append("<li>");
                sb.Append(trNode.listLeve1[0].RootCaption);
                sb.Append("<ul>");
                foreach (TreeNodeLevel1 t1 in trNode.listLeve1)
                {
                    sb.Append("<li>");
                    sb.Append(t1.SectionCaption);
                    sb.Append("<ul>");

                    foreach (TreeNodeLevel2 t2 in trNode.listLeve2.Where(t => t.SectionID == t1.SectionID))
                    {
                        sb.Append("<li>");
                        sb.Append(t2.QuestionCaption);
                        sb.Append("<ul>");

                        foreach (TreeNodeLevel3 t3 in trNode.listLeve3.Where(t => t.SectionID == t1.SectionID && t.QuestionID == t2.QuestionID))
                        {
                            string nodeID = t1.SectionID + "#" + t1.SectionCaption + "#" + t2.QuestionID + "#" + t2.QuestionCaption + "#" + t3.AnswerID + "#" + t3.AnswerCaption;
                            sb.Append("<li id='" + nodeID + "'>");
                            sb.Append(t3.AnswerCaption);
                            //sb.Append("<ul>");

                            //foreach (TreeNodeLevel4 t4 in trNode.listLeve4.Where(t => t.SectionID == t1.SectionID && t.QuestionID == t2.QuestionID && t.AnswerID == t3.AnswerID))
                            //{
                            //    sb.Append("<li>");
                            //    sb.Append(t4.AdditionalAnswerCaption);
                            //    sb.Append("</li>");
                            //}
                            //sb.Append("</ul>");
                            sb.Append("</li>");
                        }
                        sb.Append("</ul>");
                        sb.Append("</li>");
                    }
                    sb.Append("</ul>");
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
                sb.Append("</li>");
                sb.Append("</ul>");
            }

            return sb.ToString();
        }

        public string LoadArea()
        {
            ReportRepository repository = new ReportRepository();
            StringBuilder sbArea = new StringBuilder("");
            TreeNode trNode = repository.GetTrendAnalysisTreeNodes();

            sbArea.Append("<li style='cursor:pointer'><a>[ALL]</a></li>");
            foreach (string area in trNode.listAreaName)
                sbArea.Append("<li style='cursor:pointer'><a>" + area + "</a></li>");

            return sbArea.ToString();
        }
    }
}