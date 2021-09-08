﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblFaultyItemStockDetail")]
    public class FaultyItemStockDetail
    {
        [Key]
        public long FaultyItemStockDetailId { get; set; }
        public long? ProductionFloorId { get; set; }
        public long? AsseemblyLineId { get; set; }
        public long? QCId { get; set; }
        public long? RepairLineId { get; set; }
        public long? DescriptionId { get; set; }
        public long? WarehouseId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public long? UnitId { get; set; }
        public bool IsChinaFaulty { get; set; }
        public int Quantity { get; set; }
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

        // 17-July-2020
        public long? TransferId { get; set; }
        [StringLength(150)]
        public string TransferCode { get; set; }
    }
}
