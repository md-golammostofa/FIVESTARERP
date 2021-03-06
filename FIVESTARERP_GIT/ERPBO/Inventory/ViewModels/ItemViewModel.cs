using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.ViewModels
{
   public class ItemViewModel
    {
        public long ItemId { get; set; }
        [Required,StringLength(100)]
        public string ItemName { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        [StringLength(20)]
        public string ItemCode { get; set; }
        public bool IsActive { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }


        // Navigation Property
        [Range(1, long.MaxValue)]
        public long ItemTypeId { get; set; }
        [StringLength(100)]
        public string ItemTypeName { get; set; }
        [Range(1, long.MaxValue)]
        public long UnitId { get; set; }
        [StringLength(100)]
        public string UnitName { get; set; }

        //Custom Property
        [StringLength(10)]
        public string StateStatus { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }

        public long? DescriptionId { get; set; }
        public string ModelName { get; set; }
        public long? ColorId { get; set; }
        public string ColorName { get; set; }

    }
}
