using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface ITempQRCodeTraceBusiness
    {
        IEnumerable<TempQRCodeTrace> GetTempQRCodeTraceByOrg(long orgId);
        TempQRCodeTrace GetTempQRCodeTraceByCode(string code, long orgId);
        Task<TempQRCodeTrace> GetTempQRCodeTraceByCodeAsync(string code, long orgId);
        bool SaveTempQRCodeTrace(List<TempQRCodeTraceDTO> dtos, long userId, long orgId);
        bool IsExistQRCodeWithStatus(string qrCode, string status, long orgId);
        bool UpdateQRCodeStatus(string qrCode, string status, long orgId);
        Task<bool> UpdateQRCodeStatusAsync(string qrCode, string status, long orgId);
        Task<IEnumerable<TempQRCodeTrace>> GetTempQRCodeTracesByQRCodesAsync(List<string> qrCodes, long orgId);
        Task<bool> UpdateQRCodeBatchAsync(List<TempQRCodeTrace> qrCodes, long orgId);
        Task<TempQRCodeTrace> GetTempQRCodeTraceByCodeWithFloorAsync(string code, string status, long? floorId, long orgId);
        Task<bool> UpdateQRCodeStatusWithQCAsync(string qrCode, string status, long qcId, string qcName, long orgId);
        Task<bool> SaveQRCodeIEMIAsync(IMEIWriteDTO dto, long userId, long orgId);
        Task<TempQRCodeTrace> GetIMEIinQRCode(string imei, string status, long floorId, long packagingId, long orgId);
        Task<bool> SaveBatteryCodeAsync(BatteryWriteDTO dto, long userId, long orgId);
        bool IsExistIMEIWithStatus(string imei, string status, long orgId);
        TempQRCodeTrace GetTempQRCodeTraceByIMEI(string imei, long orgId);
        Task<TempQRCodeTrace> GetIMEIWithThisQRCode(string imei, long codeId, long orgId);

        Task<TempQRCodeTrace> GetIMEIWithOutThisQRCode(string imei, long codeId, long orgId);
        bool IsExistQRCodeWithStatus(string qrCode, DateTime date, string status, long orgId);
        bool SaveQRCodeStatusByLotIn(string qrCode, long orgId, long userId);
        IEnumerable<TempQRCodeTraceDTO> GetAssemblyLineWiseDataForDashBoard(long assemblyId, long orgId);
        DashboardAssemblyLineWiseDataDTO GetAssemblyDashBoard(long assemblyId, long orgId);
        IEnumerable<TempQRCodeTraceDTO> GetDailyProductionSummeryReport(long assemblyId, long modelId, string fromDate, string toDate, long orgId);
        T_StockDTO GetIMEIFromTStock(string imei);
        IEnumerable<TempQRCodeTraceDTO> GetPackegingLineWiseDataForDashBoard(long packegingId, long orgId);
        PackegingLineWiseDashboardDataDTO GetPackegingDashBoard(long packegingId, long orgId);
        Task<IEnumerable<TempQRCodeTrace>> GetTempQRCodeTracesByIMEIAsync(List<string> imei, long orgId);
        bool IsExistIMEIWithStatusAndPackagingLine(long lineId, string imei, string status, long orgId);
    }
}
