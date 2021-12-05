using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production;
using ERPBLL.Production.Interface;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using ERPBO.Inventory.ReportModels;
using ERPDAL.InventoryDAL;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
    public class WarehouseStockDetailBusiness : IWarehouseStockDetailBusiness
    {
        /// <summary>
        ///  BC Stands for          - Business Class
        ///  db Stands for          - Database
        ///  repo Stands for       - Repository
        /// </summary>

        private readonly IInventoryUnitOfWork _inventoryDb; // db
        private readonly IItemBusiness _itemBusiness;
        private readonly IWarehouseStockInfoBusiness _warehouseStockInfoBusiness;
        private readonly WarehouseStockDetailRepository warehouseStockDetailRepository; // repo 
        private readonly WarehouseStockInfoRepository warehouseStockInfoRepository; // repo 
        private readonly IRequsitionInfoBusiness _requsitionInfoBusiness; // BC
        private readonly IRequsitionDetailBusiness _requsitionDetailBusiness; // BC
        private readonly IItemReturnInfoBusiness _itemReturnInfoBusiness; // BC
        private readonly IItemReturnDetailBusiness _itemReturnDetailBusiness; // BC
        private readonly IWarehouseFaultyStockDetailBusiness _repairStockDetailBusiness; //BC

        public WarehouseStockDetailBusiness(IInventoryUnitOfWork inventoryDb, IItemBusiness itemBusiness, IWarehouseStockInfoBusiness warehouseStockInfoBusiness, IRequsitionInfoBusiness requsitionInfoBusiness, IRequsitionDetailBusiness requsitionDetailBusiness, IItemReturnInfoBusiness itemReturnInfoBusiness, IItemReturnDetailBusiness itemReturnDetailBusiness, IWarehouseFaultyStockDetailBusiness repairStockDetailBusiness)
        {
            this._inventoryDb = inventoryDb;
            warehouseStockDetailRepository = new WarehouseStockDetailRepository(this._inventoryDb);
            warehouseStockInfoRepository = new WarehouseStockInfoRepository(this._inventoryDb);

            this._warehouseStockInfoBusiness = warehouseStockInfoBusiness;
            this._itemBusiness = itemBusiness;
            this._requsitionInfoBusiness = requsitionInfoBusiness;
            this._requsitionDetailBusiness = requsitionDetailBusiness;
            this._itemReturnInfoBusiness = itemReturnInfoBusiness;
            this._itemReturnDetailBusiness = itemReturnDetailBusiness;
            this._repairStockDetailBusiness = repairStockDetailBusiness;
        }

        public IEnumerable<WarehouseStockDetail> GelAllWarehouseStockDetailByOrgId(long orgId)
        {
            return warehouseStockDetailRepository.GetAll(ware => ware.OrganizationId == orgId).ToList();
        }
        public bool SaveWarehouseStockIn(List<WarehouseStockDetailDTO> warehouseStockDetailDTO, long userId, long orgId)
        {
            List<WarehouseStockDetail> warehouseStockDetails = new List<WarehouseStockDetail>();
            //List<WarehouseStockInfo> warehouseStockInfos = new List<WarehouseStockInfo>();
            foreach (var item in warehouseStockDetailDTO)
            {
                WarehouseStockDetail stockDetail = new WarehouseStockDetail();
                stockDetail.WarehouseId = item.WarehouseId;
                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.ChinaFaultyQty = 0;
                stockDetail.ManMadeFaultyQty = 0;
                stockDetail.GoodStockQty = 0;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = item.Remarks;
                stockDetail.UnitId = _itemBusiness.GetItemById(item.ItemId.Value, orgId).UnitId;
                if (item.EntryDate != null)
                {
                    stockDetail.EntryDate = item.EntryDate;
                }
                else
                {
                    stockDetail.EntryDate = DateTime.Now;
                }
                stockDetail.StockStatus = StockStatus.StockIn;
                stockDetail.RefferenceNumber = item.RefferenceNumber;
                stockDetail.DescriptionId = item.DescriptionId;
                stockDetail.OrderQty = item.OrderQty;
                stockDetail.SupplierId = item.SupplierId;

                var warehouseInfo = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId && o.DescriptionId == item.DescriptionId).FirstOrDefault();
                if (warehouseInfo != null)
                {
                    warehouseInfo.StockInQty += item.Quantity;
                    warehouseInfo.UpUserId = userId;
                    warehouseInfo.UpdateDate = DateTime.Now;
                    warehouseStockInfoRepository.Update(warehouseInfo);
                    if (warehouseStockInfoRepository.Save())
                    {
                        stockDetail.StockInfoId = warehouseInfo.StockInfoId;
                    }
                }
                else
                {
                    WarehouseStockInfo warehouseStockInfo = new WarehouseStockInfo()
                    {
                        WarehouseId = item.WarehouseId,
                        DescriptionId = item.DescriptionId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        UnitId = stockDetail.UnitId,
                        StockInQty = item.Quantity,
                        StockOutQty = 0,
                        ManMadeFaultyStockInQty = 0,
                        ManMadeFaultyStockOutQty = 0,
                        ChinaMadeFaultyStockInQty = 0,
                        ChinaMadeFaultyStockOutQty = 0,
                        OrganizationId = orgId,
                        EUserId = userId,
                        Remarks = item.Remarks,
                        EntryDate = DateTime.Now,
                    };
                    warehouseStockInfoRepository.Insert(warehouseStockInfo);
                    if (warehouseStockInfoRepository.Save())
                    {
                        stockDetail.StockInfoId = warehouseStockInfo.StockInfoId;
                    }
                    warehouseStockDetails.Add(stockDetail);

                    //if (warehouseStockInfos.Count > 0)
                    //{
                    //    var warehouseStockInfoInQueue = warehouseStockInfos.FirstOrDefault(s => s.DescriptionId == item.DescriptionId && s.ItemId == item.ItemId);
                    //    if (warehouseStockInfoInQueue != null)
                    //    {
                    //        warehouseStockInfoInQueue.StockInQty += item.Quantity;
                    //    }
                    //    else
                    //    {
                    //        warehouseStockInfo.WarehouseId = item.WarehouseId;
                    //        warehouseStockInfo.DescriptionId = item.DescriptionId;
                    //        warehouseStockInfo.ItemTypeId = item.ItemTypeId;
                    //        warehouseStockInfo.ItemId = item.ItemId;
                    //        warehouseStockInfo.UnitId = stockDetail.UnitId;
                    //        warehouseStockInfo.StockInQty = item.Quantity;
                    //        warehouseStockInfo.StockOutQty = 0;
                    //        warehouseStockInfo.ManMadeFaultyStockInQty = 0;
                    //        warehouseStockInfo.ManMadeFaultyStockOutQty = 0;
                    //        warehouseStockInfo.ChinaMadeFaultyStockInQty = 0;
                    //        warehouseStockInfo.ChinaMadeFaultyStockOutQty = 0;
                    //        warehouseStockInfo.OrganizationId = orgId;
                    //        warehouseStockInfo.EUserId = userId;
                    //        warehouseStockInfo.Remarks = item.Remarks;
                    //        if (item.EntryDate != null)
                    //        {
                    //            warehouseStockInfo.EntryDate = item.EntryDate;
                    //        }
                    //        else
                    //        {
                    //            warehouseStockInfo.EntryDate = DateTime.Now;
                    //        }
                    //        warehouseStockInfos.Add(warehouseStockInfo);
                    //    }
                    //}
                    //else
                    //{
                    //    warehouseStockInfo.WarehouseId = item.WarehouseId;
                    //    warehouseStockInfo.DescriptionId = item.DescriptionId;
                    //    warehouseStockInfo.ItemTypeId = item.ItemTypeId;
                    //    warehouseStockInfo.ItemId = item.ItemId;
                    //    warehouseStockInfo.UnitId = stockDetail.UnitId;
                    //    warehouseStockInfo.StockInQty = item.Quantity;
                    //    warehouseStockInfo.StockOutQty = 0;
                    //    warehouseStockInfo.ManMadeFaultyStockInQty = 0;
                    //    warehouseStockInfo.ManMadeFaultyStockOutQty = 0;
                    //    warehouseStockInfo.ChinaMadeFaultyStockInQty = 0;
                    //    warehouseStockInfo.ChinaMadeFaultyStockOutQty = 0;
                    //    warehouseStockInfo.OrganizationId = orgId;
                    //    warehouseStockInfo.EUserId = userId;
                    //    warehouseStockInfo.Remarks = string.Empty;
                    //    if (item.EntryDate != null)
                    //    {
                    //        warehouseStockInfo.EntryDate = item.EntryDate;
                    //    }
                    //    else
                    //    {
                    //        warehouseStockInfo.EntryDate = DateTime.Now;
                    //    }
                    //    warehouseStockInfos.Add(warehouseStockInfo);
                    //}
                    //warehouseStockInfoRepository.Insert(warehouseStockInfo);
                }
                //warehouseStockDetails.Add(stockDetail);
            }
            //if (warehouseStockInfos.Count > 0)
            //{
            //    warehouseStockInfoRepository.InsertAll(warehouseStockInfos);
            //}
            warehouseStockDetailRepository.InsertAll(warehouseStockDetails);
            return warehouseStockDetailRepository.Save();
        }
        public bool SaveWarehouseStockOut(List<WarehouseStockDetailDTO> warehouseStockDetailDTOs, long userId, long orgId, string flag)
        {
            List<WarehouseStockDetail> warehouseStockDetails = new List<WarehouseStockDetail>();
            bool isValidate = true;
            warehouseStockDetailDTOs = warehouseStockDetailDTOs.Where(s => s.Quantity > 0).ToList();
            var items = warehouseStockDetailDTOs.Select(s => s.ItemId).ToList();
            var des = warehouseStockDetailDTOs.Select(s => s.DescriptionId).FirstOrDefault();
            var stock = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(orgId).Where(s => items.Contains(s.ItemId.Value) && des == s.DescriptionId).Select(s => s.ItemId).ToList();


            if (items.Count() == stock.Count())
            {
                foreach (var item in warehouseStockDetailDTOs)
                {
                    WarehouseStockDetail warehouseStockDetail = new WarehouseStockDetail()
                    {
                        WarehouseId = item.WarehouseId,
                        DescriptionId = item.DescriptionId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        OrganizationId = orgId,
                        Remarks = item.Remarks,
                        StockStatus = StockStatus.StockOut,
                        RefferenceNumber = item.RefferenceNumber,
                        UnitId = item.UnitId,
                        GoodStockQty = 0,
                        ChinaFaultyQty = 0,
                        ManMadeFaultyQty = 0
                    };

                    var warehouseInfo = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(orgId).Where(s => s.ItemId == item.ItemId && (s.StockInQty - s.StockOutQty) >= item.Quantity && s.DescriptionId == item.DescriptionId).FirstOrDefault();
                    if (warehouseInfo != null)
                    {
                        warehouseInfo.StockOutQty += item.Quantity;
                        warehouseInfo.UpUserId = userId;
                        warehouseInfo.UpdateDate = DateTime.Now;
                        warehouseStockInfoRepository.Update(warehouseInfo);
                        if (warehouseStockInfoRepository.Save())
                        {
                            warehouseStockDetail.StockInfoId = warehouseInfo.StockInfoId;
                        } 
                        warehouseStockDetails.Add(warehouseStockDetail);
                    }
                    else
                    {
                        isValidate = false;
                    }
                }
            }

            if (isValidate == true)
            {
                warehouseStockDetailRepository.InsertAll(warehouseStockDetails);
                return warehouseStockDetailRepository.Save();
            }
            return false;
        }
        public bool SaveWarehouseStockOutByProductionRequistion(long reqId, string status, long orgId, long userId)
        {
            var reqInfo = _requsitionInfoBusiness.GetRequisitionById(reqId, orgId);
            var reqDetail = _requsitionDetailBusiness.GetRequsitionDetailByReqId(reqId, orgId);
            if (reqInfo != null && reqDetail.Count() > 0)
            {
                List<WarehouseStockDetailDTO> stockDetailDTOs = new List<WarehouseStockDetailDTO>();
                foreach (var item in reqDetail)
                {
                    WarehouseStockDetailDTO stockDetailDTO = new WarehouseStockDetailDTO
                    {
                        WarehouseId = reqInfo.WarehouseId,
                        DescriptionId = reqInfo.DescriptionId,
                        ItemTypeId = item.ItemTypeId.Value,
                        ItemId = item.ItemId,
                        UnitId = item.UnitId.Value,
                        OrganizationId = item.OrganizationId,
                        Quantity = (int)item.Quantity.Value,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        Remarks = "Stock Out By Production Requistion " + "(" + reqInfo.ReqInfoCode + ")",
                        RefferenceNumber = reqInfo.ReqInfoCode,
                        StockStatus = StockStatus.StockOut,
                        GoodStockQty = 0,
                        ChinaFaultyQty = 0,
                        ManMadeFaultyQty = 0
                    };
                    stockDetailDTOs.Add(stockDetailDTO);
                }
                if (SaveWarehouseStockOut(stockDetailDTOs, userId, orgId, "Production Requistion") == true)
                {
                    return _requsitionInfoBusiness.SaveRequisitionStatus(reqId, status, orgId, userId);
                }
            }
            return false;
        }
        public bool SaveWarehouseStockInByProductionItemReturn(long irInfoId, string status, long orgId, long userId)
        {
            bool executionStatus = false;
            if (status == RequisitionStatus.Accepted)
            {
                var irInfo = _itemReturnInfoBusiness.GetItemReturnInfo(orgId, irInfoId);
                var irDetails = _itemReturnDetailBusiness.GetItemReturnDetailsByReturnInfoId(orgId, irInfoId);
                if (irInfo.StateStatus == RequisitionStatus.Approved)
                {
                    if (irInfo.ReturnType == ReturnType.ProductionGoodsReturn || irInfo.ReturnType == ReturnType.RepairGoodsReturn)
                    {
                        // Warehouse Stock-In
                        List<WarehouseStockDetailDTO> warehouseStockDetailDTOs = new List<WarehouseStockDetailDTO>();
                        foreach (var item in irDetails)
                        {
                            WarehouseStockDetailDTO stockDetailDTO = new WarehouseStockDetailDTO()
                            {
                                WarehouseId = irInfo.WarehouseId,
                                ItemTypeId = item.ItemTypeId,
                                ItemId = item.ItemId,
                                Quantity = item.Quantity,
                                OrganizationId = orgId,
                                EUserId = userId,
                                Remarks = "Stock In By Production Item Return" + "(" + irInfo.IRCode + ")",
                                UnitId = _itemBusiness.GetItemById(item.ItemId, orgId).UnitId,
                                EntryDate = DateTime.Now,
                                StockStatus = StockStatus.StockIn,
                                RefferenceNumber = irInfo.IRCode,
                                GoodStockQty = 0,
                                ChinaFaultyQty = 0,
                                ManMadeFaultyQty = 0
                            };
                            warehouseStockDetailDTOs.Add(stockDetailDTO);
                        }
                        if (SaveWarehouseStockIn(warehouseStockDetailDTOs, userId, orgId) == true)
                        {
                            executionStatus = _itemReturnInfoBusiness.SaveItemReturnStatus(irInfoId, status, orgId);
                        }
                    }

                    else if (irInfo.ReturnType == ReturnType.ProductionFaultyReturn || irInfo.ReturnType == ReturnType.RepairFaultyReturn)
                    {
                        List<WarehouseFaultyStockDetailDTO> repairStockDetailDTOs = new List<WarehouseFaultyStockDetailDTO>();
                        foreach (var item in irDetails)
                        {
                            WarehouseFaultyStockDetailDTO stockDetailDTO = new WarehouseFaultyStockDetailDTO()
                            {
                                WarehouseId = irInfo.WarehouseId,
                                ItemTypeId = item.ItemTypeId,
                                ItemId = item.ItemId,
                                Quantity = item.Quantity,
                                OrganizationId = orgId,
                                EUserId = userId,
                                Remarks = "Stock In By Production Faulty Return" + "(" + irInfo.IRCode + ")",
                                UnitId = _itemBusiness.GetItemById(item.ItemId, orgId).UnitId,
                                EntryDate = DateTime.Now,
                                StockStatus = StockStatus.StockIn,
                                RefferenceNumber = irInfo.IRCode,
                                DescriptionId = irInfo.DescriptionId,
                                LineId = irInfo.LineId,
                                ReturnType = irInfo.ReturnType,
                                FaultyCase = irInfo.FaultyCase
                            };
                            repairStockDetailDTOs.Add(stockDetailDTO);
                        }
                        if (_repairStockDetailBusiness.SaveWarehouseFaultyStockIn(repairStockDetailDTOs, userId, orgId) == true)
                        {
                            executionStatus = _itemReturnInfoBusiness.SaveItemReturnStatus(irInfoId, status, orgId);
                        }
                    }
                }
            }
            return executionStatus;
        }
        public IEnumerable<WarehouseStockDetailInfoListDTO> GetWarehouseStockDetailInfoLists(long? warehouseId, long? modelId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum, long? supplierId, long orgId)
        {
            IEnumerable<WarehouseStockDetailInfoListDTO> warehouseStockDetailInfoLists = new List<WarehouseStockDetailInfoListDTO>();
            warehouseStockDetailInfoLists = this._inventoryDb.Db.Database.SqlQuery<WarehouseStockDetailInfoListDTO>(QueryForWarehouseStockDetailInfo(warehouseId, modelId, itemTypeId, itemId, stockStatus, fromDate, toDate, refNum, supplierId, orgId)).ToList();
            return warehouseStockDetailInfoLists;
        }
        private string QueryForWarehouseStockDetailInfo(long? warehouseId, long? modelId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum, long? supplierId, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            param += string.Format(@" and wsd.OrganizationId={0}", orgId);
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and wh.Id={0}", warehouseId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and de.DescriptionId={0}", modelId);
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
                param += string.Format(@" and wsd.StockStatus='{0}'", stockStatus);
            }
            if (!string.IsNullOrEmpty(refNum) && refNum.Trim() != "")
            {
                param += string.Format(@" and wsd.RefferenceNumber Like'%{0}%'", refNum);
            }
            if (supplierId != null && supplierId > 0)
            {
                param += string.Format(@" and sup.SupplierId={0}", supplierId);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(wsd.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(wsd.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(wsd.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@"Select wsd.StockDetailId,wh.WarehouseName,de.DescriptionName 'ModelName',it.ItemName 'ItemTypeName',i.ItemName,u.UnitSymbol 'UnitName',wsd.Quantity,wsd.StockStatus
,Convert(nvarchar(20),wsd.EntryDate,106) 'EntryDate', ISNULL(wsd.RefferenceNumber,'N/A') as 'RefferenceNumber',au.UserName 'EntryUser',sup.SupplierName,wsd.OrderQty  From tblWarehouseStockDetails wsd
Left Join tblWarehouses wh on wsd.WarehouseId = wh.Id
Left Join tblDescriptions de on wsd.DescriptionId =de.DescriptionId
Left Join tblItemTypes it on wsd.ItemTypeId = it.ItemId
Left Join tblItems i on wsd.ItemId  = i.ItemId
Left Join tblUnits u on wsd.UnitId= u.UnitId
Left Join tblSupplier sup on wsd.SupplierId = sup.SupplierId      
Left Join [ControlPanel].dbo.tblApplicationUsers au on wsd.EUserId = au.UserId
Where 1=1 {0}", Utility.ParamChecker(param));
            return query;
        }
        public IEnumerable<StockShortageOrExcessQty> StockShortageOrExcessQty(long orgId, string fromDate, string toDate, long modelId)
        {
            return this._inventoryDb.Db.Database.SqlQuery<StockShortageOrExcessQty>("Exec spWarehouseShortageReport {0},{1},{2},{3}", orgId, fromDate, toDate, modelId).ToList();
        }

        public IEnumerable<StockExcelUploaderData> GetStockExcelUploaderData(long orgId)
        {
            return this._inventoryDb.Db.Database.SqlQuery<StockExcelUploaderData>("Exec spExcelUploaderItemList {0}", orgId).ToList();
        }
        public bool SavePartsStockInFromProduction(List<WarehouseStockDetailDTO> dTOs, long userId, long orgId)
        {
            List<WarehouseStockDetail> warehouseStockDetails = new List<WarehouseStockDetail>();
            foreach (var item in dTOs)
            {
                WarehouseStockDetail stockDetail = new WarehouseStockDetail
                {
                    ChinaFaultyQty = item.ChinaFaultyQty,
                    DescriptionId = item.DescriptionId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    GoodStockQty = item.GoodStockQty,
                    ItemId = item.ItemId,
                    ItemTypeId = item.ItemTypeId,
                    ManMadeFaultyQty = item.ManMadeFaultyQty,
                    OrderQty = 0,
                    OrganizationId = orgId,
                    RefferenceNumber = item.RefferenceNumber,
                    Remarks = item.Remarks,
                    Quantity = item.Quantity,
                    StockStatus = StockStatus.StockIn,
                    UnitId = item.UnitId,
                    WarehouseId = item.WarehouseId
                };

                var info = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId && o.DescriptionId == item.DescriptionId).FirstOrDefault();

                if (info != null)
                {
                    info.StockInQty += item.GoodStockQty;
                    info.ManMadeFaultyStockInQty += item.ManMadeFaultyQty;
                    info.ChinaMadeFaultyStockInQty += item.ChinaFaultyQty;
                    info.UpdateDate = DateTime.Now;
                    info.UpUserId = userId;

                    warehouseStockInfoRepository.Update(info);
                    if (warehouseStockInfoRepository.Save())
                    {
                        stockDetail.StockInfoId = info.StockInfoId;
                    }
                }
                warehouseStockDetails.Add(stockDetail);
            }

            warehouseStockDetailRepository.InsertAll(warehouseStockDetails);
            return warehouseStockDetailRepository.Save();
        }
    }
}
