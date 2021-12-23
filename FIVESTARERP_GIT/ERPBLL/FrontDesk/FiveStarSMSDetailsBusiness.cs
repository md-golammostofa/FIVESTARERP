using ERPBLL.FrontDesk.Interface;
using ERPBO.FrontDesk.DomainModels;
using ERPBO.FrontDesk.DTOModels;
using ERPDAL.FrontDeskDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.FrontDesk
{
    public class FiveStarSMSDetailsBusiness: IFiveStarSMSDetailsBusiness
    {
        private readonly IFrontDeskUnitOfWork _frontDeskUnitOfWork;
        private readonly FiveStarSMSDetailsRepository _fiveStarSMSDetailsRepository;

        public FiveStarSMSDetailsBusiness(IFrontDeskUnitOfWork frontDeskUnitOfWork)
        {
            this._frontDeskUnitOfWork = frontDeskUnitOfWork;
            this._fiveStarSMSDetailsRepository = new FiveStarSMSDetailsRepository(this._frontDeskUnitOfWork);
        }

        public bool SaveSMSDetails(FiveStarSMSDetailsDTO dto, long userId, long orgId, long branchId)
        {
            if (dto.SmsId == 0)
            {
                FiveStarSMSDetails details = new FiveStarSMSDetails();
                details.MobileNo = dto.MobileNo;
                details.Message = dto.Message;
                details.Response = dto.Response;
                details.Purpose = dto.Purpose;
                details.BranchId = branchId;
                details.OrganizationId = orgId;
                details.EUserId = userId;
                details.EntryDate = DateTime.Now;
                _fiveStarSMSDetailsRepository.Insert(details);
            }
            return _fiveStarSMSDetailsRepository.Save();
        }
    }
}
