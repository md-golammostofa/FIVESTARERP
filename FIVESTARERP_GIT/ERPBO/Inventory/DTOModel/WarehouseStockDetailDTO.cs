using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ERPBO.Inventory.DTOModel
{
   public class WarehouseStockDetailDTO
    {
        public long StockDetailId { get; set; }
        [Range(1, long.MaxValue)]
        public long? WarehouseId { get; set; }
        [Range(1, long.MaxValue)]
        public long? DescriptionId { get; set; }
        [Range(1, long.MaxValue)]
        public long? ItemTypeId { get; set; }
        [Range(1, long.MaxValue)]
        public long? ItemId { get; set; }
        //[Range(1, long.MaxValue)]
        public long? UnitId { get; set; }
        [Range(1, long.MaxValue)]
        public int Quantity { get; set; }
        public int GoodStockQty { get; set; }
        public int ManMadeFaultyQty { get; set; }
        public int ChinaFaultyQty { get; set; }
        public int OrderQty { get; set; }
        public long? SupplierId { get; set; }
        public Nullable<DateTime> ExpireDate { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        [Range(1, long.MaxValue)]
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [Range(1, long.MaxValue)]
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long StockInfoId { get; set; }

        //Custom Pop
        [StringLength(100)]
        public string Warehouse { get; set; }
        [StringLength(100)]
        public string ModelName { get; set; }
        [StringLength(100)]
        public string ItemType { get; set; }
        [StringLength(100)]
        public string Item { get; set; }
        [StringLength(100)]
        public string Unit { get; set; }
        [StringLength(150)]
        public string RefferenceNumber { get; set; }
        [StringLength(150)]
        public string StockStatus { get; set; }
        [StringLength(150)]
        public string SupplierName { get; set; }
        public HttpPostedFileBase FileUpload { get; set; }
        public string BomItemId { get; set; }
        public int? ConsumptionQty { get; set; }
        public string BomType { get; set; }
    }
}
