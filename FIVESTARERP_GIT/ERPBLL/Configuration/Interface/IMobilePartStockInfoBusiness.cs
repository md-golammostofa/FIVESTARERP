﻿using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface IMobilePartStockInfoBusiness
    {
        IEnumerable<MobilePartStockInfo> GetAllMobilePartStockInfoByOrgId(long orgId,long branchId);
        IEnumerable<MobilePartStockInfo> GetAllMobilePartStockInfoById(long orgId, long branchId);
        MobilePartStockInfo GetAllMobilePartStockInfoByInfoId(long warehouseId, long partsId, double cprice, long orgId, long branchId,long model);
        MobilePartStockInfo GetAllMobilePartStockInfoBySellPrice(long warehouseId, long partsId, double sprice, long orgId, long branchId);
        MobilePartStockInfo GetAllMobilePartStockById(long orgId, long branchId);
       IEnumerable<MobilePartStockInfo> GetAllMobilePartStockByParts(long warehouseId, long partsId,long orgId, long branchId,long modelId);
        IEnumerable<MobilePartStockInfo> GetAllMobilePartStockByPartsSales(long warehouseId, long partsId, long orgId, long branchId,long modelId);
        IEnumerable<MobilePartStockInfoDTO> GetCurrentStock(long orgId, long branchId);
        MobilePartStockInfo GetMobilePartStockInfoByModelAndMobilePartsAndCostPrice(long modelId,long mobilePartsId,double costprice,long orgId, long branchId);
        IEnumerable<MobilePartStockInfoDTO> GetMobilePartsStockInformations(long? warehouseId,long? modelId, long? partsId,string lessOrEq, long orgId, long branchId);
        IEnumerable<MobilePartStockInfo> GetAllMobilePartStockInfoByModelAndBranch(long orgId, long modelId, long branchId);
        IEnumerable<MobilePartStockInfoDTO> GetAllGoodMobilePartsAndCode(long orgId);
        MobilePartStockInfo GetPriceByModel(long modelId,long partsId,long orgId, long branchId);
        IEnumerable<MobilePartStockInfoDTO> GetPartsPriceList(long orgId,long branchId,long? model,long? parts);
        IEnumerable<MobilePartStockInfo> GetPriceByModelAndParts(long modelId, long partsId, long orgId, long branchId);
        MobilePartStockInfo GetMobilePartStockInfoByModelAndMobilePartsAndCostPriceAndSellPrice(long modelId, long mobilePartsId, double costprice,double sellprice, long orgId, long branchId);
        IEnumerable<MobilePartStockInfo> GetPriceByModelAndPartsWithCost(long modelId, long partsId,double costPrice, long orgId, long branchId);
    }
}
