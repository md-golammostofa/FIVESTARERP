using ERPBLL.Common;
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

        public IEnumerable<FaultyStockTransferInfoDTO> GetAllFaultyStockReceiveList(long orgId, long branchId, long? branch, string fromDate, string toDate)
        {
            var data= this._configurationDb.Db.Database.SqlQuery<FaultyStockTransferInfoDTO>(QueryForGetAllFaultyStockReceiveList(orgId,branchId,branch,fromDate,toDate)).ToList();
            return data;
        }
        private string QueryForGetAllFaultyStockReceiveList(long orgId, long branchId,long? branch,string fromDate,string toDate)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (orgId > 0)
            {
                param += string.Format(@" and fst.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and fst.BranchTo={0}", branchId);
            }
            if (branch != null && branch > 0)
            {
                param += string.Format(@"and fst.BranchId ={0}", branch);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(fst.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(fst.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(fst.EntryDate as date)='{0}'", tDate);
            }
            query = string.Format(@"Select FSTInfoId,fst.TransferCode,fst.StateStatus,b.BranchName,u.UserName,fst.EntryDate From tblFaultyStockTransferInfo fst
Left Join [ControlPanel].dbo.tblBranch b on fst.BranchId=b.BranchId
Left Join [ControlPanel].dbo.tblApplicationUsers u on fst.EUserId=u.UserId
Where 1=1{0} order by fst.EntryDate desc", Utility.ParamChecker(param));
            return query;
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

        public FaultyStockTransferInfoDTO GetTransferInfoData(long infoId, long orgId)
        {
            return this._configurationDb.Db.Database.SqlQuery<FaultyStockTransferInfoDTO>(string.Format(@"Select FSTInfoId,fst.TransferCode,fst.StateStatus,b.BranchName,u.UserName,fst.EntryDate From tblFaultyStockTransferInfo fst
Left Join [ControlPanel].dbo.tblBranch b on fst.BranchId=b.BranchId
Left Join [ControlPanel].dbo.tblApplicationUsers u on fst.EUserId=u.UserId
Where FSTInfoId={0} and fst.OrganizationId={1} order by fst.EntryDate desc", infoId, orgId)).FirstOrDefault();
        }
    }
}
