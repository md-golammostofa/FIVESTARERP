using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.DomainModels
{
    [Table("tblHalfDoneWarehouseStockDetail")]
    public class HalfDoneWarehouseStockDetail
    {
        [Key]
        public long HalfDoneStockDetailId { get; set; }
        public long? ProductionFloorId { get; set; }
        public long? AssemblyLineId { get; set; }
        public long? QCId { get; set; }
        public long? DescriptionId { get; set; }
        public long? RepairLineId { get; set; }
        public long? WarehouseId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public int Quantity { get; set; }
        public string StockStatus { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long HalfDoneStockInfoId { get; set; }
    }
}
