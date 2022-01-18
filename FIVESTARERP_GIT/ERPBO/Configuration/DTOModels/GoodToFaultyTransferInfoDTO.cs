using ERPBO.Configuration.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DTOModels
{
    public class GoodToFaultyTransferInfoDTO
    {
        public long GTFTInfoId { get; set; }
        public string TransferCode { get; set; }
        public string TransferStatus { get; set; }
        public string Remarks { get; set; }
        public long BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public List<GoodToFaultyTransferDetails> goodToFaultyTransferDetails { get; set; }
        //
        public long WarehouseId { get; set; }
        public string UserName { get; set; }
    }
}
