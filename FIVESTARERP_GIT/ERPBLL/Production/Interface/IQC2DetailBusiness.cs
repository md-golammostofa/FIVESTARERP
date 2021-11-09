using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IQC2DetailBusiness
    {
        IEnumerable<QC2Detail> GetAllQC2ProblemByAssemblyId(long assemblyId, DateTime date, long orgId);
    }
}
