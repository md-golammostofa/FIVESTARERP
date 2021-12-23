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
    public class WarehouseToFactoryReturnInfoBusiness: IWarehouseToFactoryReturnInfoBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly WarehouseToFactoryReturnInfoRepository _warehouseToFactoryReturnInfoRepository; // repo
        private readonly IMobilePartStockDetailBusiness _mobilePartStockDetailBusiness;
        private readonly IMobilePartStockInfoBusiness _mobilePartStockInfoBusiness;
        private readonly MobilePartStockDetailRepository mobilePartStockDetailRepository; // repo
        private readonly MobilePartStockInfoRepository mobilePartStockInfoRepository; //repo
        private readonly IServicesWarehouseBusiness _servicesWarehouseBusiness;
        public WarehouseToFactoryReturnInfoBusiness(IConfigurationUnitOfWork configurationDb, IMobilePartStockDetailBusiness mobilePartStockDetailBusiness, IMobilePartStockInfoBusiness mobilePartStockInfoBusiness, IServicesWarehouseBusiness servicesWarehouseBusiness)
        {
            this._configurationDb = configurationDb;
            _warehouseToFactoryReturnInfoRepository = new WarehouseToFactoryReturnInfoRepository(this._configurationDb);
            mobilePartStockDetailRepository = new MobilePartStockDetailRepository(this._configurationDb);
            mobilePartStockInfoRepository = new MobilePartStockInfoRepository(this._configurationDb);
            this._mobilePartStockDetailBusiness = mobilePartStockDetailBusiness;
            this._mobilePartStockInfoBusiness = mobilePartStockInfoBusiness;
            this._servicesWarehouseBusiness = servicesWarehouseBusiness;

        }

        public IEnumerable<WarehouseToFactoryReturnInfoDTO> GetAllReturnList(long orgId, long branchId)
        {
            var data = this._configurationDb.Db.Database.SqlQuery<WarehouseToFactoryReturnInfoDTO>(string.Format(@"Select fi.WTFInfoId,fi.ReturnCode,fi.StateStatus,UserName,fi.EntryDate 
From tblWarehouseToFactoryReturnInfo fi
Left Join [ControlPanel].dbo.tblApplicationUsers u on fi.EUserId=u.UserId
Where fi.OrganizationId={0} and fi.BranchId={1}
Order By fi.EntryDate desc", orgId, branchId)).ToList();
            return data;
        }

        public bool SaveStockReturnWarehouseToFactory(List<WarehouseToFactoryReturnDetailsDTO> dto, long userId, long orgId, long branchId)
        {

            Random random = new Random();
            var ran = random.Next().ToString();
            ran = ran.Substring(0, 4);
            var transferCode = ("WTF-" + ran + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
            int availQty = 0;
            WarehouseToFactoryReturnInfo info = new WarehouseToFactoryReturnInfo();
            List<WarehouseToFactoryReturnDetails> detailsList = new List<WarehouseToFactoryReturnDetails>();
            List<MobilePartStockDetailDTO> stockout = new List<MobilePartStockDetailDTO>();
            if (dto.Count() > 0)
            {
                foreach(var item in dto)
                {
                    var stock = _mobilePartStockInfoBusiness.GetPriceByModelAndParts(item.ModelId, item.PartsId, orgId, branchId);
                    availQty = stock.Sum(s => (s.StockInQty.Value - s.StockOutQty.Value));
                    if(availQty > item.Quantity || availQty == item.Quantity)
                    {
                        WarehouseToFactoryReturnDetails detail = new WarehouseToFactoryReturnDetails
                        {
                            ModelId = item.ModelId,
                            PartsId = item.PartsId,
                            SellPrice = item.SellPrice,
                            CostPrice = item.CostPrice,
                            Quantity = item.Quantity,
                            ReferenceCode = transferCode,
                            OrganizationId = orgId,
                            Remarks="Stock-Out By Return",
                            EUserId = userId,
                            BranchId=branchId,
                            EntryDate = DateTime.Now,
                        };
                        detailsList.Add(detail);
                    }
                }
                if (detailsList.Count() > 0)
                {
                    info.ReturnCode = transferCode;
                    info.StateStatus = "Return Factory";
                    info.EntryDate = DateTime.Now;
                    info.EUserId = userId;
                    info.OrganizationId = orgId;
                    info.BranchId = branchId;
                    info.warehouseToFactoryReturnDetails = detailsList;
                }
            }
            _warehouseToFactoryReturnInfoRepository.Insert(info);
            return _warehouseToFactoryReturnInfoRepository.Save();
        }

        public bool SaveStockReturnWarehouseToFactoryAndStockOut(List<MobilePartStockDetailDTO> mobilePartStockDetailDTO, long orgId, long branchId, long userId)
        {
            bool IsSuccess = false;
            var warehouse = _servicesWarehouseBusiness.GetAllServiceWarehouseByOrgId(orgId, branchId).FirstOrDefault();
            List<MobilePartStockDetail> stockDetails = new List<MobilePartStockDetail>();
            List<WarehouseToFactoryReturnDetailsDTO> dtoList = new List<WarehouseToFactoryReturnDetailsDTO>();

            foreach (var item in mobilePartStockDetailDTO)
            {
                var reqQty = item.Quantity;
                var partsInStock = _mobilePartStockInfoBusiness.GetAllMobilePartStockInfoByModelAndBranch(orgId, item.DescriptionId.Value, branchId).Where(i => i.MobilePartId == item.MobilePartId && (i.StockInQty - i.StockOutQty) > 0).OrderBy(i => i.MobilePartStockInfoId).ToList();

                if (partsInStock.Count() > 0)
                {
                    int remainQty = reqQty;
                    foreach (var stock in partsInStock)
                    {

                        var totalStockqty = (stock.StockInQty - stock.StockOutQty); // total stock
                        var stockOutQty = 0;
                        if (totalStockqty <= remainQty)
                        {
                            stock.StockOutQty += totalStockqty;
                            stockOutQty = totalStockqty.Value;
                            remainQty -= totalStockqty.Value;
                        }
                        else
                        {
                            stockOutQty = remainQty;
                            stock.StockOutQty += remainQty;
                            remainQty = 0;
                        }


                        MobilePartStockDetail stockDetail = new MobilePartStockDetail()
                        {
                            DescriptionId = item.DescriptionId,
                            SWarehouseId = warehouse.SWarehouseId,
                            MobilePartId = item.MobilePartId,
                            CostPrice = stock.CostPrice,
                            SellPrice = stock.SellPrice,
                            Quantity = stockOutQty,
                            Remarks = "Stock-Out By Factory Return",
                            OrganizationId = orgId,
                            BranchId = branchId,
                            EUserId = userId,
                            EntryDate = DateTime.Now,
                            StockStatus = StockStatus.StockOut,
                            //ReferrenceNumber = reqInfo.RequsitionCode
                        };
                        WarehouseToFactoryReturnDetailsDTO returnlist = new WarehouseToFactoryReturnDetailsDTO
                        {
                            ModelId = item.DescriptionId.Value,
                            PartsId = item.MobilePartId.Value,
                            Quantity = stockOutQty,
                            CostPrice = stock.CostPrice,
                            SellPrice = stock.SellPrice,
                        };
                        dtoList.Add(returnlist);
                        stockDetails.Add(stockDetail);
                        mobilePartStockInfoRepository.Update(stock);
                        if (remainQty == 0)
                        {
                            break;
                        }
                    }
                }
            }
            mobilePartStockDetailRepository.InsertAll(stockDetails);
            if (mobilePartStockDetailRepository.Save() == true)
            {
                IsSuccess = SaveStockReturnWarehouseToFactory(dtoList, userId, orgId, branchId);
            }
            return IsSuccess;
        }
    }
}
