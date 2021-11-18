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
    public class MiniStockTransferToSemiFinishGoodsWarehouseBusiness : IMiniStockTransferToSemiFinishGoodsWarehouseBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly IProductionAssembleStockInfoBusiness _productionAssembleStockInfoBusiness;
        private readonly IProductionAssembleStockDetailBusiness _productionAssembleStockDetailBusiness;
        private readonly MiniStockTransferToSemiFinishGoodsWarehouseRepository _miniStockTransferToSemiFinishGoodsWarehouseRepository;
        public MiniStockTransferToSemiFinishGoodsWarehouseBusiness(IProductionUnitOfWork productionDb, IProductionAssembleStockInfoBusiness productionAssembleStockInfoBusiness, IProductionAssembleStockDetailBusiness productionAssembleStockDetailBusiness)
        {
            this._productionDb = productionDb;
            this._productionAssembleStockInfoBusiness = productionAssembleStockInfoBusiness;
            this._productionAssembleStockDetailBusiness = productionAssembleStockDetailBusiness;
            this._miniStockTransferToSemiFinishGoodsWarehouseRepository = new MiniStockTransferToSemiFinishGoodsWarehouseRepository(this._productionDb);
        }

        public bool SaveMiniStockTransferToSemiFinishGoodsWarehouse(MiniStockTransferToSemiFinishGoodsWarehouseDTO dto, long userId, long orgId)
        {
            List<ProductionAssembleStockDetailDTO> stockDetails = new List<ProductionAssembleStockDetailDTO>();
            if (dto.MSTSFGWId == 0)
            {
                var miniStock = _productionAssembleStockInfoBusiness.productionAssembleStockInfoByFloorAndModelAndItem(dto.ProductionFloorId, dto.DescriptionId, dto.ItemId, orgId);
                MiniStockTransferToSemiFinishGoodsWarehouse miniStockTransferToSemiFinishGoodsWarehouse = new MiniStockTransferToSemiFinishGoodsWarehouse
                {
                    DescriptionId = dto.DescriptionId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    ItemId = dto.ItemId,
                    OrganizationId = orgId,
                    Quantity = dto.Quantity,
                    ProductionFloorId = dto.ProductionFloorId,
                    Remarks = dto.Remarks,
                    StateStatus = "Pending",
                    UnitId = miniStock.UnitId,
                    WarehouseId = miniStock.WarehouseId,
                    ItemTypeId = miniStock.ItemTypeId,
                };

                ProductionAssembleStockDetailDTO stockDetailDTO = new ProductionAssembleStockDetailDTO()
                {
                    ProductionFloorId = dto.ProductionFloorId,
                    DescriptionId = dto.DescriptionId,
                    WarehouseId = dto.WarehouseId,
                    ItemTypeId = miniStock.ItemTypeId,
                    ItemId = dto.ItemId,
                    UnitId = miniStock.UnitId,
                    UnitName = miniStock.UnitName,
                    RefferenceNumber = "TransferToWarehouse",
                    Quantity = dto.Quantity,
                };
                stockDetails.Add(stockDetailDTO);
                if (_productionAssembleStockDetailBusiness.SaveProductionAssembleStockDetailStockTransfer(stockDetails,userId,orgId))
                {
                    _miniStockTransferToSemiFinishGoodsWarehouseRepository.Insert(miniStockTransferToSemiFinishGoodsWarehouse);
                }
            }
            return _miniStockTransferToSemiFinishGoodsWarehouseRepository.Save();
        }
        public IEnumerable<MiniStockTransferToSemiFinishGoodsWarehouseDTO> GetMiniStockTransferToSemiFinishGoodsWarehouseByQuery(long? floorId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<MiniStockTransferToSemiFinishGoodsWarehouseDTO>(QueryForMiniStockTransferToSemiFinishGoodsWarehouse(floorId, modelId, warehouseId, itemTypeId, itemId, lessOrEq, orgId)).ToList();
        }

        private string QueryForMiniStockTransferToSemiFinishGoodsWarehouse(long? floorId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and  mstsgw.ProductionFloorId={0}", floorId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and mstsgw.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and mstsgw.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and mstsgw.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and mstsgw.ItemId={0}", itemId);
            }
            if (!string.IsNullOrEmpty(lessOrEq) && lessOrEq.Trim() != "")
            {
                int qty = Utility.TryParseInt(lessOrEq);
                param += string.Format(@" and  mstsgw.Quantity <= {0}", qty);
            }

            query = string.Format(@"Select mstsgw.MSTSFGWId,mstsgw.ProductionFloorId,pl.LineNumber 'FloorName',mstsgw.DescriptionId,de.DescriptionName'ModelName',mstsgw.WarehouseId,w.WarehouseName,mstsgw.ItemTypeId,it.ItemName'ItemTypeName',mstsgw.ItemId,i.ItemName,mstsgw.Quantity,mstsgw.StateStatus,mstsgw.UnitId,u.UnitSymbol 'UnitName',mstsgw.EntryDate,au.UserName'EntryUser'
From [Production].dbo.tblMiniStockTransferToSemiFinishGoodsWarehouse mstsgw
Left Join [Production].dbo.tblProductionLines pl on mstsgw.ProductionFloorId = pl.LineId
Left Join [Inventory].dbo.tblDescriptions de on mstsgw.DescriptionId = de.DescriptionId
Left Join [Inventory].dbo.tblWarehouses w on mstsgw.WarehouseId = w.Id
Left Join [Inventory].dbo.tblItemTypes it on mstsgw.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.tblItems i on mstsgw.ItemId= i.ItemId
Left Join [Inventory].dbo.tblUnits u on mstsgw.UnitId= u.UnitId 
Left Join [ControlPanel].dbo.tblApplicationUsers au on mstsgw.EUserId = au.UserId 
Where 1=1  and mstsgw.OrganizationId={0} {1}", orgId, Utility.ParamChecker(param));
            return query;
        }
    }
}
