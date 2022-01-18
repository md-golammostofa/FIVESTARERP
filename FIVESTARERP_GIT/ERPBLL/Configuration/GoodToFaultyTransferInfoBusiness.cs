using ERPBLL.Common;
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
    public class GoodToFaultyTransferInfoBusiness: IGoodToFaultyTransferInfoBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly GoodToFaultyTransferInfoRepository goodToFaultyTransferInfoRepository; // repo
        public GoodToFaultyTransferInfoBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            goodToFaultyTransferInfoRepository = new GoodToFaultyTransferInfoRepository(this._configurationDb);
        }

        public IEnumerable<GoodToFaultyTransferInfoDTO> GetGoodToFaultyInfoData(long orgId, long branchId)
        {
            return this._configurationDb.Db.Database.SqlQuery<GoodToFaultyTransferInfoDTO>(QueryForGoodToFaultyTransferInfoData(orgId, branchId)).ToList();
        }
        private string QueryForGoodToFaultyTransferInfoData(long orgId,long branchId)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (orgId > 0)
            {
                param += string.Format(@"and t.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and t.BranchId={0}", branchId);
            }

            query = string.Format(@"Select t.GTFTInfoId,t.TransferCode,t.TransferStatus,u.UserName,t.EntryDate 
From tblGoodToFaultyTransferInfo t
Left Join [ControlPanel].dbo.tblApplicationUsers u on t.EUserId=u.UserId
Where 1=1{0}
Order By t.EntryDate desc", Utility.ParamChecker(param));
            return query;
        }
    }
}
