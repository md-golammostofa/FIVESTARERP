﻿using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
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
  public class JobOrderTSBusiness: IJobOrderTSBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;
        private readonly JobOrderTSRepository _jobOrderTSRepository;
        private readonly IJobOrderRepairBusiness _jobOrderRepairBusiness;
        private readonly IRepairBusiness _repairBusiness;

        public JobOrderTSBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork, IJobOrderRepairBusiness jobOrderRepairBusiness, IRepairBusiness repairBusiness)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this._jobOrderTSRepository = new JobOrderTSRepository(this._frontDeskUnitOfWork);
            this._jobOrderRepairBusiness = jobOrderRepairBusiness;
            this._repairBusiness = repairBusiness;
        }

        public IEnumerable<DashboardDailySingInAndOutDTO> DashboardDailySingInAndOuts(long orgId, long branchId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<DashboardDailySingInAndOutDTO>(
                string.Format(@"Select (select COUNT(*) From tblJobOrderTS Where Cast(AssignDate as date)=Cast(GETDATE()  as date) and OrganizationId={0} and BranchId={1}) 'TotalSignInToday' ,
(select COUNT(*) From tblJobOrderTS Where Cast(SignOutDate as date)=Cast(GETDATE()  as date) and OrganizationId={0} and BranchId={1}) 'TotalSignOutToday'", orgId, branchId)).ToList();
        }
        public IEnumerable<DashboardDailySingInAndOutDTO> DashboardDailySingInAndOutByEng(long orgId, long branchId,long userId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<DashboardDailySingInAndOutDTO>(
                string.Format(@"Select (select COUNT(*) From tblJobOrderTS Where Cast(AssignDate as date)=Cast(GETDATE()  as date) and OrganizationId={0} and BranchId={1} and TSId={2}) 'TotalSignInToday' ,
(select COUNT(*) From tblJobOrderTS Where Cast(SignOutDate as date)=Cast(GETDATE()  as date) and OrganizationId={0} and BranchId={1} and TSId={2}) 'TotalSignOutToday',
(select COUNT(*) From tblJobOrderTS Where StateStatus='Sign-In' and OrganizationId={0} and BranchId={1} and TSId={2}) 'PendingEng',
(Select COUNT(TsRepairStatus)'TodayCallCenterEng' From tblJobOrders
Where Cast(CallCenterAssignDate as Date)=Cast(GETDATE() as date) and OrganizationId={0} and BranchId={1} and TSId={2} and TsRepairStatus='CALL CENTER')'TodayCallCenterEng',
(Select COUNT(TsRepairStatus) From tblJobOrders
Where OrganizationId={0} and BranchId={1} and TSId={2} and TsRepairStatus='RETURN FOR ENGINEER HEAD')'TransferToTIEng',
(Select COUNT(TsRepairStatus)'TotalCallCenterEng' From tblJobOrders
Where OrganizationId={0} and BranchId={1} and TSId={2} and TsRepairStatus='CALL CENTER')'TotalCallCenterEng'
", orgId, branchId,userId)).ToList();
        }

        public JobOrderTS GetJobOrderActiveTsByJobOrderId(long joborderId, long orgId, long branchId)
        {
            return _jobOrderTSRepository.GetOneByOrg(ts => ts.JodOrderId == joborderId && ts.OrganizationId == orgId && ts.BranchId == branchId && ts.IsActive == true);
        }

        public IEnumerable<JobOrderTSDTO> JobSignInAndOut(long? tsId, string jobCode, long orgId, long branchId, string fromDate, string toDate, long? modelId, string imei)
        {
            return _frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderTSDTO>(QueryForJobSignInAndOut(tsId,jobCode,orgId,branchId,fromDate,toDate,modelId,imei)).ToList();
        }
        private string QueryForJobSignInAndOut(long? tsId, string jobCode, long orgId, long branchId, string fromDate, string toDate,long? modelId,string imei)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (orgId > 0)
            {
                param += string.Format(@"and ts.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and ts.BranchId={0} ", branchId);
            }
            if (tsId != null && tsId > 0)
            {
                param += string.Format(@"and ts.TSId ={0}", tsId);
            }
            if (!string.IsNullOrEmpty(jobCode))
            {
                param += string.Format(@"and ts.JobOrderCode Like '%{0}%'", jobCode);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@"and jo.DescriptionId ={0}", modelId);
            }
            if (!string.IsNullOrEmpty(imei))
            {
                param += string.Format(@"and jo.IMEI Like '%{0}%'", imei);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ts.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ts.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(ts.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"select ts.JobOrderCode,ts.TSId,ts.StateStatus,ts.Remarks,jo.DescriptionId,m.ModelName,
jo.IMEI,apu.UserName'TSName',ts.EntryDate,ts.AssignDate,ts.SignOutDate 
from tblJobOrderTS ts
left join [ControlPanel].dbo.tblApplicationUsers apu on ts.TSId=apu.UserId
Left Join tblJobOrders jo on ts.JodOrderId=jo.JodOrderId
Left Join [Configuration].dbo.tblModelSS m on jo.DescriptionId=m.ModelId
where 1=1{0} order by ts.EntryDate desc", Utility.ParamChecker(param));
            return query;
        }

        public bool UpdateJobOrderTsStatus(long joborderId, long userId, long orgId, long branchId)
        {
            var jobOrderTsStatus = GetJobOrderActiveTsByJobOrderId(joborderId, orgId,branchId);
            if (jobOrderTsStatus != null)
            {
                jobOrderTsStatus.StateStatus = "Sign-Out";
                jobOrderTsStatus.IsActive = false;
                jobOrderTsStatus.UpUserId = userId;
                jobOrderTsStatus.SignOutDate = DateTime.Now;
                _jobOrderTSRepository.Update(jobOrderTsStatus);
            }
            return _jobOrderTSRepository.Save();
        }
        public bool UpdateJobOrderTsForQcFail(long joborderId, long userId, long orgId, long branchId)
        {
            var jobOrderTsStatus = GetJobOrderTsByJobOrderId(joborderId, orgId, branchId);
            if (jobOrderTsStatus != null)
            {
                jobOrderTsStatus.StateStatus = "Sign-In";
                jobOrderTsStatus.IsActive = true;
                jobOrderTsStatus.UpUserId = userId;
                jobOrderTsStatus.SignOutDate = DateTime.Now;
                _jobOrderTSRepository.Update(jobOrderTsStatus);
            }
            return _jobOrderTSRepository.Save();
        }

        public JobOrderTS GetJobOrderTsByJobOrderId(long joborderId, long orgId, long branchId)
        {
            return _jobOrderTSRepository.GetOneByOrg(ts => ts.JodOrderId == joborderId && ts.OrganizationId == orgId && ts.BranchId == branchId);
        }

        public IEnumerable<JobOrderTSDTO> GetDailyJobSignOut(long orgId, long branchId, long userId)
        {
            return this._frontDeskUnitOfWork.Db.Database.SqlQuery<JobOrderTSDTO>(QueryForDailyJobSignOut(orgId, branchId, userId)).ToList();
        }
        private string QueryForDailyJobSignOut(long orgId,long branchId,long userId)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (orgId > 0)
            {
                param += string.Format(@" and jt.OrganizationId={0}", orgId);
            }
            if (branchId > 0)
            {
                param += string.Format(@"and jt.BranchId={0}", branchId);
            }
            if (userId > 0)
            {
                param += string.Format(@"and jt.TSId={0}", userId);
            }

            query = string.Format(@"Select jt.JodOrderId,jo.JobOrderCode,m.ModelName,jo.DescriptionId,jo.IMEI,
jt.StateStatus,jt.AssignDate,jt.SignOutDate,jt.TSId 
From tblJobOrderTS jt
Left Join tblJobOrders jo on jt.JodOrderId=jo.JodOrderId
Left Join [Configuration].dbo.tblModelSS m on jo.DescriptionId=m.ModelId
Where 1=1{0} and jt.StateStatus='Sign-Out' and ((Cast(jt.SignOutDate as Date)=Cast(GetDate() as Date))) order By jt.SignOutDate desc", Utility.ParamChecker(param));

            return query;
        }
    }
}
