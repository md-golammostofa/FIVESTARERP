using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.ViewModels
{
    public class DustStockDetailsViewModel
    {
        public long DetailsId { get; set; }
        public long ModelId { get; set; }
        public long PartsId { get; set; }
        public int Quantity { get; set; }
        public string StateStatus { get; set; }
        public string Remarks { get; set; }
        public long BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        //
        public string ModelName { get; set; }
        public string PartsName { get; set; }
        public string PartsCode { get; set; }
    }
}
