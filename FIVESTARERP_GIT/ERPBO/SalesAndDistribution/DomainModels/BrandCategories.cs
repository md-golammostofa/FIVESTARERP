using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.SalesAndDistribution.DomainModels
{
    [Table("tblBrandCategories")]
    public class BrandCategories
    {
        [Key, Column(Order = 0)]
        public long BrandId { get; set; }
        public Brand Brand { get; set; }
        [Key, Column(Order = 1)]
        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public long OrganizationId { get; set; }
        public long BranchId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public  Nullable<DateTime> UpdateDate { get; set; }
    }
}
