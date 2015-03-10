using CASWCFService.Model;
using Progress.Open4GL.Proxy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CASWCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CASWCFService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CASWCFService.svc or CASWCFService.svc.cs at the Solution Explorer and start debugging.
    public class CASWCFService : ICASWCFService
    {
        public bool LoginProcess(string CompanyID, string CompanyPassword, string CustomerID, string CustomerPassword, out int lvl4id)
        {
            try
            {
                bool status;
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_getwebcust(CompanyID, CustomerID, CustomerPassword, out lvl4id, out status);

                return status;
            }
            catch (Exception ex)
            {
                lvl4id = 0;
                return false;
            }
        }

        /// <summary>
        /// This method will return the dataset of customer information, private and public folder names
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="CompanyPassword"></param>
        /// <param name="CustomerPassword"></param>
        /// <param name="Level4ID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public Welcome GetCASDefaultData(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, string CustomerID)
        {
            DataSet ds = new DataSet();
            Welcome welcome = new Welcome();
            StrongTypesNS.ds_companyinfodataDataSet comInfoDs;

            string privateFolders = "";
            string publicFolders = "";

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_pubgetcasdef(Level4ID, CustomerID, out comInfoDs, out privateFolders, out publicFolders);
                ds = (DataSet)comInfoDs;

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    welcome.MessageOfTheDay = row["tt_motd"].ToString();
                    welcome.CompanyName = row["tt_company_name"].ToString();
                    welcome.LogoFileName = row["tt_logo_filename"].ToString();
                    try
                    {
                        welcome.CompanyLogo = (byte[])row["tt_company_logo"];
                    }
                    catch (Exception ex)
                    {
                        welcome.CompanyLogo = new byte[2];
                    }
                    welcome.PrivateFolders = privateFolders;
                    welcome.PublicFolders = publicFolders;
                }
                else
                    return null;

                return welcome;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// This method will return the dataset of customer information
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="Level4ID"></param>
        /// <param name="CustomerID"></param>
        /// <returns> Customer information in dataset</returns>
        public List<Welcome> LoginMessageOfDay(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, string CustomerID)
        {
            Welcome cs;
            DataSet ds = new DataSet();
            List<Welcome> welcomes = new List<Welcome>();
            StrongTypesNS.ds_companyinfodataDataSet comInfoDs;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_webcasmotd(Level4ID, CustomerID, out comInfoDs);
                ds = (DataSet)comInfoDs;

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        cs = new Welcome();

                        cs.MessageOfTheDay = row["tt_motd"].ToString();
                        cs.CompanyName = row["tt_company_name"].ToString();
                        cs.LogoFileName = row["tt_logo_filename"].ToString();
                        try
                        {
                            cs.CompanyLogo = (byte[])row["tt_company_logo"];
                        }
                        catch (Exception ex)
                        {
                            cs.CompanyLogo = new byte[2];
                        }

                        welcomes.Add(cs);
                    }
                }
                else
                    return null;

                return welcomes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// This method will return private folder names
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="CompanyPassword"></param>
        /// <param name="CustomerPassword"></param>
        /// <param name="Level4ID"></param>
        /// <param name="CustomerID"></param>
        /// <returns>Private folder names</returns>
        public string GetPrivateFolders(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, string CustomerID)
        {
            string folderNames;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_webcustinfo(Level4ID, out folderNames);

                return folderNames;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// This method will return public folder names
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="CompanyPassword"></param>
        /// <param name="CustomerPassword"></param>
        /// <param name="Level4ID"></param>
        /// <param name="CustomerID"></param>
        /// <returns>Public folder names</returns>
        public string GetPublicFolders(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID)
        {
            string folderNames;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_webpubcustinfo(Level4ID, out folderNames);

                return folderNames;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// This method will return the list of files of private folder
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="CompanyPassword"></param>
        /// <param name="CustomerPassword"></param>
        /// <param name="Level4ID"></param>
        /// <param name="CustomerID"></param>
        /// <param name="FolderName"></param>
        /// <param name="TimeZoneOffset"></param>
        /// <returns>List of files of private folder</returns>
        public List<Folder> GetPrivateFolderFiles(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, string CustomerID, string FolderName, string TimeZoneOffset)
        {
            DataSet ds;
            Folder folder;
            List<Folder> folders = new List<Folder>();
            StrongTypesNS.ds_filelistDataSet stypefileList;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_webdbfilelist(Level4ID, CustomerID, FolderName, TimeZoneOffset, out stypefileList);
                ds = (DataSet)stypefileList;

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        folder = new Folder();

                        folder.lvl4sequence = Convert.ToInt32(row["tt_lvl4_sequence"]);
                        folder.fileName = row["tt_filename"].ToString();
                        folder.fileDescription = row["tt_filedesc"].ToString();
                        folder.fileUploadedDate = row["tt_uploaddatetime"].ToString();
                        folder.fileUploadedBy = row["tt_uploadedby"].ToString();

                        folders.Add(folder);
                    }
                }
                else
                    return null;

                return folders;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// This method will return the list of files of public folder
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="CompanyPassword"></param>
        /// <param name="CustomerPassword"></param>
        /// <param name="Level4ID"></param>
        /// <param name="CustomerID"></param>
        /// <param name="FolderName"></param>
        /// <param name="TimeZoneOffset"></param>
        /// <returns>List of files of public folder</returns>
        public List<Folder> GetPublicFolderFiles(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, string FolderName, string TimeZoneOffset)
        {
            DataSet ds;
            Folder folder;
            List<Folder> folders = new List<Folder>();
            StrongTypesNS.ds_filelistDataSet stypefileList;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_webpubdbfilelist(Level4ID, FolderName, TimeZoneOffset, out stypefileList);
                ds = (DataSet)stypefileList;

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        folder = new Folder();

                        folder.lvl4sequence = Convert.ToInt32(row["tt_lvl4_sequence"]);
                        folder.fileName = row["tt_filename"].ToString();
                        folder.fileDescription = row["tt_filedesc"].ToString();
                        folder.fileUploadedDate = row["tt_uploaddatetime"].ToString();
                        folder.fileUploadedBy = row["tt_uploadedby"].ToString();

                        folders.Add(folder);
                    }
                }
                else
                    return null;

                return folders;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// This method will return the file of private folder in binary
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="CompanyPassword"></param>
        /// <param name="CustomerPassword"></param>
        /// <param name="Level4ID"></param>
        /// <param name="CustomerID"></param>
        /// <param name="FolderName"></param>
        /// <param name="TimeZoneOffset"></param>
        /// <returns>Binary file</returns>
        public byte[] GetPrivateFileInfo(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, string CustomerID, string FolderName, string FileName)
        {
            DataSet ds;
            string outputMessage;
            StrongTypesNS.ds_filedataDataSet stypefile;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_webfileinfo(Level4ID, CustomerID, FolderName, FileName, out stypefile, out outputMessage);
                ds = (DataSet)stypefile;

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                    return (byte[])(ds.Tables[0].Rows[0]["ttf_fileinfo"]);

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// This method will return the file of public folder in binary
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="CompanyPassword"></param>
        /// <param name="CustomerPassword"></param>
        /// <param name="Level4ID"></param>
        /// <param name="CustomerID"></param>
        /// <param name="FolderName"></param>
        /// <param name="FileName"></param>
        /// <returns>Binary file</returns>
        public byte[] GetPublicFileInfo(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, string FolderName, string FileName)
        {
            DataSet ds;
            string outputMessage;
            StrongTypesNS.ds_filedataDataSet stypefile;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_webpubfileinfo(Level4ID, FolderName, FileName, out stypefile, out outputMessage);
                ds = (DataSet)stypefile;

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                    return (byte[])(ds.Tables[0].Rows[0]["ttf_fileinfo"]);

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DayHour> GetBusinessTime(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID)
        {
            DataSet ds;
            DayHour dayHour;
            List<DayHour> dayHours = new List<DayHour>();
            StrongTypesNS.ds_company_hoursDataSet companyHours;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_webgetbustime(Level4ID, out companyHours);
                ds = (DataSet)companyHours;

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        dayHour = new DayHour();

                        dayHour.IsDay1WorkingDay = Convert.ToBoolean(row["tt_workday_1"]);
                        dayHour.Day1StartHour = row["tt_start_time_1"].ToString();
                        dayHour.Day1EndHour = row["tt_end_time_1"].ToString();

                        dayHour.IsDay2WorkingDay = Convert.ToBoolean(row["tt_workday_2"]);
                        dayHour.Day2StartHour = row["tt_start_time_2"].ToString();
                        dayHour.Day2EndHour = row["tt_end_time_2"].ToString();

                        dayHour.IsDay3WorkingDay = Convert.ToBoolean(row["tt_workday_3"]);
                        dayHour.Day3StartHour = row["tt_start_time_3"].ToString();
                        dayHour.Day3EndHour = row["tt_end_time_3"].ToString();

                        dayHour.IsDay4WorkingDay = Convert.ToBoolean(row["tt_workday_4"]);
                        dayHour.Day4StartHour = row["tt_start_time_4"].ToString();
                        dayHour.Day4EndHour = row["tt_end_time_4"].ToString();

                        dayHour.IsDay5WorkingDay = Convert.ToBoolean(row["tt_workday_5"]);
                        dayHour.Day5StartHour = row["tt_start_time_5"].ToString();
                        dayHour.Day5EndHour = row["tt_end_time_5"].ToString();

                        dayHour.IsDay6WorkingDay = Convert.ToBoolean(row["tt_workday_6"]);
                        dayHour.Day6StartHour = row["tt_start_time_6"].ToString();
                        dayHour.Day6EndHour = row["tt_end_time_6"].ToString();

                        dayHour.IsDay7WorkingDay = Convert.ToBoolean(row["tt_workday_7"]);
                        dayHour.Day7StartHour = row["tt_start_time_7"].ToString();
                        dayHour.Day7EndHour = row["tt_end_time_7"].ToString();

                        dayHours.Add(dayHour);
                    }
                }
                else
                    return null;

                return dayHours;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<TimeSlot> GetScheduledTime(string CompanyID, string CompanyPassword, string CustomerPassword, DateTime startDate)
        {
            DataSet ds;
            TimeSlot timeSlot;
            List<TimeSlot> timeSlots = new List<TimeSlot>();
            StrongTypesNS.ds_timeslot_hoursDataSet dsTimeSlot;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_webgetschtime(startDate, out dsTimeSlot);
                ds = (DataSet)dsTimeSlot;

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        timeSlot = new TimeSlot();

                        timeSlot.Date = row["tt_ts_date"].ToString();
                        timeSlot.StartTime = row["tt_ts_start_time"].ToString();
                        timeSlot.EndTime = row["tt_ts_end_time"].ToString();

                        timeSlots.Add(timeSlot);
                    }
                }
                else
                    return null;

                return timeSlots;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public List<Item> GetCategoryProductServiceOld(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID)
        //{
        //    DataSet ds;
        //    Item item;
        //    List<Item> items = new List<Item>();
        //    StrongTypesNS.ds_catproditemDataSet services;

        //    try
        //    {
        //        Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
        //        CustWebAccProj cus = new CustWebAccProj(conn);

        //        cus.p_webgetcatprod(Level4ID, out services);
        //        ds = (DataSet)services;

        //        if (ds != null && ds.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow row in ds.Tables[0].Rows)
        //            {
        //                item = new Item();

        //                item.CategoryName = row["pd_category"].ToString();
        //                item.ProductName = row["pd_code"].ToString();
        //                item.ItemName = row["pdl_prcode"].ToString();
        //                item.ItemID = Convert.ToInt32(row["pdl_lineno"]);
        //                item.Description = row["pdl_desc"].ToString();
        //                item.Price = Convert.ToDouble(row["pdl_price"]);
        //                item.Duration = Convert.ToInt32(row["pdl_duration"]);

        //                items.Add(item);
        //            }
        //        }
        //        else
        //            return null;

        //        return items;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public SiteNItem GetCategoryProductService(string CompanyID, string CompanyPassword, decimal CustomerID, string CustomerPassword, int Level4ID)
        {
            DataSet dsServices;
            DataSet dsSites;
            Service item;
            Site site;
            List<Service> serviceList = new List<Service>();
            List<Site> sites = new List<Site>();
            SiteNItem siteNitem = new SiteNItem();
            StrongTypesNS.ds_catproditemDataSet services;
            StrongTypesNS.ds_csmstrDataSet master;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_webgetcatprod(Level4ID, CustomerID, out services, out master);
                dsServices = (DataSet)services;
                dsSites = (DataSet)master;

                if (dsSites != null && dsSites.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsSites.Tables[0].Rows)
                    {
                        site = new Site();

                        site.CompanyName = row["cs_company"].ToString();
                        site.Address1 = row["cs_addr1"].ToString();
                        site.Address2 = row["cs_addr2"].ToString();
                        site.Address3 = row["cs_addr3"].ToString();
                        site.Suburb = row["cs_suburb"].ToString();
                        site.PhoneNo = row["cs_phone"].ToString();
                        site.MobileNo = row["cs_mobile"].ToString();
                        site.PostCode = row["cs_pcode"].ToString();
                        site.State = row["cs_state"].ToString();
                        site.SiteNo = Convert.ToInt32(row["cs_siteno"].ToString());
                        site.StreetNo = row["cs_streetno"].ToString();
                        site.SiteCode = Convert.ToInt32(row["cs_sitecode"].ToString());
                        site.Level4 = Convert.ToInt32(row["cs_lvl4_sequence"]);
                        site.LastName = row["cs_lastname"].ToString();
                        site.Email = row["cs_email"].ToString();

                        sites.Add(site);
                    }
                }
                else
                    return null;

                if (dsServices != null && dsServices.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsServices.Tables[0].Rows)
                    {
                        item = new Service();

                        item.CategoryName = row["pd_category"].ToString();
                        item.ProductName = row["pd_code"].ToString();
                        item.ItemName = row["pdl_prcode"].ToString();
                        item.ItemID = Convert.ToInt32(row["pdl_lineno"]);
                        item.Description = row["pdl_desc"].ToString();
                        item.Price = Convert.ToDouble(row["pdl_price"]);
                        item.Duration = Convert.ToInt32(row["pdl_duration"]);

                        serviceList.Add(item);
                    }
                }
                else
                    return null;

                siteNitem.sites = sites;
                siteNitem.listOfItems = serviceList;

                return siteNitem;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Site> GetCustomerSite(string CompanyID, string CompanyPassword, string CustomerPassword, string CustomerID, int Level4ID)
        {
            DataSet ds;
            Site site;
            List<Site> sites = new List<Site>();
            StrongTypesNS.ds_csmstrDataSet dsSite;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_webgetcsite(decimal.Parse(CustomerID), Level4ID, out dsSite);
                ds = (DataSet)dsSite;

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        site = new Site();

                        site.CompanyName = row["cs_company"].ToString();
                        site.Address1 = row["cs_addr1"].ToString();
                        site.Address2 = row["cs_addr2"].ToString();
                        site.Address3 = row["cs_addr3"].ToString();
                        site.Suburb = row["cs_suburb"].ToString();
                        site.PhoneNo = row["cs_phone"].ToString();
                        site.MobileNo = row["cs_mobile"].ToString();
                        site.PostCode = row["cs_pcode"].ToString();
                        site.State = row["cs_state"].ToString();
                        site.SiteNo = Convert.ToInt32(row["cs_siteno"].ToString());
                        site.StreetNo = row["cs_streetno"].ToString();
                        site.SiteCode = Convert.ToInt32(row["cs_sitecode"].ToString());
                        site.Level4 = Convert.ToInt32(row["cs_lvl4_sequence"]);
                        site.LastName = row["cs_lastname"].ToString();
                        site.Email = row["cs_email"].ToString();

                        sites.Add(site);
                    }
                }
                else
                    return null;

                return sites;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public TreeNode GetTrendAnalysisTreeNodes(string CompanyID, string CompanyPassword, decimal CustomerID, string CustomerPassword, int Level4ID)
        {
            DataSet dsLevel1;
            DataSet dsLevel2;
            DataSet dsLevel3;
            DataSet dsLevel4;
            DataSet dsArea;
            TreeNodeLevel1 trLevel1;
            TreeNodeLevel2 trLevel2;
            TreeNodeLevel3 trLevel3;
            TreeNodeLevel4 trLevel4;
            TreeNode treeNodes = new TreeNode();
            StrongTypesNS.ds_rsmstrDataSet level1;
            StrongTypesNS.ds_rsddetDataSet level2;
            StrongTypesNS.ds_rsadetDataSet level3;
            StrongTypesNS.ds_rsmdetDataSet level4;
            StrongTypesNS.ds_areaDataSet area;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.ws_ldbardet(Level4ID, out level1, out level2, out level3, out level4, out area);
                dsLevel1 = (DataSet)level1;
                dsLevel2 = (DataSet)level2;
                dsLevel3 = (DataSet)level3;
                dsLevel4 = (DataSet)level4;
                dsArea = (DataSet)area;

                //dsLevel1.WriteXml("C:\\Arefin\\level1.xml");
                //dsLevel2.WriteXml("C:\\Arefin\\level2.xml");
                //dsLevel3.WriteXml("C:\\Arefin\\level3.xml");
                //dsLevel4.WriteXml("C:\\Arefin\\level4.xml");

                if (dsLevel1 != null && dsLevel1.Tables[0].Rows.Count > 0)
                {
                    DataView dv = dsLevel1.Tables[0].DefaultView;
                    dv.Sort = "rsm_sec_id asc";
                    DataTable dt = dv.ToTable();
                    treeNodes.listLeve1 = new List<TreeNodeLevel1>();

                    foreach (DataRow row in dt.Rows)
                    {
                        trLevel1 = new TreeNodeLevel1();
                        trLevel1.SectionID = Convert.ToInt32(row["rsm_sec_id"].ToString());
                        trLevel1.SectionCaption = row["rsm_sec_desc"].ToString();
                        trLevel1.RootCaption = row["rsm_name"].ToString();

                        treeNodes.listLeve1.Add(trLevel1);
                    }
                }
                else
                    return null;

                if (dsLevel2 != null && dsLevel2.Tables[0].Rows.Count > 0)
                {
                    DataView dv = dsLevel2.Tables[0].DefaultView;
                    dv.Sort = "rsd_qu_id asc";
                    DataTable dt = dv.ToTable();
                    treeNodes.listLeve2 = new List<TreeNodeLevel2>();

                    foreach (DataRow row in dt.Rows)
                    {
                        trLevel2 = new TreeNodeLevel2();
                        trLevel2.QuestionID = Convert.ToInt32(row["rsd_qu_id"].ToString());
                        trLevel2.SectionID = Convert.ToInt32(row["rsd_sec_id"].ToString());
                        trLevel2.QuestionCaption = row["rsd_qu_desc"].ToString();

                        treeNodes.listLeve2.Add(trLevel2);
                    }
                }
                else
                    return null;

                if (dsLevel3 != null && dsLevel3.Tables[0].Rows.Count > 0)
                {
                    DataView dv = dsLevel3.Tables[0].DefaultView;
                    dv.Sort = "rsa_ans_id asc";
                    DataTable dt = dv.ToTable();
                    treeNodes.listLeve3 = new List<TreeNodeLevel3>();

                    foreach (DataRow row in dt.Rows)
                    {
                        trLevel3 = new TreeNodeLevel3();
                        trLevel3.AnswerID = Convert.ToInt32(row["rsa_ans_id"].ToString());
                        trLevel3.QuestionID = Convert.ToInt32(row["rsa_qu_id"].ToString());
                        trLevel3.SectionID = Convert.ToInt32(row["rsa_sec_id"].ToString());
                        trLevel3.AnswerCaption = row["rsa_ans_choice"].ToString();

                        treeNodes.listLeve3.Add(trLevel3);
                    }
                }
                else
                    return null;

                if (dsLevel4 != null && dsLevel4.Tables[0].Rows.Count > 0)
                {
                    DataView dv = dsLevel4.Tables[0].DefaultView;
                    dv.Sort = "rsm_add_ans_id asc";
                    DataTable dt = dv.ToTable();
                    treeNodes.listLeve4 = new List<TreeNodeLevel4>();

                    foreach (DataRow row in dt.Rows)
                    {
                        trLevel4 = new TreeNodeLevel4();
                        trLevel4.AdditionalAnswerID = Convert.ToInt32(row["rsm_add_ans_id"].ToString());
                        trLevel4.AnswerID = Convert.ToInt32(row["rsm_ans_id"].ToString());
                        trLevel4.QuestionID = Convert.ToInt32(row["rsm_qu_id"].ToString());
                        trLevel4.SectionID = Convert.ToInt32(row["rsm_sec_id"].ToString());
                        trLevel4.AdditionalAnswerCaption = row["rsm_ans_choice"].ToString();

                        treeNodes.listLeve4.Add(trLevel4);
                    }
                }
                else
                    return null;

                if (dsArea != null && dsArea.Tables[0].Rows.Count > 0)
                {
                    treeNodes.listAreaName = new List<string>();

                    foreach (DataRow row in dsArea.Tables[0].Rows)
                    {
                        treeNodes.listAreaName.Add(row["tta_area"].ToString());
                    }
                }
                else
                    return null;

                return treeNodes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ChartData> GetTrendAnalysis(string CompanyID, string CompanyPassword, decimal CustomerID, string CustomerPassword, int Level4ID, int SiteNo, int ContractNo, DataTable answers, string Area, int Frequency, DateTime FromDate, DateTime ToDate, int GroupBy, bool IsUseDate)
        {
            string responseMessage = "";
            string message = "";
            DataSet dsChart;
            DataSet dsLegend;
            ChartLegend legend = new ChartLegend();
            ChartData chart = new ChartData();
            List<ChartData> charts = new List<ChartData>();
            StrongTypesNS.ds_legendDataSet legendDataset;
            StrongTypesNS.ds_chart_bar2DataSet chartDataset;
            StrongTypesNS.ds_answers2DataSet dsAnswers = new StrongTypesNS.ds_answers2DataSet();

            try
            {
                foreach (DataRow row in answers.Rows)
                {
                    dsAnswers.Tables[0].Rows.Add(row["RootNode"].ToString(), Convert.ToDecimal(row["SectionID"]),
                        Convert.ToDecimal(row["QuestionID"]), "", Convert.ToInt32(row["AnswerID"]), row["SectionDesc"].ToString(),
                        row["QuestionDesc"].ToString(), row["AnswerDesc"].ToString(), row["SectionCaption"].ToString(),
                        row["QuestionCaption"].ToString());
                }

                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                message = cus.ws_bartrdalys(Level4ID, CustomerID, SiteNo, ContractNo, dsAnswers, Area, Frequency, FromDate, ToDate, GroupBy, IsUseDate, out chartDataset, out legendDataset, out responseMessage);
                dsChart = (DataSet)chartDataset;
                dsLegend = (DataSet)legendDataset;

                legend = GetLegend(dsLegend);

                if (dsChart != null && dsChart.Tables["tt_chart_bar"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsChart.Tables["tt_chart_bar"].Rows)
                    {
                        chart = new ChartData();

                        chart.SerialNumber = row["ttcb_serialnumber"].ToString();
                        chart.Area = row["ttcb_area"].ToString();
                        chart.DateLabel = Convert.ToDateTime(row["ttcb_date"]).ToString("dd/MM/yyyy");
                        chart.Point = Convert.ToDouble(row["ttcb_points"]);
                        chart.Legend = legend;

                        charts.Add(chart);
                    }
                }
                else
                    return null;

                return charts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }        
        
        public List<ChartData> GetTrendAnalysisByJob(string CompanyID, string CompanyPassword, decimal CustomerID, string CustomerPassword, int Level4ID, int SiteNo, int ContractNo, DataTable answers, string Area, DateTime FromDate, DateTime ToDate)
        {
            string responseMessage;
            string message = "";
            DataSet dsChart;
            DataSet dsLegend;
            ChartLegend legend = new ChartLegend();
            ChartData chart = new ChartData();
            List<ChartData> charts = new List<ChartData>();
            StrongTypesNS.ds_legendDataSet legendDataset;
            StrongTypesNS.ds_chart_barDataSet chartDataset;
            StrongTypesNS.ds_answersDataSet dsAnswers = new StrongTypesNS.ds_answersDataSet();

            try
            {
                foreach (DataRow row in answers.Rows)
                {
                    dsAnswers.Tables[0].Rows.Add(row["RootNode"].ToString(), Convert.ToDecimal(row["SectionID"]),
                        Convert.ToDecimal(row["QuestionID"]), "", Convert.ToInt32(row["AnswerID"]), row["SectionDesc"].ToString(),
                        row["QuestionDesc"].ToString(), row["AnswerDesc"].ToString());
                }

                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                message = cus.ws_barjobtrd(Level4ID, CustomerID, SiteNo, ContractNo, dsAnswers, Area, FromDate, ToDate, out chartDataset, out legendDataset, out responseMessage);
                dsChart = (DataSet)chartDataset;
                dsLegend = (DataSet)legendDataset;

                legend = GetLegend(dsLegend);

                if (dsChart != null && dsChart.Tables["tt_chart_bar"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsChart.Tables["tt_chart_bar"].Rows)
                    {
                        chart = new ChartData();

                        chart.Section = row["ttcb_area"].ToString();
                        chart.DateLabel = Convert.ToDateTime(row["ttcb_date"]).ToString("dd/MM/yyyy");
                        chart.Point = Convert.ToDouble(row["ttcb_points"]);
                        chart.Legend = legend;

                        charts.Add(chart);
                    }
                }
                else
                    return null;

                return charts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ChartData> GetTrendAnalysisByQuestion(string CompanyID, string CompanyPassword, decimal CustomerID, string CustomerPassword, int Level4ID, int SiteNo, int ContractNo, DataTable answers, int Frequency, DateTime FromDate, DateTime ToDate, int GroupBy)
        {
            string responseMessage = "";
            string message = "";
            DataSet dsChart;
            ChartData chart = new ChartData();
            ChartLegend legend = new ChartLegend();
            DataSet dsLegend;
            List<ChartData> charts = new List<ChartData>();
            StrongTypesNS.ds_legendDataSet legendDataset;
            StrongTypesNS.ds_chart_bar4DataSet chartDataset;
            StrongTypesNS.ds_answers2DataSet dsAnswers = new StrongTypesNS.ds_answers2DataSet();

            try
            {
                foreach (DataRow row in answers.Rows)
                {
                    dsAnswers.Tables[0].Rows.Add(row["RootNode"].ToString(), Convert.ToDecimal(row["SectionID"]), 
                        Convert.ToDecimal(row["QuestionID"]), "", Convert.ToInt32(row["AnswerID"]), row["SectionDesc"].ToString(), 
                        row["QuestionDesc"].ToString(), row["AnswerDesc"].ToString(), row["SectionCaption"].ToString(), 
                        row["QuestionCaption"].ToString());
                }

                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                message = cus.ws_bartrdbyque(Level4ID, CustomerID, SiteNo, ContractNo, dsAnswers, Frequency, FromDate, ToDate, GroupBy, out chartDataset, out legendDataset, out responseMessage);
                dsChart = (DataSet)chartDataset;
                dsLegend = (DataSet)legendDataset;

                legend = GetLegend(dsLegend);

                if (dsChart != null && dsChart.Tables["tt_chart_bar"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsChart.Tables["tt_chart_bar"].Rows)
                    {
                        chart = new ChartData();

                        chart.Section = row["ttcb_section"].ToString();
                        chart.Question = " (" + row["ttcb_question"].ToString() + ")";
                        chart.DateLabel = Convert.ToDateTime(row["ttcb_date"]).ToString("dd/MM/yyyy");
                        chart.Point = Convert.ToDouble(row["ttcb_points"]);
                        chart.Legend = legend;

                        charts.Add(chart);
                    }
                }
                else
                    return null;

                return charts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ChartData> GetTrendAnalysisByEquipment(string CompanyID, string CompanyPassword, decimal CustomerID, string CustomerPassword, int Level4ID, int SiteNo, int ContractNo, DataTable answers, int Frequency, DateTime FromDate, DateTime ToDate, int GroupBy, bool SortBy, bool IsExclude)
        {
            string responseMessage = "";
            string message = "";
            DataSet dsChart;
            DataSet dsLegend;
            ChartLegend legend = new ChartLegend();
            ChartData chart = new ChartData();
            List<ChartData> charts = new List<ChartData>();
            StrongTypesNS.ds_legendDataSet legendDataset;
            StrongTypesNS.ds_chart_bar3DataSet chartDataset;
            StrongTypesNS.ds_answers2DataSet dsAnswers = new StrongTypesNS.ds_answers2DataSet();

            try
            {
                foreach (DataRow row in answers.Rows)
                {
                    dsAnswers.Tables[0].Rows.Add(row["RootNode"].ToString(), Convert.ToDecimal(row["SectionID"]),
                        Convert.ToDecimal(row["QuestionID"]), "", Convert.ToInt32(row["AnswerID"]), row["SectionDesc"].ToString(),
                        row["QuestionDesc"].ToString(), row["AnswerDesc"].ToString(), row["SectionCaption"].ToString(),
                        row["QuestionCaption"].ToString());
                }

                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                message = cus.ws_bartrdbyequjob(Level4ID, CustomerID, SiteNo, ContractNo, dsAnswers, Frequency, FromDate, ToDate, GroupBy, SortBy, IsExclude, out chartDataset, out legendDataset, out responseMessage);
                dsChart = (DataSet)chartDataset;
                dsLegend = (DataSet)legendDataset;

                legend = GetLegend(dsLegend);

                if (dsChart != null && dsChart.Tables["tt_chart_bar"].Rows.Count > 0)
                {
                    foreach (DataRow row in dsChart.Tables["tt_chart_bar"].Rows)
                    {
                        chart = new ChartData();

                        chart.SerialNumber = row["ttcb_serialnumber"].ToString();
                        chart.Section = row["ttcb_section"].ToString();
                        chart.Question = " (" + row["ttcb_question"].ToString() + ")";
                        chart.DateLabel = Convert.ToDateTime(row["ttcb_date"]).ToString("dd/MM/yyyy");
                        chart.Point = Convert.ToDouble(row["ttcb_points"]);
                        chart.Legend = legend;

                        charts.Add(chart);
                    }
                }
                else
                    return null;

                return charts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private ChartLegend GetLegend(DataSet dsLegend)
        {
            ChartLegend legend = new ChartLegend();

            if (dsLegend != null && dsLegend.Tables["tt_legend"].Rows.Count > 0)
            {
                DataRow row = dsLegend.Tables["tt_legend"].Rows[0];

                legend.Line1 = row["ttl_head_line1"].ToString();
                legend.Line2 = row["ttl_head_line2"].ToString();
                legend.Line3 = row["ttl_head_line3"].ToString();
                legend.Line4 = row["ttl_head_line4"].ToString();
                legend.Footer = row["ttl_footer"].ToString();
            }

            return legend;
        }

        public List<Contract> GetContracts(string CompanyID, string CompanyPassword, string CustomerPassword, decimal CustomerID, string SiteNo, int Level4ID)
        {
            DataSet ds;
            Contract contract;
            List<Contract> contracts = new List<Contract>();
            StrongTypesNS.ds_comstrDataSet dsComstr;
            StrongTypesNS.ds_coddetDataSet dsContract;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.ldcustcont(Level4ID, CustomerID.ToString(), SiteNo, out dsComstr, out dsContract);
                ds = (DataSet)dsContract;

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        contract = new Contract();

                        contract.ContractNo = Convert.ToInt32(row["co_contractno"].ToString());
                        contract.ContractName = row["cod_prcode"].ToString();
                        contract.ContractDescription = row["cod_invdesc"].ToString();

                        contracts.Add(contract);
                    }
                }
                else
                    return null;

                return contracts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Equipment GetEquipmentTransactionBLOB(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, decimal CustomerIDFrom, decimal CustomerIDTo, DateTime DateFrom, DateTime DateTo, bool IsPrintDetails, bool IsPrintMaterials, int Selection, string AssignedTo, int Sorting, int ContractFrom, int ContractTo, bool IsInactive, bool IsShowTime, string GlAssignedTo)
        {
            DataSet ds;
            string outputMessage = "";
            Equipment equip = new Equipment();
            StrongTypesNS.ds_filedataDataSet stypefile;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.ws_equtranrpt(Level4ID, CustomerIDFrom, CustomerIDTo, DateFrom, DateTo, IsPrintDetails, IsPrintMaterials, Selection, AssignedTo, Sorting, ContractFrom, ContractTo, IsInactive, IsShowTime, GlAssignedTo, out stypefile, out outputMessage);
                ds = (DataSet)stypefile;

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                    equip.FileBLOB = (byte[])(ds.Tables[0].Rows[0]["ttf_fileinfo"]);

                equip.OutputMessage = outputMessage;

                return equip;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Equipment> GetInstalledEquipment(string CompanyID, string CompanyPassword, decimal CustomerID, string CustomerPassword, int Level4ID, int ContractNo, string EquipmentType)
        {
            DataSet ds;
            Equipment equip;
            List<Equipment> equips = new List<Equipment>();
            StrongTypesNS.ds_equdetDataSet dsEquipment;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.ldequdet(Level4ID, ContractNo, EquipmentType, out dsEquipment);
                ds = (DataSet)dsEquipment;

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        equip = new Equipment();

                        equip.EquipmentType = row["eqd_type"].ToString();
                        equip.Location = row["eqd_location"].ToString();
                        equip.Serial = row["eqd_serial"].ToString();
                        equip.JC = row["eqd_jc"].ToString();
                        equip.JobNo = Convert.ToInt32(row["eqd_jobno"]);
                        equip.SequenceNo = Convert.ToInt32(row["eqd_seqno"]);
                        equip.DateInstalled = DateTime.Parse(row["eqd_date_inst"].ToString());
                        equip.Quantity = Convert.ToInt32(row["eqd_qty"]);
                        equip.Area = row["equ_area"].ToString();
                        equip.Status = bool.Parse(row["eqd_status"].ToString());
                        equip.ReportName = row["eqd_name"].ToString();
                        equip.SectionID = Convert.ToDecimal(row["eqd_sec_id"]);
                        equip.QuestionID = Convert.ToDecimal(row["eqd_qu_id"]);
                        equip.Frequency = Convert.ToInt32(row["eqd_freq"]);
                        //equip.LastJob = DateTime.Parse(row["eqd_lastjob"].ToString());
                        //equip.NextJob = DateTime.Parse(row["eqd_nextjob"].ToString());
                        equip.Manufacturer = row["eqd_manuf"].ToString();
                        equip.Level4 = Convert.ToInt32(row["eqd_lvl4_sequence"]);
                        //equip.ManufactureDate = DateTime.Parse(row["eqd_manuf_date"].ToString());

                        equips.Add(equip);
                    }
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }

            return equips;
        }

        public Equipment GetEquipmentReportBLOB(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, decimal CustomerIDFrom, decimal CustomerIDTo, int ContractFrom, int ContractTo, int Sorting, int Status)
        {
            DataSet ds;
            string outputMessage = "";
            Equipment equip = new Equipment();
            StrongTypesNS.ds_filedataDataSet stypefile;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.ws_equrep(Level4ID, CustomerIDFrom, CustomerIDTo, ContractFrom, ContractTo, Sorting, Status, out stypefile, out outputMessage);
                ds = (DataSet)stypefile;

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                    equip.FileBLOB = (byte[])(ds.Tables[0].Rows[0]["ttf_fileinfo"]);

                equip.OutputMessage = outputMessage;

                return equip;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<NavigationMenu> GetNavigationMenu(string CompanyID, string CompanyPassword, decimal CustomerID, string CustomerPassword, int Level4ID, string RootMenu)
        {
            DataSet ds;
            NavigationMenu navMenu;
            List<NavigationMenu> navMenus = new List<NavigationMenu>();
            StrongTypesNS.ds_menulineDataSet dsMenu;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.ldwebmenu(Level4ID, CompanyID, RootMenu, out dsMenu);
                ds = (DataSet)dsMenu;

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        navMenu = new NavigationMenu();

                        navMenu.MenuName = row["m_name"].ToString();
                        navMenu.MenuOrder = Convert.ToInt32(row["ml_order"]);
                        navMenu.MenuTitle = row["ml_title"].ToString();
                        navMenu.MenuCalls = row["ml_calls"].ToString();
                        navMenu.MenuCanSee = row["ml_cansee"].ToString();
                        navMenu.MenuType = Convert.ToBoolean(row["ml_type"]);
                        navMenu.MenuImageNo = Convert.ToInt32(row["ml_imageno"]);
                        navMenu.Level4 = Convert.ToInt32(row["ml_lvl4_sequence"]);
                        navMenu.MenuDescription = row["ml_menu_desc"].ToString();

                        navMenus.Add(navMenu);
                    }
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }

            return navMenus;
        }

        public List<EmployeeTech> GetEmployeeTech(string CompanyID, string CompanyPassword, decimal CustomerID, string CustomerPassword, int Level4ID)
        {
            DataSet ds;
            EmployeeTech emp = new EmployeeTech();
            List<EmployeeTech> emps = new List<EmployeeTech>();
            StrongTypesNS.ds_emmstrDataSet dsEmployee;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.ws_getemmstr(Level4ID, out dsEmployee);
                ds = (DataSet)dsEmployee;

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        emp = new EmployeeTech();

                        emp.Code = row["emm_code"].ToString();
                        emp.FirstName = row["emm_fname"].ToString();
                        emp.LastName = row["emm_lname"].ToString();

                        emps.Add(emp);
                    }
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }

            return emps;
        }

        private Connection GetConnection(string CompanyID, string CompanyPassword, string CustomerPassword)
        {
            string appServerURL = ConfigurationManager.AppSettings["AppServerURL"];
            string appServerInfo = CompanyID + (char)1 + CustomerPassword + (char)1 + "" + (char)1;
            Connection conn = new Connection(appServerURL, CompanyID, CompanyPassword, appServerInfo);

            return conn;
        }
    }
}
