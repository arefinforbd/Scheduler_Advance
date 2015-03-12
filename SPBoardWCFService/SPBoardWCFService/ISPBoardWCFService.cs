using SPBoardWCFService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SPBoardWCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServiceBoardWCFService" in both code and config file together.
    [ServiceContract]
    public interface ISPBoardWCFService
    {
        [OperationContract]
        bool Login(string CompanyID, string CompanyPassword, out int Level4ID, out string Message);

        [OperationContract]
        List<ChartData> GetSalesAnalysis(string CompanyID, string CompanyPassword, int Level4ID, int ReportType);

        [OperationContract]
        List<ChartData> GetSalesAnalysisByCategorySum(string CompanyID, string CompanyPassword, int Level4ID, int ReportType);

        [OperationContract]
        List<ChartData> GetSalesAnalysisByCategoryDetail(string CompanyID, string CompanyPassword, int Level4ID, string Category, DateTime FromDate, DateTime ToDate);

        [OperationContract]
        List<ChartData> GetDebtorAnalysis(string CompanyID, string CompanyPassword, int Level4ID, string InvoiceType, decimal CustomerFrom, decimal CustomerTo, int SortBy, string Area, bool PrintCredit, string NameFrom, string NameTo, int AgeBal, bool UnIndJobs, DateTime BalanceDate, bool Retention);

        [OperationContract]
        List<Category> GetCategory(string CompanyID, string CompanyPassword, int Level4ID);

        [OperationContract]
        List<ResourceUtilization> GetResourceUtilization(string CompanyID, string CompanyPassword, int Level4ID, DateTime FromDate, DateTime ToDate);
    }
}
