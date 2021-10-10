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
    public class FaultyStockTransferInfoBusiness: IFaultyStockTransferInfoBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb;
        private readonly FaultyStockTransferInfoRepository _faultyStockTransferInfoRepository;
        public FaultyStockTransferInfoBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            _faultyStockTransferInfoRepository = new FaultyStockTransferInfoRepository(this._configurationDb);
        }

        public IEnumerable<FaultyStockTransferInfoDTO> GetAllFaultyStockReceiveList(long orgId, long branchId)
        {
            var data= this._configurationDb.Db.Database.SqlQuery<FaultyStockTransferInfoDTO>(string.Format(@"Select FSTInfoId,fst.TransferCode,fst.StateStatus,b.BranchName,u.UserName,fst.EntryDate From tblFaultyStockTransferInfo fst
Left Join [ControlPanel].dbo.tblBranch b on fst.BranchId=b.BranchId
Left Join [ControlPanel].dbo.tblApplicationUsers u on fst.EUserId=u.UserId
Where fst.OrganizationId={0} and fst.BranchTo={1} order by fst.EntryDate desc", orgId, branchId)).ToList();
            return data;
        }

        public IEnumerable<FaultyStockTransferInfo> GetAllStockInfo(long orgId, long branchId)
        {
            return _faultyStockTransferInfoRepository.GetAll(i => i.OrganizationId == orgId && i.BranchId == branchId).ToList();
        }

        public IEnumerable<FaultyStockTransferInfoDTO> GetAllStockInfoList(long orgId, long branchId)
        {
            return this._configurationDb.Db.Database.SqlQuery<FaultyStockTransferInfoDTO>(string.Format(@"Select FSTInfoId,fst.TransferCode,fst.StateStatus,b.BranchName,u.UserName,fst.EntryDate From tblFaultyStockTransferInfo fst
Left Join [ControlPanel].dbo.tblBranch b on fst.BranchTo=b.BranchId
Left Join [ControlPanel].dbo.tblApplicationUsers u on fst.EUserId=u.UserId
Where fst.OrganizationId={0} and fst.BranchId={1} order by fst.EntryDate desc", orgId, branchId)).ToList();
        }

        public FaultyStockTransferInfo GetOneByOneInfo(long infoId, long orgId, long branchId)
        {
            return _faultyStockTransferInfoRepository.GetOneByOrg(i => i.FSTInfoId == infoId && i.OrganizationId == orgId && i.BranchId == branchId);
        }

        public FaultyStockTransferInfo GetOneByOneInfoByModel(long modelId, long partsId, long orgId, long branchId)
        {
            throw new NotImplementedException();
        }

        public FaultyStockTransferInfo GetOneByOneInfoByStatus(long infoId, long orgId)
        {
            return _faultyStockTransferInfoRepository.GetOneByOrg(i => i.FSTInfoId == infoId && i.OrganizationId == orgId);
        }
    }
}
