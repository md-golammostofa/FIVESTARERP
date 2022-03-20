using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
    public interface IStockTransferInfoModelToModelBusiness
    {
        bool SaveStockTransferModelToModel(List<StockTransferDetailModelToModelDTO> dto, long userId, long branchId, long orgId);
        IEnumerable<StockTransferInfoModelToModel> GetAllStockTransferInfoModelToModelByOrgIdAndBranch(long orgId, long branchId);
        StockTransferInfoModelToModel GetStockTransferMMInfoById(long id, long orgId);
        IEnumerable<StockTransferInfoModelToModelDTO> GetModelToModelTransferAllData(long orgId, long branchId);
    }
}
