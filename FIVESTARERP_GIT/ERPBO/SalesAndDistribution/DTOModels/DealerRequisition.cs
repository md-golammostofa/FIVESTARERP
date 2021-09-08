﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPBO.SalesAndDistribution.DTOModels
{
    public class DealerRequisitionInfoDTO
    {
        public long DREQInfoId { get; set; }
        public long DealerId { get; set; }
        [StringLength(100)]
        public string RequisitionCode { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        [StringLength(100)]
        public string Remarks { get; set; }
        public long? BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long? ApprovedBy { get; set; }
        public Nullable<DateTime> ApprovedDate { get; set; }
        public List<DealerRequisitionDetailDTO> DealerRequisitionDetails { get; set; }
        // Custom Properties
        public string DealerName { get; set; }
        public string DivisionName { get; set; }
        public string DistrictName { get; set; }
        public string ZoneName { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
        public string ApprovedUser { get; set; }
        public string Flag { get; set; }
        public long SRID { get; set; }
        public string SRName { get; set; }
    }
    public class DealerRequisitionDetailDTO
    {
        public long DREQDetailId { get; set; }
        public long CategoryId { get; set; }
        public long BrandId { get; set; }
        public long ModelId { get; set; }
        public long ColorId { get; set; }
        public int Quantity { get; set; }
        public int IssueQuantity { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long? ApprovedBy { get; set; }
        public Nullable<DateTime> ApprovedDate { get; set; }
        public long DREQInfoId { get; set; }
        // Custom Properties
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public string ColorName { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
        public string ApprovedUser { get; set; }
    }
}
