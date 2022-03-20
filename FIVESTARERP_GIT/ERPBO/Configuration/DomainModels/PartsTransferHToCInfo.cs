﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.DomainModels
{
    [Table("tblPartsTransferHToCInfo")]
    public class PartsTransferHToCInfo
    {
        [Key]
        public long InfoId { get; set; }
        public string TransferCode { get; set; }
        public string StateStatus { get; set; }
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long BranchToId { get; set; }
        public long BranchFromId { get; set; }
        public long BranchId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public List<PartsTransferHToCDetails> partsTransferHToCDetails { get; set; }
    }
}
