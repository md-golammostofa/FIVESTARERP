using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblHalfDoneStockTransferToWarehouseInfo")]
    public class HalfDoneStockTransferToWarehouseInfo
    {
        [Key]
        public long HalfDoneTransferInfoId { get; set; }
        public string TransferCode { get; set; }
        public string StateStatus { get; set; }
        public int TotalQuantity { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
    }
}
