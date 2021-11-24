using ERPBO.Common;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
   public interface IRepairSectionSemiFinishStockInfoBusiness
    {
        bool SaveStockRepairSectionSemiFinishGood(List<RepairSectionSemiFinishStockDetailsDTO> dtos,long infoId, long userId, long orgId);
        RepairSectionSemiFinishStockInfo GetStockByOneById(long flId, long qcId, long rqId, long assId, long warId, long moId,long orgId);
        IEnumerable<RepairSectionSemiFinishStockInfoDTO> GetAllStockInfo(long? flId, long? qcId, long? rqId, long? assId, long? warId, long? moId, long orgId);
        IEnumerable<RepairSectionSemiFinishStockDetailsDTO> GetAllDetailsRepairSectionSemiFinish(long? flId, long? qcId, long? rqId, long? assId, long? warId, long? moId, long orgId);
        bool QRCodeCheckMiniStock(string qrCode, string status, long orgId);
        bool UpdateStockAndReceiveQRCodeMiniStock(string qrCode,long userId,long orgId);
        RepairSectionSemiFinishStockInfo GetInfoQRCodeStocKByQRCode(long flId, long assId, long rpId, long qcId, long orgId);
        RepairSectionSemiFinishStockDetails GetDetailsQRCodeStocKByQRCode(string qrCode, long orgId);
        RepairSectionSemiFinishStockInfo GetStockByAllId(long flId, long assId, long rpId, long qcId,long model,long orgId);
        List<Dropdown> GetAllAssemblyInStockByFloor(long floorId, long orgId);
        List<Dropdown> GetAllQCLineInStockByFloorAndAssembly(long floorId, long assemblyId, long orgId);
        List<Dropdown> GetAllRepairLineInStockByFloorAndAssemblyAndQC(long floorId, long assemblyId, long qcId, long orgId);
        List<Dropdown> GetAllModelsInStockByFloorAndAssemblyAndQCAndRepair(long floorId, long assemblyId, long qcId, long repairId, long orgId);
        bool SaveRepairSectionSemiFinishGoodStockOutForMiniStock(List<RepairSectionSemiFinishStockDetailsDTO> dTOs, long userId, long orgId);
        bool SaveRepairSectionSemiFinishGoodStockInForMiniStock(List<RepairSectionSemiFinishStockDetailsDTO> dTOs, long userId, long orgId);

    }
}
