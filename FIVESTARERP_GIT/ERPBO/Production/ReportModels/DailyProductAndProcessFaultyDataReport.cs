using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ReportModels
{
    public class DailyProductAndProcessFaultyDataReport
    {
        public Nullable<DateTime> EntryDate { get; set; }
        public string DescriptionName { get; set; }
        public int TargetQty { get; set; }
        public int TotalProduction { get; set; }
        public string ItemName { get; set; }
        public int TotalFaulty { get; set; }
        public string TotalFaultyRate { get; set; }
        public int ProductFaulty { get; set; }
        public string ProductFaultyRate { get; set; }
        public int ProcessFaulty { get; set; }
        public string ProcessFaultyRate { get; set; }
        public string Remarks { get; set; }
    }
}
