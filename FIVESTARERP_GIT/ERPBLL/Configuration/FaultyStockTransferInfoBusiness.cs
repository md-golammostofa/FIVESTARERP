using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DomainModels;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
    public class FaultyStockTransferInfoBusiness: IFaultyStockTransferInfoBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb;
        private readonly FaultyStockTransferInfoRepository _faultyStockTransferInfoRepository;
        public FaultyStockTransferInfoBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            _faultyStockTransferInfoRepository = new FaultyStockTransferInfoRepository(this._configurationDb);
        }

        public IEnumerable<FaultyStockTransferInfo> GetAllStockInfo(long orgId, long branchId)
        {
            return _faultyStockTransferInfoRepository.GetAll(i => i.OrganizationId == orgId && i.BranchId == branchId).ToList();
        }

        public FaultyStockTransferInfo GetOneByOneInfo(long infoId, long orgId, long branchId)
        {
            return _faultyStockTransferInfoRepository.GetOneByOrg(i => i.FSTInfoId == infoId && i.OrganizationId == orgId && i.BranchId == branchId);
        }
    }
}
