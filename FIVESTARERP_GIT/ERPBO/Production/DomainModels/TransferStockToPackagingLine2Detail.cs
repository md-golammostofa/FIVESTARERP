﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblTransferStockToPackagingLine2Detail")]
    public class TransferStockToPackagingLine2Detail
    {
        [Key]
        public long TP2DetailId { get; set; }
        public long? WarehouseId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public long? UnitId { get; set; }
        public int Quantity { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        [ForeignKey("TransferStockToPackagingLine2Info")]
        public long TP2InfoId { get; set; }
        public TransferStockToPackagingLine2Info TransferStockToPackagingLine2Info { get; set; }
    }
}
