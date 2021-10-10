using ERPBO.Configuration.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.ViewModels
{
    public class FaultyStockTransferInfoViewModel
    {
        public long FSTInfoId { get; set; }
        public string TransferCode { get; set; }
        public string StateStatus { get; set; }
        public long BranchFrom { get; set; }
        public long BranchTo { get; set; }
        public string Remarks { get; set; }
        public long BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public IEnumerable<FaultyStockTransferDetails> faultyStockTransferDetails { get; set; }
        //
        public string BranchName { get; set; }
        public string UserName { get; set; }
    }
}
