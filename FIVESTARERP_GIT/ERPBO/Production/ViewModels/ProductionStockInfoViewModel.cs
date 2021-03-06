using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
   public class ProductionStockInfoViewModel
    {
        public long StockInfoId { get; set; }
        [Range(1, long.MaxValue)]
        public long? LineId { get; set; }
        [Range(1,long.MaxValue)]
        public long? WarehouseId { get; set; }
        [Range(1, long.MaxValue)]
        public long? ItemTypeId { get; set; }
        [Range(1, long.MaxValue)]
        public long? ItemId { get; set; }
        [Range(1, long.MaxValue)]
        public long? UnitId { get; set; }
        public int? StockInQty { get; set; }
        public int? StockOutQty { get; set; }
        public long? DescriptionId { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        [Range(1, long.MaxValue)]
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [Range(1, long.MaxValue)]
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [StringLength(100)]
        public string StockFor { get; set; }

        //Custom Pop
        [StringLength(100)]
        public string Warehouse { get; set; }
        [StringLength(100)]
        public string ItemType { get; set; }
        [StringLength(100)]
        public string Item { get; set; }
        [StringLength(100)]
        public string Unit { get; set; }
        [StringLength(100)]
        public string LineNumber { get; set; }
        [StringLength(100)]
        public string ModelName { get; set; }
    }
}
