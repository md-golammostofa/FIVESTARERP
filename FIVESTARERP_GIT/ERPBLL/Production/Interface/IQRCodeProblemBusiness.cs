using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
    public interface IQRCodeProblemBusiness
    {
        IEnumerable<QRCodeProblemDTO> GetQRCodeProblemDTOByQuery(long transferId, string qrCode, long orgId);
        IEnumerable<QRCodeProblemDTO> GetQRCodeProblemList(long? qcLine, string qrCode, long? modelId,string prbName,long? qcId,long orgId);
    }
}
