using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblRepairSectionFaultyItemTransferInfo")]
    public class RepairSectionFaultyItemTransferInfo
    {
        [Key]
        public long RSFIRInfoId { get; set; }
        [StringLength(100)]
        public string TransferCode { get; set; }
        public long ProductionFloorId { get; set; }
        [StringLength(100)]
        public string ProductionFloorName { get; set; }
        public long RepairLineId { get; set; }
        [StringLength(100)]
        public string RepairLineName { get; set; }
        [StringLength(150)]
        public string StateStatus { get; set; }
        public int TotalUnit { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public ICollection<RepairSectionFaultyItemTransferDetail> RepairSectionFaultyItemRequisitionDetails { get; set; }
    }
}
