using ERPBO.SalesAndDistribution.CommonModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.SalesAndDistribution.ViewModels
{
    public class SalesRepresentativeViewModel
    {
        public long SRID { get; set; }
        [StringLength(150)]
        public string FullName { get; set; }
        [StringLength(100)]
        public string SRType { get; set; }
        public long DivisionId { get; set; }
        public long DistrictId { get; set; }
        public long ZoneId { get; set; }
        public long? ReportingSRId { get; set; }
        public bool IsAllowToLogIn { get; set; }
        [StringLength(200)]
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public long BranchId { get; set; }
        [StringLength(50)]
        public string MobileNo { get; set; }
        [StringLength(200)]
        public string Address { get; set; }
        public long OrganizationId { get; set; }
        public long? UserId { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        // Custom Properties //
        public List<SRUser> SRUsers { get; set; }
        public string ZoneName { get; set; }
        public string DivisionName { get; set; }
        public string DistrictName { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
        public string ReportingSRName { get; set; }
    }
}
