using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DomainModels
{
    [Table("tblFiveStarSMSDetails")]
    public class FiveStarSMSDetails
    {
        [Key]
        public long SmsId { get; set; }
        public string MobileNo { get; set; }
        public string Message { get; set; }
        public string Purpose { get; set; }
        public string Response { get; set; }
        public long? BranchId { get; set; }
        public long? OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
    }
}
