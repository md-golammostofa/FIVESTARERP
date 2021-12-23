using ERPBO.Configuration.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DTOModels
{
    public class FaultyStockRepairedInfoDTO
    {
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
        //
        public string TSName { get; set; }
    }
}
