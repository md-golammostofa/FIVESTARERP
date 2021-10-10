using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
    public interface IFaultyStockTransferInfoBusiness
    {
        IEnumerable<FaultyStockTransferInfo> GetAllStockInfo(long orgId, long branchId);
        FaultyStockTransferInfo GetOneByOneInfo(long infoId, long orgId, long branchId);
        IEnumerable<FaultyStockTransferInfoDTO> GetAllStockInfoList(long orgId, long branchId);
        FaultyStockTransferInfo GetOneByOneInfoByModel(long modelId,long partsId, long orgId, long branchId);
        IEnumerable<FaultyStockTransferInfoDTO> GetAllFaultyStockReceiveList(long orgId, long branchId);
        FaultyStockTransferInfo GetOneByOneInfoByStatus(long infoId, long orgId);
    }
}
