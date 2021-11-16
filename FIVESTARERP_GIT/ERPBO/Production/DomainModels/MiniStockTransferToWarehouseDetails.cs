﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblMiniStockTransferToWarehouseDetails")]
    public class MiniStockTransferToWarehouseDetails
    {
        [Key]
        public long MSTWDetailsId { get; set; }
        public long? FloorId { get; set; }
        public long? AssemblyLineId { get; set; }
        public long? QCLine { get; set; }
        public long? DescriptionId { get; set; }
        public long? RepairLineId { get; set; }
        public long? WarehouseId { get; set; }
        public int Quantity { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        [ForeignKey("MiniStockTransferToWarehouseInfo")]
        public long MSTWInfoId { get; set; }
        public MiniStockTransferToWarehouseInfo MiniStockTransferToWarehouseInfo { get; set; }
    }
}
