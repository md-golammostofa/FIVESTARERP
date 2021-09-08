﻿using ERPBO.SalesAndDistribution.CommonModels;
using System.Collections.Generic;

namespace ERPBO.SalesAndDistribution.DTOModels
{
    public class ASMDTO : SalesHierarchyCommonModel
    {
        public long ASMID { get; set; }
        public long RSMId { get; set; }
        public long RSMUserId { get; set; }
        // Customer Properties
        public string ZoneName { get; set; }
        public List<SRUser> SRUsers { get; set; }
        public string DivisionName { get; set; }
        public string DistrictName { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
        public string RSMName { get; set; }
    }
}
