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

        public List<ChartData> GetTrendAnalysisByEquipment(int siteNo, int contractNo, DataTable answers, string area, int frequency, DateTime dtFrom, DateTime dtTo, int groupBy, bool sortBy, bool exclude)
        {
            ReportParser parser = new ReportParser();
            List<ChartData> charts = new List<ChartData>();

            charts = parser.GetTrendAnalysisByEquipment(siteNo, contractNo, answers, area, frequency, dtFrom, dtTo, groupBy, sortBy, exclude);

            return charts;
        }

        public Equipment GetEquipmentTransactionBLOB(DateTime dateFrom, DateTime dateTo, bool isPrintDetails, bool isPrintMaterials, int selection, string assignedTo, int sorting, int contractFrom, int contractTo, bool isInactive, bool isShowTime, string glAssignedTo)
        {
            Equipment equip = new Equipment();
            ReportParser parser = new ReportParser();

            equip = parser.GetEquipmentTransactionBLOB(dateFrom, dateTo, isPrintDetails, isPrintMaterials, selection, assignedTo, sorting, contractFrom, contractTo, isInactive, isShowTime, glAssignedTo);

            return equip;
        }

        public List<Equipment> GetInstalledEquipment(int contractNo, string equipmentType)
        {
            ReportParser parser = new ReportParser();
            List<Equipment> equips = new List<Equipment>();

            equips = parser.GetInstalledEquipment(contractNo, equipmentType);

            return equips;
        }

        public Equipment GetEquipmentReportBLOB(int contractFrom, int contractTo, int sorting, int status)
        {
            Equipment equip = new Equipment();
            ReportParser parser = new ReportParser();

            equip = parser.GetEquipmentReportBLOB(contractFrom, contractTo, sorting, status);

            return equip;
        }
    }
}