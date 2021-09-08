﻿using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Common;
using ERPBO.Inventory.DomainModels;
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
    public class FinishGoodsStockDetailBusiness : IFinishGoodsStockDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb; // database
       
        // Business 
        private readonly IFinishGoodsStockInfoBusiness _finishGoodsStockInfoBusiness;
        private readonly IItemBusiness _itemBusiness;  // BC
        private readonly IPackagingLineStockDetailBusiness _packagingLineStockDetailBusiness;
        private readonly IPackagingItemStockDetailBusiness _packagingItemStockDetailBusiness;
        private readonly ITempQRCodeTraceBusiness _tempQRCodeTraceBusiness;
        private readonly IQRCodeTraceBusiness _qRCodeTraceBusiness;
        private readonly IItemPreparationInfoBusiness _itemPreparationInfoBusiness;
        private readonly IItemPreparationDetailBusiness _itemPreparationDetailBusiness;

        // Repository
        private readonly FinishGoodsStockDetailRepository _finishGoodsStockDetailRepository; // repo
        private readonly FinishGoodsStockInfoRepository _finishGoodsStockInfoRepository; // repo
        private readonly TempQRCodeTraceRepository _tempQRCodeTraceRepository;

        public FinishGoodsStockDetailBusiness(IProductionUnitOfWork productionDb, IItemBusiness itemBusiness, IFinishGoodsStockInfoBusiness finishGoodsStockInfoBusiness, IPackagingLineStockDetailBusiness packagingLineStockDetailBusiness, IPackagingItemStockDetailBusiness packagingItemStockDetailBusiness, ITempQRCodeTraceBusiness tempQRCodeTraceBusiness, IQRCodeTraceBusiness qRCodeTraceBusiness, IItemPreparationInfoBusiness itemPreparationInfoBusiness, IItemPreparationDetailBusiness itemPreparationDetailBusiness, TempQRCodeTraceRepository tempQRCodeTraceRepository)
        {
            // Database
            this._productionDb = productionDb;
            // Business
            this._itemBusiness = itemBusiness;
            this._finishGoodsStockInfoBusiness = finishGoodsStockInfoBusiness;
            this._packagingItemStockDetailBusiness = packagingItemStockDetailBusiness;
            this._packagingLineStockDetailBusiness = packagingLineStockDetailBusiness;
            this._tempQRCodeTraceBusiness = tempQRCodeTraceBusiness;
            this._qRCodeTraceBusiness = qRCodeTraceBusiness;
            this._itemPreparationInfoBusiness = itemPreparationInfoBusiness;
            this._itemPreparationDetailBusiness = itemPreparationDetailBusiness;
            // Repository
            this._finishGoodsStockDetailRepository = new FinishGoodsStockDetailRepository(this._productionDb);
            this._finishGoodsStockInfoRepository = new FinishGoodsStockInfoRepository(this._productionDb);
            this._tempQRCodeTraceRepository = new TempQRCodeTraceRepository(this._productionDb);
        }

        public IEnumerable<FinishGoodsStockDetail> GelAllFinishGoodsStockDetailByOrgId(long orgId)
        {
            return _finishGoodsStockDetailRepository.GetAll(fd => fd.OrganizationId == orgId).ToList();
        }

        public bool SaveFinishGoodsStockIn(List<FinishGoodsStockDetailDTO> finishGoodsStockDetailDTOs, long userId, long orgId)
        {
            List<FinishGoodsStockDetail> finishGoodsStockDetail = new List<FinishGoodsStockDetail>();
            foreach (var item in finishGoodsStockDetailDTOs)
            {
                FinishGoodsStockDetail stockDetail = new FinishGoodsStockDetail();
                stockDetail.WarehouseId = item.WarehouseId;
                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = item.Remarks;
                stockDetail.UnitId = _itemBusiness.GetItemById(item.ItemId.Value, orgId).UnitId;
                stockDetail.EntryDate = DateTime.Now;
                stockDetail.StockStatus = StockStatus.StockIn;
                stockDetail.RefferenceNumber = item.RefferenceNumber;
                stockDetail.LineId = item.LineId;
                stockDetail.DescriptionId = item.DescriptionId;
                stockDetail.PackagingLineId = item.PackagingLineId;

                var finishGoodsStockInfoInDb = _finishGoodsStockInfoBusiness.GetAllFinishGoodsStockInfoByOrgId(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId && o.LineId == item.LineId && o.PackagingLineId == stockDetail.PackagingLineId && o.DescriptionId == item.DescriptionId && o.WarehouseId == item.WarehouseId).FirstOrDefault();
                if (finishGoodsStockInfoInDb != null)
                {
                    finishGoodsStockInfoInDb.StockInQty += item.Quantity;
                    _finishGoodsStockInfoRepository.Update(finishGoodsStockInfoInDb);
                }
                else
                {
                    FinishGoodsStockInfo finishGoodsStockInfo= new FinishGoodsStockInfo();
                    finishGoodsStockInfo.LineId = item.LineId;
                    finishGoodsStockInfo.PackagingLineId = item.PackagingLineId;
                    finishGoodsStockInfo.WarehouseId = item.WarehouseId;
                    finishGoodsStockInfo.ItemTypeId = item.ItemTypeId;
                    finishGoodsStockInfo.ItemId = item.ItemId;
                    finishGoodsStockInfo.UnitId = stockDetail.UnitId;
                    finishGoodsStockInfo.StockInQty = item.Quantity;
                    finishGoodsStockInfo.StockOutQty = 0;
                    finishGoodsStockInfo.OrganizationId = orgId;
                    finishGoodsStockInfo.EUserId = userId;
                    finishGoodsStockInfo.EntryDate = DateTime.Now;
                    finishGoodsStockInfo.DescriptionId = item.DescriptionId;
                    _finishGoodsStockInfoRepository.Insert(finishGoodsStockInfo);
                }
                finishGoodsStockDetail.Add(stockDetail);
            }
            _finishGoodsStockDetailRepository.InsertAll(finishGoodsStockDetail);
            return _finishGoodsStockDetailRepository.Save();
        }

        public async Task<bool> SaveFinishGoodsStockInAsync(List<FinishGoodsStockDetailDTO> finishGoodsStockDetailDTOs, long userId, long orgId)
        {
            List<FinishGoodsStockDetail> finishGoodsStockDetail = new List<FinishGoodsStockDetail>();
            foreach (var item in finishGoodsStockDetailDTOs)
            {
                FinishGoodsStockDetail stockDetail = new FinishGoodsStockDetail();
                stockDetail.WarehouseId = item.WarehouseId;
                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = item.Remarks;
                stockDetail.UnitId = _itemBusiness.GetItemById(item.ItemId.Value, orgId).UnitId;
                stockDetail.EntryDate = DateTime.Now;
                stockDetail.StockStatus = StockStatus.StockIn;
                stockDetail.RefferenceNumber = item.RefferenceNumber;
                stockDetail.LineId = item.LineId;
                stockDetail.DescriptionId = item.DescriptionId;
                stockDetail.PackagingLineId = item.PackagingLineId;

                var finishGoodsStockInfoInDb =await _finishGoodsStockInfoBusiness.GetAllFinishGoodsStockInfoByLineAndModelIdAsync(orgId,item.ItemId.Value,item.LineId.Value, item.PackagingLineId.Value, item.DescriptionId.Value);
                if (finishGoodsStockInfoInDb != null)
                {
                    finishGoodsStockInfoInDb.StockInQty += item.Quantity;
                    _finishGoodsStockInfoRepository.Update(finishGoodsStockInfoInDb);
                }
                else
                {
                    FinishGoodsStockInfo finishGoodsStockInfo = new FinishGoodsStockInfo();
                    finishGoodsStockInfo.LineId = item.LineId;
                    finishGoodsStockInfo.PackagingLineId = item.PackagingLineId;
                    finishGoodsStockInfo.WarehouseId = item.WarehouseId;
                    finishGoodsStockInfo.ItemTypeId = item.ItemTypeId;
                    finishGoodsStockInfo.ItemId = item.ItemId;
                    finishGoodsStockInfo.UnitId = stockDetail.UnitId;
                    finishGoodsStockInfo.StockInQty = item.Quantity;
                    finishGoodsStockInfo.StockOutQty = 0;
                    finishGoodsStockInfo.OrganizationId = orgId;
                    finishGoodsStockInfo.EUserId = userId;
                    finishGoodsStockInfo.EntryDate = DateTime.Now;
                    finishGoodsStockInfo.DescriptionId = item.DescriptionId;
                    _finishGoodsStockInfoRepository.Insert(finishGoodsStockInfo);
                }
                finishGoodsStockDetail.Add(stockDetail);
            }
            _finishGoodsStockDetailRepository.InsertAll(finishGoodsStockDetail);
            return await _finishGoodsStockDetailRepository.SaveAsync();
        }

        public IEnumerable<FinishGoodsStockDetailInfoListDTO> GetFinishGoodsStockDetailInfoList(long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum)
        {
            IEnumerable<FinishGoodsStockDetailInfoListDTO> finishGoodsStockDetailInfoListDTOs = new List<FinishGoodsStockDetailInfoListDTO>();
            finishGoodsStockDetailInfoListDTOs = this._productionDb.Db.Database.SqlQuery<FinishGoodsStockDetailInfoListDTO>(QueryForFinishGoodsStockDetailInfoList(lineId, modelId, warehouseId, itemTypeId, itemId, stockStatus, fromDate, toDate, refNum)).ToList();
            return finishGoodsStockDetailInfoListDTOs;
        }

        private string QueryForFinishGoodsStockDetailInfoList(long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (lineId != null && lineId > 0)
            {
                param += string.Format(@" and pl.LineId={0}", lineId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and de.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and wh.Id={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and it.ItemId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and i.ItemId ={0}", itemId);
            }
            if (!string.IsNullOrEmpty(stockStatus) && stockStatus.Trim() != "")
            {
                param += string.Format(@" and fsd.StockStatus='{0}'", stockStatus);
            }
            if (!string.IsNullOrEmpty(refNum) && refNum.Trim() != "")
            {
                param += string.Format(@" and fsd.RefferenceNumber Like'%{0}%'", refNum);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(fsd.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(fsd.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(fsd.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select fsd.FinishGoodsStockDetailId 'StockDetailId',ISNULL(pl.LineNumber,'') 'LineNumber', ISNULL(de.DescriptionName,'') 'ModelName', 
ISNULL(wh.WarehouseName,'') 'WarehouseName', ISNULL(it.ItemName,'') 'ItemTypeName', ISNULL(i.ItemName,'') 'ItemName',
ISNULL(u.UnitSymbol,'') 'UnitName', fsd.Quantity,fsd.StockStatus,CONVERT(nvarchar(30),fsd.EntryDate,100) 'EntryDate',
fsd.RefferenceNumber,au.UserName 'EntryUser'
From tblFinishGoodsStockDetail fsd
Left Join tblProductionLines pl on fsd.LineId= pl.LineId
Left Join [Inventory].dbo.[tblDescriptions] de on fsd.DescriptionId= de.DescriptionId
Left Join [Inventory].dbo.[tblWarehouses] wh on fsd.WarehouseId = wh.Id
Left Join [Inventory].dbo.[tblItemTypes] it on fsd.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.[tblItems] i on fsd.ItemId = i.ItemId
Left Join [Inventory].dbo.[tblUnits] u on fsd.UnitId = u.UnitId
Left Join [ControlPanel].dbo.tblApplicationUsers au on fsd.EUserId = au.UserId
Where 1=1 {0}", Utility.ParamChecker(param));

            return query;
        }
        public IEnumerable<DashboardLineWiseProductionDTO> DashboardLineWiseDailyProduction(long orgId)
        {
                return this._productionDb.Db.Database.SqlQuery<DashboardLineWiseProductionDTO>(
                string.Format(@"select  l.LineNumber,sum(d.Quantity) as Total from tblFinishGoodsStockDetail d
                inner join tblProductionLines l on d.LineId=l.LineId
                Where d.StockStatus ='Stock-In' And Cast(GETDATE() as date) = Cast(d.EntryDate as date) and d.OrganizationId={0}
                group by l.LineId,l.LineNumber", orgId)).ToList();
        }

        public IEnumerable<DashboardLineWiseProductionDTO> DashboardLineWiseOverAllProduction(long orgId)
        {
                return this._productionDb.Db.Database.SqlQuery<DashboardLineWiseProductionDTO>(
                string.Format(@"select  l.LineNumber,sum(d.Quantity) as Total from tblFinishGoodsStockDetail d
                inner join tblProductionLines l on d.LineId=l.LineId
                Where d.StockStatus ='Stock-In' and d.OrganizationId={0}
                group by l.LineId,l.LineNumber", orgId)).ToList();
        }

        public bool SaveFinishGoodsStockOut(List<FinishGoodsStockDetailDTO> finishGoodsStockDetailDTOs, long userId, long orgId)
        {
            List<FinishGoodsStockDetail> finishGoodsStockDetail = new List<FinishGoodsStockDetail>();
            foreach (var item in finishGoodsStockDetailDTOs)
            {
                FinishGoodsStockDetail stockDetail = new FinishGoodsStockDetail();
                stockDetail.WarehouseId = item.WarehouseId;
                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = item.Remarks;
                stockDetail.UnitId = _itemBusiness.GetItemById(item.ItemId.Value, orgId).UnitId;
                stockDetail.EntryDate = DateTime.Now;
                stockDetail.StockStatus = StockStatus.StockOut;
                stockDetail.RefferenceNumber = item.RefferenceNumber;
                stockDetail.LineId = item.LineId;
                stockDetail.DescriptionId = item.DescriptionId;
                stockDetail.PackagingLineId = item.PackagingLineId;

                var finishGoodsStockInfoInDb = _finishGoodsStockInfoBusiness.GetAllFinishGoodsStockInfoByOrgId(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId && o.LineId == item.LineId && o.DescriptionId == item.DescriptionId && o.WarehouseId == item.WarehouseId && o.PackagingLineId == item.PackagingLineId).FirstOrDefault();

                finishGoodsStockInfoInDb.StockOutQty += item.Quantity;
                _finishGoodsStockInfoRepository.Update(finishGoodsStockInfoInDb);
                finishGoodsStockDetail.Add(stockDetail);
            }
            _finishGoodsStockDetailRepository.InsertAll(finishGoodsStockDetail);
            return _finishGoodsStockDetailRepository.Save();
        }

        public async Task<bool> SaveFinishGoodsStockOutAsync(List<FinishGoodsStockDetailDTO> finishGoodsStockDetailDTOs, long userId, long orgId)
        {
            List<FinishGoodsStockDetail> finishGoodsStockDetail = new List<FinishGoodsStockDetail>();
            foreach (var item in finishGoodsStockDetailDTOs)
            {
                FinishGoodsStockDetail stockDetail = new FinishGoodsStockDetail();
                stockDetail.WarehouseId = item.WarehouseId;
                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = item.Remarks;
                stockDetail.UnitId = _itemBusiness.GetItemById(item.ItemId.Value, orgId).UnitId;
                stockDetail.EntryDate = DateTime.Now;
                stockDetail.StockStatus = StockStatus.StockOut;
                stockDetail.RefferenceNumber = item.RefferenceNumber;
                stockDetail.LineId = item.LineId;
                stockDetail.DescriptionId = item.DescriptionId;
                stockDetail.PackagingLineId = item.PackagingLineId;

                var finishGoodsStockInfoInDb = await _finishGoodsStockInfoBusiness.GetAllFinishGoodsStockInfoByLineAndModelIdAsync(orgId, item.ItemId.Value, item.LineId.Value, item.PackagingLineId.Value, item.DescriptionId.Value);

                finishGoodsStockInfoInDb.StockOutQty += item.Quantity;
                _finishGoodsStockInfoRepository.Update(finishGoodsStockInfoInDb);
                finishGoodsStockDetail.Add(stockDetail);
            }
            _finishGoodsStockDetailRepository.InsertAll(finishGoodsStockDetail);
            return await _finishGoodsStockDetailRepository.SaveAsync();
        }

        public IEnumerable<DaysAndLineWiseProductionChart> GetDayAndLineProductionChartsData(long orgId)
        {
            IEnumerable<DaysAndLineWiseProductionChart> data = new List<DaysAndLineWiseProductionChart>();
            if (orgId> 0)
            {
                data = this._productionDb.Db.Database.SqlQuery<DaysAndLineWiseProductionChart>(string.Format(@"Exec spDaysAndLineWiseProductionChart {0}",orgId)).ToList();
            }
            return data;
        }

        public IEnumerable<DayAndModelWiseProductionChart> GetDayAndModelWiseProductionChart(long orgId)
        {
            IEnumerable<DayAndModelWiseProductionChart> data = new List<DayAndModelWiseProductionChart>();
            if (orgId > 0)
            {
                data = this._productionDb.Db.Database.SqlQuery<DayAndModelWiseProductionChart>(string.Format(@"Exec [spDaysAndModelWiseProductionChart] {0}", orgId)).ToList();
            }
            return data;
        }

        // Finish Goods By IMEI Scanning //
        public async Task<bool> SaveFinishGoodsByIMEIAsync(string imei, long userId, long orgId)
        {
            var imeiInDb = _tempQRCodeTraceBusiness.GetTempQRCodeTraceByIMEI(imei, orgId);
            if(imeiInDb != null && imeiInDb.StateStatus == QRCodeStatus.Packaging)
            {
                // Item Preparation Info //
                var itemPreparationInfo = await _itemPreparationInfoBusiness.GetPreparationInfoByModelAndItemAndTypeAsync(ItemPreparationType.Packaging, imeiInDb.DescriptionId.Value, imeiInDb.ItemId.Value, orgId);

                // Item Preparation Detail //
                var itemPreparationDetail = (List<ItemPreparationDetail>)await _itemPreparationDetailBusiness.GetItemPreparationDetailsByInfoIdAsync(itemPreparationInfo.PreparationInfoId, orgId);

                // All items in Db
                var allItemsInDb = _itemBusiness.GetAllItemByOrgId(orgId);

                // Packaging Line Stock //
                List<PackagingLineStockDetailDTO> packagingRawStocks = new List<PackagingLineStockDetailDTO>();
                
                foreach (var item in itemPreparationDetail)
                {
                    PackagingLineStockDetailDTO packagingRawStock = new PackagingLineStockDetailDTO()
                    {
                        ProductionLineId = imeiInDb.PackagingLineId,
                        PackagingLineId = imeiInDb.PackagingLineId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        DescriptionId = imeiInDb.DescriptionId,
                        UnitId = item.UnitId,
                        OrganizationId = orgId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        StockStatus = StockStatus.StockOut,
                        RefferenceNumber = imeiInDb.IMEI,
                        Remarks = imeiInDb.CodeNo
                    };
                    packagingRawStocks.Add(packagingRawStock);
                }

                // Packaging Item Stock //
                List<PackagingItemStockDetailDTO> packagingItemStocks = new List<PackagingItemStockDetailDTO>() {
                    new PackagingItemStockDetailDTO (){
                        ProductionFloorId = imeiInDb.PackagingLineId,
                        PackagingLineId = imeiInDb.PackagingLineId,
                        WarehouseId = imeiInDb.WarehouseId,
                        ItemTypeId = imeiInDb.ItemTypeId,
                        ItemId = imeiInDb.ItemId,
                        Quantity  = 1,
                        DescriptionId = imeiInDb.DescriptionId,
                        UnitId = allItemsInDb.FirstOrDefault(s=> s.ItemId == imeiInDb.ItemId).UnitId,
                        OrganizationId = orgId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        StockStatus = StockStatus.StockOut,
                        ReferenceNumber = imeiInDb.IMEI,
                        Remarks = imeiInDb.CodeNo
                    }
                };

                // Add Finish Goods Stock
                List<FinishGoodsStockDetailDTO> finishGoodsStocks = new List<FinishGoodsStockDetailDTO>() {
                    new FinishGoodsStockDetailDTO()
                {
                    LineId = imeiInDb.ProductionFloorId,
                    PackagingLineId = imeiInDb.PackagingLineId,
                    WarehouseId = imeiInDb.WarehouseId,
                    ItemTypeId = imeiInDb.ItemTypeId,
                    ItemId = imeiInDb.ItemId,
                    Quantity = 1,
                    DescriptionId = imeiInDb.DescriptionId,
                    UnitId = allItemsInDb.FirstOrDefault(s=> s.ItemId == imeiInDb.ItemId).UnitId,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    StockStatus = StockStatus.StockIn,
                    RefferenceNumber = imeiInDb.IMEI,
                    Remarks = imeiInDb.CodeNo
                }};

                // Temp QRCode
                imeiInDb.StateStatus = QRCodeStatus.Finish;
                imeiInDb.UpdateDate = DateTime.Now;
                imeiInDb.UpUserId = userId;
                _tempQRCodeTraceRepository.Update(imeiInDb);
                if (await _tempQRCodeTraceRepository.SaveAsync())
                {
                    if(await _packagingLineStockDetailBusiness.SavePackagingLineStockOutAsync(packagingRawStocks, userId, orgId, string.Empty))
                    {
                        if (await _packagingItemStockDetailBusiness.SavePackagingItemStockOutAsync(packagingItemStocks, userId, orgId))
                        {
                            return await this.SaveFinishGoodsStockInAsync(finishGoodsStocks, userId, orgId);
                        }
                    }
                }
                
            }
            return false;
        }
    }
}
