﻿using ERPBLL.Common;
using ERPBLL.Production.Interface;
using ERPBO.Production.DTOModel;
using ERPBO.Production.DomainModels;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class FaultyItemStockDetailBusiness : IFaultyItemStockDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly FaultyItemStockInfoRepository _faultyItemStockInfoRepository;
        private readonly FaultyItemStockDetailRepository _faultyItemStockDetailRepository;
        private readonly IFaultyItemStockInfoBusiness _faultyItemStockInfoBusiness;
        public FaultyItemStockDetailBusiness(IProductionUnitOfWork productionDb, IFaultyItemStockInfoBusiness faultyItemStockInfoBusiness)
        {
            this._productionDb = productionDb;
            this._faultyItemStockInfoRepository = new FaultyItemStockInfoRepository(this._productionDb);
            this._faultyItemStockDetailRepository = new FaultyItemStockDetailRepository(this._productionDb);
            this._faultyItemStockInfoBusiness = faultyItemStockInfoBusiness;
        }

        public bool DeleteAFaultyItemByVoidItem(long stockDetailId, long userId, long orgId)
        {
            var data = GetFaultyItemDetailById(stockDetailId, orgId);
            if(data != null)
            {
               var info = _faultyItemStockInfoBusiness.GetFaultyItemStockInfoByRepairAndModelAndItem(data.RepairLineId.Value, data.DescriptionId.Value, data.ItemId.Value, orgId);
                if(info != null)
                {
                    info.StockInQty = info.StockInQty - 1;
                    _faultyItemStockInfoRepository.Update(info);
                }
                _faultyItemStockDetailRepository.Delete(data.FaultyItemStockDetailId);
            }
            return _faultyItemStockDetailRepository.Save();
        }

        public FaultyItemStockDetail GetFaultyItemDetailById(long id, long orgId)
        {
            return _faultyItemStockDetailRepository.GetOneByOrg(s => s.FaultyItemStockDetailId == id && s.OrganizationId == orgId);
        }

        public IEnumerable<FaultyItemStockDetailDTO> GetFaultyItemStockDetailsByQrCode(string QRCode, long transferId, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and fsd.OrganizationId={0}", orgId);
            if (transferId > 0)
            {
                param += string.Format(@" and fsd.TransferId={0}", transferId);
            }
            if (!string.IsNullOrEmpty(QRCode) && QRCode.Trim() != "")
            {
                param += string.Format(@" and fsd.ReferenceNumber='{0}'", QRCode);
            }

            query = string.Format(@"Select fsd.FaultyItemStockDetailId,fsd.ReferenceNumber,fsd.TransferId,fsd.TransferCode,w.Id 'WarehouseId',w.WarehouseName,it.ItemId 'ItemTypeId',
it.ItemName 'ItemTypeName',i.ItemId,i.ItemName,fsd.Quantity,fsd.IsChinaFaulty
 From [Production].dbo.tblFaultyItemStockDetail fsd
Inner Join [Inventory].dbo.tblWarehouses w on fsd.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on fsd.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on fsd.ItemId = i.ItemId
Where 1= 1 {0}", Utility.ParamChecker(param));

            var data = this._productionDb.Db.Database.SqlQuery<FaultyItemStockDetailDTO>(query).ToList();
            return data;
        }

        public FaultyItemStockDetail GetFaultyItemStockInDetailByTransferId(long transferId, string qrCode, long itemId,long orgId)
        {
            return _faultyItemStockDetailRepository.GetOneByOrg(s => s.TransferId == transferId && s.ReferenceNumber == qrCode && s.ItemId == itemId && s.OrganizationId == orgId);
        }

        public IEnumerable<FaultyItemStockDetail> GetFaultyItemStocks(long orgId)
        {
            return _faultyItemStockDetailRepository.GetAll(f => f.OrganizationId == orgId);
        }
        public bool SaveFaultyItemStockIn(List<FaultyItemStockDetailDTO> stockDetails, long userId, long orgId)
        {
            bool IsSuccess = false;
            List<FaultyItemStockDetail> faultyItemStocks = new List<FaultyItemStockDetail>();
            foreach (var item in stockDetails)
            {
                FaultyItemStockDetail faultyItem = new FaultyItemStockDetail()
                {
                    ProductionFloorId = item.ProductionFloorId,
                    AsseemblyLineId = item.AsseemblyLineId,
                    DescriptionId = item.DescriptionId,
                    QCId = item.QCId,
                    RepairLineId = item.RepairLineId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    UnitId = item.UnitId,
                    OrganizationId = orgId,
                    EUserId = userId,
                    Quantity = item.Quantity,
                    ReferenceNumber = item.ReferenceNumber,
                    Remarks = "Stock In By Repair Item Stock",
                    StockStatus = StockStatus.StockIn,
                    EntryDate = DateTime.Now,
                    TransferCode = item.TransferCode,
                    TransferId = item.TransferId,
                    IsChinaFaulty = item.IsChinaFaulty
                };
                var stockInfoInDb = this._faultyItemStockInfoBusiness.GetFaultyItemStockInfoByRepairAndModelAndItemAndFultyType(item.RepairLineId.Value, item.DescriptionId.Value, item.ItemId.Value, item.IsChinaFaulty, orgId);

                if (stockInfoInDb != null)
                {
                    stockInfoInDb.StockInQty += item.Quantity;
                    stockInfoInDb.UpdateDate = DateTime.Now;
                    stockInfoInDb.UpUserId = userId;
                    _faultyItemStockInfoRepository.Update(stockInfoInDb);
                }
                else
                {
                    FaultyItemStockInfo stockInfo = new FaultyItemStockInfo()
                    {
                        ProductionFloorId = item.ProductionFloorId,
                        DescriptionId = item.DescriptionId,
                        QCId = item.QCId,
                        RepairLineId = item.RepairLineId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        UnitId = item.UnitId,
                        OrganizationId = orgId,
                        EUserId = userId,
                        StockInQty = item.Quantity,
                        StockOutQty = 0,
                        Remarks = item.Remarks,
                        EntryDate = DateTime.Now,
                        IsChinaFaulty = item.IsChinaFaulty
                    };
                    _faultyItemStockInfoRepository.Insert(stockInfo);
                }

                faultyItemStocks.Add(faultyItem);
            }
            if (faultyItemStocks.Count > 0)
            {
                this._faultyItemStockDetailRepository.InsertAll(faultyItemStocks);
                IsSuccess = _faultyItemStockDetailRepository.Save();
            }
            return IsSuccess;
        }
        public bool SaveFaultyItemStockOut(List<FaultyItemStockDetailDTO> stockDetails, long userId, long orgId)
        {
            bool IsSuccess = false;
            List<FaultyItemStockDetail> faultyItemStocks = new List<FaultyItemStockDetail>();
            foreach (var item in stockDetails)
            {
                FaultyItemStockDetail faultyItem = new FaultyItemStockDetail()
                {
                    ProductionFloorId = item.ProductionFloorId,
                    DescriptionId = item.DescriptionId,
                    QCId = item.QCId,
                    RepairLineId = item.RepairLineId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    OrganizationId = orgId,
                    EUserId = userId,
                    Quantity = item.Quantity,
                    ReferenceNumber = item.ReferenceNumber,
                    Remarks = item.Remarks,
                    StockStatus = StockStatus.StockOut,
                    EntryDate = DateTime.Now,
                    IsChinaFaulty = item.IsChinaFaulty
                };
                var stockInfoInDb = this._faultyItemStockInfoBusiness.GetFaultyItemStockInfoByRepairAndModelAndItemAndFultyType(item.RepairLineId.Value, item.DescriptionId.Value, item.ItemId.Value, item.IsChinaFaulty, orgId);
                stockInfoInDb.StockOutQty += item.Quantity;
                stockInfoInDb.UpdateDate = DateTime.Now;
                stockInfoInDb.UpUserId = userId;
                _faultyItemStockInfoRepository.Update(stockInfoInDb);
                faultyItemStocks.Add(faultyItem);
            }
            if (faultyItemStocks.Count > 0)
            {
                this._faultyItemStockDetailRepository.InsertAll(faultyItemStocks);
                IsSuccess = _faultyItemStockDetailRepository.Save();
            }
            return IsSuccess;
        }

        
    }
}
