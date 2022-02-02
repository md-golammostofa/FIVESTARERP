using ERPBLL.Common;
using ERPBLL.FrontDesk.Interface;
using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using ERPDAL.FrontDeskDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk
{
   public class RequsitionDetailForJobOrderBusiness: IRequsitionDetailForJobOrderBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;//db
        private readonly RequsitionDetailForJobOrderRepository requsitionDetailForJobOrderRepository;

        public RequsitionDetailForJobOrderBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this.requsitionDetailForJobOrderRepository = new RequsitionDetailForJobOrderRepository(this._frontDeskUnitOfWork);
        }

        public IEnumerable<RequsitionDetailForJobOrder> GetAllRequsitionDetailForJobOrder(long orgId, long branchId)
        {
            return requsitionDetailForJobOrderRepository.GetAll(detail => detail.OrganizationId == orgId && detail.BranchId == branchId).ToList();
        }

        public IEnumerable<RequsitionDetailForJobOrder> GetAllRequsitionDetailForJobOrderId(long reqInfoId, long orgId, long branchId)
        {
            return requsitionDetailForJobOrderRepository.GetAll(detail =>detail.RequsitionInfoForJobOrderId == reqInfoId && detail.OrganizationId == orgId && detail.BranchId == branchId).ToList();
        }

        public IEnumerable<RequsitionDetailForJobOrderDTO> GetAvailableQtyByRequsition(long reqInfoId, long orgId, long branchId)
        {
            var data= this._frontDeskUnitOfWork.Db.Database.SqlQuery<RequsitionDetailForJobOrderDTO>(
                string.Format(@"select rd.RequsitionInfoForJobOrderId,rd.RequsitionDetailForJobOrderId,rd.PartsId,parts.MobilePartName 'PartsName',parts.MobilePartCode
,rd.Quantity,ISNULL(Sum(std.StockInQty-std.StockOutQty),0) 'AvailableQty' 
from tblRequsitionDetailForJobOrders rd
inner join [Configuration].dbo.tblMobilePartStockInfo std on rd.PartsId=std.MobilePartId and rd.BranchId = std.BranchId
left join [Configuration].dbo.tblMobileParts parts on rd.PartsId=parts.MobilePartId
inner join [FrontDesk].dbo.tblJobOrders jo on rd.JobOrderId=jo.JodOrderId
where rd.RequsitionInfoForJobOrderId={0} and std.OrganizationId={1} and jo.BranchId={2}
group by rd.RequsitionDetailForJobOrderId,rd.PartsId,rd.Quantity,parts.MobilePartName,parts.MobilePartCode,rd.RequsitionInfoForJobOrderId", reqInfoId, orgId, branchId)).ToList();
            return data;
        }

        public RequsitionDetailForJobOrder GetDetailsByDetailsId(long detailId, long orgId, long branchId)
        {
            return requsitionDetailForJobOrderRepository.GetOneByOrg(d => d.RequsitionDetailForJobOrderId == detailId && d.OrganizationId == orgId && d.BranchId == branchId);
        }
        public IEnumerable<RequsitionDetailForJobOrderDTO> GetModelWiseAvailableQtyByRequsition(long reqInfoId, long orgId, long branchId, long modelId)
        {
            var data = this._frontDeskUnitOfWork.Db.Database.SqlQuery<RequsitionDetailForJobOrderDTO>(
                string.Format(@"select rd.RequsitionInfoForJobOrderId,rd.RequsitionDetailForJobOrderId,rd.PartsId,parts.MobilePartName 'PartsName',parts.MobilePartCode
,rd.Quantity,ISNULL((select (ISNULL(Sum(ISNULL(StockInQty,0)-ISNULL(StockOutQty,0)),0)) 'AvailableQty' from [Configuration].dbo.tblMobilePartStockInfo where DescriptionId=jo.DescriptionId and MobilePartId=rd.PartsId and BranchId=rd.UserBranchId),0)'AvailableQty'
from tblRequsitionDetailForJobOrders rd
inner join [Configuration].dbo.tblMobilePartStockInfo std on rd.PartsId=std.MobilePartId 
left join [Configuration].dbo.tblMobileParts parts on rd.PartsId=parts.MobilePartId
inner join [FrontDesk].dbo.tblJobOrders jo on rd.JobOrderId=jo.JodOrderId 
where rd.RequsitionInfoForJobOrderId={0} and jo.OrganizationId={1} and rd.UserBranchId={2}
and jo.DescriptionId={3}
group by rd.RequsitionDetailForJobOrderId,rd.PartsId,rd.Quantity,parts.MobilePartName,parts.MobilePartCode,rd.RequsitionInfoForJobOrderId,jo.DescriptionId,rd.UserBranchId", reqInfoId, orgId, branchId, modelId)).ToList();
            return data;
        }

        public IEnumerable<RequsitionDetailForJobOrderDTO> GetRequsitionDetailAndAvailableQty(long reqInfoId, long orgId, long branchId)
        {
            var data = this._frontDeskUnitOfWork.Db.Database.SqlQuery<RequsitionDetailForJobOrderDTO>(
                string.Format(@"Select rd.RequsitionInfoForJobOrderId,rd.RequsitionDetailForJobOrderId,jo.DescriptionId,m.ModelName,rd.PartsId,
p.MobilePartName'PartsName',p.MobilePartCode'MobilePartCode',rd.Quantity,rd.IssueQty,
(Select ISNULL(Sum(StockInQty-StockOutQty),0) From [Configuration].dbo.tblMobilePartStockInfo
Where DescriptionId=jo.DescriptionId and MobilePartId=rd.PartsId and BranchId=rd.BranchId)'AvailableQty'
From tblRequsitionDetailForJobOrders rd
Left Join [Configuration].dbo.tblMobileParts p on rd.PartsId=p.MobilePartId
Left Join [FrontDesk].dbo.tblJobOrders jo on rd.JobOrderId=jo.JodOrderId
Left Join [Configuration].dbo.tblModelSS m on jo.DescriptionId=m.ModelId and rd.JobOrderId=jo.JodOrderId
Where rd.RequsitionInfoForJobOrderId={0} and rd.OrganizationId={1} and rd.BranchId={2}
", reqInfoId, orgId, branchId)).ToList();
            return data;
        }

        public IEnumerable<RequsitionDetailForJobOrderDTO> GetRequsitionDetailsData(long reqInfoId, long orgId, long branchId)
        {
            var data = this._frontDeskUnitOfWork.Db.Database.SqlQuery<RequsitionDetailForJobOrderDTO>(
                string.Format(@"Select rd.RequsitionDetailForJobOrderId,rd.RequsitionInfoForJobOrderId,rd.PartsId,p.MobilePartName'PartsName',
p.MobilePartCode,rd.Quantity,rd.IssueQty,rd.CostPrice,rd.SellPrice,
rd.EntryDate From tblRequsitionDetailForJobOrders rd
Left Join [Configuration].dbo.tblMobileParts p on rd.PartsId=p.MobilePartId
Where rd.RequsitionInfoForJobOrderId={0} and rd.OrganizationId={1} and rd.BranchId={2}
", reqInfoId, orgId, branchId)).ToList();
            return data;
        }

        public IEnumerable<RequsitionDetailsReportDTO> GetRequsitionDetailsReport(long orgId, long branchId, long? modelId)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<RequsitionDetailsReportDTO>(QueryForRequsitionDetailsReport(orgId, branchId, modelId)).ToList();
        }
        private string QueryForRequsitionDetailsReport(long orgId, long branchId, long? modelId)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (orgId > 0)
            {
                param += string.Format(@"and ri.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@" and ri.BranchId={0}", branchId);
            }
            if(modelId != null && modelId > 0)
            {
                param += string.Format(@"and jo.DescriptionId={0}", modelId);
            }
            query = string.Format(@"Select ri.RequsitionCode,jo.JobOrderCode,jo.DescriptionId,m.ModelName,jo.IMEI,jo.CustomerName,

Cast((Select ProblemName+',' From [Configuration].dbo.tblClientProblems prob
Inner Join tblJobOrderProblems jop on prob.ProblemId = jop.ProblemId
Where jop.JobOrderId = ri.JobOrderId 
Order BY ProblemName For XML PATH(''))as nvarchar(MAX)) 'Problems',

Cast((Select FaultName+',' From [Configuration].dbo.tblFault fa
Inner Join tblJobOrderFault jof on fa.FaultId = jof.FaultId
Where jof.JobOrderId = ri.JobOrderId
Order BY FaultName For XML PATH('')) as nvarchar(MAX))  'EngProblems',

Cast((Select  (parts.MobilePartName+' (Qty-' + Cast(rd.Quantity as nvarchar(20)))+')'+',' from [FrontDesk].dbo.tblRequsitionDetailForJobOrders rd
inner join [Configuration].dbo.tblMobileParts parts on rd.PartsId=parts.MobilePartId
Left Join [FrontDesk].dbo.tblRequsitionInfoForJobOrders rin on rd.RequsitionInfoForJobOrderId=rin.RequsitionInfoForJobOrderId
Where rd.Quantity>0 and rd.JobOrderId = ri.JobOrderId and rin.StateStatus='Pending'
Order BY (parts.MobilePartName+'#' + Cast(rd.Quantity as nvarchar(20))) For XML PATH('')) as nvarchar(MAX)) 'PartsName',
jo.EntryDate'ReceiveDate',ri.EntryDate'RequsionDate',us.UserName,ri.Remarks

From [FrontDesk].dbo.tblRequsitionInfoForJobOrders ri
Left Join [FrontDesk].dbo.tblJobOrders jo on ri.JobOrderId=jo.JodOrderId
Left Join [Configuration].dbo.tblModelSS m on jo.DescriptionId=m.ModelId
Left Join [ControlPanel].dbo.tblApplicationUsers us on ri.EUserId=us.UserId
Where 1=1{0} and ri.StateStatus='Pending'", Utility.ParamChecker(param));
            return query;
        }
    }
}
