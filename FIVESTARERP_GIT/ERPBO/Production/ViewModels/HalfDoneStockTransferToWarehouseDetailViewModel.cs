﻿using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class HalfDoneStockTransferToWarehouseDetailViewModel
    {
        public long HalfDoneTransferDetailId { get; set; }
        public long? ProductionFloorId { get; set; }
        public long? AssemblyLineId { get; set; }
        public long? QCId { get; set; }
        public long? DescriptionId { get; set; }
        public long? RepairLineId { get; set; }
        public long? WarehouseId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public int Quantity { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long HalfDoneTransferInfoId { get; set; }
        //Custom
        public string FloorName { get; set; }
        public string AssemblyLineName { get; set; }
        public string QCName { get; set; }
        public string RepairLineName { get; set; }
        public string ModelName { get; set; }
        public string WarehouseName { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemName { get; set; }
        public string EntryUser { get; set; }
    }
}
