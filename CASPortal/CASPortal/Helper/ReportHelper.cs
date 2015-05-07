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
            string contractDescription = "";
            List<Contract> contracts = new List<Contract>();
            StringBuilder sb = new StringBuilder("");
            ReportRepository repository = new ReportRepository();

            sb.Append("<li style='cursor:pointer'><a>Select Contract</a></li>");

            if (HttpContext.Current.Session["ContractList"] != null)
                contracts = (List<Contract>)HttpContext.Current.Session["ContractList"];
            else
            {
                contracts = repository.GetContracts(siteNo);
                HttpContext.Current.Session.Add("ContractList", contracts);
            }

            if (contracts == null)
                return sb.ToString();

            foreach (var contract in contracts)
            {
                contractDescription = contract.ContractDescription.Length > 30 ? contract.ContractDescription.Substring(0, 30) : contract.ContractDescription;
                sb.Append("<li id=" + contract.ContractNo + " style='cursor:pointer'><a>" + contract.ContractName + " (" + contractDescription + ")</a></li>");
            }
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
                sb.Append("<li id='" + item.ItemID + "' duration='" + item.Duration + "' desc='" + item.Description + "' style='cursor:pointer'><a>" + item.ItemID + "</a></li>");

            return sb.ToString();
        }

        public string LoadTree()
        {
            ReportRepository repository = new ReportRepository();
            StringBuilder sb = new StringBuilder("");
            TreeNode trNode;

            if (HttpContext.Current.Session["TrendAnalysisTreeNodes"] != null)
                trNode = (TreeNode)HttpContext.Current.Session["TrendAnalysisTreeNodes"];
            else
            {
                trNode = repository.GetTrendAnalysisTreeNodes();
                HttpContext.Current.Session.Add("TrendAnalysisTreeNodes", trNode);
            }

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
            TreeNode trNode;

            if (HttpContext.Current.Session["TrendAnalysisTreeNodes"] != null)
                trNode = (TreeNode)HttpContext.Current.Session["TrendAnalysisTreeNodes"];
            else
            {
                trNode = repository.GetTrendAnalysisTreeNodes();
                HttpContext.Current.Session.Add("TrendAnalysisTreeNodes", trNode);
            }

            sbArea.Append("<li style='cursor:pointer'><a>[ALL]</a></li>");
            foreach (string area in trNode.listAreaName)
                sbArea.Append("<li style='cursor:pointer'><a>" + area + "</a></li>");

            return sbArea.ToString();
        }

        public string LoadTech()
        {
            List<EmployeeTech> emps = new List<EmployeeTech>();
            StringBuilder sb = new StringBuilder("");
            ReportRepository repository = new ReportRepository();

            sb.Append("<li style='cursor:pointer'><a>[ALL]</a></li>");
            emps = repository.GetEmployeeTech();

            if (emps == null)
                return sb.ToString();

            foreach (var emp in emps)
                sb.Append("<li id=" + emp.Code + " style='cursor:pointer'><a>" + emp.Code + "</a></li>");

            return sb.ToString();
        }
    }
}