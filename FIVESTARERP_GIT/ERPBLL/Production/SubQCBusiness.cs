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
   public class SubQCBusiness: ISubQCBusiness
    {
        private readonly IProductionUnitOfWork _productionDb;
        private readonly SubQCRepository _subQCRepository;
        public SubQCBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            this._subQCRepository = new SubQCRepository(this._productionDb);
        }

        public IEnumerable<SubQC> GetAllQCByOrgId(long orgId)
        {
            return _subQCRepository.GetAll(q => q.OrganizationId == orgId).ToList();
        }

        public SubQC GetOneById(long id, long orgId)
        {
            return _subQCRepository.GetOneByOrg(q => q.OrganizationId == orgId && q.SubQCId == id);
        }

        public bool SaveSubQC(SubQCDTO dto, long orgId, long userId)
        {
            if (dto.SubQCId == 0)
            {
                SubQC sub = new SubQC();
                sub.SubQCName = dto.SubQCName;
                sub.QCId = dto.QCId;
                sub.OrganizationId = orgId;
                sub.Remarks = dto.Remarks;
                sub.EntryDate = DateTime.Now;
                sub.EUserId = userId;
                _subQCRepository.Insert(sub);
            }
            else
            {
                var subQc = GetOneById(dto.SubQCId, orgId);
                if(subQc != null)
                {
                    subQc.SubQCName = dto.SubQCName;
                    subQc.QCId = dto.QCId;
                    subQc.Remarks = dto.Remarks;
                    subQc.UpdateDate = DateTime.Now;
                    subQc.UpUserId = userId;
                    _subQCRepository.Update(subQc);
                }
            }
            return _subQCRepository.Save();
        }
    }
}
