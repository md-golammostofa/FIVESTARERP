using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class MiniStockTransferToWarehouseInfoDTO
    {
        public long MSTWInfoId { get; set; }
        public string TransferCode { get; set; }
        public long? FloorId { get; set; }
        public long? AssemblyLineId { get; set; }
        public long? QCLine { get; set; }
        public long? DescriptionId { get; set; }
        public long? RepairLineId { get; set; }
        public long? WarehouseId { get; set; }
        public string TransferStatus { get; set; }
        public string ReturnStatus { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public IEnumerable<MiniStockTransferToWarehouseDetails> miniStockTransferToWarehouseDetails { get; set; }
    }
}
