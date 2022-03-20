using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
    public interface IPartsTransferHToCInfoBusiness
    {
        bool SavePartsTransferHeadToCare(PartsTransferHToCInfoDTO dto,List<PartsTransferHToCDetailsDTO> details, long orgId, long branchId, long userId);
        bool SaveMobilePartStockOut(PartsTransferHToCInfoDTO dto, long orgId, long branchId, long userId);
        IEnumerable<PartsTransferHToCInfoDTO> GetAllPartsTransferData(long orgId, long branchId);
        IEnumerable<PartsTransferHToCInfoDTO> GetAllPartsReceiveData(long orgId, long branchId,string fromDate,string toDate);
        PartsTransferHToCInfo GetInfoByInfoId(long infoId, long orgId);
        bool SaveTransferItemReceive(long infoId, long userId, long orgId, long branchId);
    }
}
