using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
   [Table("tblProductionLines")]
   public class ProductionLine
    {
        [Key]
        public long LineId { get; set; }
        [StringLength(100)]
        public string LineNumber { get; set; }
        [StringLength(100)]
        public string LineIncharge { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public ICollection<AssemblyLine> AssemblyLines { get; set; }
        public ICollection<QualityControl> QualityControls { get; set; }
        public ICollection<RepairLine> RepairLines { get; set; }
        public ICollection<PackagingLine> PackagingLines { get; set; }
    }
}
