﻿using CASWCFService.Model;
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
            DataSet dsItems;
            DataSet dsSites;
            Item item;
            Site site;
            List<Item> items = new List<Item>();
            List<Site> sites = new List<Site>();
            SiteNItem siteNitem = new SiteNItem();
            StrongTypesNS.ds_catproditemDataSet services;
            StrongTypesNS.ds_csmstrDataSet master;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_webgetcatprod(Level4ID, CustomerID, out services, out master);
                dsItems = (DataSet)services;
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

                if (dsItems != null && dsItems.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dsItems.Tables[0].Rows)
                    {
                        item = new Item();

                        item.CategoryName = row["pd_category"].ToString();
                        item.ProductName = row["pd_code"].ToString();
                        item.ItemName = row["pdl_prcode"].ToString();
                        item.ItemID = Convert.ToInt32(row["pdl_lineno"]);
                        item.Description = row["pdl_desc"].ToString();
                        item.Price = Convert.ToDouble(row["pdl_price"]);
                        item.Duration = Convert.ToInt32(row["pdl_duration"]);

                        items.Add(item);
                    }
                }
                else
                    return null;

                siteNitem.sites = sites;
                siteNitem.items = items;

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

        private Connection GetConnection(string CompanyID, string CompanyPassword, string CustomerPassword)
        {
            string appServerURL = ConfigurationManager.AppSettings["AppServerURL"];
            string appServerInfo = CompanyID + (char)1 + CustomerPassword + (char)1 + "" + (char)1;
            Connection conn = new Connection(appServerURL, CompanyID, CompanyPassword, appServerInfo);

            return conn;
        }
    }
}