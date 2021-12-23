using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DTOModels;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
    public class WarehouseToFactoryReturnDetailsBusiness: IWarehouseToFactoryReturnDetailsBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly WarehouseToFactoryReturnDetailsRepository _warehouseToFactoryReturnDetailsRepository; // repo
        public WarehouseToFactoryReturnDetailsBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            _warehouseToFactoryReturnDetailsRepository = new WarehouseToFactoryReturnDetailsRepository(this._configurationDb);
        }

        public IEnumerable<WarehouseToFactoryReturnDetailsDTO> GetDetailsAllData(long orgId, long branchId)
        {
            return this._configurationDb.Db.Database.SqlQuery<WarehouseToFactoryReturnDetailsDTO>(string.Format(@"Select fd.WTFInfoId,fd.WTFDetailsId,fd.ReferenceCode,m.ModelName,p.MobilePartName'PartsName',
p.MobilePartCode'PartsCode',fd.CostPrice,fd.SellPrice,fd.Quantity ,fd.EntryDate
From tblWarehouseToFactoryReturnDetails fd
Left Join tblModelSS m on fd.ModelId=m.ModelId
Left Join tblMobileParts p on fd.PartsId=p.MobilePartId
Where fd.OrganizationId={0} and fd.BranchId={1}", orgId, branchId)).ToList();
        }

        public IEnumerable<WarehouseToFactoryReturnDetailsDTO> GetDetailsDataByInfoId(long infoId, long orgId, long branchId)
        {
            return this._configurationDb.Db.Database.SqlQuery<WarehouseToFactoryReturnDetailsDTO>(string.Format(@"Select fd.WTFInfoId,fd.WTFDetailsId,fd.ReferenceCode,m.ModelName,p.MobilePartName'PartsName',
p.MobilePartCode'PartsCode',fd.CostPrice,fd.SellPrice,fd.Quantity ,fd.EntryDate
From tblWarehouseToFactoryReturnDetails fd
Left Join tblModelSS m on fd.ModelId=m.ModelId
Left Join tblMobileParts p on fd.PartsId=p.MobilePartId
Where fd.WTFInfoId={0} And fd.OrganizationId={1} and fd.BranchId={2}", infoId, orgId, branchId)).ToList();
        }
    }
}
