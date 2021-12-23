using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DomainModels
{
    [Table("tblWarehouseToFactoryReturnInfo")]
    public class WarehouseToFactoryReturnInfo
    {
        [Key]
        public long WTFInfoId { get; set; }
        public string ReturnCode { get; set; }
        public string StateStatus { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long BranchId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public ICollection<WarehouseToFactoryReturnDetails> warehouseToFactoryReturnDetails { get; set; }
    }
}
