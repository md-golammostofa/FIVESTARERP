using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
    public interface IFaultyStockRepairedInfoBusiness
    {
        bool SaveFaultyStockRepairedAndStockOut(FaultyStockRepairedInfoDTO dto, long userId, long orgId, long branchId);
        IEnumerable<FaultyStockRepairedInfoDTO> GetAllRepairedList(long orgId, long branchId, long? tsId, string fromDate, string toDate);
        IEnumerable<FaultyStockRepairedInfoDTO> GetAllRepairedListForTS(long tsId,long orgId, long branchId);
        FaultyStockRepairedInfo GetRepairedInfoById(long infoId, long orgId, long branchId);
        FaultyStockRepairedInfoDTO GetRepairedInfoDataByInfoId(long infoId, long orgId, long branchId);
        bool UpdateInfoStatusAndRepairedDetails(FaultyStockRepairedInfoDTO dto, long userId, long orgId, long branchId);
        bool UpdateInfoStatusAndRepairedStockIn(FaultyStockRepairedInfoDTO dto, long userId, long orgId, long branchId);
    }
}
