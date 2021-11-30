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
    public class StockTransferDetailModelToModelBusiness : IStockTransferDetailModelToModelBusiness
    {
        private readonly IConfigurationUnitOfWork _configDb;
        private readonly StockTransferDetailModelToModelRepository _stockTransferDetailModelToModelRepository;

        public StockTransferDetailModelToModelBusiness(IConfigurationUnitOfWork configDb)
        {
            this._configDb = configDb;
            _stockTransferDetailModelToModelRepository = new StockTransferDetailModelToModelRepository(this._configDb = configDb);
        }

        public IEnumerable<StockTransferDetailModelToModelDTO> GetAllTransferDetail(long? model, long? parts, long orgId, long branchId, string fromDate, string toDate)
        {
            return _configDb.Db.Database.SqlQuery<StockTransferDetailModelToModelDTO>(QueryForGetAllTransferDetail(model,parts,orgId,branchId,fromDate,toDate)).ToList();
        }
        private string QueryForGetAllTransferDetail(long? model, long? parts, long orgId, long branchId, string fromDate, string toDate)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (orgId > 0)
            {
                param += string.Format(@"and d.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and d.BranchId={0}", branchId);
            }
            if (model != null && model > 0)
            {
                param += string.Format(@"and d.DescriptionId ={0}", model);
            }
            if (parts != null && parts > 0)
            {
                param += string.Format(@"and d.PartsId ={0}", parts);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(d.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(d.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(d.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select d.DescriptionId,d.PartsId,m1.ModelName'ModelFrom',m2.ModelName'ModelTo',p.MobilePartName'PartsName',d.Quantity,d.EntryDate,u.UserName From StockTransferDetailModelToModels d
Left Join tblModelSS m1 on d.DescriptionId=m1.ModelId
Left Join tblModelSS m2 on d.ToDescriptionId=m2.ModelId
Left Join tblMobileParts p on d.PartsId=p.MobilePartId
Left Join [ControlPanel].dbo.tblApplicationUsers u on d.EUserId=u.UserId
Where 1=1{0} Order By d.EntryDate desc", Utility.ParamChecker(param));
            return query;
        }

        public IEnumerable<StockTransferDetailModelToModel> GetAllTransferDetailMMByInfoId(long transferId, long orgId)
        {
            return _stockTransferDetailModelToModelRepository.GetAll(ts => ts.OrganizationId == orgId && ts.TransferInfoModelToModelId == transferId).ToList();
        }
    }
}
