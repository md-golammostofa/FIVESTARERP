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
    public class HalfDoneWarehouseStockInfoBusiness : IHalfDoneWarehouseStockInfoBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryDB;
        private readonly HalfDoneWarehouseStockInfoRepository _halfDoneWarehouseStockInfoRepository;
        private readonly HalfDoneWarehouseStockDetailRepository _halfDoneWarehouseStockDetailRepository;
        public HalfDoneWarehouseStockInfoBusiness(IInventoryUnitOfWork inventoryDB)
        {
            this._inventoryDB = inventoryDB;
            this._halfDoneWarehouseStockDetailRepository = new HalfDoneWarehouseStockDetailRepository(this._inventoryDB);
            this._halfDoneWarehouseStockInfoRepository = new HalfDoneWarehouseStockInfoRepository(this._inventoryDB);
        }

        public IEnumerable<HalfDoneWarehouseStockInfo> GetAllHalfDoneStockInfoByOrgId(long orgId)
        {
            return _halfDoneWarehouseStockInfoRepository.GetAll(s => s.OrganizationId == orgId);
        }
        public bool SaveHalfDoneWarehouseStockIn(List<HalfDoneWarehouseStockDetailDTO> dTOs, long userId, long orgId)
        {
            List<HalfDoneWarehouseStockDetail> stockDetails = new List<HalfDoneWarehouseStockDetail>();
            foreach (var item in dTOs)
            {
                HalfDoneWarehouseStockDetail stockDetail = new HalfDoneWarehouseStockDetail
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
                    WarehouseId = item.WarehouseId,
                    AssemblyLineId = item.AssemblyLineId,
                    QCId = item.QCId,
                    RepairLineId = item.RepairLineId,
                };

                var info = this.GetAllHalfDoneStockInfoByOrgId(orgId).Where(s => s.ProductionFloorId == item.ProductionFloorId && s.AssemblyLineId == item.AssemblyLineId && s.QCId == item.QCId && s.RepairLineId == item.RepairLineId && s.WarehouseId == item.WarehouseId && s.DescriptionId == item.DescriptionId).FirstOrDefault();
                if (info != null)
                {
                    info.StockInQty += item.Quantity;
                    info.UpdateDate = DateTime.Now;
                    info.UpUserId = userId;

                    _halfDoneWarehouseStockInfoRepository.Update(info);
                    if (_halfDoneWarehouseStockInfoRepository.Save())
                    {
                        stockDetail.HalfDoneStockInfoId = info.HalfDoneStockInfoId;
                    }
                }
                else
                {
                    HalfDoneWarehouseStockInfo stockInfo = new HalfDoneWarehouseStockInfo
                    {
                        DescriptionId = item.DescriptionId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        ItemId = item.ItemId,
                        ItemTypeId = item.ItemTypeId,
                        OrganizationId = orgId,
                        ProductionFloorId = item.ProductionFloorId,
                        StockInQty = item.Quantity,
                        StockOutQty = 0,
                        Remarks = item.Remarks,
                        WarehouseId = item.WarehouseId,
                        AssemblyLineId = item.AssemblyLineId,
                        QCId = item.QCId,
                        RepairLineId = item.RepairLineId,
                    };
                    _halfDoneWarehouseStockInfoRepository.Insert(stockInfo);
                    if (_halfDoneWarehouseStockInfoRepository.Save())
                    {
                        stockDetail.HalfDoneStockInfoId = stockInfo.HalfDoneStockInfoId;
                    }
                }
                stockDetails.Add(stockDetail);
            }
            _halfDoneWarehouseStockDetailRepository.InsertAll(stockDetails);
            return _halfDoneWarehouseStockDetailRepository.Save();
        }
        public bool SaveHalfDoneWarehouseStockOut(List<HalfDoneWarehouseStockDetailDTO> dTOs, long userId, long orgId)
        {
            List<HalfDoneWarehouseStockDetail> stockDetails = new List<HalfDoneWarehouseStockDetail>();
            foreach (var item in dTOs)
            {
                HalfDoneWarehouseStockDetail stockDetail = new HalfDoneWarehouseStockDetail
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
                    WarehouseId = item.WarehouseId,
                    AssemblyLineId = item.AssemblyLineId,
                    QCId = item.QCId,
                    RepairLineId = item.RepairLineId,
                };

                var info = this.GetAllHalfDoneStockInfoByOrgId(orgId).Where(s => s.ProductionFloorId == item.ProductionFloorId && s.AssemblyLineId == item.AssemblyLineId && s.QCId == item.QCId && s.RepairLineId == item.RepairLineId && s.WarehouseId == item.WarehouseId && s.DescriptionId == item.DescriptionId).FirstOrDefault();
                if (info != null)
                {
                    info.StockOutQty += item.Quantity;
                    info.UpdateDate = DateTime.Now;
                    info.UpUserId = userId;

                    _halfDoneWarehouseStockInfoRepository.Update(info);
                    if (_halfDoneWarehouseStockInfoRepository.Save())
                    {
                        stockDetail.HalfDoneStockInfoId = info.HalfDoneStockInfoId;
                    }
                }
                stockDetails.Add(stockDetail);
            }
            _halfDoneWarehouseStockDetailRepository.InsertAll(stockDetails);
            return _halfDoneWarehouseStockDetailRepository.Save();
        }
        public IEnumerable<HalfDoneWarehouseStockInfoDTO> GetHalfDoneStockByQuery(long? floorId, long? assemblyId, long? qcId, long? repairId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            return this._inventoryDB.Db.Database.SqlQuery<HalfDoneWarehouseStockInfoDTO>(QueryForHalfDoneStock(floorId, assemblyId, qcId, repairId, modelId, warehouseId, itemTypeId, itemId, lessOrEq, orgId)).ToList();
        }

        private string QueryForHalfDoneStock(long? floorId, long? assemblyId, long? qcId, long? repairId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and  mstsgw.ProductionFloorId={0}", floorId);
            }
            if (assemblyId != null && assemblyId > 0)
            {
                param += string.Format(@" and  mstsgw.AssemblyLineId={0}", assemblyId);
            }
            if (qcId != null && qcId > 0)
            {
                param += string.Format(@" and  mstsgw.QCId={0}", qcId);
            }
            if (repairId != null && repairId > 0)
            {
                param += string.Format(@" and  mstsgw.RepairLineId={0}", repairId);
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
                param += string.Format(@" and  (mstsgw.StockInQty - mstsgw.StockOutQty) <= {0}", qty);
            }

            query = string.Format(@"Select mstsgw.HalfDoneStockInfoId,mstsgw.ProductionFloorId,pl.LineNumber'FloorName',mstsgw.AssemblyLineId,asl.AssemblyLineName,mstsgw.QCId,qc.QCName,mstsgw.RepairLineId,rp.RepairLineName,mstsgw.DescriptionId,de.DescriptionName'ModelName',mstsgw.WarehouseId,w.WarehouseName,mstsgw.ItemTypeId,it.ItemName'ItemTypeName',mstsgw.ItemId,i.ItemName,mstsgw.EntryDate,au.UserName'EntryUser',mstsgw.StockInQty, mstsgw.StockOutQty
From [Inventory].dbo.tblHalfDoneWarehouseStockInfo mstsgw
Left Join [Production].dbo.tblProductionLines pl on mstsgw.ProductionFloorId = pl.LineId
Left Join [Production].dbo.tblAssemblyLines asl on mstsgw.AssemblyLineId=asl.AssemblyLineId
Left Join [Production].dbo.tblQualityControl qc on mstsgw.QCId=qc.QCId
Left Join [Production].dbo.tblRepairLine rp on mstsgw.RepairLineId=rp.RepairLineId
Left Join [Inventory].dbo.tblDescriptions de on mstsgw.DescriptionId = de.DescriptionId
Left Join [Inventory].dbo.tblWarehouses w on mstsgw.WarehouseId = w.Id
Left Join [Inventory].dbo.tblItemTypes it on mstsgw.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.tblItems i on mstsgw.ItemId= i.ItemId
Left Join [ControlPanel].dbo.tblApplicationUsers au on mstsgw.EUserId = au.UserId 
Where 1=1  and mstsgw.OrganizationId={0} {1}", orgId, Utility.ParamChecker(param));
            return query;
        }
    }
}
