using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DTOModels
{
    public class RequsitionDetailsReportDTO
    {
        public string RequsitionCode { get; set; }
        public string JobOrderCode { get; set; }
        public long DescriptionId { get; set; }
        public string ModelName { get; set; }
        public string IMEI { get; set; }
        public string CustomerName { get; set; }
        public string Problems { get; set; }
        public string EngProblems { get; set; }
        public string PartsName { get; set; }
        public DateTime ReceiveDate { get; set; }
        public DateTime RequsionDate { get; set; }
        public string UserName { get; set; }
        public string Remarks { get; set; }
    }
}
