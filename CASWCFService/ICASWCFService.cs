using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Progress.Open4GL.Proxy;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Configuration;
using CASWCFService.Model;

namespace CASWCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICASWCFService" in both code and config file together.
    [ServiceContract]
    public interface ICASWCFService
    {
        [OperationContract]
        bool LoginProcess(string CompanyID, string CompanyPassword, string CustomerID, string CustomerPassword, out int lvl4id);

        [OperationContract]
        Welcome GetCASDefaultData(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, string CustomerID);

        [OperationContract]
        List<Welcome> LoginMessageOfDay(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, string CustomerID);

        [OperationContract]
        string GetPrivateFolders(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, string CustomerID);

        [OperationContract]
        string GetPublicFolders(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID);

        [OperationContract]
        List<Folder> GetPrivateFolderFiles(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, string CustomerID, string FolderName, string TimeZoneOffset);

        [OperationContract]
        List<Folder> GetPublicFolderFiles(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, string FolderName, string TimeZoneOffset);

        [OperationContract]
        byte[] GetPrivateFileInfo(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, string CustomerID, string FolderName, string FileName);

        [OperationContract]
        byte[] GetPublicFileInfo(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, string FolderName, string FileName);

        [OperationContract]
        List<DayHour> GetBusinessTime(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID);

        [OperationContract]
        List<TimeSlot> GetScheduledTime(string CompanyID, string CompanyPassword, string CustomerPassword, DateTime startDate);

        [OperationContract]
        SiteNItem GetCategoryProductService(string CompanyID, string CompanyPassword, decimal CustomerID, string CustomerPassword, int Level4ID);

        [OperationContract]
        List<Site> GetCustomerSite(string CompanyID, string CompanyPassword, string CustomerPassword, string CustomerID, int Level4ID);

        [OperationContract]
        TreeNode GetTrendAnalysisTreeNodes(string CompanyID, string CompanyPassword, decimal CustomerID, string CustomerPassword, int Level4ID);

        [OperationContract]
        List<ChartData> PostTrendAnalysisReportData(string CompanyID, string CompanyPassword, decimal CustomerID, string CustomerPassword, int Level4ID, int SiteNo, int ContractNo, DataTable answers, string Area, int Frequency, DateTime FromDate, DateTime ToDate, int GroupBy);
    }
}
