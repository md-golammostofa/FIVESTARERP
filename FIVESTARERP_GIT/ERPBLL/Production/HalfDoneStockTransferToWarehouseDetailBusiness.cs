using ERPBLL.Common;
using ERPBLL.Production.Interface;
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
    public class HalfDoneStockTransferToWarehouseDetailBusiness: IHalfDoneStockTransferToWarehouseDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly HalfDoneStockTransferToWarehouseDetailsRepository _transferToWarehouseDetailsRepository;
        public HalfDoneStockTransferToWarehouseDetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._transferToWarehouseDetailsRepository = new HalfDoneStockTransferToWarehouseDetailsRepository(this._productionDb);
        }
        public IEnumerable<HalfDoneStockTransferToWarehouseDetail> GetAllTransferDetails(long orgId)
        {
            return _transferToWarehouseDetailsRepository.GetAll(s => s.OrganizationId == orgId);
        }
        public IEnumerable<HalfDoneStockTransferToWarehouseDetailDTO> GetHalfDoneTransferDetailByQuery(long? transferInfoId, long? floorId, long? assemblyId, long? qcId, long? repairId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<HalfDoneStockTransferToWarehouseDetailDTO>(QueryForHalfDoneTransferDetail(transferInfoId, floorId, assemblyId,qcId,repairId, modelId, warehouseId, itemTypeId, itemId, orgId)).ToList();
        }

        private string QueryForHalfDoneTransferDetail(long? transferInfoId, long? floorId, long? assemblyId, long? qcId, long? repairId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (transferInfoId != null && transferInfoId > 0)
            {
                param += string.Format(@" and  msrsf.HalfDoneTransferInfoId={0}", transferInfoId);
            }
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and  msrsf.ProductionFloorId={0}", floorId);
            }
            if (assemblyId != null && assemblyId > 0)
            {
                param += string.Format(@" and  msrsf.AssemblyLineId={0}", assemblyId);
            }
            if (qcId != null && qcId > 0)
            {
                param += string.Format(@" and  msrsf.QCId={0}", qcId);
            }
            if (repairId != null && repairId > 0)
            {
                param += string.Format(@" and  msrsf.RepairLineId={0}", repairId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and msrsf.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and msrsf.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and msrsf.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and msrsf.ItemId={0}", itemId);
            }

            query = string.Format(@"Select msrsf.HalfDoneTransferInfoId,msrsf.ProductionFloorId,pl.LineNumber'FloorName',msrsf.AssemblyLineId,asl.AssemblyLineName,msrsf.QCId,qc.QCName,msrsf.RepairLineId,rp.RepairLineName,msrsf.Quantity,msrsf.HalfDoneTransferDetailId,msrsf.DescriptionId,de.DescriptionName 'ModelName',msrsf.WarehouseId,w.WarehouseName,msrsf.ItemTypeId,it.ItemName 'ItemTypeName',msrsf.ItemId,i.ItemName
From [Production].dbo.tblHalfDoneStockTransferToWarehouseDetail msrsf
Left Join [Production].dbo.tblProductionLines pl on msrsf.ProductionFloorId = pl.LineId
Left Join [Production].dbo.tblAssemblyLines asl on msrsf.AssemblyLineId=asl.AssemblyLineId
Left Join [Production].dbo.tblQualityControl qc on msrsf.QCId=qc.QCId
Left Join [Production].dbo.tblRepairLine rp on msrsf.RepairLineId=rp.RepairLineId
Left Join [Inventory].dbo.tblDescriptions de on msrsf.DescriptionId = de.DescriptionId
Left Join [Inventory].dbo.tblWarehouses w on msrsf.WarehouseId = w.Id
Left Join [Inventory].dbo.tblItemTypes it on msrsf.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.tblItems i on msrsf.ItemId= i.ItemId
--Left Join [Inventory].dbo.tblUnits u on msrsf.UnitId= u.UnitId
Left Join [ControlPanel].dbo.tblApplicationUsers au on msrsf.EUserId = au.UserId 
Where 1=1  and msrsf.OrganizationId={0} {1}", orgId, Utility.ParamChecker(param));
            return query;
        }
    }
}
