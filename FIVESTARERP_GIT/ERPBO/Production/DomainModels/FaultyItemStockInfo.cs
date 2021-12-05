using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblFaultyItemStockInfo")]
    public class FaultyItemStockInfo
    {
        [Key]
        public long FaultyItemStockInfoId { get; set; }
        public long? ProductionFloorId { get; set; }
        public long? DescriptionId { get; set; }
        public long? AsseemblyLineId { get; set; }
        public long? QCId { get; set; }
        public long? RepairLineId { get; set; }
        public long? WarehouseId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public long? UnitId { get; set; }
        public int ChinaMadeFaultyStockInQty { get; set; }
        public int ChinaMadeFaultyStockOutQty { get; set; }
        public int ManMadeFaultyStockInQty { get; set; }
        public int ManMadeFaultyStockOutQty { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
    }
}
