using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.DomainModels
{
    [Table("tblWarehouseStockDetails")]
    public class WarehouseStockDetail
    {
        [Key]
        public long StockDetailId { get; set; }
        public long? WarehouseId { get; set; }
        public long? DescriptionId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public long? UnitId { get; set; }
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
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        [StringLength(150)]
        public string StockStatus { get; set; }

        [StringLength(150)]
        public string RefferenceNumber { get; set; }
        public long StockInfoId { get; set; }
    }
}
