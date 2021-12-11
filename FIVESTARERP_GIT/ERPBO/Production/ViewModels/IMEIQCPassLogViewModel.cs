using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
    public class IMEIQCPassLogViewModel
    {
        public long IMEIQCPassLogId { get; set; }
        public string IMEI { get; set; }
        public long? CodeId { get; set; }
        public string CodeNo { get; set; }
        public long? ProductionFloorId { get; set; }
        public string ProductionFloorName { get; set; }
        public long? AssemblyId { get; set; }
        public string AssemblyLineName { get; set; }
        public long? PackagingLineId { get; set; }
        public string PackagingLineName { get; set; }
        public long? QCLineId { get; set; }
        public string QCLineName { get; set; }
        public long? DescriptionId { get; set; }
        public string ModelName { get; set; }
        public long? ColorId { get; set; }
        public string ColorName { get; set; }
        public long? WarehouseId { get; set; }
        public long? ItemTypeId { get; set; }
        public long? ItemId { get; set; }
        public string ItemName { get; set; }
        public string StateStatus { get; set; }
        public string ReferenceNumber { get; set; }
        public string ReferenceId { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
    }
}
