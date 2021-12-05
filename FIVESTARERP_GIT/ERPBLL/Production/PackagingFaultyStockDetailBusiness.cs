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
    public class PackagingFaultyStockDetailBusiness : IPackagingFaultyStockDetailBusiness
    {
        // Database
        private readonly IProductionUnitOfWork _productionDb;
        // Business Class
        private readonly IPackagingFaultyStockInfoBusiness _packagingFaultyStockInfoBusiness;
        // Repository
        private readonly PackagingFaultyStockDetailRepository _packagingFaultyStockDetailRepository;
        private readonly PackagingFaultyStockInfoRepository _packagingFaultyStockInfoRepository;
        public PackagingFaultyStockDetailBusiness(IProductionUnitOfWork productionDb, IPackagingFaultyStockInfoBusiness packagingFaultyStockInfoBusiness)
        {
            this._productionDb = productionDb;
            this._packagingFaultyStockInfoBusiness = packagingFaultyStockInfoBusiness;
            // Repository Initialization //
            this._packagingFaultyStockDetailRepository = new PackagingFaultyStockDetailRepository
                (this._productionDb);
            this._packagingFaultyStockInfoRepository = new PackagingFaultyStockInfoRepository
             (this._productionDb);
        }
        public IEnumerable<PackagingFaultyStockDetailDTO> GetPackagingFaultyItemStockDetailsByQrCode(string QRCode, string imei, long transferId, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;
            if (transferId > 0)
            {
                param += string.Format(@" and pfs.TransferId={0}", transferId);
            }
            if (!string.IsNullOrEmpty(QRCode) && QRCode.Trim() != "")
            {
                param += string.Format(@" and pfs.ReferenceNumber='{0}'", QRCode);
            }

            query = string.Format(@"Select pfs.PackagingFaultyStockDetailId,pfs.ReferenceNumber,pfs.TransferId,pfs.TransferCode,w.Id 'WarehouseId',w.WarehouseName,it.ItemId 'ItemTypeId',
it.ItemName 'ItemTypeName',i.ItemId,i.ItemName,pfs.Quantity,pfs.IsChinaFaulty
 From [Production].dbo.tblPackagingFaultyStockDetail pfs
Inner Join [Inventory].dbo.tblWarehouses w on pfs.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on pfs.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on pfs.ItemId = i.ItemId
Where 1= 1 and pfs.OrganizationId={0} {1}", orgId, Utility.ParamChecker(param));

            var data = this._productionDb.Db.Database.SqlQuery<PackagingFaultyStockDetailDTO>(query).ToList();
            return data;
        }

        public IEnumerable<PackagingFaultyStockDetail> GetPackagingFaultyItemStocks(long orgId)
        {
            return _packagingFaultyStockDetailRepository.GetAll(f => f.OrganizationId == orgId);
        }

        public bool SavePackagingFaultyItemStockIn(List<PackagingFaultyStockDetailDTO> stockDetails, long userId, long orgId)
        {
            bool IsSuccess = false;
            List<PackagingFaultyStockDetail> faultyItemStocks = new List<PackagingFaultyStockDetail>();
            foreach (var item in stockDetails)
            {
                PackagingFaultyStockDetail faultyItem = new PackagingFaultyStockDetail()
                {
                    ProductionFloorId = item.ProductionFloorId,
                    PackagingLineId = item.PackagingLineId,
                    DescriptionId = item.DescriptionId,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    UnitId = item.UnitId,
                    OrganizationId = orgId,
                    EUserId = userId,
                    Quantity = item.Quantity,
                    ReferenceNumber = item.ReferenceNumber,
                    Remarks = item.Remarks,
                    StockStatus = StockStatus.StockIn,
                    EntryDate = DateTime.Now,
                    TransferCode = item.TransferCode,
                    TransferId = item.TransferId,
                    IsChinaFaulty = item.IsChinaFaulty,
                };
                if (item.IsChinaFaulty)
                {
                    var stockInfoInDb = this._packagingFaultyStockInfoBusiness.GetPackagingFaultyStockInfoByRepairAndModelAndItemAndFultyType(item.PackagingLineId.Value, item.DescriptionId.Value, item.ItemId.Value, item.IsChinaFaulty, orgId);
                    if (stockInfoInDb != null)
                    {
                        stockInfoInDb.ChinaMadeFaultyStockInQty += item.Quantity;
                        stockInfoInDb.UpdateDate = DateTime.Now;
                        stockInfoInDb.UpUserId = userId;
                        _packagingFaultyStockInfoRepository.Update(stockInfoInDb);
                        if (_packagingFaultyStockInfoRepository.Save())
                        {
                            faultyItem.PackagingFaultyStockInfoId = stockInfoInDb.PackagingFaultyStockInfoId;
                        }
                    }
                    else
                    {
                        PackagingFaultyStockInfo stockInfo = new PackagingFaultyStockInfo()
                        {
                            ProductionFloorId = item.ProductionFloorId,
                            PackagingLineId = item.PackagingLineId,
                            DescriptionId = item.DescriptionId,
                            WarehouseId = item.WarehouseId,
                            ItemTypeId = item.ItemTypeId,
                            ItemId = item.ItemId,
                            UnitId = item.UnitId,
                            OrganizationId = orgId,
                            EUserId = userId,
                            ChinaMadeFaultyStockInQty = item.Quantity,
                            ChinaMadeFaultyStockOutQty = 0,
                            ManMadeFaultyStockInQty = 0,
                            ManMadeFaultyStockOutQty = 0,
                            Remarks = item.Remarks,
                            EntryDate = DateTime.Now,
                        };
                        _packagingFaultyStockInfoRepository.Insert(stockInfo);
                        if (_packagingFaultyStockInfoRepository.Save())
                        {
                            faultyItem.PackagingFaultyStockInfoId = stockInfo.PackagingFaultyStockInfoId;
                        }
                    }
                }
                else
                {
                    var stockInfoInDb = this._packagingFaultyStockInfoBusiness.GetPackagingFaultyStockInfoByRepairAndModelAndItemAndFultyType(item.PackagingLineId.Value, item.DescriptionId.Value, item.ItemId.Value, item.IsChinaFaulty, orgId);
                    if (stockInfoInDb != null)
                    {
                        stockInfoInDb.ManMadeFaultyStockInQty += item.Quantity;
                        stockInfoInDb.UpdateDate = DateTime.Now;
                        stockInfoInDb.UpUserId = userId;
                        _packagingFaultyStockInfoRepository.Update(stockInfoInDb);
                        if (_packagingFaultyStockInfoRepository.Save())
                        {
                            faultyItem.PackagingFaultyStockInfoId = stockInfoInDb.PackagingFaultyStockInfoId;
                        }
                    }
                    else
                    {
                        PackagingFaultyStockInfo stockInfo = new PackagingFaultyStockInfo()
                        {
                            ProductionFloorId = item.ProductionFloorId,
                            PackagingLineId = item.PackagingLineId,
                            DescriptionId = item.DescriptionId,
                            WarehouseId = item.WarehouseId,
                            ItemTypeId = item.ItemTypeId,
                            ItemId = item.ItemId,
                            UnitId = item.UnitId,
                            OrganizationId = orgId,
                            EUserId = userId,
                            ChinaMadeFaultyStockInQty = 0,
                            ChinaMadeFaultyStockOutQty = 0,
                            ManMadeFaultyStockInQty = item.Quantity,
                            ManMadeFaultyStockOutQty = 0,
                            Remarks = item.Remarks,
                            EntryDate = DateTime.Now,
                        };
                        _packagingFaultyStockInfoRepository.Insert(stockInfo);
                        if (_packagingFaultyStockInfoRepository.Save())
                        {
                            faultyItem.PackagingFaultyStockInfoId = stockInfo.PackagingFaultyStockInfoId;
                        }
                    }
                }
                faultyItemStocks.Add(faultyItem);
            }
            if (faultyItemStocks.Count > 0)
            {
                this._packagingFaultyStockDetailRepository.InsertAll(faultyItemStocks);
                IsSuccess = _packagingFaultyStockDetailRepository.Save();
            }
            return IsSuccess;
        }

        public bool SavePackagingFaultyItemStockOut(List<PackagingFaultyStockDetailDTO> stockDetails, long userId, long orgId)
        {
            bool IsSuccess = false;
            List<PackagingFaultyStockDetail> faultyItemStocks = new List<PackagingFaultyStockDetail>();
            foreach (var item in stockDetails)
            {
                PackagingFaultyStockDetail faultyItem = new PackagingFaultyStockDetail()
                {
                    ProductionFloorId = item.ProductionFloorId,
                    DescriptionId = item.DescriptionId,
                    PackagingLineId = item.PackagingLineId,
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
                if (item.IsChinaFaulty)
                {
                    var stockInfoInDb = this._packagingFaultyStockInfoBusiness.GetPackagingFaultyStockInfoByRepairAndModelAndItemAndFultyType(item.PackagingLineId.Value, item.DescriptionId.Value, item.ItemId.Value, item.IsChinaFaulty, orgId);

                    stockInfoInDb.ChinaMadeFaultyStockOutQty += item.Quantity;
                    stockInfoInDb.UpdateDate = DateTime.Now;
                    stockInfoInDb.UpUserId = userId;
                    _packagingFaultyStockInfoRepository.Update(stockInfoInDb);
                    if (_packagingFaultyStockInfoRepository.Save())
                    {
                        faultyItem.PackagingFaultyStockInfoId = stockInfoInDb.PackagingFaultyStockInfoId;
                    }
                }
                else
                {
                    var stockInfoInDb = this._packagingFaultyStockInfoBusiness.GetPackagingFaultyStockInfoByRepairAndModelAndItemAndFultyType(item.PackagingLineId.Value, item.DescriptionId.Value, item.ItemId.Value, item.IsChinaFaulty, orgId);

                    stockInfoInDb.ManMadeFaultyStockOutQty += item.Quantity;
                    stockInfoInDb.UpdateDate = DateTime.Now;
                    stockInfoInDb.UpUserId = userId;
                    _packagingFaultyStockInfoRepository.Update(stockInfoInDb);
                    if (_packagingFaultyStockInfoRepository.Save())
                    {
                        faultyItem.PackagingFaultyStockInfoId = stockInfoInDb.PackagingFaultyStockInfoId;
                    }
                }
                faultyItemStocks.Add(faultyItem);
            }
            if (faultyItemStocks.Count > 0)
            {
                this._packagingFaultyStockDetailRepository.InsertAll(faultyItemStocks);
                IsSuccess = _packagingFaultyStockDetailRepository.Save();
            }
            return IsSuccess;
        }
        public bool SaveFaultyStockReturn(List<PackagingFaultyStockDetailDTO> stockDetails, long userId, long orgId)
        {
            bool IsSuccess = false;
            List<PackagingFaultyStockDetail> faultyItemStocks = new List<PackagingFaultyStockDetail>();
            foreach (var item in stockDetails)
            {
                PackagingFaultyStockDetail faultyItem = new PackagingFaultyStockDetail()
                {
                    ProductionFloorId = item.ProductionFloorId,
                    DescriptionId = item.DescriptionId,
                    PackagingLineId = item.PackagingLineId,
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
                var stockInfoInDb = this._packagingFaultyStockInfoBusiness.GetPackagingFaultyStockInfoByRepairAndModelAndItemAndFultyType(item.PackagingLineId.Value, item.DescriptionId.Value, item.ItemId.Value, item.IsChinaFaulty, orgId);
                if (stockInfoInDb != null)
                {
                    stockInfoInDb.ChinaMadeFaultyStockOutQty += item.ChinaReturnQty;
                    stockInfoInDb.ManMadeFaultyStockOutQty += item.ManReturnQty;
                    stockInfoInDb.UpdateDate = DateTime.Now;
                    stockInfoInDb.UpUserId = userId;
                    _packagingFaultyStockInfoRepository.Update(stockInfoInDb);

                    if (_packagingFaultyStockInfoRepository.Save())
                    {
                        faultyItem.PackagingFaultyStockInfoId = stockInfoInDb.PackagingFaultyStockInfoId;
                    }
                }
                faultyItemStocks.Add(faultyItem);
            }
            if (faultyItemStocks.Count > 0)
            {
                this._packagingFaultyStockDetailRepository.InsertAll(faultyItemStocks);
                IsSuccess = _packagingFaultyStockDetailRepository.Save();
            }
            return IsSuccess;
        }
    }
}
