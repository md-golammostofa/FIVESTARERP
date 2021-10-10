using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DomainModels
{
    [Table("tblFaultyStockTransferInfo")]
    public class FaultyStockTransferInfo
    {
        [Key]
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
        public List<FaultyStockTransferDetails> faultyStockTransferDetails { get; set; }
    }
}
