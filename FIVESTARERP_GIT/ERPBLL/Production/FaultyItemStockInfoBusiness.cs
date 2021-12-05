using ERPBLL.Common;
using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class FaultyItemStockInfoBusiness : IFaultyItemStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly FaultyItemStockInfoRepository _faultyItemStockInfoRepository;
        public FaultyItemStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._faultyItemStockInfoRepository = new FaultyItemStockInfoRepository(this._productionDb);
        }
        public IEnumerable<FaultyItemStockInfo> GetFaultyItemByQCandRepair(long qcId, long repairLine, long orgId)
        {
            return this._faultyItemStockInfoRepository.GetAll(f => f.QCId == qcId && f.RepairLineId == repairLine && f.OrganizationId == orgId);
        }
        public FaultyItemStockInfo GetFaultyItemStockInfoByQCandRepairandItem(long qcId, long repairLine, long itemId, long orgId)
        {
            return this._faultyItemStockInfoRepository.GetOneByOrg(f => f.QCId== qcId && f.RepairLineId == repairLine && f.ItemId == itemId && f.OrganizationId == orgId);
        }

        public FaultyItemStockInfo GetFaultyItemStockInfoByRepairAndItem(long repairLine, long itemId, long orgId)
        {
            return this._faultyItemStockInfoRepository.GetOneByOrg(f => f.RepairLineId == repairLine && f.ItemId == itemId && f.OrganizationId == orgId);
        }

        public FaultyItemStockInfo GetFaultyItemStockInfoByRepairAndModelAndItem(long repairLine, long modelId, long itemId, long orgId)
        {
            return this._faultyItemStockInfoRepository.GetOneByOrg(f => f.RepairLineId == repairLine && f.DescriptionId == modelId && f.ItemId == itemId && f.OrganizationId == orgId);
        }

        public FaultyItemStockInfo GetFaultyItemStockInfoByRepairAndModelAndItemAndFultyType(long repairLine, long modelId, long itemId, bool isChinaFaulty, long orgId)
        {
            return this._faultyItemStockInfoRepository.GetOneByOrg(f => f.RepairLineId == repairLine && f.DescriptionId == modelId && f.ItemId == itemId /*&& f.IsChinaFaulty == isChinaFaulty*/ && f.OrganizationId == orgId);
        }

        public IEnumerable<FaultyItemStockInfo> GetFaultyItemStockInfos(long orgId)
        {
            return this._faultyItemStockInfoRepository.GetAll(f => f.OrganizationId == orgId);
        }

        public IEnumerable<FaultyItemStockInfoDTO> GetFaultyItemStockInfosByQuery(long? floorId, long? repairId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq,string reqFor, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<FaultyItemStockInfoDTO>(QueryForFaultyItemStockInfos(floorId,repairId,modelId,warehouseId,itemTypeId,itemId,lessOrEq,reqFor,orgId)).ToList();
        }

        private string QueryForFaultyItemStockInfos(long? floorId, long? repairId, long? modelId, long? warehouseId, long? itemTypeId, long? itemId, string lessOrEq, string reqFor, long orgId)
        {
            string param = string.Empty;
            string query = string.Empty;

            param += string.Format(@" and fs.OrganizationId={0}", orgId);
            if (floorId != null && floorId > 0)
            {
                param += string.Format(@" and fs.ProductionFloorId={0}", floorId);
            }
            if (repairId != null && repairId > 0)
            {
                param += string.Format(@" and fs.RepairLineId={0}", repairId);
            }
            if (modelId != null && modelId > 0)
            {
                param += string.Format(@" and fs.DescriptionId={0}", modelId);
            }
            if (warehouseId != null && warehouseId > 0)
            {
                param += string.Format(@" and fs.WarehouseId={0}", warehouseId);
            }
            if (itemTypeId != null && itemTypeId > 0)
            {
                param += string.Format(@" and fs.ItemTypeId={0}", itemTypeId);
            }
            if (itemId != null && itemId > 0)
            {
                param += string.Format(@" and fs.ItemId={0}", itemId);
            }    

            query = string.Format(@"Select fs.FaultyItemStockInfoId,ISNULL(pl.LineNumber,'') 'ProductionFloorName',fs.RepairLineId,rl.RepairLineName 'RepairName', ISNULL(de.DescriptionName,'') 'ModelName', 
ISNULL(w.WarehouseName,'') 'WarehouseName', ISNULL(it.ItemName,'') 'ItemTypeName', ISNULL(i.ItemName,'') 'ItemName',
ISNULL(u.UnitSymbol,'') 'UnitName',fs.ChinaMadeFaultyStockInQty,fs.ChinaMadeFaultyStockOutQty,fs.ManMadeFaultyStockInQty,fs.ManMadeFaultyStockOutQty
From tblFaultyItemStockInfo fs
Left Join tblProductionLines pl on fs.ProductionFloorId= pl.LineId
Left Join tblRepairLine rl on fs.RepairLineId= rl.RepairLineId
Left Join [Inventory].dbo.tblDescriptions de on fs.DescriptionId= de.DescriptionId
Left Join [Inventory].dbo.[tblWarehouses] w on fs.WarehouseId = w.Id
Left Join [Inventory].dbo.[tblItemTypes] it on fs.ItemTypeId = it.ItemId
Left Join [Inventory].dbo.[tblItems] i on fs.ItemId = i.ItemId
Left Join [Inventory].dbo.[tblUnits] u on fs.UnitId = u.UnitId
Where 1=1 {0}", Utility.ParamChecker(param));
            return query;
        }
        public IEnumerable<FaultyItemStockInfoDTO> GetRepairLineStocksForReturnStock(long repairLineId, long floorId, long modelId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<FaultyItemStockInfoDTO>(string.Format(@"Select rsi.FaultyItemStockInfoId,rsi.WarehouseId,w.WarehouseName,rsi.ItemTypeId,it.ItemName'ItemTypeName',rsi.ItemId,i.ItemName,rsi.UnitId ,u.UnitSymbol 'UnitName',rsi.ChinaMadeFaultyStockInQty,rsi.ChinaMadeFaultyStockOutQty,rsi.ManMadeFaultyStockInQty,rsi.ManMadeFaultyStockOutQty
From tblFaultyItemStockInfo rsi
Inner Join [Inventory].dbo.tblWarehouses w on rsi.WarehouseId = w.Id
Inner Join [Inventory].dbo.tblItemTypes it on rsi.ItemTypeId = it.ItemId
Inner Join [Inventory].dbo.tblItems i on rsi.ItemId = i.ItemId
Inner Join [Inventory].dbo.tblUnits u on rsi.UnitId = u.UnitId
Where 1=1 and rsi.OrganizationId={0} and rsi.ProductionFloorId={1} and rsi.RepairLineId={2} and rsi.DescriptionId= {3}  and ((rsi.ChinaMadeFaultyStockInQty-rsi.ChinaMadeFaultyStockOutQty) > 0 OR (rsi.ManMadeFaultyStockInQty-rsi.ManMadeFaultyStockOutQty) > 0)", orgId, floorId, repairLineId, modelId)).ToList();
        }
    }
}
