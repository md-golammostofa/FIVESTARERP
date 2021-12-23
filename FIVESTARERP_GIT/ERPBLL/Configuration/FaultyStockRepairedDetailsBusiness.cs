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
   public class FaultyStockRepairedDetailsBusiness: IFaultyStockRepairedDetailsBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly FaultyStockRepairedDetailsRepository _faultyStockRepairedDetailsRepository; // repo
        public FaultyStockRepairedDetailsBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            _faultyStockRepairedDetailsRepository = new FaultyStockRepairedDetailsRepository(this._configurationDb);
        }

        public IEnumerable<FaultyStockRepairedDetailsDTO> GetAssignStockByInfoId(long infoId, long orgId, long branchId)
        {
            return this._configurationDb.Db.Database.SqlQuery<FaultyStockRepairedDetailsDTO>(string.Format(@"Select fd.FSRInfoId,fd.FSRDetailsId,fd.ModelId,m.ModelName,fd.PartsId,p.MobilePartName'PartsName',
p.MobilePartCode'PartsCode',fd.AssignQty,fd.RepairedQty,fd.ScrapedQty 
From tblFaultyStockRepairedDetails fd
Left Join tblModelSS m on fd.ModelId=m.ModelId
Left Join tblMobileParts p on fd.PartsId=p.MobilePartId
Where fd.FSRInfoId={0} and fd.OrganizationId={1} and fd.BranchId={2}", infoId, orgId, branchId)).ToList();
        }

        public FaultyStockRepairedDetails GetDetailsByDetailsId(long detailsId, long orgId, long branchId)
        {
            return _faultyStockRepairedDetailsRepository.GetOneByOrg(d => d.FSRDetailsId == detailsId && d.OrganizationId == orgId && d.BranchId == branchId);
        }
    }
}
