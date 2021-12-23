using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.ViewModels
{
    public class ServicesSummaryViewModel
    {
        public long UserId { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public int ReceiveJob { get; set; }
        public int SmartPhone { get; set; }
        public int Featurephone { get; set; }
        public int Accessories { get; set; }
        public int EngAssign { get; set; }
        public int EngSignOut { get; set; }
        public int FeaturephoneTS { get; set; }
        public int SmartphoneTS { get; set; }
        public int AccessoriesTS { get; set; }
        public int TotalQCAssign { get; set; }
        public int QCPass { get; set; }
        public int Delivery { get; set; }
        public int CCAppORDisAPP { get; set; }
        public double InCome { get; set; }
        public int Requsition { get; set; }
        public int EngPending { get; set; }
        public int QCPending { get; set; }
        public int CallCenterPending { get; set; }
    }
}
