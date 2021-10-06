using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DTOModel
{
    public class QRCodeProblemDTO
    {
        public long QRProbId { get; set; }
        public long TransferId { get; set; }
        [StringLength(100)]
        public string TransferCode { get; set; }
        public long FloorId { get; set; }
        public long QCLineId { get; set; }
        public long? SubQCId { get; set; }
        public long RepairLineId { get; set; }
        [StringLength(100)]
        public string QRCode { get; set; }

        public long AssemblyLineId { get; set; }
        public long DescriptionId { get; set; }
        public long ProblemId { get; set; }
        [StringLength(250)]
        public string ProblemName { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long QRTRInfoId { get; set; }

        // Custom Properties
        [StringLength(100)]
        public string FloorName { get; set; }
        [StringLength(100)]
        public string QCLineName { get; set; }
        [StringLength(100)]
        public string RepairLineName { get; set; }
        [StringLength(100)]
        public string AssemblyLineName { get; set; }
        [StringLength(100)]
        public string ModelName { get; set; }
        [StringLength(50)]
        public string EntryUser { get; set; }
        [StringLength(50)]
        public string UpdateUser { get; set; }
        public string QCLineNo { get; set; }
        public string SubQCName { get; set; }
    }
}
