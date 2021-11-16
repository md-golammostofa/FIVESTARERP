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
    public class MiniStockTransferToWarehouseInfoBusiness: IMiniStockTransferToWarehouseInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly MiniStockTransferToWarehouseInfoRepository _transferToWarehouseInfoRepository;
        private readonly MiniStockTransferToWarehouseDetailsRepository _transferToWarehouseDetailsRepository;
        public MiniStockTransferToWarehouseInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._transferToWarehouseInfoRepository = new MiniStockTransferToWarehouseInfoRepository(this._productionDb);
            this._transferToWarehouseDetailsRepository = new MiniStockTransferToWarehouseDetailsRepository(this._productionDb);
        }

        public IEnumerable<MiniStockTransferToWarehouseInfo> GetAllTransferList(long orgId)
        {
            return this._transferToWarehouseInfoRepository.GetAll(t => t.OrganizationId == orgId).ToList();
        }

        public bool SaveMiniStockTransferToWarehouse(MiniStockTransferToWarehouseInfoDTO dto, long orgId, long userId)
        {
            bool IsSuccess = false;
            if (dto.miniStockTransferToWarehouseDetails.Count() > 0)
            {
                var code = ("JOB-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));
                MiniStockTransferToWarehouseInfo miniStockTransfer = new MiniStockTransferToWarehouseInfo();
                miniStockTransfer.TransferCode = code;
                miniStockTransfer.FloorId = dto.FloorId;
                miniStockTransfer.AssemblyLineId = dto.AssemblyLineId;
                miniStockTransfer.QCLine = dto.QCLine;
                miniStockTransfer.RepairLineId = dto.RepairLineId;
                miniStockTransfer.DescriptionId = dto.DescriptionId;
                miniStockTransfer.TransferStatus = "Pending";
                miniStockTransfer.ReturnStatus = "";
                miniStockTransfer.OrganizationId = orgId;
                miniStockTransfer.EUserId = userId;
                miniStockTransfer.EntryDate = DateTime.Now;
                _transferToWarehouseInfoRepository.Insert(miniStockTransfer);

                List<MiniStockTransferToWarehouseDetails> detailslist = new List<MiniStockTransferToWarehouseDetails>();
                foreach(var item in dto.miniStockTransferToWarehouseDetails)
                {
                    MiniStockTransferToWarehouseDetails details = new MiniStockTransferToWarehouseDetails
                    {
                        FloorId = dto.FloorId,
                        AssemblyLineId = dto.AssemblyLineId,
                        QCLine = dto.QCLine,
                        RepairLineId = dto.RepairLineId,
                        DescriptionId = dto.DescriptionId,
                        Quantity = item.Quantity,
                        Remarks= code,
                        OrganizationId=orgId,
                        EUserId=userId,
                        EntryDate=DateTime.Now
                    };
                    detailslist.Add(details);
                }
                miniStockTransfer.MiniStockTransferToWarehouseDetails = detailslist;
                if (_transferToWarehouseInfoRepository.Save()==true)
                {
                    IsSuccess = true;
                }
            }
            return IsSuccess;
        }
    }
}
