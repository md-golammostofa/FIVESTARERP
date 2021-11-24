using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Inventory.DTOModel;
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
    public class HalfDoneStockTransferToWarehouseInfoBusiness: IHalfDoneStockTransferToWarehouseInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly IHalfDoneWarehouseStockInfoBusiness _halfDoneWarehouseStockInfoBusiness;
        private readonly IRepairSectionSemiFinishStockInfoBusiness _repairSectionSemiFinishStockInfoBusiness;
        private readonly IHalfDoneStockTransferToWarehouseDetailBusiness _halfDoneStockTransferToWarehouseDetailBusiness;
        private readonly HalfDoneStockTransferToWarehouseInfoRepository _transferToWarehouseInfoRepository;
        private readonly HalfDoneStockTransferToWarehouseDetailsRepository _transferToWarehouseDetailsRepository;
        public HalfDoneStockTransferToWarehouseInfoBusiness(IProductionUnitOfWork productionDb, IRepairSectionSemiFinishStockInfoBusiness repairSectionSemiFinishStockInfoBusiness, IHalfDoneStockTransferToWarehouseDetailBusiness halfDoneStockTransferToWarehouseDetailBusiness, IHalfDoneWarehouseStockInfoBusiness halfDoneWarehouseStockInfoBusiness)
        {
            this._productionDb = productionDb;
            this._halfDoneWarehouseStockInfoBusiness = halfDoneWarehouseStockInfoBusiness;
            this._repairSectionSemiFinishStockInfoBusiness = repairSectionSemiFinishStockInfoBusiness;
            this._halfDoneStockTransferToWarehouseDetailBusiness = halfDoneStockTransferToWarehouseDetailBusiness;
            this._transferToWarehouseInfoRepository = new HalfDoneStockTransferToWarehouseInfoRepository(this._productionDb);
            this._transferToWarehouseDetailsRepository = new HalfDoneStockTransferToWarehouseDetailsRepository(this._productionDb);
        }

        public IEnumerable<HalfDoneStockTransferToWarehouseInfo> GetAllTransferList(long orgId)
        {
            return this._transferToWarehouseInfoRepository.GetAll(t => t.OrganizationId == orgId).ToList();
        }
        public HalfDoneStockTransferToWarehouseInfo GetTransferInfoById(long id, long orgId)
        {
            return _transferToWarehouseInfoRepository.GetOneByOrg(s => s.HalfDoneTransferInfoId == id && s.OrganizationId == orgId);
        }
        public bool SaveHalfDoneStockTransferToWarehouse(List<HalfDoneStockTransferToWarehouseDetailDTO> dTOs, int totalQty, long userId, long orgId)
        {
            List<RepairSectionSemiFinishStockDetailsDTO> finishStockDetailsDTOs = new List<RepairSectionSemiFinishStockDetailsDTO>();
            List<HalfDoneStockTransferToWarehouseDetail> halfDoneStockTransfers = new List<HalfDoneStockTransferToWarehouseDetail>();

            HalfDoneStockTransferToWarehouseInfo transferInfo = new HalfDoneStockTransferToWarehouseInfo
            {
                EntryDate = DateTime.Now,
                EUserId = userId,
                OrganizationId = orgId,
                Remarks = null,
                TransferCode = ("HDT-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss")),
                StateStatus = "Pending",
                TotalQuantity = totalQty,
            };
            _transferToWarehouseInfoRepository.Insert(transferInfo);
            if (_transferToWarehouseInfoRepository.Save())
            {
                foreach (var item in dTOs)
                {
                    var halfDoneStock = _repairSectionSemiFinishStockInfoBusiness.GetAllStockInfo(item.ProductionFloorId, item.QCId, item.RepairLineId, item.AssemblyLineId, null, item.DescriptionId, orgId).FirstOrDefault(); 
                    HalfDoneStockTransferToWarehouseDetail transferDetail = new HalfDoneStockTransferToWarehouseDetail
                    {
                        DescriptionId = item.DescriptionId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        ItemId = item.ItemId,
                        ItemTypeId = item.ItemTypeId,
                        OrganizationId = orgId,
                        ProductionFloorId = item.ProductionFloorId,
                        Quantity = item.Quantity,
                        WarehouseId = halfDoneStock.WarehouseId,
                        Remarks = item.Remarks,
                        HalfDoneTransferInfoId = transferInfo.HalfDoneTransferInfoId,
                        AssemblyLineId = item.AssemblyLineId,
                        QCId = item.QCId,
                        RepairLineId = item.RepairLineId,
                    };
                    halfDoneStockTransfers.Add(transferDetail);
                    RepairSectionSemiFinishStockDetailsDTO stockDetail = new RepairSectionSemiFinishStockDetailsDTO
                    {
                        FloorId = item.ProductionFloorId.Value,
                        AssemblyLineId = item.AssemblyLineId.Value,
                        QCLineId = item.QCId.Value,
                        RepairLineId = item.RepairLineId.Value,
                        DescriptionId = item.DescriptionId.Value,
                        WarehouseId = halfDoneStock.WarehouseId,
                        StateStatus = StockStatus.StockOut,
                        OrganizationId = orgId,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        ItemId = item.ItemId,
                        ItemTypeId = item.ItemTypeId,
                        StockQty = item.Quantity,
                    };
                    finishStockDetailsDTOs.Add(stockDetail);
                }
                if (_repairSectionSemiFinishStockInfoBusiness.SaveRepairSectionSemiFinishGoodStockOutForMiniStock(finishStockDetailsDTOs,userId,orgId))
                {
                    _transferToWarehouseDetailsRepository.InsertAll(halfDoneStockTransfers);
                }
            }
            return _transferToWarehouseDetailsRepository.Save();
        }
        public IEnumerable<HalfDoneStockTransferToWarehouseInfoDTO> GetHalfDoneStockTransferInfoByQuery(string reqNo, string status, string lessOrEq, string fromDate, string toDate, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<HalfDoneStockTransferToWarehouseInfoDTO>(QueryForHalfDoneStockTransferInfo(reqNo, status, lessOrEq, fromDate, toDate, orgId)).ToList();
        }

        private string QueryForHalfDoneStockTransferInfo(string reqNo, string status, string lessOrEq, string fromDate, string toDate, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (!string.IsNullOrEmpty(reqNo) && reqNo.Trim() != "")
            {
                param += string.Format(@" and msrsf.TransferCode LIKE '%{0}%'", reqNo.Trim());
            }
            if (!string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                param += string.Format(@" and msrsf.StateStatus='{0}'", status);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                var fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                var tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@"and Cast(msrsf.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                var fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@"and Cast(msrsf.EntryDate as date) = '{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                var tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@"and Cast(msrsf.EntryDate as date) = '{0}'", tDate);
            }
            if (!string.IsNullOrEmpty(lessOrEq) && lessOrEq.Trim() != "")
            {
                int qty = Utility.TryParseInt(lessOrEq);
                param += string.Format(@" and  msrsf.TotalQuantity <= {0}", qty);
            }

            query = string.Format(@"Select msrsf.HalfDoneTransferInfoId,msrsf.TransferCode,msrsf.TotalQuantity,msrsf.StateStatus,msrsf.EntryDate,au.UserName'EntryUser'
From [Production].dbo.tblHalfDoneStockTransferToWarehouseInfo msrsf
Left Join [ControlPanel].dbo.tblApplicationUsers au on msrsf.EUserId = au.UserId 
Where 1=1  and msrsf.OrganizationId={0} {1} Order By msrsf.HalfDoneTransferInfoId Desc", orgId, Utility.ParamChecker(param));
            return query;
        }
        public bool UpdateHalfDoneTransferStatusForHalfDoneStockIn(long transferInfoId, long userId, long orgId)
        {
            List<HalfDoneWarehouseStockDetailDTO> stockDetailDTOs = new List<HalfDoneWarehouseStockDetailDTO>();
            var transferInfo = this.GetTransferInfoById(transferInfoId, orgId);
            if (transferInfoId > 0 && transferInfo.StateStatus == "Pending")
            {
                var transferDetails = _halfDoneStockTransferToWarehouseDetailBusiness.GetHalfDoneTransferDetailByQuery(transferInfoId, null, null, null, null, null,null,null,null, orgId);
                foreach (var item in transferDetails)
                {
                    HalfDoneWarehouseStockDetailDTO stockDetailDTO = new HalfDoneWarehouseStockDetailDTO
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
                    stockDetailDTOs.Add(stockDetailDTO);
                }
                if (_halfDoneWarehouseStockInfoBusiness.SaveHalfDoneWarehouseStockIn(stockDetailDTOs, userId, orgId))
                {
                    transferInfo.StateStatus = "Approved";
                    transferInfo.UpdateDate = DateTime.Now;
                    transferInfo.UpUserId = userId;
                    _transferToWarehouseInfoRepository.Update(transferInfo);
                }
            }
            return _transferToWarehouseInfoRepository.Save();
        }
        public bool UpdateHalfDoneTransferStatusForHalfDoneStockOut(long transferInfoId, long userId, long orgId)
        {
            List<HalfDoneWarehouseStockDetailDTO> stockDetailDTOs = new List<HalfDoneWarehouseStockDetailDTO>();
            var transferInfo = this.GetTransferInfoById(transferInfoId, orgId);
            if (transferInfoId > 0 && transferInfo.StateStatus == "Approved")
            {
                var transferDetails = _halfDoneStockTransferToWarehouseDetailBusiness.GetHalfDoneTransferDetailByQuery(transferInfoId, null, null, null, null, null, null, null, null, orgId);
                foreach (var item in transferDetails)
                {
                    HalfDoneWarehouseStockDetailDTO stockDetailDTO = new HalfDoneWarehouseStockDetailDTO
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
                    stockDetailDTOs.Add(stockDetailDTO);
                }
                if (_halfDoneWarehouseStockInfoBusiness.SaveHalfDoneWarehouseStockOut(stockDetailDTOs, userId, orgId))
                {
                    transferInfo.StateStatus = "Return";
                    transferInfo.UpdateDate = DateTime.Now;
                    transferInfo.UpUserId = userId;
                    _transferToWarehouseInfoRepository.Update(transferInfo);
                }
            }
            return _transferToWarehouseInfoRepository.Save();
        }
        public bool UpdateHalfDoneTransferStatusForMiniStock(long transferInfoId, long userId, long orgId)
        {
            List<RepairSectionSemiFinishStockDetailsDTO> stockDetailDTOs = new List<RepairSectionSemiFinishStockDetailsDTO>();
            var transferInfo = this.GetTransferInfoById(transferInfoId, orgId);
            if (transferInfoId > 0 && transferInfo.StateStatus == "Return")
            {
                var transferDetails = _halfDoneStockTransferToWarehouseDetailBusiness.GetHalfDoneTransferDetailByQuery(transferInfoId, null, null, null, null, null,null,null,null, orgId);
                foreach (var item in transferDetails)
                {
                    RepairSectionSemiFinishStockDetailsDTO stockDetail = new RepairSectionSemiFinishStockDetailsDTO
                    {
                        FloorId = item.ProductionFloorId.Value,
                        AssemblyLineId = item.AssemblyLineId.Value,
                        QCLineId = item.QCId.Value,
                        RepairLineId = item.RepairLineId.Value,
                        DescriptionId = item.DescriptionId.Value,
                        WarehouseId = item.WarehouseId,
                        StateStatus = StockStatus.StockOut,
                        OrganizationId = orgId,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        ItemId = item.ItemId,
                        ItemTypeId = item.ItemTypeId,
                        StockQty = item.Quantity,
                    };
                    stockDetailDTOs.Add(stockDetail);
                }
                if (_repairSectionSemiFinishStockInfoBusiness.SaveRepairSectionSemiFinishGoodStockInForMiniStock(stockDetailDTOs, userId, orgId))
                {
                    transferInfo.StateStatus = "Received";
                    transferInfo.UpdateDate = DateTime.Now;
                    transferInfo.UpUserId = userId;
                    _transferToWarehouseInfoRepository.Update(transferInfo);
                }
            }
            return _transferToWarehouseInfoRepository.Save();
        }
    }
}
