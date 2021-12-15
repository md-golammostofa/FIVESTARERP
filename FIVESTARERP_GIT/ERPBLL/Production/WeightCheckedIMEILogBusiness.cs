using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Inventory.DomainModels;
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
    public class WeightCheckedIMEILogBusiness : IWeightCheckedIMEILogBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly ITempQRCodeTraceBusiness _tempQRCodeTraceBusiness;
        private readonly IPackagingLineStockDetailBusiness _packagingLineStockDetailBusiness;
        private readonly IItemPreparationInfoBusiness _itemPreparationInfoBusiness;
        private readonly IItemPreparationDetailBusiness _itemPreparationDetailBusiness;
        private readonly WeightCheckedIMEILogRepository _weightCheckedIMEILogRepository;
        private readonly TempQRCodeTraceRepository _tempQRCodeTraceRepository;
        public WeightCheckedIMEILogBusiness(IProductionUnitOfWork productionDb, ITempQRCodeTraceBusiness tempQRCodeTraceBusiness, IItemPreparationInfoBusiness itemPreparationInfoBusiness, IItemPreparationDetailBusiness itemPreparationDetailBusiness, IPackagingLineStockDetailBusiness packagingLineStockDetailBusiness)
        {
            this._productionDb = productionDb;
            this._tempQRCodeTraceBusiness = tempQRCodeTraceBusiness;
            this._packagingLineStockDetailBusiness = packagingLineStockDetailBusiness;
            this._itemPreparationDetailBusiness = itemPreparationDetailBusiness;
            this._itemPreparationInfoBusiness = itemPreparationInfoBusiness;
            this._weightCheckedIMEILogRepository = new WeightCheckedIMEILogRepository(this._productionDb);
            this._tempQRCodeTraceRepository = new TempQRCodeTraceRepository(this._productionDb);
        }
        public IEnumerable<WeightCheckedIMEILog> GetAllWeightCheckedInfoByUserId(long userId, long orgId, DateTime date)
        {
            return _weightCheckedIMEILogRepository.GetAll(s => s.EUserId == userId && s.OrganizationId == orgId && DbFunctions.TruncateTime(s.EntryDate) == DbFunctions.TruncateTime(date)).ToList();
        }
        public IEnumerable<WeightCheckedIMEILogDTO> GetAllDataByDateWise(string fromDate, string toDate, string imei, long userId)
        {
            IEnumerable<WeightCheckedIMEILogDTO> iMEIDTOs = new List<WeightCheckedIMEILogDTO>();
            iMEIDTOs = this._productionDb.Db.Database.SqlQuery<WeightCheckedIMEILogDTO>(QueryForWeightCheckedDateWiseData(fromDate, toDate, imei, userId)).ToList();
            return iMEIDTOs;
        }

        private string QueryForWeightCheckedDateWiseData(string fromDate, string toDate, string imei, long userId)
        {
            string query = string.Empty;
            string param = string.Empty;
            string i = imei.Trim();
            if (!string.IsNullOrEmpty(i) && i != "")
            {
                param += string.Format(@" and bs.IMEI Like'%{0}%'", i);
            }

            if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "" && !string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(bs.EntryDate as date) between '{0}' and '{1}'", fDate, tDate);
            }
            else if (!string.IsNullOrEmpty(fromDate) && fromDate.Trim() != "")
            {
                string fDate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(bs.EntryDate as date)='{0}'", fDate);
            }
            else if (!string.IsNullOrEmpty(toDate) && toDate.Trim() != "")
            {
                string tDate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");
                param += string.Format(@" and Cast(bs.EntryDate as date)='{0}'", tDate);
            }

            query = string.Format(@" Select bs.EntryDate, au.UserName 'EUserName', bs.StateStatus, bs.IMEI, bs.WeightCheckedIMEILogId, bs.Remarks, bs.CodeNo, bs.ReferenceNumber
From tblWeightCheckedIMEILog bs
Left Join [ControlPanel-MC].dbo.tblApplicationUsers au on bs.EUserId = au.UserId
Where bs.EUserId= {0}{1}", userId, Utility.ParamChecker(param));
            return query;
        }
        public async Task<bool> SaveIMEIStatusForWeightCheck(string imei, long orgId, long userId)
        {
            var imeiInfo = _tempQRCodeTraceBusiness.GetTempQRCodeTraceByIMEI(imei, orgId);
            if (imeiInfo != null && imeiInfo.StateStatus == QRCodeStatus.Bettery)
            {
                // Item Preparation Info //
                var itemPreparationInfo = await _itemPreparationInfoBusiness.GetPreparationInfoByModelAndItemAndTypeAsync(ItemPreparationType.Packaging, imeiInfo.DescriptionId.Value, imeiInfo.ItemId.Value, orgId);

                // Item Preparation Detail //
                var itemPreparationDetail = (List<ItemPreparationDetail>)await _itemPreparationDetailBusiness.GetItemPreparationDetailsByInfoIdAsync(itemPreparationInfo.PreparationInfoId, orgId);

                // Packaging Line Stock //
                List<PackagingLineStockDetailDTO> packagingRawStocks = new List<PackagingLineStockDetailDTO>();

                foreach (var item in itemPreparationDetail)
                {
                    PackagingLineStockDetailDTO packagingRawStock = new PackagingLineStockDetailDTO()
                    {
                        ProductionLineId = imeiInfo.PackagingLineId,
                        PackagingLineId = imeiInfo.PackagingLineId,
                        WarehouseId = item.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        DescriptionId = imeiInfo.DescriptionId,
                        UnitId = item.UnitId,
                        OrganizationId = orgId,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        StockStatus = StockStatus.StockOut,
                        RefferenceNumber = imeiInfo.IMEI,
                        Remarks = imeiInfo.CodeNo
                    };
                    packagingRawStocks.Add(packagingRawStock);
                }

                imeiInfo.StateStatus = QRCodeStatus.Weight;
                imeiInfo.UpdateDate = DateTime.Now;
                imeiInfo.UpUserId = userId;
                _tempQRCodeTraceRepository.Update(imeiInfo);

                WeightCheckedIMEILog weightCheck = new WeightCheckedIMEILog();
                weightCheck.AssemblyId = imeiInfo.AssemblyId;
                weightCheck.CodeId = imeiInfo.CodeId;
                weightCheck.CodeNo = imeiInfo.CodeNo;
                weightCheck.DescriptionId = imeiInfo.DescriptionId;
                weightCheck.EntryDate = DateTime.Now;
                weightCheck.EUserId = userId;
                weightCheck.IMEI = imeiInfo.IMEI;
                weightCheck.ItemId = imeiInfo.ItemId;
                weightCheck.ItemTypeId = imeiInfo.ItemTypeId;
                weightCheck.OrganizationId = imeiInfo.OrganizationId;
                weightCheck.ProductionFloorId = imeiInfo.ProductionFloorId;
                weightCheck.ReferenceId = imeiInfo.ReferenceId;
                weightCheck.ReferenceNumber = imeiInfo.ReferenceNumber;
                weightCheck.Remarks = imeiInfo.Remarks;
                weightCheck.StateStatus = imeiInfo.StateStatus;
                weightCheck.WarehouseId = imeiInfo.WarehouseId;

                _weightCheckedIMEILogRepository.Insert(weightCheck);
                if (await _weightCheckedIMEILogRepository.SaveAsync())
                {
                    return await _packagingLineStockDetailBusiness.SavePackagingLineStockOutAsync(packagingRawStocks, userId, orgId, string.Empty);
                }
            }
            return false;
        }
    }
}
