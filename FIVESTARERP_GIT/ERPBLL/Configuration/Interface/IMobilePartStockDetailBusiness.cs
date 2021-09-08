﻿using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
   public interface IMobilePartStockDetailBusiness
    {
        IEnumerable<MobilePartStockDetail> GelAllMobilePartStockDetailByOrgId(long orgId,long branchId);
        bool SaveMobilePartStockIn(List<MobilePartStockDetailDTO> mobilePartStockDetailDTO, long userId, long orgId, long branchId);
        bool SaveMobilePartStockOut(List<MobilePartStockDetailDTO> mobilePartStockDetailDTO, long userId, long orgId, long branchId);
        bool StockInByBranchTransferApproval(long transferId, string status, long userId, long branchId, long orgId);
        bool SaveMobilePartsStockOutByTSRequistion(long reqId, string status, long orgId, long userId,long branchId);
        bool SaveMobilePartStockOutByReq(long reqId,string status, long orgId, long branchId, long userId);
        bool SaveReturnPartsStockIn(List<MobilePartStockDetailDTO> mobilePartStockDetailDTO,long returnInfoId,string status, long userId, long orgId, long branchId);
        MobilePartStockInfo GetPriceByModel(long modelId, long partsId, long orgId, long branchId);
        //bool StockOutAccessoriesSells(long invoiceId, long orgId, long branchId, long userId);
        bool SaveMobilePartStockInByBranchRequsition(List<MobilePartStockDetailDTO> mobilePartStockDetailDTO, long userId, long orgId, long branchId);
        IEnumerable<TotalStockDetailsDTO> TotalStockDetailsReport(long orgId, long branchId, long? modelId, long? partsId);
    }
}
