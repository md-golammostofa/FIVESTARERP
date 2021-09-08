﻿using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface ITransferRepairItemToQcInfoBusiness
    {
        TransferRepairItemToQcInfo GetTransferRepairItemToQcInfoById(long transferId, long orgId);
        IEnumerable<TransferRepairItemToQcInfo> GetTransferRepairItemToQcInfos(long orgId);
        Task<TransferRepairItemToQcInfo> GetTransferRepairItemToQcInfoByIdAsync(long assemblyLineId,long repairLineId, long qcLineId, long modelId, long itemId,string status , long orgId);
        bool SaveTransferInfoStateStatus(long transferId, string status, long userId, long orgId);
        bool SaveTransfer(TransferRepairItemToQcInfoDTO infoDto, List<TransferRepairItemToQcDetailDTO> detailDto, long userId, long orgId);
        Task<bool> SaveTransferByQRCodeScanningAsync(TransferRepairItemByQRCodeScanningDTO dto, long user, long orgId);
        IEnumerable<TransferRepairItemToQcInfoDTO> GetTransferRepairItemToQcInfosByQuery(long? floorId,long? assemblyId, long? repairLineId, long? qcLineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string status, string transferCode, string fromDate, string toDate, long orgId);

        Task<bool> SaveTransferInfoStateStatusAsync(long transferId, string status, long userId, long orgId);
        Task<TransferRepairItemToQcInfo> GetTransferRepairItemToQcInfoByIdAsync(long transferId, long orgId);

    }
}
