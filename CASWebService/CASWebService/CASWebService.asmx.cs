using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Progress.Open4GL.Proxy;
using System.Data;
using System.Configuration;

namespace CASWebService
{
    /// <summary>
    /// Summary description for CASWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CASWebService : System.Web.Services.WebService
    {
        /// <summary>
        /// This method will return login status
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="CompanyPassword"></param>
        /// <param name="CustomerID"></param>
        /// <param name="CustomerPassword"></param>
        /// <returns>Login status in boolean</returns>
        [WebMethod]
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
        /// This method will return the dataset of customer information
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="Level4ID"></param>
        /// <param name="CustomerID"></param>
        /// <returns> Customer information in dataset</returns>
        [WebMethod]
        public DataSet LoginMessageOfDay(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, string CustomerID)
        {
            DataSet ds = new DataSet();
            StrongTypesNS.ds_companyinfodataDataSet comInfoDs;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_webcasmotd(Level4ID, CustomerID, out comInfoDs);
                ds = (DataSet)comInfoDs;

                return ds;
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
        [WebMethod]
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
        [WebMethod]
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
        [WebMethod]
        public DataSet GetPrivateFolderFiles(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, string CustomerID, string FolderName, string TimeZoneOffset)
        {
            DataSet ds;
            StrongTypesNS.ds_filelistDataSet stypefileList;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_webdbfilelist(Level4ID, CustomerID, FolderName, TimeZoneOffset, out stypefileList);
                ds = (DataSet)stypefileList;

                return ds;
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
        [WebMethod]
        public DataSet GetPublicFolderFiles(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, string FolderName, string TimeZoneOffset)
        {
            DataSet ds;
            StrongTypesNS.ds_filelistDataSet stypefileList;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_webpubdbfilelist(Level4ID, FolderName, TimeZoneOffset, out stypefileList);
                ds = (DataSet)stypefileList;

                return ds;
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
        [WebMethod]
        public DataSet GetPrivateFileInfo(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, string CustomerID, string FolderName, string FileName)
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

                return ds;
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
        [WebMethod]
        public DataSet GetPublicFileInfo(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, string FolderName, string FileName)
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

                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public DataSet GetBusinessTime(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID)
        {
            DataSet ds;
            StrongTypesNS.ds_company_hoursDataSet companyHours;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_webgetbustime(Level4ID, out companyHours);
                ds = (DataSet)companyHours;

                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public DataSet GetScheduledTime(string CompanyID, string CompanyPassword, string CustomerPassword, DateTime startDate)
        {
            DataSet ds;
            StrongTypesNS.ds_timeslot_hoursDataSet timeSlots;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_webgetschtime(startDate, out timeSlots);
                ds = (DataSet)timeSlots;

                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public DataSet GetCategoryProductService(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID)
        {
            DataSet ds;
            StrongTypesNS.ds_catproditemDataSet services;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_webgetcatprod(Level4ID, out services);
                ds = (DataSet)services;

                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public DataSet GetCustomerSite(string CompanyID, string CompanyPassword, string CustomerPassword, string CustomerID, int Level4ID)
        {
            DataSet ds;
            StrongTypesNS.ds_csmstrDataSet site;

            try
            {
                Connection conn = GetConnection(CompanyID, CompanyPassword, CustomerPassword);
                CustWebAccProj cus = new CustWebAccProj(conn);

                cus.p_webgetcsite(decimal.Parse(CustomerID), Level4ID, out site);
                ds = (DataSet)site;

                return ds;
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
