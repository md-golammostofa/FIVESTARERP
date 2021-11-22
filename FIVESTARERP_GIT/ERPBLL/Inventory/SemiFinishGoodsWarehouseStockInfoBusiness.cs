using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using ERPDAL.InventoryDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
    public class SemiFinishGoodsWarehouseStockInfoBusiness : ISemiFinishGoodsWarehouseStockInfoBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryDB;
        private readonly SemiFinishGoodsWarehouseStockInfoRepository _semiFinishGoodsWarehouseStockInfoRepository;
        private readonly SemiFinishGoodsWarehouseStockDetailRepository _semiFinishGoodsWarehouseStockDetailRepository;
        public SemiFinishGoodsWarehouseStockInfoBusiness(IInventoryUnitOfWork inventoryDB)
        {
            this._inventoryDB = inventoryDB;
            this._semiFinishGoodsWarehouseStockDetailRepository = new SemiFinishGoodsWarehouseStockDetailRepository(this._inventoryDB);
            this._semiFinishGoodsWarehouseStockInfoRepository = new SemiFinishGoodsWarehouseStockInfoRepository(this._inventoryDB);
        }
        public IEnumerable<SemiFinishGoodsWarehouseStockInfo> GetAllSemiFinishGoodStockByOrgId(long orgId)
        {
            return _semiFinishGoodsWarehouseStockInfoRepository.GetAll(s => s.OrganizationId == orgId).ToList();
        }
        public bool SaveSemiFinishGoodStockIn(List<SemiFinishGoodsWarehouseStockDetailDTO> dTOs, long userId, long orgId)
        {
            List<SemiFinishGoodsWarehouseStockDetail> stockDetails = new List<SemiFinishGoodsWarehouseStockDetail>();
            foreach (var item in dTOs)
            {
                SemiFinishGoodsWarehouseStockDetail stockDetail = new SemiFinishGoodsWarehouseStockDetail
                {
                    DescriptionId = item.DescriptionId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    ItemId = item.ItemId,
                    ItemTypeId = item.ItemTypeId,
                    OrganizationId = orgId,
                    ProductionFloorId = item.ProductionFloorId,
                    Quantity = item.Quantity,
                    Remarks = item.Remarks,
                    StockStatus = StockStatus.StockIn,
                    UnitId = item.UnitId,
                    WarehouseId = item.WarehouseId,
                };

                var info = this.GetAllSemiFinishGoodStockByOrgId(orgId).Where(s => s.ProductionFloorId == item.ProductionFloorId && s.DescriptionId == item.DescriptionId && s.ItemId == item.ItemId).FirstOrDefault();
                if (info != null)
                {
                    info.StockInQty += item.Quantity;
                    info.UpdateDate = DateTime.Now;
                    info.UpUserId = userId;
                    _semiFinishGoodsWarehouseStockInfoRepository.Update(info);
                    if (_semiFinishGoodsWarehouseStockInfoRepository.Save())
                    {
                        stockDetail.SemiFinishGoodsStockInfoId = info.SemiFinishGoodsStockInfoId;
                    }
                }
                else
                {
                    SemiFinishGoodsWarehouseStockInfo stockInfo = new SemiFinishGoodsWarehouseStockInfo
                    {
                        DescriptionId = item.DescriptionId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        ItemId = item.ItemId,
                        ItemTypeId = item.ItemTypeId,
                        OrganizationId = orgId,
                        ProductionFloorId = item.ProductionFloorId,
                        Remarks = item.Remarks,
                        UnitId = item.UnitId,
                        WarehouseId = item.WarehouseId,
                        StockInQty = item.Quantity,
                        StockOutQty = 0,
                    };
                    _semiFinishGoodsWarehouseStockInfoRepository.Insert(stockInfo);
                    if (_semiFinishGoodsWarehouseStockInfoRepository.Save())
                    {
                        stockDetail.SemiFinishGoodsStockInfoId = stockInfo.SemiFinishGoodsStockInfoId;
                    }
                }

                stockDetails.Add(stockDetail);
            }
            _semiFinishGoodsWarehouseStockDetailRepository.InsertAll(stockDetails);
            return _semiFinishGoodsWarehouseStockDetailRepository.Save();
        }
        public bool SaveSemiFinishGoodStockOut(List<SemiFinishGoodsWarehouseStockDetailDTO> dTOs, long userId, long orgId)
        {
            List<SemiFinishGoodsWarehouseStockDetail> stockDetails = new List<SemiFinishGoodsWarehouseStockDetail>();
            foreach (var item in dTOs)
            {
                SemiFinishGoodsWarehouseStockDetail stockDetail = new SemiFinishGoodsWarehouseStockDetail
                {
                    DescriptionId = item.DescriptionId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    ItemId = item.ItemId,
                    ItemTypeId = item.ItemTypeId,
                    OrganizationId = orgId,
                    ProductionFloorId = item.ProductionFloorId,
                    Quantity = item.Quantity,
                    Remarks = item.Remarks,
                    StockStatus = StockStatus.StockOut,
                    UnitId = item.UnitId,
                    WarehouseId = item.WarehouseId,
                };

                var info = this.GetAllSemiFinishGoodStockByOrgId(orgId).Where(s => s.ProductionFloorId == item.ProductionFloorId && s.DescriptionId == item.DescriptionId && s.ItemId == item.ItemId).FirstOrDefault();
                if (info != null)
                {
                    info.StockOutQty += item.Quantity;
                    info.UpdateDate = DateTime.Now;
                    info.UpUserId = userId;
                    _semiFinishGoodsWarehouseStockInfoRepository.Update(info);
                    if (_semiFinishGoodsWarehouseStockInfoRepository.Save())
                    {
                        stockDetail.SemiFinishGoodsStockInfoId = info.SemiFinishGoodsStockInfoId;
                    }
                }
                stockDetails.Add(stockDetail);
            }
            _semiFinishGoodsWarehouseStockDetailRepository.InsertAll(stockDetails);
            return _semiFinishGoodsWarehouseStockDetailRepository.Save();
        }
        public IEnumerable<SemiFinishGoodsWarehouseStockInfoDTO> GetSemiFinishGoodsStockByQuery(long? floorId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string lessOrEq, long orgId)
        {
            return this._inventoryDB.Db.Database.SqlQuery<SemiFinishGoodsWarehouseStockInfoDTO>(QueryForSemiFinishGoodsStock(floorId, modelId, warehouseId, itemTypeId, itemId, status, lessOrEq, orgId)).ToList();
        }

        private string QueryForSemiFinishGoodsStock(long? floorId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string lessOrEq, long orgId)
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
            //if (!string.IsNullOrEmpty(status) && status.Trim() != "")
            //{
            //    param += string.Format(@" and mstsgw.StateStatus='{0}'", status);
            //}
            if (!string.IsNullOrEmpty(lessOrEq) && lessOrEq.Trim() != "")
            {
                int qty = Utility.TryParseInt(lessOrEq);
                param += string.Format(@" and  (mstsgw.StockInQty - mstsgw.StockOutQty) <= {0}", qty);
            }

            query = string.Format(@"Select mstsgw.SemiFinishGoodsStockInfoId,mstsgw.ProductionFloorId,pl.LineNumber'FloorName',mstsgw.DescriptionId,de.DescriptionName'ModelName',mstsgw.WarehouseId,w.WarehouseName,mstsgw.ItemTypeId,it.ItemName'ItemTypeName',mstsgw.ItemId,i.ItemName,mstsgw.UnitId,u.UnitSymbol 'UnitName',mstsgw.EntryDate,au.UserName'EntryUser',mstsgw.StockInQty, mstsgw.StockOutQty
From [Inventory].dbo.tblSemiFinishGoodsWarehouseStockInfo mstsgw
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
