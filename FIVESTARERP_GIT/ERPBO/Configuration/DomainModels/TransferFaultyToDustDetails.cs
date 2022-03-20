using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DomainModels
{
    [Table("tblTransferFaultyToDustDetails")]
    public class TransferFaultyToDustDetails
    {
        [Key]
        public long DetailsId { get; set; }
        public long ModelId { get; set; }
        public long PartsId { get; set; }
        public double CostPrice { get; set; }
        public double SellPrice { get; set; }
        public int Quantity { get; set; }
        public string RefCode { get; set; }
        public string Remarks { get; set; }
        public long BranchId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        //
        [ForeignKey("TransferFaultyToDustInfo")]
        public long InfoId { get; set; }
        public TransferFaultyToDustInfo TransferFaultyToDustInfo { get; set; }

    }
}
