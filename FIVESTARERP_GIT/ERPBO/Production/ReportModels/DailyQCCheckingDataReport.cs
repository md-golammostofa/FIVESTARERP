using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ReportModels
{
    public class DailyQCCheckingDataReport
    {
        public Nullable<DateTime> EntryDate { get; set; }
        public string ModelName { get; set; }
        public string AssemblyLineName { get; set; }
        public int Input { get; set; }
        public int TargetQty { get; set; }
        public int QCPass { get; set; }
        public int QC1 { get; set; }
        public int QC2 { get; set; }
        public int QC3 { get; set; }
        public long ProblemId { get; set; }
        public string ProblemName { get; set; }
        public int Qty { get; set; }
        public string Remarks { get; set; }
        public string QCPassRate { get; set; }
        public string QCFailRate { get; set; }
    }
}
