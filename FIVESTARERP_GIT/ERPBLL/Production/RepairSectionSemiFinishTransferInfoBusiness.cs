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
    public class RepairSectionSemiFinishTransferInfoBusiness : IRepairSectionSemiFinishTransferInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly RepairSectionSemiFinishTransferInfoRepository _repairSectionSemiFinishTransferInfoRepository;
        private readonly RepairSectionSemiFinishTransferDetailsRepository _repairSectionSemiFinishTransferDetailsRepository;
        private readonly IQRCodeTransferToRepairInfoBusiness _qRCodeTransferToRepairInfoBusiness;
        private readonly IRepairItemStockDetailBusiness _repairItemStockDetailBusiness;

        public RepairSectionSemiFinishTransferInfoBusiness(IProductionUnitOfWork productionDb, IQRCodeTransferToRepairInfoBusiness qRCodeTransferToRepairInfoBusiness, RepairSectionSemiFinishTransferDetailsRepository repairSectionSemiFinishTransferDetailsRepository, IRepairItemStockDetailBusiness repairItemStockDetailBusiness)
        {
            this._productionDb = productionDb;
            this._repairSectionSemiFinishTransferInfoRepository = new RepairSectionSemiFinishTransferInfoRepository(this._productionDb);
            this._repairSectionSemiFinishTransferDetailsRepository = new RepairSectionSemiFinishTransferDetailsRepository(this._productionDb);
            this._qRCodeTransferToRepairInfoBusiness = qRCodeTransferToRepairInfoBusiness;
            this._repairItemStockDetailBusiness = repairItemStockDetailBusiness;
        }

        public RepairSectionSemiFinishTransferInfo GetQRCodeDetailsByInfoId(long infoId, long orgId)
        {
            return _repairSectionSemiFinishTransferInfoRepository.GetOneByOrg(i => i.TransferInfoId == infoId && i.OrganizationId == orgId);
        }

        public IEnumerable<RepairSectionSemiFinishTransferInfoDTO> RepairSectionSemiFinishGoodReceive(long orgId)
        {
            var data = this._productionDb.Db.Database.SqlQuery<RepairSectionSemiFinishTransferInfoDTO>(string.Format(@"Select TransferInfoId,TransferCode,StateStatus,Qty,EntryDate
From tblRepairSectionSemiFinishTransferInfo Where OrganizationId={0}", orgId)).ToList();
            return data;
        }

        public async Task<bool> SaveRepairSectionSemiFinishTransferItem(long[] qRCodesId, int qty, long userId, long orgId)
        {
            List<RepairItemStockDetailDTO> repairItemStocks = new List<RepairItemStockDetailDTO>();
            bool isSuccess = false;
            if (qty > 0)
            {
                var code = ("RSST-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
                List<RepairSectionSemiFinishTransferDetails> details = new List<RepairSectionSemiFinishTransferDetails>();

                RepairSectionSemiFinishTransferInfo info = new RepairSectionSemiFinishTransferInfo();
                info.TransferCode = code;
                info.StateStatus = "Send By Repair Section";
                info.Qty = qty;
                info.EUserId = userId;
                info.EntryDate = DateTime.Now;
                info.OrganizationId = orgId;
                _repairSectionSemiFinishTransferInfoRepository.Insert(info);

                foreach (var qRCode in qRCodesId)
                {
                    var qrCode = _qRCodeTransferToRepairInfoBusiness.GetOneByQRCodeById(qRCode, orgId);
                    RepairSectionSemiFinishTransferDetails detail = new RepairSectionSemiFinishTransferDetails()
                    {
                        FloorId = qrCode.FloorId,
                        QCLineId = qrCode.QCLineId,
                        RepairLineId = qrCode.RepairLineId,
                        QRCode = qrCode.QRCode,
                        AssemblyLineId = qrCode.AssemblyLineId,
                        DescriptionId = qrCode.DescriptionId,
                        WarehouseId = qrCode.WarehouseId,
                        ItemTypeId = qrCode.ItemTypeId,
                        ItemId = qrCode.ItemId,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        OrganizationId = orgId,
                        StateStatus = code,
                    };
                    details.Add(detail);
                    // Repair Item Stocks
                    RepairItemStockDetailDTO repairItemStock = new RepairItemStockDetailDTO()
                    {
                        ProductionFloorId = qrCode.FloorId,
                        AssemblyLineId = qrCode.AssemblyLineId,
                        RepairLineId = qrCode.RepairLineId,
                        QCId = qrCode.QCLineId,
                        DescriptionId = qrCode.DescriptionId,
                        ItemId = qrCode.ItemId,
                        ItemTypeId = qrCode.ItemTypeId,
                        WarehouseId = qrCode.WarehouseId,
                        Quantity = 1,
                        OrganizationId = orgId,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        StockStatus = StockStatus.StockOut,
                        ReferenceNumber = code,
                        Remarks = "Stock Out By Mini Stock Transfer-" + qrCode.QRCode
                    };
                    repairItemStocks.Add(repairItemStock);
                }
                _repairSectionSemiFinishTransferDetailsRepository.InsertAll(details);
                info.RepairSectionSemiFinishTransferDetails = details;
                if (_repairSectionSemiFinishTransferInfoRepository.Save() == true)
                {
                    if (await _repairItemStockDetailBusiness.SaveRepairItemStockOutAsync(repairItemStocks, userId, orgId))
                    {
                        isSuccess = _qRCodeTransferToRepairInfoBusiness.QRCodeUpdateStatus(qRCodesId, orgId, userId);
                    }
                }
            }
            return isSuccess;
        }

        public bool UpdateStatusRepairSection(long infoId, long userId, long orgId)
        {
            var qrCode = GetQRCodeDetailsByInfoId(infoId, orgId);
            if (qrCode != null)
            {
                qrCode.StateStatus = "Received By MiniStock";
                qrCode.UpUserId = userId;
                qrCode.UpdateDate = DateTime.Now;
            }
            _repairSectionSemiFinishTransferInfoRepository.Update(qrCode);
            return _repairSectionSemiFinishTransferInfoRepository.Save();
        }
    }
}
