﻿using ERPBLL.Common;
using ERPBLL.Inventory;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPDAL.InventoryDAL;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class ProductionStockDetailBusiness : IProductionStockDetailBusiness
    {
        private readonly IProductionUnitOfWork _productionDb; // database;
        private readonly IInventoryUnitOfWork _inventoryDb; // database

        private readonly ProductionStockDetailRepository _productionStockDetailRepository; //repo
        private readonly ProductionStockInfoRepository _productionStockInfoRepository; // repo

        private readonly IRequsitionInfoBusiness _requsitionInfoBusiness; // BC
        private readonly IRequsitionDetailBusiness _requsitionDetailBusiness; // BC
        private readonly IProductionStockInfoBusiness _productionStockInfoBusiness; // BC
        private readonly IItemBusiness _itemBusiness;  // BC

        public ProductionStockDetailBusiness(IProductionUnitOfWork productionDb, IInventoryUnitOfWork inventoryDb, IRequsitionInfoBusiness requsitionInfoBusiness, IRequsitionDetailBusiness requsitionDetailBusiness, IProductionStockInfoBusiness productionStockInfoBusiness, IItemBusiness itemBusiness)
        {
            this._productionDb = productionDb;
            this._inventoryDb = inventoryDb;
            _productionStockDetailRepository = new ProductionStockDetailRepository(this._productionDb);
            _productionStockInfoRepository = new ProductionStockInfoRepository(this._productionDb);

            _productionStockInfoBusiness = productionStockInfoBusiness;
            _requsitionInfoBusiness = requsitionInfoBusiness;
            _requsitionDetailBusiness = requsitionDetailBusiness;
            _itemBusiness = itemBusiness;
        }
        public IEnumerable<ProductionStockDetail> GelAllProductionStockDetailByOrgId(long orgId)
        {
            return _productionStockDetailRepository.GetAll(ware => ware.OrganizationId == orgId).ToList();
        }
        public bool SaveProductionStockIn(List<ProductionStockDetailDTO> productionStockDetailDTOs, long userId, long orgId)
        {
            List<ProductionStockDetail> productionStockDetails = new List<ProductionStockDetail>();
            foreach (var item in productionStockDetailDTOs)
            {
                ProductionStockDetail stockDetail = new ProductionStockDetail();
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
                stockDetail.StockFor = item.StockFor;

                var productionInfo = _productionStockInfoBusiness.GetAllProductionStockInfoByLineAndModelAndItemId(orgId, item.ItemId.Value, item.LineId.Value, item.DescriptionId.Value, item.StockFor);
                if (productionInfo != null)
                {
                    productionInfo.StockInQty += item.Quantity;
                    _productionStockInfoRepository.Update(productionInfo);
                }
                else
                {
                    ProductionStockInfo productionStockInfo = new ProductionStockInfo();
                    productionStockInfo.LineId = item.LineId;
                    productionStockInfo.WarehouseId = item.WarehouseId;
                    productionStockInfo.ItemTypeId = item.ItemTypeId;
                    productionStockInfo.ItemId = item.ItemId;
                    productionStockInfo.UnitId = stockDetail.UnitId;
                    productionStockInfo.StockInQty = item.Quantity;
                    productionStockInfo.StockOutQty = 0;
                    productionStockInfo.OrganizationId = orgId;
                    productionStockInfo.EUserId = userId;
                    productionStockInfo.EntryDate = DateTime.Now;
                    productionStockInfo.DescriptionId = item.DescriptionId;
                    productionStockInfo.StockFor = item.StockFor;
                    _productionStockInfoRepository.Insert(productionStockInfo);
                }
                productionStockDetails.Add(stockDetail);
            }
            _productionStockDetailRepository.InsertAll(productionStockDetails);
            return _productionStockDetailRepository.Save();
        }

        public bool SaveProductionStockOut(List<ProductionStockDetailDTO> productionStockDetailDTOs, long userId, long orgId, string flag)
        {
            var executionStatus = false;
            List<ProductionStockDetail> productionStockDetails = new List<ProductionStockDetail>();
            //if (flag == ReturnType.ProductionGoodsReturn || flag == ReturnType.RepairGoodsReturn || flag == ReturnType.ProductionFaultyReturn || flag == ReturnType.RepairFaultyReturn || flag == StockOutReason.StockOutByProductionForProduceGoods || flag == StockOutReason.StockOutByTransferToAssembly)
            //{
            //    string status = (flag == StockOutReason.StockOutByProductionForProduceGoods || flag == StockOutReason.StockOutByTransferToAssembly) ? StockStatus.StockOut : StockStatus.StockReturn;

            //}
            foreach (var item in productionStockDetailDTOs)
            {
                ProductionStockDetail stockDetail = new ProductionStockDetail();
                stockDetail.WarehouseId = item.WarehouseId;
                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = flag;
                stockDetail.UnitId = item.UnitId;
                stockDetail.RefferenceNumber = item.RefferenceNumber;
                stockDetail.LineId = item.LineId;
                stockDetail.DescriptionId = item.DescriptionId;
                stockDetail.EntryDate = item.EntryDate;
                stockDetail.StockStatus = StockStatus.StockOut;
                stockDetail.StockFor = item.StockFor;
                productionStockDetails.Add(stockDetail);
                var stockInfo = _productionStockInfoBusiness.GetAllProductionStockInfoByLineAndModelAndItemId(orgId, item.ItemId.Value, item.LineId.Value, item.DescriptionId.Value, item.StockFor);
                stockInfo.StockOutQty += stockDetail.Quantity;
                _productionStockInfoRepository.Update(stockInfo);
            }
            _productionStockDetailRepository.InsertAll(productionStockDetails);
            executionStatus = _productionStockDetailRepository.Save();
            return executionStatus;
        }
        public bool SaveProductionStockInByProductionRequistion(long reqId, string status, long orgId, long userId)
        {
            var reqInfo = _requsitionInfoBusiness.GetRequisitionById(reqId, orgId);
            var reqDetail = _requsitionDetailBusiness.GetRequsitionDetailByReqId(reqId, orgId).ToList();
            if (reqInfo != null && reqInfo.StateStatus == RequisitionStatus.Approved && reqDetail.Count > 0)
            {
                List<ProductionStockDetailDTO> productionStockDetailDTOs = new List<ProductionStockDetailDTO>();
                foreach (var item in reqDetail)
                {
                    ProductionStockDetailDTO productionStockDetailDTO = new ProductionStockDetailDTO
                    {
                        WarehouseId = reqInfo.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        OrganizationId = orgId,
                        EUserId = userId,
                        UnitId = item.UnitId,
                        StockStatus = StockStatus.StockIn,
                        Remarks = item.Remarks,
                        Quantity = (int)item.Quantity.Value,
                        RefferenceNumber = reqInfo.ReqInfoCode,
                        LineId = reqInfo.LineId,
                        DescriptionId = reqInfo.DescriptionId,
                        StockFor = reqInfo.RequisitionFor
                    };
                    productionStockDetailDTOs.Add(productionStockDetailDTO);
                }
                if (SaveProductionStockIn(productionStockDetailDTOs, userId, orgId) == true)
                {
                    return _requsitionInfoBusiness.SaveRequisitionStatus(reqId, status, orgId, userId);
                }
            }
            return false;
        }

        public IEnumerable<ProductionStockDetailInfoListDTO> GetProductionStockDetailInfoList(long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum, long orgId)
        {
            IEnumerable<ProductionStockDetailInfoListDTO> productionStockDetailInfoListDTOs = new List<ProductionStockDetailInfoListDTO>();
            productionStockDetailInfoListDTOs = this._productionDb.Db.Database.SqlQuery<ProductionStockDetailInfoListDTO>(QueryForProductionStockDetailInfoList(lineId, modelId, warehouseId, itemTypeId, itemId, stockStatus, fromDate, toDate, refNum, orgId)).ToList();
            return productionStockDetailInfoListDTOs;
        }

        private string QueryForProductionStockDetailInfoList(long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;
            param += string.Format(@" and psd.OrganizationId={0}", orgId);
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
                param += string.Format(@" and w.Id={0}", warehouseId);
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
                param += string.Format(@" and psd.StockStatus='{0}'", stockStatus);
            }
            if (!string.IsNullOrEmpty(refNum) && refNum.Trim() != "")
            {
                param += string.Format(@" and psd.RefferenceNumber Like'%{0}%'", refNum);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(psd.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(psd.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(psd.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select psd.StockDetailId,ISNULL(pl.LineNumber,'') 'LineNumber', ISNULL(de.DescriptionName,'') 'ModelName', 
ISNULL(w.WarehouseName,'') 'WarehouseName', ISNULL(it.ItemName,'') 'ItemTypeName', ISNULL(i.ItemName,'') 'ItemName',
ISNULL(u.UnitSymbol,'') 'UnitName', psd.Quantity,psd.StockStatus,CONVERT(nvarchar(30),psd.EntryDate,100) 'EntryDate',
psd.RefferenceNumber,au.UserName 'EntryUser'
From tblProductionStockDetail psd
Left Join tblProductionLines pl on psd.LineId= pl.LineId
Left Join [Inventory].dbo.tblDescriptions de on psd.DescriptionId= de.DescriptionId
Left Join [Inventory].dbo.[tblWarehouses] w on psd.WarehouseId = w.Id
Left Join [Inventory].dbo.[tblItemTypes] it on psd.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.[tblItems] i on psd.ItemId = i.ItemId
Left Join [Inventory].dbo.[tblUnits] u on psd.UnitId = u.UnitId
Left Join [ControlPanel].dbo.tblApplicationUsers au on psd.EUserId = au.UserId
Where 1=1 {0}", Utility.ParamChecker(param));

            return query;
        }

        
    }
}
