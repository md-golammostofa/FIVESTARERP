﻿using ERPBLL.Production.Interface;
using ERPBO.Common;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class PackagingLineBusiness : IPackagingLineBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly PackagingLineRepository _packagingLineRepository;
        public PackagingLineBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._packagingLineRepository = new PackagingLineRepository(this._productionDb);
        }
        public PackagingLine GetPackagingLineById(long id, long orgId)
        {
            return _packagingLineRepository.GetOneByOrg(r => r.PackagingLineId == id && r.OrganizationId == orgId);
        }
        public IEnumerable<PackagingLine> GetPackagingLinesByOrgId(long orgId)
        {
            return _packagingLineRepository.GetAll(r => r.OrganizationId == orgId);
        }
        public bool SavePackagingLine(PackagingLineDTO dto, long userId, long orgId)
        {
            if(dto.PackagingLineId == 0)
            {
                PackagingLine pacakgingLine = new PackagingLine
                {
                    PackagingLineName = dto.PackagingLineName,
                    IsActive = dto.IsActive,
                    Remarks = dto.Remarks,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    ProductionLineId = dto.ProductionLineId,
                    OrganizationId = orgId
                };
                _packagingLineRepository.Insert(pacakgingLine);
            }
            else
            {
                var pacakgingLineInDb = GetPackagingLineById(dto.PackagingLineId, orgId);
                if(pacakgingLineInDb != null)
                {
                    pacakgingLineInDb.PackagingLineName = dto.PackagingLineName;
                    pacakgingLineInDb.IsActive = dto.IsActive;
                    pacakgingLineInDb.Remarks = dto.Remarks;
                    pacakgingLineInDb.UpUserId = userId;
                    pacakgingLineInDb.UpdateDate = DateTime.Now;
                    _packagingLineRepository.Update(pacakgingLineInDb);
                }
            }
            return _packagingLineRepository.Save();
        }
        public IEnumerable<Dropdown> GetPackagingLinesWithProduction(long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<Dropdown>(string.Format(@"Select (pac.PackagingLineName+' ['+pl.LineNumber+']') 'text'
, Cast(pac.PackagingLineId as Nvarchar(50))+'#'+Cast(pl.LineId as Nvarchar(50)) 'value'
From [Production].dbo.tblPackagingLine pac
Inner Join  [Production].dbo.tblProductionLines pl on pl.LineId = pac.ProductionLineId
Where 1=1 and pac.OrganizationId={0}", orgId)).ToList();
        }

        public IEnumerable<PackagingLineDashboardDTO> GetPackagingLineDashboard(long orgId)
        {
            var q = string.Format(@"Select PackagingLineId,PackagingLineName,IMEIWrite,QCPassQty,QCFailQty,Cartoon From (Select PackagingLineId,PackagingLineName,

(Select ISNULL(Count(IMEI),0) From tblTempQRCodeTrace
Where IMEI is not null and PackagingLineId=pck.PackagingLineId and OrganizationId={0})'IMEIWrite',

(Select ISNULL(Sum(TotalQty),0) From tblFinishGoodsSendToWarehouseInfo
 Where PackagingLineId= pck.PackagingLineId and StateStatus='Received' and OrganizationId={0})'Cartoon',

(Select ISNULL(Count(StateStatus),0) From tblTempQRCodeTrace
Where (StateStatus='Finish' or StateStatus='Carton') and PackagingLineId=pck.PackagingLineId and OrganizationId={0})'QCPassQty',

 (Select ISNULL(Count(StateStatus),0) From tblTempQRCodeTrace
 Where (StateStatus='Packaging-Repair') and PackagingLineId=pck.PackagingLineId and OrganizationId={0})'QCFailQty'

 From tblPackagingLine pck) tbl Where 1=1 and IMEIWrite > 0
", orgId);
            var data = this._productionDb.Db.Database.SqlQuery<PackagingLineDashboardDTO>(q).ToList();
            return data;
        }
    }
}
