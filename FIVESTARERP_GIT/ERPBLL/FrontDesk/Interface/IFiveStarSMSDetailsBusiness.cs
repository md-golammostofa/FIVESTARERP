using ERPBO.FrontDesk.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk.Interface
{
    public interface IFiveStarSMSDetailsBusiness
    {
        bool SaveSMSDetails(FiveStarSMSDetailsDTO dto, long userId, long orgId, long branchId);
    }
}
