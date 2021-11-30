using ERPBLL.Common;
using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
    public class StockTransferInfoModelToModelBusiness : IStockTransferInfoModelToModelBusiness
    {
        private readonly IConfigurationUnitOfWork _configDb;
        private readonly StockTransferInfoModelToModelRepository _stockTransferInfoModelToModelRepository;
       // private readonly StockTransferDetailModelToModelRepository _stockTransferDetailModelToModelRepository;
        private readonly IMobilePartStockDetailBusiness _mobilePartStockDetailBusiness;
        private readonly IServicesWarehouseBusiness _servicesWarehouseBusiness;
        public StockTransferInfoModelToModelBusiness(IConfigurationUnitOfWork configDb, IMobilePartStockDetailBusiness mobilePartStockDetailBusiness, IServicesWarehouseBusiness servicesWarehouseBusiness)
        {
            this._configDb = configDb;
            this._mobilePartStockDetailBusiness = mobilePartStockDetailBusiness;
            _stockTransferInfoModelToModelRepository = new StockTransferInfoModelToModelRepository(this._configDb);
            this._servicesWarehouseBusiness = servicesWarehouseBusiness;
            //_stockTransferDetailModelToModelRepository = new StockTransferDetailModelToModelRepository(this._configDb);
        }
        public bool SaveStockTransferModelToModel(List<StockTransferDetailModelToModelDTO> dto, long userId, long branchId, long orgId)
        {
            bool IsSuccess = false;
            List<StockTransferInfoModelToModel> infolist = new List<StockTransferInfoModelToModel>();
            
            List<MobilePartStockDetailDTO> partsDetailsForStockOut = new List<MobilePartStockDetailDTO>();
            List<MobilePartStockDetailDTO> partsDetailsForStockIn = new List<MobilePartStockDetailDTO>();
            var trasnferCode = ("STM-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
            var warehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(orgId, branchId).FirstOrDefault();
            
            foreach (var item in dto)
            {
                StockTransferInfoModelToModel info = new StockTransferInfoModelToModel();
                info.BranchId = branchId;
                info.DescriptionId = item.DescriptionId;
                info.ToDescriptionId = item.ToDescriptionId;
                info.EntryDate = DateTime.Now;
                info.EUserId = userId;
                info.OrganizationId = orgId;
                info.Remarks = item.Remarks;
                info.StateStatus = RequisitionStatus.Approved;
                info.TransferCode = trasnferCode;
                info.WarehouseId = warehouse.SWarehouseId;

                List<StockTransferDetailModelToModel> details = new List<StockTransferDetailModelToModel>();
                StockTransferDetailModelToModel detail = new StockTransferDetailModelToModel()
                {
                    BranchId = branchId,
                    CostPrice = item.CostPrice,
                    SellPrice = item.SellPrice,
                    DescriptionId = item.DescriptionId,
                    ToDescriptionId = item.ToDescriptionId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    OrganizationId = orgId,
                    PartsId = item.PartsId,
                    Quantity = item.Quantity,
                    Remarks = item.Remarks,
                };
                details.Add(detail);
                info.StockTransferDetailModelToModels = details;

                MobilePartStockDetailDTO stockOutItem = new MobilePartStockDetailDTO
                {
                    SWarehouseId = warehouse.SWarehouseId,
                    MobilePartId = item.PartsId,
                    CostPrice = item.CostPrice,
                    SellPrice = item.SellPrice,
                    Quantity = item.Quantity,
                    Remarks = info.TransferCode,
                    StockStatus = StockStatus.StockOut,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    BranchId = branchId,
                    ReferrenceNumber = info.TransferCode,
                    DescriptionId = info.DescriptionId
                };
                partsDetailsForStockOut.Add(stockOutItem);

                MobilePartStockDetailDTO stockInItem = new MobilePartStockDetailDTO
                {
                    SWarehouseId = warehouse.SWarehouseId,
                    MobilePartId = item.PartsId,
                    CostPrice = item.CostPrice,
                    SellPrice = item.SellPrice,
                    Quantity = item.Quantity,
                    Remarks = info.TransferCode,
                    StockStatus = StockStatus.StockIn,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    BranchId = branchId,
                    ReferrenceNumber = info.TransferCode,
                    DescriptionId = info.ToDescriptionId
                };
                partsDetailsForStockIn.Add(stockInItem);
                _stockTransferInfoModelToModelRepository.Insert(info);
                _stockTransferInfoModelToModelRepository.Save();
                IsSuccess = true;
            }
            if (IsSuccess == true)
            {
                if(_mobilePartStockDetailBusiness.SaveMobilePartStockOut(partsDetailsForStockOut, orgId, branchId, userId))
                {
                    IsSuccess = _mobilePartStockDetailBusiness.SaveMobilePartStockIn(partsDetailsForStockIn, userId, orgId, branchId);
                }
            }
            return IsSuccess;
        }
        public IEnumerable<StockTransferInfoModelToModel> GetAllStockTransferInfoModelToModelByOrgIdAndBranch(long orgId, long branchId)
        {
            return _stockTransferInfoModelToModelRepository.GetAll(ts => ts.OrganizationId == orgId && ts.BranchId == branchId).ToList();
        }
        public StockTransferInfoModelToModel GetStockTransferMMInfoById(long id, long orgId)
        {
            return _stockTransferInfoModelToModelRepository.GetOneByOrg(t => t.TransferInfoModelToModelId == id && t.OrganizationId == orgId);
        }
    }
}
