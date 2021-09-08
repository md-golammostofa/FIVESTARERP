using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface ITransferDetailBusiness
    {
        IEnumerable<TransferDetail> GetAllTransferDetailByOrgId(long orgId, long branchId);
        IEnumerable<TransferDetail> GetAllTransferDetailByInfoId(long transferId,long orgId, long branchId);
        IEnumerable<TransferDetail> GetAllTransferDetailByInfoId(long transferId, long orgId);
        IEnumerable<TransferDetailDTO> GetAllTransferDetailDataByInfoId(long requsitionId, long orgId,long branchId);
        TransferDetail GetOneByDetailId(long reqDetailsId, long orgId, long branchId);
        TransferDetail GetOneByOneDetailId(long reqDetailsId, long orgId, long branchId);
    }
}
