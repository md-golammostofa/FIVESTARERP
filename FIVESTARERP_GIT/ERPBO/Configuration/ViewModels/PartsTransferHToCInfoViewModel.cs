﻿using ERPBO.Configuration.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Configuration.ViewModels
{
    public class PartsTransferHToCInfoViewModel
    {
        public long InfoId { get; set; }
        public string TransferCode { get; set; }
        public string StateStatus { get; set; }
        public string Remarks { get; set; }
        public long BranchToId { get; set; }
        public long BranchFromId { get; set; }
        public long OrganizationId { get; set; }
        public long BranchId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        //
        public string BranchToName { get; set; }
        public string BranchFromName { get; set; }
        public List<PartsTransferHToCDetails> partsTransferHToCDetails { get; set; }
    }
}
