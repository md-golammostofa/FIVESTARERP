using ERPBO.Inventory.DTOModel;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
   public interface IRequsitionDetailBusiness
    {
        IEnumerable<RequsitionDetail> GetAllReqDetailByOrgId(long orgId);
        bool SaveReqDetails(RequsitionDetailDTO detailDTO, long userId, long orgId);
        IEnumerable<RequsitionDetail> GetRequsitionDetailByReqId(long id, long orgId);
        RequsitionDetail GetRequsitionDetailById(long id, long orgId);
        bool SaveRequisitionDetail(ReqInfoDTO reqInfoDTO, long userId, long orgId);
        IEnumerable<RequsitionDetailDTO> GetRequisitionDetailsByQuery(long? reqInfoId,long? reqDetailId,long? itemType,long? itemId,long orgId);
        IEnumerable<RequsitionDetailDTO> GetRequsitionDetailsForReport(long infoId, long orgId);
        
    }
}
