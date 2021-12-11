using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class DashboardAssemblyLineWiseDataDTO
    {
        public string AssemblyLineName { get; set; }
        public int TotalLotIn { get; set; }
        public int TargetQty { get; set; }
        public int TotalQCPass { get; set; }
        public int TotalQC1 { get; set; }
        public int TotalQC2 { get; set; }
        public int TotalQC3 { get; set; }
        public int TotalQCFail { get; set; }
        public int TotalRepairDone { get; set; }
        public int TotalHandset { get; set; }
        public int MiniStockReceivedQty { get; set; }
    }
    public class PackegingLineWiseDashboardDataDTO
    {
        public string PackegingLineName { get; set; }
        public int TotalIMEIWrite { get; set; }
        public int TotalHandset { get; set; }
        public int TotalQCPass { get; set; }
        public int TotalBatteryWrite { get; set; }
        public int TotalQCFail { get; set; }
        public int TotalRepairDone { get; set; }
        public int TotalCarton { get; set; }
    }
}
