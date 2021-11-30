using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
    public interface IStockTransferDetailModelToModelBusiness
    {
        IEnumerable<StockTransferDetailModelToModel> GetAllTransferDetailMMByInfoId(long transferId, long orgId);
        IEnumerable<StockTransferDetailModelToModelDTO> GetAllTransferDetail(long? model,long? parts,long orgId,long branchId,string fromDate,string toDate);
    }
}
