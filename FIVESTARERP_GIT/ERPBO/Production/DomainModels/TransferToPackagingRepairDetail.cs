using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblTransferToPackagingRepairDetail")]
    public class TransferToPackagingRepairDetail
    {
        [Key]
        public long TPRDetailId { get; set; }
        public long ProductionFloorId { get; set; }
        public long PackagingLineId { get; set; }
        public long DescriptionId { get; set; }
        public long WarehouseId { get; set; }
        public long ItemTypeId { get; set; }
        public long ItemId { get; set; }
        public long? UnitId { get; set; }
        public int Quantity { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [StringLength(100)]
        public string TransferCode { get; set; }
        [ForeignKey("TransferToPackagingRepairInfo")]
        public long TPRInfoId { get; set; }
        public TransferToPackagingRepairInfo TransferToPackagingRepairInfo { get; set; }
    }
}
