using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPBO.Production.DTOModel
{
    public class ProductionToPackagingStockTransferDetailDTO
    {
        public long PTPSTDetailId { get; set; }
        [StringLength(100)]
        public string TransferCode { get; set; }
        public long FloorId { get; set; }
        public long PackagingLineId { get; set; }
        public long ModelId { get; set; }
        public long WarehouseId { get; set; }
        public long ItemTypeId { get; set; }
        public long ItemId { get; set; }
        public int Quantity { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long PTPSTInfoId { get; set; }

        // Custom Properties
        [StringLength(100)]
        public string FloorName { get; set; }
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
        public string EntryUser { get; set; }
        [StringLength(100)]
        public string UpdateUser { get; set; }

    }
}
