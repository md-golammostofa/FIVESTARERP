using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblQC3Detail")]
    public class QC3Detail
    {
        [Key]
        public long QC3DetailId { get; set; }
        public long TransferId { get; set; }
        public string TransferCode { get; set; }
        public long FloorId { get; set; }
        public long AssemblyLineId { get; set; }
        public long QCLineId { get; set; }
        public long? SubQCId { get; set; }
        public long RepairLineId { get; set; }
        public string QRCode { get; set; }
        public long DescriptionId { get; set; }
        public long ProblemId { get; set; }
        public string ProblemName { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        [ForeignKey("QC3Info")]
        public long QC3InfoId { get; set; }
        public QC3Info QC3Info { get; set; }
    }
}
