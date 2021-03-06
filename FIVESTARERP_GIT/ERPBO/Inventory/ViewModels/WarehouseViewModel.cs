using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.ViewModels
{
    public class WarehouseViewModel
    {
        public long Id { get; set; }
        [StringLength(100),Required]
        public string WarehouseName { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public long OrganizationId { get; set; }
        [StringLength(50)]
        public string EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [StringLength(50)]
        public string UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        // Custom Property
        [StringLength(10)]
        public string StateStatus { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
