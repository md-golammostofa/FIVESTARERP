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
   public class TransferDetailBusiness: ITransferDetailBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly TransferDetailRepository transferDetailRepository; // repo
        public TransferDetailBusiness(IConfigurationUnitOfWork configurationDb)
        {
            this._configurationDb = configurationDb;
            transferDetailRepository = new TransferDetailRepository(this._configurationDb);
        }

        public IEnumerable<TransferDetail> GetAllTransferDetailByInfoId(long transferId, long orgId, long branchId)
        {
            return transferDetailRepository.GetAll(ts =>ts.TransferInfoId == transferId && ts.OrganizationId == orgId && ts.BranchId == branchId).ToList();
        }

        public IEnumerable<TransferDetail> GetAllTransferDetailByInfoId(long transferId, long orgId)
        {
            return transferDetailRepository.GetAll(ts => ts.OrganizationId == orgId && ts.TransferInfoId == transferId).ToList();
        }

        public IEnumerable<TransferDetail> GetAllTransferDetailByOrgId(long orgId, long branchId)
        {
            return transferDetailRepository.GetAll(ts => ts.OrganizationId == orgId && ts.BranchId == branchId).ToList();
        }

        public IEnumerable<TransferDetailDTO> GetAllTransferDetailDataByInfoId(long requsitionId, long orgId, long branchId)
        {
            return this._configurationDb.Db.Database.SqlQuery<TransferDetailDTO>(string.Format(@"Select TransferInfoId,TransferDetailId,td.PartsId,p.MobilePartName'PartsName',p.MobilePartCode'PartsCode',
td.Quantity
,ISNULL((ISNULL(st.StockInQty,0)-ISNULL(st.StockOutQty,0)),0)'AvailableQty' 
From tblTransferDetails td
Left Join tblMobileParts p on td.PartsId=p.MobilePartId
Left Join tblMobilePartStockInfo st on td.PartsId=st.MobilePartId
Where td.TransferInfoId={0} and st.OrganizationId={1} and st.BranchId={2}", requsitionId,orgId,branchId)).ToList();
        }

        public TransferDetail GetOneByDetailId(long reqDetailsId, long orgId, long branchId)
        {
            return transferDetailRepository.GetOneByOrg(d => d.TransferDetailId == reqDetailsId && d.OrganizationId == orgId && d.BranchTo == branchId);
        }

        public TransferDetail GetOneByOneDetailId(long reqDetailsId, long orgId, long branchId)
        {
            return transferDetailRepository.GetOneByOrg(d => d.TransferDetailId == reqDetailsId && d.OrganizationId == orgId && d.BranchId == branchId);
        }
    }
}
