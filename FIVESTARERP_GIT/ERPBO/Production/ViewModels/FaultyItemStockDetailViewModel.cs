using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class FaultyItemStockDetailViewModel
    {
        public long FaultyItemStockDetailId { get; set; }
        [Range(1,long.MaxValue)]
        public long? ProductionFloorId { get; set; }
        //[Range(1, long.MaxValue)]
        public long? AsseemblyLineId { get; set; }
        [Range(1, long.MaxValue)]
        public long? QCId { get; set; }
        [Range(1, long.MaxValue)]
        public long? RepairLineId { get; set; }
        [Range(1, long.MaxValue)]
        public long? DescriptionId { get; set; }
        [Range(1, long.MaxValue)]
        public long? WarehouseId { get; set; }
        [Range(1, long.MaxValue)]
        public long? ItemTypeId { get; set; }
        [Range(1, long.MaxValue)]
        public long? ItemId { get; set; }
        public long? UnitId { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        public bool IsChinaFaulty { get; set; }
        public string StockStatus { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        [StringLength(100)]
        public string ReferenceNumber { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long? TransferId { get; set; }
        [StringLength(150)]
        public string TransferCode { get; set; }
        public long FaultyItemStockInfoId { get; set; }
        // Custom Property
        [StringLength(100)]
        public string ProductionFloorName { get; set; }
        [StringLength(100)]
        public string AssemblyLineName { get; set; }
        [StringLength(100)]
        public string ModelName { get; set; }
        [StringLength(100)]
        public string QCName { get; set; }
        [StringLength(100)]
        public string RepairName { get; set; }
        [StringLength(100)]
        public string WarehouseName { get; set; }
        [StringLength(100)]
        public string ItemTypeName { get; set; }
        [StringLength(100)]
        public string ItemName { get; set; }
        [StringLength(100)]
        public string UnitName { get; set; }
        public string FaultyReason { get; set; }
        public int ChinaReturnQty { get; set; }
        public int ManReturnQty { get; set; }
    }
}
