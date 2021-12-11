using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ReportModels
{
    public class BarChatTimeWiseData
    {
        public string Hour { get; set; }
        public int QCPassCount { get; set; }
        public int LotInCount { get; set; }
        public int RepairInCount { get; set; }
        public int RepairOutCount { get; set; }
        public int IMEIInCount { get; set; }
        public int IMEIOutCount { get; set; }
    }
    public class QCPassAndLotInHourSameData
    {
        public string Hour { get; set; }
        public int Count { get; set; }
    }
    public class RepairInAndRepairOutHourSameData
    {
        public string Hour { get; set; }
        public int Count { get; set; }
    }
}
