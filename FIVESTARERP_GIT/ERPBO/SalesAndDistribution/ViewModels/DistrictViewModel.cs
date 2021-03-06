using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.SalesAndDistribution.ViewModels
{
    public class DistrictViewModel
    {
        public long DistrictId { get; set; }
        [Required,StringLength(100)]
        public string DistrictName { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public long? BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long DivisionId { get; set; }
        // Customer property
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
        public string DivisionName { get; set; }
    }
}
