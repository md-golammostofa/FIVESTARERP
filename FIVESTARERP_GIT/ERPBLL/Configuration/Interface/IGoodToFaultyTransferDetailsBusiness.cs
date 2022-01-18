using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
    public interface IGoodToFaultyTransferDetailsBusiness
    {
        bool SaveStockOutAndFaultyStockIn(GoodToFaultyTransferInfoDTO dto, long userId, long orgId, long branchId);
        IEnumerable<GoodToFaultyTransferDetailsDTO> GetGoodToFaultyTransferDetailsList(long orgId, long branchId);
    }
}
