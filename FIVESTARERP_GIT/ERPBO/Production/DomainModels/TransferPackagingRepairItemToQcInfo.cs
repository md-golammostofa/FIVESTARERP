using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblTransferPackagingRepairItemToQcInfo")]
    public class TransferPackagingRepairItemToQcInfo
    {
        [Key]
        public long TPRQInfoId { get; set; }
        [StringLength(100)]
        public string TransferCode { get; set; }
        public long DescriptionId { get; set; }
        public long FloorId { get; set; }
        public long PackagingLineId { get; set; }
        public long WarehouseId { get; set; }
        public long ItemTypeId { get; set; }
        public long ItemId { get; set; }
        public int Quantity { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public ICollection<TransferPackagingRepairItemToQcDetail> TransferPackagingRepairItemToQcDetails { get; set; }
    }

}
