using CASPortal.CASWCFService;
using CASPortal.WebParser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CASPortal.Repository
{
    public class ReportRepository
    {
        public List<Contract> GetContracts(string siteNo)
        {
            ReportParser parser = new ReportParser();
            List<Contract> contracts = new List<Contract>();

            contracts = parser.GetContracts(siteNo);

            return contracts;
        }

        public TreeNode GetTrendAnalysisTreeNodes()
        {
            TreeNode treeNode;
            ReportParser parser = new ReportParser();

            treeNode = parser.GetTrendAnalysisTreeNodes();

            return treeNode;
        }

        public List<ChartData> GetTrendAnalysisByJob(int siteNo, int contractNo, DataTable answers, string area, DateTime dtFrom, DateTime dtTo)
        {
            ReportParser parser = new ReportParser();
            List<ChartData> charts = new List<ChartData>();

            charts = parser.GetTrendAnalysisByJob(siteNo, contractNo, answers, area, dtFrom, dtTo);

            return charts;
        }

        public List<ChartData> GetTrendAnalysisByQuestion(int siteNo, int contractNo, DataTable answers, string area, int frequency, DateTime dtFrom, DateTime dtTo, int groupBy)
        {
            ReportParser parser = new ReportParser();
            List<ChartData> charts = new List<ChartData>();

            charts = parser.GetTrendAnalysisByQuestion(siteNo, contractNo, answers, area, frequency, dtFrom, dtTo, groupBy);

            return charts;
        }

        public byte[] GetEquipmentTransactionBLOB(DateTime dateFrom, DateTime dateTo, bool isPrintDetails, bool isPrintMaterials, int selection, string assignedTo, int sorting, int contractFrom, int contractTo, bool isInactive, bool isShowTime, string glAssignedTo)
        {
            byte[] fileInfo = null;
            ReportParser parser = new ReportParser();

            fileInfo = parser.GetEquipmentTransactionBLOB(dateFrom, dateTo, isPrintDetails, isPrintMaterials, selection, assignedTo, sorting, contractFrom, contractTo, isInactive, isShowTime, glAssignedTo);

            return fileInfo;
        }

        public List<Equipment> GetInstalledEquipment(int contractNo, string equipmentType)
        {
            ReportParser parser = new ReportParser();
            List<Equipment> equips = new List<Equipment>();

            equips = parser.GetInstalledEquipment(contractNo, equipmentType);

            return equips;
        }

        public byte[] GetEquipmentReportBLOB(int contractFrom, int contractTo, int sorting, int status)
        {
            byte[] fileInfo = null;
            ReportParser parser = new ReportParser();

            fileInfo = parser.GetEquipmentReportBLOB(contractFrom, contractTo, sorting, status);

            return fileInfo;
        }
    }
}