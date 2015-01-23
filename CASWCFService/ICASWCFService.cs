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
        List<ChartData> GetTrendAnalysis(string CompanyID, string CompanyPassword, decimal CustomerID, string CustomerPassword, int Level4ID, int SiteNo, int ContractNo, DataTable answers, string Area, int Frequency, DateTime FromDate, DateTime ToDate, int GroupBy, bool IsUseDate);

        [OperationContract]
        List<ChartData> GetTrendAnalysisByJob(string CompanyID, string CompanyPassword, decimal CustomerID, string CustomerPassword, int Level4ID, int SiteNo, int ContractNo, DataTable answers, string Area, DateTime FromDate, DateTime ToDate);

        [OperationContract]
        List<ChartData> GetTrendAnalysisByQuestion(string CompanyID, string CompanyPassword, decimal CustomerID, string CustomerPassword, int Level4ID, int SiteNo, int ContractNo, DataTable answers, string Area, int Frequency, DateTime FromDate, DateTime ToDate, int GroupBy);

        [OperationContract]
        List<ChartData> GetTrendAnalysisByEquipment(string CompanyID, string CompanyPassword, decimal CustomerID, string CustomerPassword, int Level4ID, int SiteNo, int ContractNo, DataTable answers, string Area, int Frequency, DateTime FromDate, DateTime ToDate, int GroupBy, bool SortBy, bool IsExclude);

        [OperationContract]
        List<Contract> GetContracts(string CompanyID, string CompanyPassword, string CustomerPassword, decimal CustomerID, string SiteNo, int Level4ID);

        [OperationContract]
        Equipment GetEquipmentTransactionBLOB(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, decimal CustomerIDFrom, decimal CustomerIDTo, DateTime DateFrom, DateTime DateTo, bool IsPrintDetails, bool IsPrintMaterials, int Selection, string AssignedTo, int Sorting, int ContractFrom, int ContractTo, bool IsInactive, bool IsShowTime, string GlAssignedTo);

        [OperationContract]
        List<Equipment> GetInstalledEquipment(string CompanyID, string CompanyPassword, decimal CustomerID, string CustomerPassword, int Level4ID, int ContractNo, string EquipmentType);

        [OperationContract]
        Equipment GetEquipmentReportBLOB(string CompanyID, string CompanyPassword, string CustomerPassword, int Level4ID, decimal CustomerIDFrom, decimal CustomerIDTo, int ContractFrom, int ContractTo, int Sorting, int Status);

        [OperationContract]
        List<NavigationMenu> GetNavigationMenu(string CompanyID, string CompanyPassword, decimal CustomerID, string CustomerPassword, int Level4ID, string RootMenu);
    }
}
