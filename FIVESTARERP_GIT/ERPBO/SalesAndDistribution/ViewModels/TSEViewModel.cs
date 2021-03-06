using ERPBO.SalesAndDistribution.CommonModels;
using System.Collections.Generic;

namespace ERPBO.SalesAndDistribution.ViewModels
{
    public class TSEViewModel : SalesHierarchyCommonModel
    {
        public long TSEID { get; set; }
        public long ASMUserId { get; set; }
        public long ASMId { get; set; }
        // Customer Properties
        public List<SRUser> SRUsers { get; set; }
        public string ZoneName { get; set; }
        public string DivisionName { get; set; }
        public string DistrictName { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
        public string ASMName { get; set; }
    }
}
