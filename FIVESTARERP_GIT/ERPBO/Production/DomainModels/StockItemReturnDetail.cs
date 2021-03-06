using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblStockItemReturnDetail")]
    public class StockItemReturnDetail
    {
        [Key]
        public long SIRDetailId { get; set; }
        [StringLength(50)]
        public string ReturnCode { get; set; }
        public long DescriptionId { get; set; }
        public long ProductionFloorId { get; set; }
        public long? AssemblyLineId { get; set; }
        public long? RepairLineId { get; set; }
        public long? PackagingLineId { get; set; }
        public long WarehouseId { get; set; }
        public long ItemTypeId { get; set; }
        public long ItemId { get; set; }
        public long UnitId { get; set; }
        public int Quantity { get; set; }
        public int GoodStockQty { get; set; }
        public int ManMadeFaultyQty { get; set; }
        public int ChinaFaultyQty { get; set; }
        [StringLength(50)]
        public string Flag { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [ForeignKey("StockItemReturnInfo")]
        public long SIRInfoId { get; set; }
        public StockItemReturnInfo StockItemReturnInfo { get; set; }
    }
}
