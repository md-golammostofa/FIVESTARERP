﻿using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPBO.Production.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface ITransferStockToAssemblyInfoBusiness
    {
        IEnumerable<TransferStockToAssemblyInfo> GetStockToAssemblyInfos(long orgId);
        TransferStockToAssemblyInfo GetStockToAssemblyInfoById(long id,long orgId);
        bool SaveTransferStockAssembly(TransferStockToAssemblyInfoDTO info, List<TransferStockToAssemblyDetailDTO> details, long userId, long orgId);
        bool SaveTransferInfoStateStatus(long transferId, string status, long userId, long orgId);
        IEnumerable<TransferStockToAssemblyInfoDTO> GetFloorStockTransferInfobyQuery(long? floorId,string transferFor, long? repairLineId, long? assemblyId, long? modelId,long? warehouseId,long? itemTypeId, long? itemId,string status,string transferCode,string fromDate, string toDate, long orgId);
        TransferStockToAssemblyInfoDTO GetTransferStockToAssemblyInfoAndDetailsByQuery(long transferId, long orgId);
        bool SaveProductionStockReceiveInByAssemblyOrRepair(long transferId, string status, long orgId, long userId);
    }
}
