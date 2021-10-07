using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
    public class FaultyStockTransferDetailsBusiness: IFaultyStockTransferDetailsBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb;
        private readonly FaultyStockTransferDetailsRepository _faultyStockTransferDetailsRepository;//repo
        private readonly FaultyStockTransferInfoRepository _faultyStockTransferInfoRepository;//repo
        private readonly IFaultyStockTransferInfoBusiness _faultyStockTransferInfoBusiness;
        public FaultyStockTransferDetailsBusiness(IConfigurationUnitOfWork configurationDb, IFaultyStockTransferInfoBusiness faultyStockTransferInfoBusiness)
        {
            this._configurationDb = configurationDb;
            _faultyStockTransferDetailsRepository = new FaultyStockTransferDetailsRepository(this._configurationDb);
            _faultyStockTransferInfoRepository = new FaultyStockTransferInfoRepository(this._configurationDb);
            this._faultyStockTransferInfoBusiness = faultyStockTransferInfoBusiness;
        }

        public IEnumerable<FaultyStockTransferDetails> GetAllDetails(long orgId, long branchId)
        {
            return _faultyStockTransferDetailsRepository.GetAll(d => d.OrganizationId == orgId && d.BranchId == branchId).ToList();
        }

        public IEnumerable<FaultyStockTransferDetailsDTO> GetAllDetailsByInfoId(long infoId, long orgId, long branchId)
        {
            throw new NotImplementedException();
        }

        public FaultyStockTransferDetails GetOneByOneDetails(long detailId, long orgId, long branchId)
        {
            return _faultyStockTransferDetailsRepository.GetOneByOrg(d => d.FSTDetailsId == detailId && d.OrganizationId == orgId && d.BranchId == branchId);
        }

        public FaultyStockTransferDetails GetOneDetailsByInfoId(long infoId, long orgId, long branchId)
        {
            return _faultyStockTransferDetailsRepository.GetOneByOrg(d => d.FSTInfoId == infoId && d.OrganizationId == orgId && d.BranchId == branchId);
        }
    }
}
