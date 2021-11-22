using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class MiniStockRequisitionToSemiFinishGoodsWarehouseInfoDTO
    {
        public long RequisitionInfoId { get; set; }
        public string RequisitionCode { get; set; }
        public int TotalQuantity { get; set; }
        public string StateStatus { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        //Custom
        public string EntryUser { get; set; }
    }
}
