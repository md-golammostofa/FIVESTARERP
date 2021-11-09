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
    public class DustStockInfoBusiness: IDustStockInfoBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly DustStockInfoRepository _dustStockInfoRepository; // repo
        public DustStockInfoBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            _dustStockInfoRepository = new DustStockInfoRepository(this._configurationDb);
        }

        public IEnumerable<DustStockInfo> GetAllStockForList(long orgId, long branchId)
        {
            return _dustStockInfoRepository.GetAll(d => d.OrganizationId == orgId && d.BranchId == branchId).ToList();
        }

        public DustStockInfo GetPartsByModel(long modelId, long partsId, long orgId, long branchId)
        {
            return _dustStockInfoRepository.GetOneByOrg(d => d.ModelId == modelId && d.PartsId == partsId && d.OrganizationId == orgId && d.BranchId == branchId);
        }
    }
}
