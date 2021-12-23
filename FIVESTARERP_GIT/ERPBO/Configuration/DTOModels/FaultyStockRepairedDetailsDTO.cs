using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DTOModels
{
    public class FaultyStockRepairedDetailsDTO
    {
        public long FSRDetailsId { get; set; }
        public long ModelId { get; set; }
        public long PartsId { get; set; }
        public long TSId { get; set; }
        public string RefCode { get; set; }
        public int AssignQty { get; set; }
        public int RepairedQty { get; set; }
        public Nullable<DateTime> RepairedDate { get; set; }
        public int ScrapedQty { get; set; }
        public Nullable<DateTime> ScrapedDate { get; set; }
        public string Remarks { get; set; }
        public long BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long FSRInfoId { get; set; }
        public string PartsName { get; set; }
        public string PartsCode { get; set; }
        public string ModelName { get; set; }
    }
}
