using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DomainModels
{
    [Table("tblFaultyStockRepairedInfo")]
   public class FaultyStockRepairedInfo
    {
        [Key]
        public long FSRInfoId { get; set; }
        public long TSId { get; set; }
        public string Code { get; set; }
        public string StateStatus { get; set; }
        public Nullable<DateTime> AssignDate { get; set; }
        public Nullable<DateTime> RepairedDate { get; set; }
        public Nullable<DateTime> ReceiveDate { get; set; }
        public long? ReceiveUserId { get; set; }
        public string Remarks { get; set; }
        public long BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public ICollection<FaultyStockRepairedDetails> faultyStockRepairedDetails { get; set; }
    }
}
