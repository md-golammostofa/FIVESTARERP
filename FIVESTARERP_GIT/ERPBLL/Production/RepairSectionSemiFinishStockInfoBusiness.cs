using ERPBLL.Common;
using ERPBLL.Production.Interface;
using ERPBO.Common;
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
   public class RepairSectionSemiFinishStockInfoBusiness: IRepairSectionSemiFinishStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly RepairSectionSemiFinishStockInfoRepository _repairSectionSemiFinishStockInfoRepository;
        private readonly RepairSectionSemiFinishStockDetailsRepository _repairSectionSemiFinishStockDetailsRepository;
        private readonly IRepairSectionSemiFinishTransferInfoBusiness _repairSectionSemiFinishTransferInfoBusiness;
        private readonly IQRCodeTransferToRepairInfoBusiness _qRCodeTransferToRepairInfoBusiness;

        public RepairSectionSemiFinishStockInfoBusiness(IProductionUnitOfWork productionDb, IRepairSectionSemiFinishTransferInfoBusiness repairSectionSemiFinishTransferInfoBusiness, IQRCodeTransferToRepairInfoBusiness qRCodeTransferToRepairInfoBusiness)
        {
            this._productionDb = productionDb;
            this._repairSectionSemiFinishStockInfoRepository = new RepairSectionSemiFinishStockInfoRepository(this._productionDb);
            this._repairSectionSemiFinishStockDetailsRepository = new RepairSectionSemiFinishStockDetailsRepository(this._productionDb);
            this._repairSectionSemiFinishTransferInfoBusiness = repairSectionSemiFinishTransferInfoBusiness;
            this._qRCodeTransferToRepairInfoBusiness = qRCodeTransferToRepairInfoBusiness;
        }

        public IEnumerable<RepairSectionSemiFinishStockInfoDTO> GetAllStockInfo(long? flId, long? qcId, long? rqId, long? assId, long? warId, long? moId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<RepairSectionSemiFinishStockInfoDTO>(QueryForGetAllStockInfo(flId,qcId,rqId,assId,warId,moId ,orgId)).ToList();
        }
        private string QueryForGetAllStockInfo(long? flId, long? qcId, long? rqId, long? assId, long? warId, long? moId, long orgId)
        {
            string param = string.Empty;
            string query = string.Empty;

            param += string.Format(@" and rsinfo.OrganizationId={0}", orgId);
            if (flId != null && flId > 0)
            {
                param += string.Format(@" and rsinfo.ProductionFloorId={0}", flId);
            }
            if (moId != null && moId > 0)
            {
                param += string.Format(@" and rsinfo.DescriptionId={0}", moId);
            }
            if (assId != null && assId > 0)
            {
                param += string.Format(@" and rsinfo.AssemblyLineId={0}", assId);
            }
            if (rqId != null && rqId > 0)
            {
                param += string.Format(@" and rsinfo.RepairLineId={0}", rqId);
            }
            if (qcId != null && qcId > 0)
            {
                param += string.Format(@" and rsinfo.QCId={0}", qcId);
            }
            if (warId != null && warId > 0)
            {
                param += string.Format(@" and rsinfo.WarehouseId={0}", warId);
            }
            query = string.Format(@"Select fl.LineNumber'FloorName',asl.AssemblyLineName,de.DescriptionName'ModelName',qc.QCName'QCLineName',rp.RepairLineName,rsinfo.StockInQty,
rsinfo.StockOutQty,Sum(rsinfo.StockInQty-rsinfo.StockOutQty)'StockQty',rsinfo.WarehouseId,war.WarehouseName,rsinfo.ProductionFloorId,rsinfo.AssemblyLineId,rsinfo.QCId,rsinfo.RepairLineId,rsinfo.DescriptionId From tblRepairSectionSemiFinishStockInfo rsinfo
Left Join tblProductionLines fl on rsinfo.ProductionFloorId=fl.LineId
Left Join tblAssemblyLines asl on rsinfo.AssemblyLineId=asl.AssemblyLineId
Left Join [Inventory].dbo.tblDescriptions de on rsinfo.DescriptionId=de.DescriptionId
Left Join tblQualityControl qc on rsinfo.QCId=qc.QCId
Left Join tblRepairLine rp on rsinfo.RepairLineId=rp.RepairLineId
Left Join [Inventory].dbo.tblWarehouses war on rsinfo.WarehouseId=war.Id
Where rsinfo.OrganizationId={0} {1}
Group By fl.LineNumber,asl.AssemblyLineName,de.DescriptionName,qc.QCName,rp.RepairLineName,rsinfo.StockInQty,
rsinfo.StockOutQty,war.WarehouseName,rsinfo.ProductionFloorId,rsinfo.AssemblyLineId,rsinfo.QCId,rsinfo.RepairLineId,rsinfo.DescriptionId,rsinfo.WarehouseId", orgId, Utility.ParamChecker(param));
            return query;
        }

        public RepairSectionSemiFinishStockInfo GetStockByOneById(long flId, long qcId, long rqId, long assId, long warId, long moId,long orgId)
        {
          return _repairSectionSemiFinishStockInfoRepository.GetOneByOrg(i => i.ProductionFloorId == flId && i.QCId == qcId && i.RepairLineId == rqId && i.AssemblyLineId == assId && i.WarehouseId == warId && i.DescriptionId == moId && i.OrganizationId==orgId);
        }

        public bool SaveStockRepairSectionSemiFinishGood(List<RepairSectionSemiFinishStockDetailsDTO> dtos,long infoId, long userId, long orgId)
        {
            bool isSuccess = false;
            List<RepairSectionSemiFinishStockDetails> stockDetails = new List<RepairSectionSemiFinishStockDetails>();
            foreach (var item in dtos)
            {
                RepairSectionSemiFinishStockDetails detail = new RepairSectionSemiFinishStockDetails()
                {
                    FloorId = item.FloorId,
                    AssemblyLineId = item.AssemblyLineId,
                    QCLineId = item.QCLineId,
                    RepairLineId = item.RepairLineId,
                    DescriptionId = item.DescriptionId,
                    WarehouseId = item.WarehouseId,
                    StateStatus = "Stock-In",
                    QRCode = item.QRCode,
                    OrganizationId = orgId,
                    EUserId = userId,
                    EntryDate = DateTime.Now,
                };
                stockDetails.Add(detail);

                var stock = GetStockByOneById(item.FloorId, item.QCLineId, item.RepairLineId, item.AssemblyLineId, item.WarehouseId.Value, item.DescriptionId, orgId);
                if (stock != null)
                {
                    stock.StockInQty += 1;
                    stock.UpdateDate = DateTime.Now;
                    stock.UpUserId = userId;

                    _repairSectionSemiFinishStockInfoRepository.Update(stock);
                    _repairSectionSemiFinishStockInfoRepository.Save();
                }
                else
                {
                    RepairSectionSemiFinishStockInfo info = new RepairSectionSemiFinishStockInfo();
                    info.ProductionFloorId = item.FloorId;
                    info.AssemblyLineId = item.AssemblyLineId;
                    info.QCId = item.QCLineId;
                    info.RepairLineId = item.RepairLineId;
                    info.DescriptionId = item.DescriptionId;
                    info.WarehouseId = item.WarehouseId;
                    info.StockInQty = 1;
                    info.StockOutQty = 0;
                    info.EUserId = userId;
                    info.EntryDate = DateTime.Now;
                    info.OrganizationId = orgId;

                    _repairSectionSemiFinishStockInfoRepository.Insert(info);
                    _repairSectionSemiFinishStockInfoRepository.Save();
                }
            }
            _repairSectionSemiFinishStockDetailsRepository.InsertAll(stockDetails);
            if (_repairSectionSemiFinishStockDetailsRepository.Save())
            {
                isSuccess = _repairSectionSemiFinishTransferInfoBusiness.UpdateStatusRepairSection(infoId, userId, orgId);
            }

            //Commented By Nishad
            //if (dtos.Count > 0)
            //{
            //    var stock = GetStockByOneById(dtos.FirstOrDefault().FloorId,dtos.FirstOrDefault().QCLineId,dtos.FirstOrDefault().RepairLineId, dtos.FirstOrDefault().AssemblyLineId, dtos.FirstOrDefault().WarehouseId.Value, dtos.FirstOrDefault().DescriptionId,orgId);
            //    if(stock != null)
            //    {
            //        stock.StockInQty += dtos.Count;
            //        stock.UpdateDate = DateTime.Now;
            //        stock.UpUserId = userId;
            //        _repairSectionSemiFinishStockInfoRepository.Update(stock);
            //        _repairSectionSemiFinishStockInfoRepository.Save();
            //    }
            //    else
            //    {
            //        RepairSectionSemiFinishStockInfo info = new RepairSectionSemiFinishStockInfo();
            //        info.ProductionFloorId = dtos.FirstOrDefault().FloorId;
            //        info.AssemblyLineId = dtos.FirstOrDefault().AssemblyLineId;
            //        info.QCId = dtos.FirstOrDefault().QCLineId;
            //        info.RepairLineId = dtos.FirstOrDefault().RepairLineId;
            //        info.DescriptionId = dtos.FirstOrDefault().DescriptionId;
            //        info.WarehouseId = dtos.FirstOrDefault().WarehouseId;
            //        info.StockInQty = dtos.Count;
            //        info.StockOutQty = 0;
            //        info.EUserId = userId;
            //        info.EntryDate = DateTime.Now;
            //        info.OrganizationId = orgId;
            //        _repairSectionSemiFinishStockInfoRepository.Insert(info);
            //        _repairSectionSemiFinishStockInfoRepository.Save();
            //    }

            //    List<RepairSectionSemiFinishStockDetails> details = new List<RepairSectionSemiFinishStockDetails>();
            //    foreach(var item in dtos)
            //    {
            //        RepairSectionSemiFinishStockDetails detail = new RepairSectionSemiFinishStockDetails()
            //        {
            //            FloorId = item.FloorId,
            //            AssemblyLineId = item.AssemblyLineId,
            //            QCLineId = item.QCLineId,
            //            RepairLineId = item.RepairLineId,
            //            DescriptionId = item.DescriptionId,
            //            WarehouseId = item.WarehouseId,
            //            StateStatus = "Stock-In",
            //            QRCode = item.QRCode,
            //            OrganizationId = orgId,
            //            EUserId = userId,
            //            EntryDate = DateTime.Now,
            //        };
            //        details.Add(detail);
            //    }
            //    _repairSectionSemiFinishStockDetailsRepository.InsertAll(details);
            //    if (_repairSectionSemiFinishStockDetailsRepository.Save() == true)
            //    {
            //        isSuccess = _repairSectionSemiFinishTransferInfoBusiness.UpdateStatusRepairSection(infoId, userId, orgId);
            //    }
            //}
            return isSuccess;
        }
        public bool SaveRepairSectionSemiFinishGoodStockInForMiniStock(List<RepairSectionSemiFinishStockDetailsDTO> dTOs, long userId, long orgId)
        {
            List<RepairSectionSemiFinishStockDetails> stockDetails = new List<RepairSectionSemiFinishStockDetails>();
            foreach (var item in dTOs)
            {
                //RepairSectionSemiFinishStockDetails stockDetail = new RepairSectionSemiFinishStockDetails
                //{
                //    FloorId = item.FloorId,
                //    AssemblyLineId = item.AssemblyLineId,
                //    QCLineId = item.QCLineId,
                //    RepairLineId = item.RepairLineId,
                //    DescriptionId = item.DescriptionId,
                //    WarehouseId = item.WarehouseId,
                //    StateStatus = "Stock-Out",
                //    QRCode = item.QRCode,
                //    OrganizationId = orgId,
                //    EUserId = userId,
                //    EntryDate = DateTime.Now,
                //    ItemId = item.ItemId,
                //    ItemTypeId = item.ItemTypeId,
                //    TransferCode = item.TransferCode,
                //};
                //stockDetails.Add(stockDetail);
                var info = this.GetStockByOneById(item.FloorId, item.QCLineId, item.RepairLineId, item.AssemblyLineId, item.WarehouseId.Value, item.DescriptionId, orgId);
                if (info != null)
                {
                    info.StockInQty += item.StockQty;
                    info.UpdateDate = DateTime.Now;
                    info.UpUserId = userId;
                    _repairSectionSemiFinishStockInfoRepository.Update(info);
                }
            }
            //_repairSectionSemiFinishStockDetailsRepository.InsertAll(stockDetails);
            //return _repairSectionSemiFinishStockDetailsRepository.Save();
            return _repairSectionSemiFinishStockInfoRepository.Save();
        }
        public bool SaveRepairSectionSemiFinishGoodStockOutForMiniStock(List<RepairSectionSemiFinishStockDetailsDTO> dTOs, long userId, long orgId)
        {
            List<RepairSectionSemiFinishStockDetails> stockDetails = new List<RepairSectionSemiFinishStockDetails>();
            foreach (var item in dTOs)
            {
                //RepairSectionSemiFinishStockDetails stockDetail = new RepairSectionSemiFinishStockDetails
                //{
                //    FloorId = item.FloorId,
                //    AssemblyLineId = item.AssemblyLineId,
                //    QCLineId = item.QCLineId,
                //    RepairLineId = item.RepairLineId,
                //    DescriptionId = item.DescriptionId,
                //    WarehouseId = item.WarehouseId,
                //    StateStatus = "Stock-Out",
                //    QRCode = item.QRCode,
                //    OrganizationId = orgId,
                //    EUserId = userId,
                //    EntryDate = DateTime.Now,
                //    ItemId = item.ItemId,
                //    ItemTypeId = item.ItemTypeId,
                //    TransferCode = item.TransferCode,
                //};
                //stockDetails.Add(stockDetail);
                var info = this.GetStockByOneById(item.FloorId, item.QCLineId, item.RepairLineId, item.AssemblyLineId, item.WarehouseId.Value, item.DescriptionId, orgId);
                if (info != null)
                {
                    info.StockOutQty += item.StockQty;
                    info.UpdateDate = DateTime.Now;
                    info.UpUserId = userId;
                    _repairSectionSemiFinishStockInfoRepository.Update(info);
                }
            }
            //_repairSectionSemiFinishStockDetailsRepository.InsertAll(stockDetails);
            //return _repairSectionSemiFinishStockDetailsRepository.Save();
            return _repairSectionSemiFinishStockInfoRepository.Save();
        }
        public IEnumerable<RepairSectionSemiFinishStockDetailsDTO> GetAllDetailsRepairSectionSemiFinish(long? flId, long? qcId, long? rqId, long? assId, long? warId, long? moId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<RepairSectionSemiFinishStockDetailsDTO>(QueryForGetAllDetailsRepairSectionSemiFinish(flId, qcId, rqId, assId, warId, moId, orgId)).ToList();
        }
        private string QueryForGetAllDetailsRepairSectionSemiFinish(long? flId, long? qcId, long? rqId, long? assId, long? warId, long? moId, long orgId)
        {
            string param = string.Empty;
            string query = string.Empty;

            param += string.Format(@" and rsinfo.OrganizationId={0}", orgId);
            if (flId != null && flId > 0)
            {
                param += string.Format(@" and rsinfo.FloorId={0}", flId);
            }
            if (moId != null && moId > 0)
            {
                param += string.Format(@" and rsinfo.DescriptionId={0}", moId);
            }
            if (assId != null && assId > 0)
            {
                param += string.Format(@" and rsinfo.AssemblyLineId={0}", assId);
            }
            if (rqId != null && rqId > 0)
            {
                param += string.Format(@" and rsinfo.RepairLineId={0}", rqId);
            }
            if (qcId != null && qcId > 0)
            {
                param += string.Format(@" and rsinfo.QCLineId={0}", qcId);
            }
            if (warId != null && warId > 0)
            {
                param += string.Format(@" and rsinfo.WarehouseId={0}", warId);
            }
            query = string.Format(@"Select fl.LineNumber'FloorName',asl.AssemblyLineName,de.DescriptionName'ModelName',rsinfo.QRCode,
qc.QCName'QCLineName',rp.RepairLineName,war.WarehouseName,StateStatus,rsinfo.EntryDate,rsinfo.UpdateDate,rsinfo.FloorId,rsinfo.AssemblyLineId,rsinfo.QCLineId,rsinfo.RepairLineId,rsinfo.DescriptionId,rsinfo.WarehouseId From tblRepairSectionSemiFinishStockDetails rsinfo
Left Join tblProductionLines fl on rsinfo.FloorId=fl.LineId
Left Join tblAssemblyLines asl on rsinfo.AssemblyLineId=asl.AssemblyLineId
Left Join [Inventory].dbo.tblDescriptions de on rsinfo.DescriptionId=de.DescriptionId
Left Join tblQualityControl qc on rsinfo.QCLineId=qc.QCId
Left Join tblRepairLine rp on rsinfo.RepairLineId=rp.RepairLineId
Left Join [Inventory].dbo.tblWarehouses war on rsinfo.WarehouseId=war.Id
Where rsinfo.OrganizationId={0} {1}
Order By rsinfo.EntryDate desc", orgId, Utility.ParamChecker(param));
            return query;
        }

        public bool QRCodeCheckMiniStock(string qrCode, string status, long orgId)
        {
            return this._repairSectionSemiFinishStockDetailsRepository.GetOneByOrg(s => s.QRCode == qrCode && s.StateStatus == status && s.OrganizationId == orgId) != null;
        }

        public bool UpdateStockAndReceiveQRCodeMiniStock(string qrCode, long userId,long orgId)
        {
            bool isSuccess = false;
            if(qrCode != null)
            {
                var qrCodeOneByOne = GetDetailsQRCodeStocKByQRCode(qrCode, orgId);
                if(qrCodeOneByOne != null)
                {
                    qrCodeOneByOne.StateStatus = "Stock-Out";
                    qrCodeOneByOne.UpUserId = userId;
                    qrCodeOneByOne.UpdateDate = DateTime.Now;
                }
                _repairSectionSemiFinishStockDetailsRepository.Update(qrCodeOneByOne);
                _repairSectionSemiFinishStockDetailsRepository.Save();
                if (qrCodeOneByOne != null)
                {
                    var stockInDb = GetInfoQRCodeStocKByQRCode(qrCodeOneByOne.FloorId, qrCodeOneByOne.AssemblyLineId, qrCodeOneByOne.RepairLineId, qrCodeOneByOne.QCLineId, orgId);
                    var reamainQty = 1;
                    if(stockInDb != null)
                    {
                        stockInDb.StockOutQty += reamainQty;
                        stockInDb.UpUserId = userId;
                        stockInDb.UpdateDate = DateTime.Now;
                    }
                    _repairSectionSemiFinishStockInfoRepository.Update(stockInDb);
                    if(_repairSectionSemiFinishStockInfoRepository.Save()==true)
                    {
                        isSuccess = _qRCodeTransferToRepairInfoBusiness.QRCodeStatusUpdate(qrCode, userId, orgId);
                    }
                }
            }
            return isSuccess;
        }

        public RepairSectionSemiFinishStockInfo GetInfoQRCodeStocKByQRCode(long flId, long assId, long rpId, long qcId, long orgId) => _repairSectionSemiFinishStockInfoRepository.GetOneByOrg(i => i.ProductionFloorId == flId && i.AssemblyLineId == assId && i.RepairLineId == rpId && i.QCId == qcId && i.OrganizationId == orgId);

        public RepairSectionSemiFinishStockDetails GetDetailsQRCodeStocKByQRCode(string qrCode, long orgId)
        {
            return _repairSectionSemiFinishStockDetailsRepository.GetOneByOrg(d =>d.QRCode==qrCode && d.OrganizationId == orgId);
        }

        public RepairSectionSemiFinishStockInfo GetStockByAllId(long flId, long assId, long rpId, long qcId, long model, long orgId)
        {
            return _repairSectionSemiFinishStockInfoRepository.GetOneByOrg(h => h.ProductionFloorId == flId && h.AssemblyLineId == assId && h.RepairLineId == rpId && h.QCId == qcId && h.DescriptionId == model && h.OrganizationId == orgId);
        }
        public List<Dropdown> GetAllAssemblyInStockByFloor(long floorId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<Dropdown>(string.Format(@"Select asm.AssemblyLineName'text',Cast(asm.AssemblyLineId as nvarchar(50))'value'
From tblRepairSectionSemiFinishStockInfo stock
Inner Join [Production].dbo.tblAssemblyLines asm on stock.AssemblyLineId =asm.AssemblyLineId
Where stock.ProductionFloorId = {0} and stock.OrganizationId={1}", floorId, orgId)).ToList();
        }
        public List<Dropdown> GetAllQCLineInStockByFloorAndAssembly(long floorId, long assemblyId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<Dropdown>(string.Format(@"Select qc.QCName'text',Cast(qc.QCId as nvarchar(50))'value'
From tblRepairSectionSemiFinishStockInfo stock
Inner Join [Production].dbo.tblQualityControl qc on stock.QCId =qc.QCId
Where stock.ProductionFloorId = {0} and stock.AssemblyLineId = {1} and stock.OrganizationId={2}", floorId, assemblyId,orgId)).ToList();
        }
        public List<Dropdown> GetAllRepairLineInStockByFloorAndAssemblyAndQC(long floorId, long assemblyId,long qcId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<Dropdown>(string.Format(@"Select rp.RepairLineName'text',Cast(rp.RepairLineId as nvarchar(50))'value'
From tblRepairSectionSemiFinishStockInfo stock
Inner Join [Production].dbo.tblRepairLine rp on stock.RepairLineId =rp.RepairLineId
Where stock.ProductionFloorId = {0} and stock.AssemblyLineId = {1} and stock.QCId={2} and stock.OrganizationId={3}", floorId, assemblyId,qcId, orgId)).ToList();
        }
        public List<Dropdown> GetAllModelsInStockByFloorAndAssemblyAndQCAndRepair(long floorId, long assemblyId, long qcId, long repairId, long orgId)
        {
            return this._productionDb.Db.Database.SqlQuery<Dropdown>(string.Format(@"Select des.DescriptionName'text',Cast(des.DescriptionId as nvarchar(50))'value'
From tblRepairSectionSemiFinishStockInfo stock
Inner Join [Inventory].dbo.tblDescriptions des on stock.DescriptionId =des.DescriptionId
Where stock.ProductionFloorId = {0} and stock.AssemblyLineId = {1} and stock.QCId={2} and stock.RepairLineId={3} and stock.OrganizationId={4}", floorId, assemblyId, qcId,repairId, orgId)).ToList();
        }
    }
}
