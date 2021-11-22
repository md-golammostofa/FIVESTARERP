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
    public class MiniStockRequisitionToSemiFinishGoodsWarehouseDetailBusiness : IMiniStockRequisitionToSemiFinishGoodsWarehouseDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly MiniStockRequisitionToSemiFinishGoodsWarehouseInfoRepository _miniStockRequisitionToSemiFinishGoodsWarehouseInfoRepository;
        private readonly MiniStockRequisitionToSemiFinishGoodsWarehouseDetailRepository _miniStockRequisitionToSemiFinishGoodsWarehouseDetailRepository;
        public MiniStockRequisitionToSemiFinishGoodsWarehouseDetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            _miniStockRequisitionToSemiFinishGoodsWarehouseInfoRepository = new MiniStockRequisitionToSemiFinishGoodsWarehouseInfoRepository(this._productionDb);
            _miniStockRequisitionToSemiFinishGoodsWarehouseDetailRepository = new MiniStockRequisitionToSemiFinishGoodsWarehouseDetailRepository(this._productionDb);
        }
        public IEnumerable<MiniStockRequisitionToSemiFinishGoodsWarehouseDetail> GetAllRequisitionDetails(long orgId)
        {
            return _miniStockRequisitionToSemiFinishGoodsWarehouseDetailRepository.GetAll(s => s.OrganizationId == orgId);
        }
        public IEnumerable<MiniStockRequisitionToSemiFinishGoodsWarehouseDetailDTO> GetMiniStockRequisitionDetailByQuery(long? reqInfoId, long? floorId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<MiniStockRequisitionToSemiFinishGoodsWarehouseDetailDTO>(QueryForMiniStockRequisitionDetail(reqInfoId, floorId, modelId, warehouseId, itemTypeId, itemId, orgId)).ToList();
        }

        private string QueryForMiniStockRequisitionDetail(long? reqInfoId, long? floorId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (reqInfoId != null && reqInfoId > 0)
            {
                param += string.Format(@" and  msrsf.RequisitionInfoId={0}", reqInfoId);
            }
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and  msrsf.ProductionFloorId={0}", floorId);
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

            query = string.Format(@"Select 
msrsf.ProductionFloorId,pl.LineNumber 'FloorName',msrsf.Quantity,msrsf.RequisitionDetailId,msrsf.ProductionFloorId,
msrsf.DescriptionId,de.DescriptionName 'ModelName',msrsf.WarehouseId,w.WarehouseName,msrsf.ItemTypeId,it.ItemName 'ItemTypeName',msrsf.ItemId,i.ItemName,msrsf.UnitId,u.UnitSymbol 'UnitName'
From [Production].dbo.tblMiniStockRequisitionToSemiFinishGoodsWarehouseDetail msrsf
Left Join [Production].dbo.tblProductionLines pl on msrsf.ProductionFloorId = pl.LineId
Left Join [Inventory].dbo.tblDescriptions de on msrsf.DescriptionId = de.DescriptionId
Left Join [Inventory].dbo.tblWarehouses w on msrsf.WarehouseId = w.Id
Left Join [Inventory].dbo.tblItemTypes it on msrsf.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.tblItems i on msrsf.ItemId= i.ItemId
Left Join [Inventory].dbo.tblUnits u on msrsf.UnitId= u.UnitId
Left Join [ControlPanel].dbo.tblApplicationUsers au on msrsf.EUserId = au.UserId 
Where 1=1  and msrsf.OrganizationId={0} {1}", orgId, Utility.ParamChecker(param));
            return query;
        }
    }
}
