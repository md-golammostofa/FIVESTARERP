using ERPBLL.Common;
using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class TempQRCodeTraceBusiness : ITempQRCodeTraceBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly TempQRCodeTraceRepository _tempQRCodeTraceRepository;
        private readonly LotInLogRepository _lotInLogRepository;
        private readonly WeightCheckedIMEILogRepository _weightCheckedIMEILogRepository;
        private readonly IIMEIWriteByQRCodeLogBusiness _iMEIWriteByQRCodeLogBusiness;
        private readonly IMEIWriteByQRCodeLogRepository _iMEIWriteByQRCodeLogRepository;
        private readonly IBatteryWriteByIMEILogBusiness _batteryWriteByIMEILogBusiness;
        private readonly BatteryWriteByIMEILogRepository _batteryWriteByIMEILogRepository;
        public TempQRCodeTraceBusiness(IProductionUnitOfWork productionDb, IIMEIWriteByQRCodeLogBusiness iMEIWriteByQRCodeLogBusiness, IBatteryWriteByIMEILogBusiness batteryWriteByIMEILogBusiness)
        {
            this._productionDb = productionDb;
            this._iMEIWriteByQRCodeLogBusiness = iMEIWriteByQRCodeLogBusiness;
            this._tempQRCodeTraceRepository = new TempQRCodeTraceRepository(this._productionDb);
            this._lotInLogRepository = new LotInLogRepository(this._productionDb);
            this._weightCheckedIMEILogRepository = new WeightCheckedIMEILogRepository(this._productionDb);
            this._iMEIWriteByQRCodeLogRepository = new IMEIWriteByQRCodeLogRepository(this._productionDb);
            this._batteryWriteByIMEILogBusiness = batteryWriteByIMEILogBusiness;
            this._batteryWriteByIMEILogRepository = new BatteryWriteByIMEILogRepository(this._productionDb);
        }

        public async Task<TempQRCodeTrace> GetIMEIinQRCode(string imei, string status, long floorId, long packagingId, long orgId)
        {
            return await _productionDb.Db.Database.SqlQuery<TempQRCodeTrace>(string.Format(@"Select * From tblTempQRCodeTrace Where  IMEI LIKE'%{0}%' and StateStatus IN({1}) and ProductionFloorId ={2} and PackagingLineId ={3} and OrganizationId = {4}", Utility.ParamChecker(imei), Utility.ParamChecker(status), floorId, packagingId, orgId)).FirstOrDefaultAsync();
        }

        public async Task<TempQRCodeTrace> GetIMEIWithOutThisQRCode(string imei, long codeId, long orgId)
        {
            return await _tempQRCodeTraceRepository.GetOneByOrgAsync(s => s.IMEI.Contains(imei) && s.CodeId != codeId && s.OrganizationId == orgId);
        }

        public async Task<TempQRCodeTrace> GetIMEIWithThisQRCode(string imei, long codeId, long orgId)
        {
            return await _tempQRCodeTraceRepository.GetOneByOrgAsync(s => s.IMEI.Contains(imei) && s.CodeId == codeId && s.OrganizationId == orgId);
        }

        public TempQRCodeTrace GetTempQRCodeTraceByCode(string code, long orgId)
        {
            return _tempQRCodeTraceRepository.GetOneByOrg(q => q.CodeNo == code && q.OrganizationId == orgId);
        }

        public async Task<TempQRCodeTrace> GetTempQRCodeTraceByCodeAsync(string code, long orgId)
        {
            return await _tempQRCodeTraceRepository.GetOneByOrgAsync(s => s.CodeNo == code && s.OrganizationId == orgId);
        }

        public async Task<TempQRCodeTrace> GetTempQRCodeTraceByCodeWithFloorAsync(string code,string status,long? floorId, long orgId)
        {
            return await _productionDb.Db.Database.SqlQuery<TempQRCodeTrace>(string.Format(@"Select * From tblTempQRCodeTrace Where  CodeNo ='{0}' and StateStatus IN({1}) and ProductionFloorId ={2} and OrganizationId = {3}", code, status, floorId, orgId)).FirstOrDefaultAsync();
                
                //_tempQRCodeTraceRepository.GetOneByOrgAsync(s => s.CodeNo == code && s.ProductionFloorId == floorId && s.OrganizationId == orgId);
        }

        public TempQRCodeTrace GetTempQRCodeTraceByIMEI(string imei, long orgId)
        {
            return _tempQRCodeTraceRepository.GetOneByOrg(s => s.IMEI.Contains(imei) && s.OrganizationId == orgId);
        }

        public IEnumerable<TempQRCodeTrace> GetTempQRCodeTraceByOrg(long orgId)
        {
            return _tempQRCodeTraceRepository.GetAll(q => q.OrganizationId == orgId);
        }

        public async Task<IEnumerable<TempQRCodeTrace>> GetTempQRCodeTracesByQRCodesAsync(List<string> qrCodes, long orgId)
        {
            return await _tempQRCodeTraceRepository.GetAllAsync(s => qrCodes.Contains(s.CodeNo) && s.OrganizationId == orgId);
        }

        public bool IsExistIMEIWithStatus(string imei, string status, long orgId)
        {
            return this._tempQRCodeTraceRepository.GetOneByOrg(s => s.OrganizationId == orgId && s.IMEI.Contains(imei) && s.StateStatus == status) != null;
        }

        public bool IsExistQRCodeWithStatus(string qrCode, string status, long orgId)
        {
            return this._tempQRCodeTraceRepository.GetOneByOrg(s => s.OrganizationId == orgId && s.CodeNo == qrCode && s.StateStatus == status) != null;
        }

        public bool IsExistQRCodeWithStatus(string qrCode, DateTime date, string status, long orgId)
        {
            return this._tempQRCodeTraceRepository.GetOneByOrg(s => s.OrganizationId == orgId && s.CodeNo == qrCode && s.StateStatus == status && DbFunctions.TruncateTime(s.EntryDate) == DbFunctions.TruncateTime(date)) != null;
        }

        public async Task<bool> SaveBatteryCodeAsync(BatteryWriteDTO dto, long userId, long orgId)
        {
            var imeiInDb = await GetIMEIinQRCode(dto.imei,string.Format(@"'IMEI-Pass'"),dto.floorId,dto.packagingLineId, orgId);

            if(imeiInDb != null && string.IsNullOrEmpty(imeiInDb.BatteryCode))
            {
                imeiInDb.StateStatus = QRCodeStatus.Bettery;
                imeiInDb.BatteryCode = dto.batteryCode;
                imeiInDb.UpdateDate = DateTime.Now;
                imeiInDb.UpUserId = userId;
                _tempQRCodeTraceRepository.Update(imeiInDb);

                //Update Battery Code
                //if (imeiInDb.BatteryCode != null)
                //{
                //    var logInfo = await _batteryWriteByIMEILogBusiness.GetLogInfoByIMEI(imeiInDb.IMEI, orgId);
                //    if (logInfo != null)
                //    {
                //        logInfo.BatteryCode = imeiInDb.BatteryCode;
                //        logInfo.UpdateDate = DateTime.Now;
                //        logInfo.UpUserId = userId;
                //        _batteryWriteByIMEILogRepository.Update(logInfo);
                //    }
                //}
                //else
                //{
                    BatteryWriteByIMEILog batteryWrite = new BatteryWriteByIMEILog()
                    {
                        AssemblyId = imeiInDb.AssemblyId,
                        AssemblyLineName = imeiInDb.AssemblyLineName,
                        BatteryCode = imeiInDb.BatteryCode,
                        CodeId = imeiInDb.CodeId,
                        CodeNo = imeiInDb.CodeNo,
                        ColorId = imeiInDb.ColorId,
                        ColorName = imeiInDb.ColorName,
                        DescriptionId = imeiInDb.DescriptionId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        IMEI = imeiInDb.IMEI,
                        ItemId = imeiInDb.ItemId,
                        ItemName = imeiInDb.ItemName,
                        ItemTypeId = imeiInDb.ItemTypeId,
                        ModelName = imeiInDb.ModelName,
                        OrganizationId = orgId,
                        PackagingLineId = imeiInDb.PackagingLineId,
                        PackagingLineName = imeiInDb.PackagingLineName,
                        ProductionFloorId = imeiInDb.ProductionFloorId,
                        ProductionFloorName = imeiInDb.ProductionFloorName,
                        QCLineId = imeiInDb.QCLineId,
                        QCLineName = imeiInDb.QCLineName,
                        ReferenceId = imeiInDb.ReferenceId,
                        ReferenceNumber = imeiInDb.ReferenceNumber,
                        Remarks = imeiInDb.Remarks,
                        StateStatus = imeiInDb.StateStatus,
                        WarehouseId = imeiInDb.WarehouseId, 
                    };
                    _batteryWriteByIMEILogRepository.Insert(batteryWrite);
                //}
            }
            return await _batteryWriteByIMEILogRepository.SaveAsync();
        }

        public async Task<bool> SaveQRCodeIEMIAsync(IMEIWriteDTO dto, long userId, long orgId)
        {
            var qrCodeInDb = await GetTempQRCodeTraceByCodeAsync(dto.qrCode, orgId);
            if(qrCodeInDb != null)
            {
                var previousBarCode = qrCodeInDb.IMEI;
                qrCodeInDb.IMEI = dto.barCode;
                //var preIEMIinDb = string.IsNullOrEmpty(qrCodeInDb.PreviousIMEI) ? "" : qrCodeInDb.PreviousIMEI +",";
                //qrCodeInDb.PreviousIMEI = preIEMIinDb+previousBarCode;
                qrCodeInDb.PreviousIMEI = previousBarCode;
                qrCodeInDb.StateStatus = "PackagingLine";
                qrCodeInDb.UpdateDate = DateTime.Now;
                qrCodeInDb.PackagingLineId = dto.packagingLineId;
                qrCodeInDb.PackagingLineName = dto.packagingLineName;
                qrCodeInDb.UpUserId = userId;
                _tempQRCodeTraceRepository.Update(qrCodeInDb);

                if (qrCodeInDb.PreviousIMEI != null)
                {
                    var logInfo = await _iMEIWriteByQRCodeLogBusiness.GetLogInfoByIMEI(qrCodeInDb.PreviousIMEI, orgId);
                    if (logInfo != null)
                    {
                        logInfo.IMEI = qrCodeInDb.IMEI;
                        logInfo.UpdateDate = DateTime.Now;
                        logInfo.UpUserId = userId;
                        _iMEIWriteByQRCodeLogRepository.Update(logInfo);
                    }
                }
                else
                {
                    IMEIWriteByQRCodeLog iMEIWrite = new IMEIWriteByQRCodeLog()
                    {
                        AssemblyId = qrCodeInDb.AssemblyId,
                        AssemblyLineName = qrCodeInDb.AssemblyLineName,
                        CodeId = qrCodeInDb.CodeId,
                        CodeNo = qrCodeInDb.CodeNo,
                        ColorId = qrCodeInDb.ColorId,
                        ColorName = qrCodeInDb.ColorName,
                        DescriptionId = qrCodeInDb.DescriptionId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        IMEI = qrCodeInDb.IMEI,
                        ItemId = qrCodeInDb.ItemId,
                        ItemName = qrCodeInDb.ItemName,
                        ItemTypeId = qrCodeInDb.ItemTypeId,
                        ModelName = qrCodeInDb.ModelName,
                        OrganizationId = orgId,
                        PackagingLineId = qrCodeInDb.PackagingLineId,
                        PackagingLineName = qrCodeInDb.PackagingLineName,
                        ProductionFloorId = qrCodeInDb.ProductionFloorId,
                        ProductionFloorName = qrCodeInDb.ProductionFloorName,
                        QCLineId = qrCodeInDb.QCLineId,
                        QCLineName = qrCodeInDb.QCLineName,
                        ReferenceId = qrCodeInDb.ReferenceId,
                        ReferenceNumber = qrCodeInDb.ReferenceNumber,
                        Remarks = qrCodeInDb.Remarks,
                        StateStatus = qrCodeInDb.StateStatus,
                        WarehouseId = qrCodeInDb.WarehouseId,
                    };
                    _iMEIWriteByQRCodeLogRepository.Insert(iMEIWrite);
                }
            }
            return await _iMEIWriteByQRCodeLogRepository.SaveAsync();
        }

        public bool SaveQRCodeStatusByLotIn(string qrCode, long orgId, long userId)
        {
            var qrCodeInfo = GetTempQRCodeTraceByCode(qrCode, orgId);
            qrCodeInfo.StateStatus = "Lot-In";
            qrCodeInfo.UpdateDate = DateTime.Now;
            qrCodeInfo.UpUserId = userId;
            _tempQRCodeTraceRepository.Update(qrCodeInfo);

            LotInLog lotInLog = new LotInLog();
            lotInLog.AssemblyId = qrCodeInfo.AssemblyId;
            lotInLog.CodeId = qrCodeInfo.CodeId;
            lotInLog.CodeNo = qrCodeInfo.CodeNo;
            lotInLog.DescriptionId = qrCodeInfo.DescriptionId;
            lotInLog.EntryDate = DateTime.Now;
            lotInLog.EUserId = userId;
            lotInLog.ItemId = qrCodeInfo.ItemId;
            lotInLog.ItemTypeId = qrCodeInfo.ItemTypeId;
            lotInLog.OrganizationId = qrCodeInfo.OrganizationId;
            lotInLog.ProductionFloorId = qrCodeInfo.ProductionFloorId;
            lotInLog.ReferenceId = qrCodeInfo.ReferenceId;
            lotInLog.ReferenceNumber = qrCodeInfo.ReferenceNumber;
            lotInLog.Remarks = qrCodeInfo.Remarks;
            lotInLog.StateStatus = qrCodeInfo.StateStatus;
            lotInLog.WarehouseId = qrCodeInfo.WarehouseId;
            _lotInLogRepository.Insert(lotInLog);

            return _lotInLogRepository.Save();
        }

        public bool SaveTempQRCodeTrace(List<TempQRCodeTraceDTO> dtos, long userId, long orgId)
        {
            List<TempQRCodeTrace> list = new List<TempQRCodeTrace>();
            foreach (var item in dtos)
            {
                TempQRCodeTrace qRCode = new TempQRCodeTrace()
                {
                    ProductionFloorId = item.ProductionFloorId,
                    DescriptionId = item.DescriptionId,
                    ColorId = item.ColorId,
                    ColorName = item.ColorName,
                    CodeNo = item.CodeNo,
                    CodeImage = item.CodeImage,
                    WarehouseId = item.WarehouseId,
                    ItemTypeId = item.ItemTypeId,
                    ItemId = item.ItemId,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                    ReferenceId = item.ReferenceId,
                    ReferenceNumber = item.ReferenceNumber,
                    Remarks = item.Remarks,
                    ProductionFloorName = item.ProductionFloorName,
                    ModelName = item.ModelName,
                    WarehouseName = item.WarehouseName,
                    ItemTypeName = item.ItemTypeName,
                    ItemName = item.ItemName,
                    AssemblyId = item.AssemblyId,
                    AssemblyLineName = item.AssemblyLineName
                };
                list.Add(qRCode);
            }
            if (list.Count > 0)
            {
                _tempQRCodeTraceRepository.InsertAll(list);
            }
            return _tempQRCodeTraceRepository.Save();
        }

        public async Task<bool> UpdateQRCodeBatchAsync(List<TempQRCodeTrace> qrCodes, long orgId)
        {
             _tempQRCodeTraceRepository.UpdateAll(qrCodes);
            return await _tempQRCodeTraceRepository.SaveAsync();
        }

        public bool UpdateQRCodeStatus(string qrCode, string status, long orgId)
        {
            var qrCodeInDb = GetTempQRCodeTraceByCode(qrCode, orgId);
            qrCodeInDb.StateStatus = status;
            _tempQRCodeTraceRepository.Update(qrCodeInDb);
            return _tempQRCodeTraceRepository.Save();
        }

        public async Task<bool> UpdateQRCodeStatusAsync(string qrCode, string status, long orgId)
        {
            var qrCodeInDb = GetTempQRCodeTraceByCode(qrCode, orgId);
            qrCodeInDb.StateStatus = status;
            qrCodeInDb.UpdateDate = DateTime.Now;
            _tempQRCodeTraceRepository.Update(qrCodeInDb);
            return await _tempQRCodeTraceRepository.SaveAsync();
        }

        public async Task<bool> UpdateQRCodeStatusWithQCAsync(string qrCode, string status,long qcId, string qcName, long orgId)
        {
            var qrCodeInDb = GetTempQRCodeTraceByCode(qrCode, orgId);
            qrCodeInDb.StateStatus = status;
            qrCodeInDb.QCLineId = qcId;
            qrCodeInDb.QCLineName = qcName;
            _tempQRCodeTraceRepository.Update(qrCodeInDb);
            return await _tempQRCodeTraceRepository.SaveAsync();
        }
        public IEnumerable<TempQRCodeTraceDTO> GetAssemblyLineWiseDataForDashBoard(long assemblyId, long orgId)
        {
            var queryForLotIn = _productionDb.Db.Database.SqlQuery<TempQRCodeTraceDTO>(string.Format(@"Select b.BrandName,tqrt.ItemName,des.DescriptionName'ModelName',cl.ColorName From tblLotInLog lil
Left Join [Production].dbo.tblTempQRCodeTrace tqrt on tqrt.CodeId = lil.CodeId
Left Join [Inventory].dbo.tblDescriptions des on lil.DescriptionId = des.DescriptionId
Left Join [Inventory].dbo.tblBrand b on des.BrandId = b.BrandId
Left Join [Inventory].dbo.tblColors cl on tqrt.ColorId = cl.ColorId
where CAST(lil.EntryDate as date) = CAST(GETDATE() as date) and lil.AssemblyId={0} and lil.OrganizationId={1} and
lil.StateStatus = 'Lot-In' order by lil.LotInLogId desc", assemblyId, orgId)).ToList();
            if (queryForLotIn.Count() == 0)
            {
                return _productionDb.Db.Database.SqlQuery<TempQRCodeTraceDTO>(string.Format(@"Select b.BrandName,tqrt.ItemName,des.DescriptionName'ModelName',cl.ColorName From tblTempQRCodeTrace tqrt
Left Join [Inventory].dbo.tblDescriptions des on tqrt.DescriptionId = des.DescriptionId
Left Join [Inventory].dbo.tblBrand b on des.BrandId = b.BrandId
Left Join [Inventory].dbo.tblColors cl on tqrt.ColorId = cl.ColorId
where CAST(tqrt.UpdateDate as date) = CAST(GETDATE() as date) and tqrt.AssemblyId={0} and tqrt.OrganizationId={1} Order By tqrt.UpdateDate Desc", assemblyId, orgId));
            }
            return queryForLotIn;
            //            return _productionDb.Db.Database.SqlQuery<TempQRCodeTraceDTO>(string.Format(@"Select * From tblTempQRCodeTrace tqrt
            //Left Join [Inventory-MC].dbo.tblDescriptions des on tqrt.DescriptionId = des.DescriptionId
            //Left Join [Inventory-MC].dbo.tblBrand b on des.BrandId = b.BrandId
            //where CAST(tqrt.UpdateDate as date) = CAST(GETDATE() as date) and tqrt.AssemblyId={0} and tqrt.OrganizationId={1} Order By tqrt.UpdateDate Desc", assemblyId, orgId));
        }
        public IEnumerable<TempQRCodeTraceDTO> GetPackegingLineWiseDataForDashBoard(long packegingId, long orgId)
        {
            var queryForIMEIWrite = _productionDb.Db.Database.SqlQuery<TempQRCodeTraceDTO>(string.Format(@"Select b.BrandName,imei.ItemName,des.DescriptionName'ModelName',imei.ColorName 
From tblIMEIWriteByQRCodeLog imei
--Left Join [Production].dbo.tblTempQRCodeTrace tqrt on tqrt.CodeId = imei.CodeId
Left Join [Inventory].dbo.tblDescriptions des on imei.DescriptionId = des.DescriptionId
Left Join [Inventory].dbo.tblBrand b on des.BrandId = b.BrandId
--Left Join [Inventory].dbo.tblColors cl on imei.ColorId = cl.ColorId
where CAST(imei.EntryDate as date) = CAST(GETDATE() as date) and imei.PackagingLineId={0} and imei.OrganizationId={1} and imei.StateStatus = 'PackagingLine' order by imei.IMEIWriteLogId desc", packegingId, orgId)).ToList();
            return queryForIMEIWrite;
        }
        public DashboardAssemblyLineWiseDataDTO GetAssemblyDashBoard(long assemblyId, long orgId)
        {
            return _productionDb.Db.Database.SqlQuery<DashboardAssemblyLineWiseDataDTO>(string.Format(@"EXEC spAssemblyDashboard {0},{1}", assemblyId, orgId)).FirstOrDefault();
        }
        public PackegingLineWiseDashboardDataDTO GetPackegingDashBoard(long packegingId, long orgId)
        {
            return _productionDb.Db.Database.SqlQuery<PackegingLineWiseDashboardDataDTO>(string.Format(@"EXEC [spPackegingDashboard] {0},{1}", packegingId, orgId)).FirstOrDefault();
        }
        public IEnumerable<TempQRCodeTraceDTO> GetDailyProductionSummeryReport(long assemblyId, long modelId, string fromDate, string toDate, long orgId)
        {
            return _productionDb.Db.Database.SqlQuery<TempQRCodeTraceDTO>(string.Format(@"Exec [spDailyProductionSummery] '{0}','{1}',{2},{3},{4}", fromDate, toDate, assemblyId, modelId, orgId)).ToList();
        }
        public T_StockDTO GetIMEIFromTStock(string imei)
        {
            return _productionDb.Db.Database.SqlQuery<T_StockDTO>(string.Format(@"EXEC spIMEIFromTStock '{0}'", imei)).FirstOrDefault();
        }
    }
}
