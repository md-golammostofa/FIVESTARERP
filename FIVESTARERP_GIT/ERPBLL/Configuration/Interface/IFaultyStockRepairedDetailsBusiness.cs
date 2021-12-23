using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
    public interface IFaultyStockRepairedDetailsBusiness
    {
        IEnumerable<FaultyStockRepairedDetailsDTO> GetAssignStockByInfoId(long infoId, long orgId, long branchId);
        FaultyStockRepairedDetails GetDetailsByDetailsId(long detailsId, long orgId, long branchId);
    }
}
