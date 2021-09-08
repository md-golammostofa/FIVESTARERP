﻿using ERPBLL.Common;
using ERPBLL.Report.Interface;
using ERPBO.Production.ReportModels;
using ERPDAL.ControlPanelDAL;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Report
{
    public class ProductionReportBusiness : IProductionReportBusiness
    {
        // database
        private readonly IProductionUnitOfWork _productionDb;
        private readonly IControlPanelUnitOfWork _controlPanelDb;

        private string query = string.Empty;
        private string param = string.Empty;
        public ProductionReportBusiness(IProductionUnitOfWork productionDb, IControlPanelUnitOfWork controlPanelDb)
        {
            this._productionDb = productionDb;
            this._controlPanelDb = controlPanelDb;
        }
        public IEnumerable<ProductionRequisitionReport> GetProductionRequisitionReport(long reqInfoId)
        {
            IEnumerable<ProductionRequisitionReport> list = new List<ProductionRequisitionReport>();
            list = this._productionDb.Db.Database.SqlQuery<ProductionRequisitionReport>(string.Format(@"Select ri.ReqInfoCode,ri.RequisitionType,de.DescriptionName 'ModelName',ri.EntryDate,us.FullName 'RequisitionBy',
pl.LineNumber,w.WarehouseName,it.ItemName 'ItemTypeName'
,i.ItemName,rd.Quantity,un.UnitSymbol 'UnitName',rd.Remarks,ri.StateStatus 
From tblRequsitionDetails rd
Inner Join tblRequsitionInfo ri on rd.ReqInfoId = ri.ReqInfoId
Inner Join tblProductionLines pl on ri.LineId = pl.LineId
Inner Join [Inventory].dbo.tblWarehouses w on ri.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on rd.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on rd.ItemId = i.ItemId and rd.ItemTypeId = i.ItemTypeId
Inner Join [Inventory].dbo.tblDescriptions de on ri.DescriptionId = de.DescriptionId
Inner Join [Inventory].dbo.tblUnits un on rd.UnitId = Un.UnitId
Inner Join [ControlPanel].dbo.tblApplicationUsers us on ri.EUserId = us.UserId
Where ri.ReqInfoId={0}", reqInfoId)).ToList();
            return list;
        }
        public IEnumerable<QRCodesByRef> GetQRCodesByRefId(long? itemId, long referenceId, long orgId)
        {
            param = string.Empty;
            param += string.Format(@" and OrganizationId={0}", orgId);
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and ItemId={0}", itemId);
            }
            if (referenceId > 0)
            {
                param += string.Format(@" and ReferenceId='{0}'", referenceId);
            }

            return this._productionDb.Db.Database.SqlQuery<QRCodesByRef>(string.Format(@"Select ReferenceId,ReferenceNumber,CodeNo,ProductionFloorName,AssemblyLineName,ModelName,WarehouseName,ItemTypeName,ItemName	 
From [Production].dbo.tblQRCodeTrace
Where 1=1 {0}", Utility.ParamChecker(param))).ToList();

        }
        public IEnumerable<ReportHead> GetReportHead(long branchId, long orgId)
        {
             query = string.Format(@"Select org.OrganizationName,bh.BranchName,bh.Address,org.OrgLogoPath,bh.PhoneNo,bh.MobileNo,bh.Fax,bh.Email From 
            [ControlPanel].dbo.tblBranch bh
            inner join [ControlPanel].dbo.tblOrganizations org
        on bh.OrganizationId=org.OrganizationId
        where bh.OrganizationId={0} and bh.BranchId={1}", orgId, branchId);
            return this._controlPanelDb.Db.Database.SqlQuery<ReportHead>(query).ToList();
        }
    }
}
