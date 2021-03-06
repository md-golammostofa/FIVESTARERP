using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface ITransferStockToAssemblyDetailBusiness
    {
        IEnumerable<TransferStockToAssemblyDetail> GetTransferStockToAssemblyDetails(long orgId);
        IEnumerable<TransferStockToAssemblyDetail> GetTransferStockToAssemblyDetailByInfo(long infoId,long orgId);
    }
}
