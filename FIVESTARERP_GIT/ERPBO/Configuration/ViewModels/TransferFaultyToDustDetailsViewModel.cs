using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.ViewModels
{
    public class TransferFaultyToDustDetailsViewModel
    {
        public long DetailsId { get; set; }
        public long ModelId { get; set; }
        public long PartsId { get; set; }
        public double CostPrice { get; set; }
        public double SellPrice { get; set; }
        public int Quantity { get; set; }
        public string RefCode { get; set; }
        public string Remarks { get; set; }
        public long BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        //
        public long InfoId { get; set; }
        public string ModelName { get; set; }
        public string PartsName { get; set; }
    }
}
