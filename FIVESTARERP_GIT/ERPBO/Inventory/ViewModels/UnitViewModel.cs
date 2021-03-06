using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.ViewModels
{
   public class UnitViewModel
    {
        public long UnitId { get; set; }
        [Required,StringLength(100)]
        public string UnitName { get; set; }
        [StringLength(50)]
        public string UnitSymbol { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
