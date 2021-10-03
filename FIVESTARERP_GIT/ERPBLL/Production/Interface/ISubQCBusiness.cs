using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface ISubQCBusiness
    {
        IEnumerable<SubQC> GetAllQCByOrgId(long orgId);
        SubQC GetOneById(long id, long orgId);
        bool SaveSubQC(SubQCDTO dto, long orgId, long userId);
    }
}
