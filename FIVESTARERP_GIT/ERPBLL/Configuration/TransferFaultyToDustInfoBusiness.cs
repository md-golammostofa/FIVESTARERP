using ERPBLL.Configuration.Interface;
using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using ERPDAL.ConfigurationDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration
{
    public class TransferFaultyToDustInfoBusiness: ITransferFaultyToDustInfoBusiness
    {
        private readonly IConfigurationUnitOfWork _configurationDb; // database
        private readonly TransferFaultyToDustInfoRepository transferFaultyToDustInfoRepository; // repo
        private readonly TransferFaultyToDustDetailsRepository transferFaultyToDustDetailsRepository;
        private readonly IFaultyStockDetailBusiness _faultyStockDetailBusiness;
        private readonly IDustStockDetailsBusiness _dustStockDetailsBusiness;
        public TransferFaultyToDustInfoBusiness(IConfigurationUnitOfWork configurationDb, IFaultyStockDetailBusiness faultyStockDetailBusiness, IDustStockDetailsBusiness dustStockDetailsBusiness)
        {
            this._configurationDb = configurationDb;
            this.transferFaultyToDustInfoRepository = new TransferFaultyToDustInfoRepository(this._configurationDb);
            this.transferFaultyToDustDetailsRepository = new TransferFaultyToDustDetailsRepository(this._configurationDb);
            this._faultyStockDetailBusiness = faultyStockDetailBusiness;
            this._dustStockDetailsBusiness = dustStockDetailsBusiness;
        }

        public bool SaveTransferFaultyToDust(List<TransferFaultyToDustDetailsDTO> dto, long userId, long branchId, long orgId)
        {
            bool IsSuccess = false;
            Random random = new Random();
            var ran = random.Next().ToString();
            ran = ran.Substring(0, 4);
            var code = ("FTD-" + ran + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));

            List<FaultyStockDetailDTO> faultyList = new List<FaultyStockDetailDTO>();
            List<DustStockDetailsDTO> dustList = new List<DustStockDetailsDTO>();
            if (dto.Count() > 0)
            {
                TransferFaultyToDustInfo info = new TransferFaultyToDustInfo();
                info.TransferCode = code;
                info.StateStatus = "Faulty Transfer To Dust";
                info.EntryDate = DateTime.Now;
                info.EUserId = userId;
                info.BranchId = branchId;
                info.OrganizationId = orgId;

                List<TransferFaultyToDustDetails> detailsList = new List<TransferFaultyToDustDetails>();
                foreach(var item in dto)
                {
                    TransferFaultyToDustDetails details = new TransferFaultyToDustDetails
                    {
                        ModelId = item.ModelId,
                        PartsId = item.PartsId,
                        Quantity = item.Quantity,
                        CostPrice = 0,
                        SellPrice = 0,
                        EntryDate = DateTime.Now,
                        EUserId = userId,
                        BranchId = branchId,
                        OrganizationId = orgId,
                        RefCode=code,
                        Remarks="Faulty Transfer To Dust",
                    };
                    detailsList.Add(details);
                    FaultyStockDetailDTO faulty = new FaultyStockDetailDTO
                    {
                        DescriptionId = item.ModelId,
                        PartsId = item.PartsId,
                        Quantity = item.Quantity,
                        CostPrice = 0,
                        SellPrice = 0,
                    };
                    faultyList.Add(faulty);

                    DustStockDetailsDTO dust = new DustStockDetailsDTO
                    {
                        ModelId = item.ModelId,
                        PartsId = item.PartsId,
                        Quantity = item.Quantity,
                    };
                    dustList.Add(dust);
                }
                info.transferFaultyToDustDetails = detailsList;
                transferFaultyToDustInfoRepository.Insert(info);
            }
            if (transferFaultyToDustInfoRepository.Save() == true)
            {
                IsSuccess = _faultyStockDetailBusiness.SaveFaultyStockOutForTransferToDust(faultyList, userId, orgId, branchId);
                if (IsSuccess == true)
                {
                    return _dustStockDetailsBusiness.SaveDustStockInFromFaulty(dustList, userId, branchId, orgId);
                }
            }
            return IsSuccess;
        }
    }
}
