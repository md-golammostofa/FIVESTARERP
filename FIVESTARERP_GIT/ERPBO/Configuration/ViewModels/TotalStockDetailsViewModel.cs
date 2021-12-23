using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.ViewModels
{
    public class TotalStockDetailsViewModel
    {
        public long DescriptionId { get; set; }
        public long MobilePartId { get; set; }
        public string ModelName { get; set; }
        public string PartsName { get; set; }
        public string PartsCode { get; set; }
        public int Stock { get; set; }
        public int GoodStock { get; set; }
        public int FaultyStock { get; set; }
        public int ScrapStock { get; set; }
        public int CareTransfer { get; set; }
        public int DustStock { get; set; }
        public int EngPending { get; set; }
        public int TransferAModel { get; set; }
        public int ReceiveAModel { get; set; }
        public int ParsesStock { get; set; }
        public int Sales { get; set; }
    }
}
