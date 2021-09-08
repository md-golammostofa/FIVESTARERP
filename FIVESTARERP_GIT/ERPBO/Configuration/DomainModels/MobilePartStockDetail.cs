﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DomainModels
{
    [Table("tblMobilePartStockDetails")]
   public class MobilePartStockDetail
    {
        [Key]
        public long MobilePartStockDetailId { get; set; }
        public long? MobilePartId { get; set; }
        public long? SWarehouseId { get; set; }
        public int Quantity { get; set; }
        public string StockStatus { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long BranchId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long? DescriptionId { get; set; }
        //
        public long? BranchFrom { get; set; }
        public string ReferrenceNumber { get; set; }

        //
        public double CostPrice { get; set; }
        public double SellPrice { get; set; }
    }
}
