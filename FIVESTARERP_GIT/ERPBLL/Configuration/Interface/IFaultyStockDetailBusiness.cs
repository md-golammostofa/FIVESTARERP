using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPBO.FrontDesk.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
    public interface IFaultyStockDetailBusiness
    {
        bool SaveFaultyStockIn(List<ERPBO.Configuration.DTOModels.FaultyStockDetailDTO> faultyStocksDto, long userId, long orgId, long branchId);
        bool SaveFaultyStock(List<ERPBO.Configuration.DTOModels.FaultyStockDetailDTO> faultyStocksDto, long userId, long orgId, long branchId);
        bool SaveMobilePartStockOut(List<MobilePartStockDetailDTO> mobilePartStockDetailDTO, long userId, long orgId, long branchId);
        FaultyStockDetails GetCostAndSellPrice(long modelId, long partsId, long orgId, long branchId);
        bool SaveFaultyStockOut(List<FaultyStockDetailDTO> faultyStocksDto, long userId, long orgId, long branchId);
        bool SaveFaultyStockOutByTSRepaired(List<FaultyStockDetailDTO> faultyStocksDto, long userId, long orgId, long branchId);
        bool GoodStockOutAndFaultyStockIn(MobilePartStockDetailDTO dto, long userId, long orgId, long branchId);
        bool SaveFaultyStockInByGoodToFaulty(List<ERPBO.Configuration.DTOModels.FaultyStockDetailDTO> faultyStocksDto, long userId, long orgId, long branchId);
        bool SaveFaultyStockOutForTransfer(List<FaultyStockDetailDTO> faultyStocksDto, long userId, long orgId, long branchId);
        bool SaveFaultyStockOutForTransferToDust(List<FaultyStockDetailDTO> faultyStocksDto, long userId, long orgId, long branchId);
    }
}
