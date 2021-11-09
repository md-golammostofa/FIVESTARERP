using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.DomainModels
{
    [Table("tblQC1Detail")]
    public class QC1Detail
    {
        [Key]
        public long QC1DetailId { get; set; }
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
        [ForeignKey("QC1Info")]
        public long QC1InfoId { get; set; }
        public QC1Info QC1Info { get; set; }
    }
}
