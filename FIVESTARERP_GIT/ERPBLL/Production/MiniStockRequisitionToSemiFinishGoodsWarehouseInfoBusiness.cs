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
    public class MiniStockRequisitionToSemiFinishGoodsWarehouseInfoBusiness : IMiniStockRequisitionToSemiFinishGoodsWarehouseInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly IProductionAssembleStockDetailBusiness _productionAssembleStockDetailBusiness;
        private readonly IMiniStockRequisitionToSemiFinishGoodsWarehouseDetailBusiness _miniStockRequisitionToSemiFinishGoodsWarehouseDetailBusiness;
        private readonly IItemBusiness _itemBusiness;
        private readonly ISemiFinishGoodsWarehouseStockInfoBusiness _semiFinishGoodsWarehouseStockInfoBusiness;
        private readonly MiniStockRequisitionToSemiFinishGoodsWarehouseInfoRepository _miniStockRequisitionToSemiFinishGoodsWarehouseInfoRepository;
        private readonly MiniStockRequisitionToSemiFinishGoodsWarehouseDetailRepository _miniStockRequisitionToSemiFinishGoodsWarehouseDetailRepository;
        public MiniStockRequisitionToSemiFinishGoodsWarehouseInfoBusiness(IProductionUnitOfWork productionDb, IMiniStockRequisitionToSemiFinishGoodsWarehouseDetailBusiness miniStockRequisitionToSemiFinishGoodsWarehouseDetailBusiness, ISemiFinishGoodsWarehouseStockInfoBusiness semiFinishGoodsWarehouseStockInfoBusiness, IProductionAssembleStockDetailBusiness productionAssembleStockDetailBusiness, IItemBusiness itemBusiness)
        {
            this._productionDb = productionDb;
            this._miniStockRequisitionToSemiFinishGoodsWarehouseDetailBusiness = miniStockRequisitionToSemiFinishGoodsWarehouseDetailBusiness;
            this._productionAssembleStockDetailBusiness = productionAssembleStockDetailBusiness;
            this._itemBusiness = itemBusiness;
            this._semiFinishGoodsWarehouseStockInfoBusiness = semiFinishGoodsWarehouseStockInfoBusiness;
            _miniStockRequisitionToSemiFinishGoodsWarehouseInfoRepository = new MiniStockRequisitionToSemiFinishGoodsWarehouseInfoRepository(this._productionDb);
            _miniStockRequisitionToSemiFinishGoodsWarehouseDetailRepository = new MiniStockRequisitionToSemiFinishGoodsWarehouseDetailRepository(this._productionDb);
        }
        public IEnumerable<MiniStockRequisitionToSemiFinishGoodsWarehouseInfo> GetAllRequisitionInfoByOrgId(long orgId)
        {
            return _miniStockRequisitionToSemiFinishGoodsWarehouseInfoRepository.GetAll(s => s.OrganizationId == orgId);
        }
        public MiniStockRequisitionToSemiFinishGoodsWarehouseInfo GetRequisitionInfoById(long id, long orgId)
        {
            return _miniStockRequisitionToSemiFinishGoodsWarehouseInfoRepository.GetOneByOrg(s => s.RequisitionInfoId == id && s.OrganizationId == orgId);
        }
        public bool SaveMiniStockRequisitionToWarehouse(List<MiniStockRequisitionToSemiFinishGoodsWarehouseDetailDTO> dTOs, int totalQty, long userId, long orgId)
        {
            List <MiniStockRequisitionToSemiFinishGoodsWarehouseDetail> requisitionDetails = new List<MiniStockRequisitionToSemiFinishGoodsWarehouseDetail>();

            MiniStockRequisitionToSemiFinishGoodsWarehouseInfo requisitionInfo = new MiniStockRequisitionToSemiFinishGoodsWarehouseInfo
            {
                EntryDate = DateTime.Now,
                EUserId = userId,
                OrganizationId = orgId,
                Remarks = null,
                RequisitionCode = ("REQ-SFG-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss")),
                StateStatus = "Send",
                TotalQuantity = totalQty,
            };
            _miniStockRequisitionToSemiFinishGoodsWarehouseInfoRepository.Insert(requisitionInfo);

            if (_miniStockRequisitionToSemiFinishGoodsWarehouseInfoRepository.Save())
            {
                foreach (var item in dTOs)
                {
                    MiniStockRequisitionToSemiFinishGoodsWarehouseDetail requisitionDetail = new MiniStockRequisitionToSemiFinishGoodsWarehouseDetail
                    {
                        DescriptionId = item.DescriptionId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        ItemId = item.ItemId,
                        ItemTypeId = item.ItemTypeId,
                        OrganizationId = orgId,
                        ProductionFloorId = item.ProductionFloorId,
                        Quantity = item.Quantity,
                        WarehouseId = item.WarehouseId,
                        Remarks = item.Remarks,
                        UnitId = _itemBusiness.GetItemById(item.ItemId, orgId).UnitId,
                        RequisitionInfoId = requisitionInfo.RequisitionInfoId,
                    };
                    requisitionDetails.Add(requisitionDetail);
                }
            }
            _miniStockRequisitionToSemiFinishGoodsWarehouseDetailRepository.InsertAll(requisitionDetails);
            return _miniStockRequisitionToSemiFinishGoodsWarehouseDetailRepository.Save();
        }
        public IEnumerable<MiniStockRequisitionToSemiFinishGoodsWarehouseInfoDTO> GetMiniStockRequisitionInfoByQuery(string reqNo, string status, string lessOrEq, string fromDate, string toDate, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<MiniStockRequisitionToSemiFinishGoodsWarehouseInfoDTO>(QueryForMiniStockRequisitionInfo(reqNo,status,lessOrEq,fromDate,toDate, orgId)).ToList();
        }

        private string QueryForMiniStockRequisitionInfo(string reqNo, string status, string lessOrEq, string fromDate, string toDate, long orgId)
        {
            string query = string.Empty;
            string param = string.Empty;

            if (!string.IsNullOrEmpty(reqNo) && reqNo.Trim() != "")
            {
                param += string.Format(@" and msrsf.RequisitionCode LIKE '%{0}%'", reqNo.Trim());
            }
            if (!string.IsNullOrEmpty(status) && status.Trim() != "")
            {
                param += string.Format(@" and msrsf.StateStatus='{0}'", status);
            }
            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim()!= "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                var fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                var tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@"and Cast(msrsf.EntryDate as date) between '{0}' and '{1}'", fDate,tDate);
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

            query = string.Format(@"Select msrsf.RequisitionInfoId,msrsf.RequisitionCode,msrsf.TotalQuantity,msrsf.StateStatus,msrsf.EntryDate,au.UserName'EntryUser'
From [Production].dbo.tblMiniStockRequisitionToSemiFinishGoodsWarehouseInfo msrsf
Left Join [ControlPanel].dbo.tblApplicationUsers au on msrsf.EUserId = au.UserId 
Where 1=1  and msrsf.OrganizationId={0} {1} Order By msrsf.RequisitionInfoId Desc", orgId, Utility.ParamChecker(param));
            return query;
        }
        public bool UpdateMiniWarehouseRequisitionStatusForSemiFinishGoodsStock(long reqInfoId, long userId, long orgId)
        {
            List<SemiFinishGoodsWarehouseStockDetailDTO> stockDetailDTOs = new List<SemiFinishGoodsWarehouseStockDetailDTO>();
            var reqInfo = this.GetRequisitionInfoById(reqInfoId, orgId);
            if (reqInfoId > 0 && reqInfo.StateStatus == "Send")
            {
                var reqDetails = _miniStockRequisitionToSemiFinishGoodsWarehouseDetailBusiness.GetMiniStockRequisitionDetailByQuery(reqInfoId, null, null, null, null, null, orgId);
                foreach (var item in reqDetails)
                {
                    SemiFinishGoodsWarehouseStockDetailDTO stockDetailDTO = new SemiFinishGoodsWarehouseStockDetailDTO
                    {
                        DescriptionId = item.DescriptionId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        ItemId = item.ItemId,
                        ItemTypeId = item.ItemTypeId,
                        OrganizationId = orgId,
                        ProductionFloorId = item.ProductionFloorId,
                        Quantity = item.Quantity,
                        Remarks = reqInfo.RequisitionCode,
                        StockStatus = StockStatus.StockOut,
                        UnitId = item.UnitId,
                        WarehouseId = item.WarehouseId,
                    };
                    stockDetailDTOs.Add(stockDetailDTO);
                }
                if (_semiFinishGoodsWarehouseStockInfoBusiness.SaveSemiFinishGoodStockOut(stockDetailDTOs, userId, orgId))
                {
                    reqInfo.StateStatus = "Approved";
                    reqInfo.UpdateDate = DateTime.Now;
                    reqInfo.UpUserId = userId;
                    _miniStockRequisitionToSemiFinishGoodsWarehouseInfoRepository.Update(reqInfo);
                }
            }
            return _miniStockRequisitionToSemiFinishGoodsWarehouseInfoRepository.Save();
        }
        public bool UpdateMiniWarehouseRequisitionStatusForMiniWarehouseStock(long reqInfoId, long userId, long orgId)
        {
            List<ProductionAssembleStockDetailDTO> stockDetailDTOs = new List<ProductionAssembleStockDetailDTO>();
            var reqInfo = this.GetRequisitionInfoById(reqInfoId, orgId);
            if (reqInfoId > 0 && reqInfo.StateStatus == "Approved")
            {
                var reqDetails = _miniStockRequisitionToSemiFinishGoodsWarehouseDetailBusiness.GetMiniStockRequisitionDetailByQuery(reqInfoId, null, null, null, null, null, orgId);
                foreach (var item in reqDetails)
                {
                    ProductionAssembleStockDetailDTO stockDetailDTO = new ProductionAssembleStockDetailDTO
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
                        RefferenceNumber = reqInfo.RequisitionCode,
                    };
                    stockDetailDTOs.Add(stockDetailDTO);
                }
                if (_productionAssembleStockDetailBusiness.SaveProductionAssembleStockDetailStockIn(stockDetailDTOs, userId, orgId))
                {
                    reqInfo.StateStatus = "Accepted";
                    reqInfo.UpdateDate = DateTime.Now;
                    reqInfo.UpUserId = userId;
                    _miniStockRequisitionToSemiFinishGoodsWarehouseInfoRepository.Update(reqInfo);
                }
            }
            return _miniStockRequisitionToSemiFinishGoodsWarehouseInfoRepository.Save();
        }
    }
}
