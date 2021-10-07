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
    public class ScrapStockDetailBusiness : IScrapStockDetailBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb;
        private readonly IScrapStockInfoBusiness _scrapStockInfoBusiness;
        private readonly ScrapStockDetailRepository _scrapStockDetailRepository;
        private readonly ScrapStockInfoRepository _scrapStockInfoRepository;
        private readonly IFaultyStockDetailBusiness _faultyStockDetailBusiness;
        public ScrapStockDetailBusiness(IConfigurationUnitOfWork configurationDb, IScrapStockInfoBusiness scrapStockInfoBusiness, FaultyStockDetailBusiness faultyStockDetailBusiness)
        {
            this._configurationDb = configurationDb;
            this._scrapStockInfoBusiness = scrapStockInfoBusiness;
            this._scrapStockDetailRepository = new ScrapStockDetailRepository(this._configurationDb);
            this._scrapStockInfoRepository = new ScrapStockInfoRepository(this._configurationDb);
            this._faultyStockDetailBusiness = faultyStockDetailBusiness;
        }

        public bool SaveFaultyScrapedStock(List<ScrapStockDetailDTO> dto, long userId, long orgId, long branchId)
        {
            List<ScrapStockDetail> faultyscrapStockDetails = new List<ScrapStockDetail>();
            ScrapStockDetail scrap = new ScrapStockDetail();
            ScrapStockInfo faultyscrapInfo = new ScrapStockInfo();

            foreach (var item in dto)
            {
                var getPrice = _faultyStockDetailBusiness.GetCostAndSellPrice(item.DescriptionId.Value, item.PartsId.Value, orgId, branchId);
                scrap = new ScrapStockDetail()
                {
                    FaultyStockAssignTSId = 0,
                    FaultyStockInfoId=0,
                    ScrapStockInfoId=0,
                    StockStatus= StockStatus.StockIn,
                    CostPrice = 0,
                    SellPrice = 0,
                    DescriptionId = item.DescriptionId,
                    PartsId = item.PartsId,
                    Quantity = item.Quantity,
                    RepairedQuantity=0,
                    TSId = 0,
                    BranchId = branchId,
                    EUserId = userId,
                    OrganizationId = orgId,
                    EntryDate = DateTime.Now,

                };
                var faultyStockScrapedInfo = _scrapStockInfoBusiness.GetOneScrapedByModel(item.DescriptionId.Value, item.PartsId.Value, orgId, branchId);
                if (faultyStockScrapedInfo != null)
                {
                    faultyStockScrapedInfo.ScrapQuantity += item.Quantity;
                    faultyStockScrapedInfo.UpUserId = userId;
                    faultyStockScrapedInfo.UpdateDate = DateTime.Now;
                    _scrapStockInfoRepository.Update(faultyStockScrapedInfo);
                }
                else
                {
                    faultyscrapInfo = new ScrapStockInfo()
                    {
                        DescriptionId = item.DescriptionId,
                        PartsId = item.PartsId,
                        ScrapQuantity = item.Quantity,
                        CostPrice = 0,
                        SellPrice = 0,
                        BranchId = branchId,
                        EUserId = userId,
                        OrganizationId = orgId,
                        EntryDate = DateTime.Now,
                    };
                    _scrapStockInfoRepository.Insert(faultyscrapInfo);
                    _scrapStockInfoRepository.Save();
                }
                faultyscrapStockDetails.Add(scrap);
            }
            _scrapStockDetailRepository.InsertAll(faultyscrapStockDetails);
            return _scrapStockDetailRepository.Save();
        }

        public bool SaveScrapStockIn(List<ScrapStockDetailDTO> dto,long userId, long orgId, long branchId)
        {
            List<ScrapStockDetail> scrapStockDetail = new List<ScrapStockDetail>();
            foreach (var item in dto)
            {
                ScrapStockDetail detail = new ScrapStockDetail()
                {
                    BranchId = branchId,
                    CostPrice = item.CostPrice,
                    SellPrice = item.SellPrice,
                    RepairedQuantity = item.RepairedQuantity,
                    EntryDate = DateTime.Now,
                    EUserId = userId,
                    DescriptionId = item.DescriptionId,
                    FaultyStockAssignTSId = item.FaultyStockAssignTSId,
                    StockStatus = StockStatus.StockIn,
                    FaultyStockInfoId = item.FaultyStockInfoId,
                    TSId = item.TSId,
                    Quantity = item.Quantity,
                    PartsId = item.PartsId,
                    OrganizationId = orgId,
                };
                scrapStockDetail.Add(detail);
                var scrapInfo = _scrapStockInfoBusiness.GetScrapStockInfoByModelAndCostPriceAndSellPrice(item.DescriptionId.Value, item.PartsId.Value, item.CostPrice, item.SellPrice, orgId, branchId);
                if (scrapInfo != null)
                {
                    scrapInfo.ScrapQuantity += item.Quantity;
                    scrapInfo.UpdateDate = DateTime.Now;
                    scrapInfo.UpUserId = userId;
                    _scrapStockInfoRepository.Update(scrapInfo);
                }
                else
                {
                    ScrapStockInfo info = new ScrapStockInfo()
                    {
                        BranchId = branchId,
                        CostPrice = item.CostPrice,
                        SellPrice = item.SellPrice,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        DescriptionId = item.DescriptionId,
                        ScrapQuantity = item.Quantity,
                        PartsId = item.PartsId,
                        OrganizationId = orgId,
                    };
                    _scrapStockInfoRepository.Insert(info);
                    _scrapStockInfoRepository.Save();
                }
            }
            _scrapStockDetailRepository.InsertAll(scrapStockDetail);
            return _scrapStockDetailRepository.Save();
        }
    }
}
