using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class PackagingRepairItemStockInfoViewModel
    {
        public long PRIStockInfoId { get; set; }
        public long? FloorId { get; set; }
        public long? PackagingLineId { get; set; }
        public long? WarehouseId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public long? UnitId { get; set; }
        public int? StockInQty { get; set; }
        public int? StockOutQty { get; set; }
        public long? DescriptionId { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        // Custom Properties //
        [StringLength(100)]
        public string ProductionFloorName { get; set; }
        [StringLength(100)]
        public string PackagingLineName { get; set; }
        [StringLength(100)]
        public string ModelName { get; set; }

        [StringLength(100)]
        public string WarehouseName { get; set; }
        [StringLength(100)]
        public string ItemTypeName { get; set; }
        [StringLength(100)]
        public string ItemName { get; set; }
        [StringLength(100)]
        public string UnitName { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
