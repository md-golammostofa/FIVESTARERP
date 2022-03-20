using ERPBO.Configuration.DomainModels;
using ERPBO.Configuration.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Configuration.Interface
{
    public interface IPartsTransferHToCDetailsBusiness
    {
        IEnumerable<PartsTransferHToCDetails> GetAllDetailsByInfoId(long infoId, long orgId);
    }
}
