﻿using ERPBO.Common;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IFinishGoodsStockDetailBusiness
    {
        IEnumerable<FinishGoodsStockDetail> GelAllFinishGoodsStockDetailByOrgId(long orgId);
        bool SaveFinishGoodsStockIn(List<FinishGoodsStockDetailDTO> finishGoodsStockDetailDTOs, long userId, long orgId);
        bool SaveFinishGoodsStockOut(List<FinishGoodsStockDetailDTO> finishGoodsStockDetailDTOs, long userId, long orgId);
        IEnumerable<FinishGoodsStockDetailInfoListDTO> GetFinishGoodsStockDetailInfoList(long? lineId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string stockStatus, string fromDate, string toDate, string refNum);
        IEnumerable<DashboardLineWiseProductionDTO> DashboardLineWiseDailyProduction(long orgId);
        IEnumerable<DashboardLineWiseProductionDTO> DashboardLineWiseOverAllProduction(long orgId);
        IEnumerable<DaysAndLineWiseProductionChart> GetDayAndLineProductionChartsData(long orgId);
        IEnumerable<DayAndModelWiseProductionChart> GetDayAndModelWiseProductionChart(long orgId);
        Task<bool> SaveFinishGoodsByIMEIAsync(string imei, long userId, long orgId);
        Task<bool> SaveFinishGoodsStockOutAsync(List<FinishGoodsStockDetailDTO> finishGoodsStockDetailDTOs, long userId, long orgId);

    }
}
