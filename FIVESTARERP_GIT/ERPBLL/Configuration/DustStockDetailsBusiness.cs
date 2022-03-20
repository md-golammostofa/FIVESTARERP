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
    public class DustStockDetailsBusiness: IDustStockDetailsBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly DustStockDetailsRepository _dustStockDetailsRepository; // repo
        private readonly IDustStockInfoBusiness _dustStockInfoBusiness;
        private readonly DustStockInfoRepository _dustStockInfoRepository; // repo
        private readonly IScrapStockDetailBusiness _scrapStockDetailBusiness;
        public DustStockDetailsBusiness(IConfigurationUnitOfWork configurationDb, IDustStockInfoBusiness dustStockInfoBusiness, IScrapStockDetailBusiness scrapStockDetailBusiness)
        {
            this._configurationDb = configurationDb;
            _dustStockDetailsRepository = new DustStockDetailsRepository(this._configurationDb);
            _dustStockInfoRepository = new DustStockInfoRepository(this._configurationDb);
            this._dustStockInfoBusiness = dustStockInfoBusiness;
            this._scrapStockDetailBusiness = scrapStockDetailBusiness;
        }

        public bool SaveDustStock(List<DustStockDetailsDTO> dto, long userId, long branchId, long orgId)
        {
            bool IsSuccess = false;
            List<ScrapStockDetailDTO> Scraped = new List<ScrapStockDetailDTO>();
            foreach(var item in dto)
            {
                DustStockDetails dust = new DustStockDetails
                {
                    ModelId = item.ModelId,
                    PartsId = item.PartsId,
                    Quantity = item.Quantity,
                    StateStatus = "Stock-In",
                    EUserId = userId,
                    OrganizationId = orgId,
                    BranchId = branchId,
                    EntryDate = DateTime.Now,
                };
                _dustStockDetailsRepository.Insert(dust);

                ScrapStockDetailDTO scrapedStock = new ScrapStockDetailDTO
                {
                    DescriptionId=item.ModelId,
                    PartsId=item.PartsId,
                    StockStatus="Stock-Out",
                    Quantity=item.Quantity,
                    EntryDate=DateTime.Now,
                    EUserId=item.EUserId,
                    OrganizationId=orgId,
                    BranchId=branchId,
                };
                Scraped.Add(scrapedStock);

                var dustInfo = _dustStockInfoBusiness.GetPartsByModel(item.ModelId, item.PartsId, orgId, branchId);
                if(dustInfo != null)
                {
                    dustInfo.StockInQty += item.Quantity;
                    dustInfo.UpdateDate = DateTime.Now;
                    dustInfo.UpUserId = userId;
                    _dustStockInfoRepository.Update(dustInfo);
                }
                else
                {
                    DustStockInfo stockInfo = new DustStockInfo();
                    stockInfo.ModelId = item.ModelId;
                    stockInfo.PartsId = item.PartsId;
                    stockInfo.StockInQty = item.Quantity;
                    stockInfo.EUserId = userId;
                    stockInfo.EntryDate = DateTime.Now;
                    stockInfo.BranchId = branchId;
                    stockInfo.OrganizationId = orgId;
                    _dustStockInfoRepository.Insert(stockInfo);
                }
                if (_dustStockInfoRepository.Save() == true)
                {
                    IsSuccess = _scrapStockDetailBusiness.SaveScrapStockOut(Scraped, userId, orgId, branchId);
                }
            }
            return IsSuccess;
        }

        public bool SaveDustStockInFromFaulty(List<DustStockDetailsDTO> dto, long userId, long branchId, long orgId)
        {
            bool IsSuccess = false;
            foreach (var item in dto)
            {
                DustStockDetails dust = new DustStockDetails
                {
                    ModelId = item.ModelId,
                    PartsId = item.PartsId,
                    Quantity = item.Quantity,
                    StateStatus = "Stock-In",
                    EUserId = userId,
                    OrganizationId = orgId,
                    BranchId = branchId,
                    EntryDate = DateTime.Now,
                    Remarks="Dust Stock In By Faulty"
                };
                _dustStockDetailsRepository.Insert(dust);
                _dustStockDetailsRepository.Save();

                var dustInfo = _dustStockInfoBusiness.GetPartsByModel(item.ModelId, item.PartsId, orgId, branchId);
                if (dustInfo != null)
                {
                    dustInfo.StockInQty += item.Quantity;
                    dustInfo.UpdateDate = DateTime.Now;
                    dustInfo.UpUserId = userId;
                    _dustStockInfoRepository.Update(dustInfo);
                }
                else
                {
                    DustStockInfo stockInfo = new DustStockInfo();
                    stockInfo.ModelId = item.ModelId;
                    stockInfo.PartsId = item.PartsId;
                    stockInfo.StockInQty = item.Quantity;
                    stockInfo.EUserId = userId;
                    stockInfo.EntryDate = DateTime.Now;
                    stockInfo.BranchId = branchId;
                    stockInfo.OrganizationId = orgId;
                    _dustStockInfoRepository.Insert(stockInfo);
                    IsSuccess = true;
                }
                 _dustStockInfoRepository.Save();
            }
            return IsSuccess;
        }
    }
}
