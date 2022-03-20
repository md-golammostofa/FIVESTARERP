using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
    public interface IFaultyStockTransferDetailsBusiness
    {
        IEnumerable<FaultyStockTransferDetails> GetAllDetails(long orgId, long branchId);
        FaultyStockTransferDetails GetOneByOneDetails(long detailId, long orgId, long branchId);
        IEnumerable<FaultyStockTransferDetails> GetAllDetailsByInfoId(long infoId, long orgId, long branchId);
        FaultyStockTransferDetails GetOneDetailsByInfoId(long infoId, long orgId, long branchId);
        bool SaveFaultyStockTransfer(FaultyStockTransferInfoDTO dto, long userId, long orgId, long branchId);
        IEnumerable<FaultyStockTransferDetails> GetAllReceiveDetailsByInfoId(long infoId, long orgId);
        bool SaveFaultyStockReceive(FaultyStockTransferInfoDTO dto, long userId, long orgId, long branchId);
        IEnumerable<FaultyStockTransferDetailsDTO> GetAllDetailsDataByInfoId(long infoId, long orgId);
        FaultyStockTransferDetails GetOneDetailsByDetailsId(long detailsId, long orgId);
    }
}
